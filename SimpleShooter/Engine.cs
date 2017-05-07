﻿using System;
using System.Linq;
using System.Collections.Generic;
using Common.Input;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.Graphics;
using SimpleShooter.Player;
using SimpleShooter.LevelLoaders;
using SimpleShooter.Player.Events;
using Common.Geometry;
using OcTreeLibrary;
using Common;
using SimpleShooter.Helpers;
using System.Windows.Forms;

namespace SimpleShooter
{
    class Engine
    {
        private KeyHandler _keyHandler;
        public GraphicsSystem _graphics;

        private IShooterPlayer _player;
        public Level _level;
        private OcTree _tree;

        private ObjectsGrouped _objects;

        public Engine(int width, int height, IObjectInitializer initFunc)
        {           
            InitObjects(initFunc);
            _graphics = new GraphicsSystem(width, height);
            _keyHandler = new KeyHandler();
            _keyHandler.KeysToWatch.Add(Keys.Space);
            _keyHandler.KeyPress += KeyPress;
        }

        private void InitObjects(IObjectInitializer initFunc)
        {
            _objects = new ObjectsGrouped();

            _level = initFunc.CreateLevel();
            _tree = new OcTree(_level.Volume);

            InitPlayer();

            foreach (var obj in _level.Objects)
            {
                AddObject(obj);
            }

            _level.Objects.Clear();
        }

        private void OctreeItem_Remove(object sender, ReinsertingEventArgs args)
        {
            var gameObj = sender as IOctreeItem;
            if (gameObj == null)
                throw new ArgumentException();
            _tree.Remove(gameObj);            
        }

        private void OctreeItem_Insert(object sender, ReinsertingEventArgs args)
        {
            var gameObj = sender as IOctreeItem;
            if (gameObj == null)
                throw new ArgumentException();
           var v = _tree.Insert(gameObj);
            if (v == null)
            {
                // remove it
            }       
        }

        private void InitPlayer()
        {
            _player = _level.Player;
            _player.Jump += Player_Jump;
            _player.Move += Player_Move;
            _player.Shot += Player_Shot;
        }

        internal void Tick(long delta, Vector2 dxdy)
        {
            _keyHandler.CheckKeys();
            _player.HandleMouseMove(dxdy);
            PhysicsStep(delta);

            List<IRenderWrapper> lst = null;

            //var wrapper = new OctreeRenderWrapper(_tree);
            //lst = new List<IRenderWrapper>(_gameObjects);
            //lst.Add(wrapper);

            //if (lst != null)
            //{
            //    _graphics.Render(lst, _level);
            //}
            //else
            //{
                _graphics.Render(_objects, _level);
          //  }
            
        }

        private void PhysicsStep(long delta)
        {
            foreach (var obj in _objects.GameObjectsLine)
            {
                TickForMovable(delta, obj);
            }
            foreach (var obj in _objects.GameObjectsSimpleModel)
            {
                TickForMovable(delta, obj);
            }
            foreach (var obj in _objects.GameObjectsTextureLess)
            {
                TickForMovable(delta, obj);
            }
            foreach (var obj in _objects.GameObjectsTextureLessNoLight)
            {
                TickForMovable(delta, obj);
            }
            _player.Tick(delta);
        }

        private static void TickForMovable(long delta, GameObjectDescriptor obj)
        {
            var movable = obj.GameIdentity as IMovableObject;
            if (movable != null)
            {
                movable.Tick(delta);
            }
        }

        private ActionStatus Player_Shot(object sender, ShotEventArgs args)
        {
            var res = new ActionStatus()
            {
                Success = false
            };

            var player = sender as IShooterPlayer;
            if (player != null)
            {
                res.Success = true;
                var projectile = Projectileshelper.CreateProjectile(player);
                AddObject(projectile);
            }

            return res;
        }

        private void AddObject(GameObject obj)
        {
            var desc = new GameObjectDescriptor(obj);
            _objects.AddObject(desc);

            if (desc.RenderIdentity.ShaderKind != ShadersNeeded.Line)
            {
                desc.GameIdentity.OctreeItem.NeedsRemoval += OctreeItem_Remove;
                desc.GameIdentity.OctreeItem.NeedsInsert += OctreeItem_Insert;

                var volume = _tree.Insert(desc.GameIdentity.OctreeItem);
                if (volume == null)
                {
                    _objects.Remove(desc);

                }
            }
        }

        private ActionStatus Player_Move(object sender, MoveEventArgs args)
        {
            var res = new ActionStatus()
            {
                Success = true
            };

            return res;
        }

        private ActionStatus Player_Jump(object sender, JumpEventArgs args)
        {
            var res = new ActionStatus()
            {
                Success = true
            };

            return res;
        }

        private void KeyPress(InputSignal signal)
        {
            _player.Handle(signal);
        }

    }
}
