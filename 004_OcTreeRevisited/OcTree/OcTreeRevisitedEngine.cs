﻿using System;
using System.Collections.Generic;
using Common;
using Common.Geometry;
using Common.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace OcTreeRevisited
{
    internal class OcTreeRevisitedEngine : AbstractEngine
    {
        public Player Player { get; set; }

        public SimpleModel Model { get; set; }

        OcTree.OcTree Tree { get; set; }

        public AbstractRenderEngine MainRender { get; set; }

        public LineRender LineRender { get; set; }

        public SkyBoxRenderer SkyRend { get; set; }

        public List<GameObject> Objects { get; set; }

        public OcTreeRevisitedEngine(int Width, int Height)
            : base(Width, Height)
        {
            Player = new Player();
            KeyHandler.KeyPress += HandleKeyPress;
            MainRender = new RenderEngine(Width, Height, Player);
            LineRender = new LineRender(Width, Height, Player, (RenderEngine)MainRender);
            Tree = CreateTree();
            SkyRend = new SkyBoxRenderer();
        }

        private void HandleKeyPress(Common.Input.InputSignal signal)
        {
            Player.OnSignal(signal);
        }

        public override void Click(Vector2 point)
        {

        }

        public override void Tick(long timeSlice, Vector2 dxdy)
        {
            KeyHandler.CheckKeys();

            Player.Tick(timeSlice, dxdy);

            GL.ClearColor(0, 0, 0.1f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var mod = Tree.GetModel();

            LineRender.Render(mod);

            foreach (var gameObj in Objects)
            {
                gameObj.Tick(timeSlice);
                LineRender.Render(gameObj.GetAsModel());
            }
            GL.Flush();
        }

        public void HandleLeavingOctant(object sender, ReinsertingEventArgs args)
        {
            if (sender == null)
            {
                throw new ArgumentNullException();
            }
            var gameObj = sender as GameObject;
            if (gameObj == null)
            {
                throw new ArgumentException($"{gameObj.GetType()}", nameof(sender));
            }

            Tree.Remove(gameObj);

            if (args.NewBox == null)
            {
                throw new ArgumentException("args.NewBox is null");
            }

            gameObj.UpdateBoundingBox(args.NewBox);
            Tree.Insert(gameObj);
        }    

        private OcTree.OcTree CreateTree()
        {
            var tree = new OcTree.OcTree(BoundingVolume.CreateVolume(new Vector3(0, 0, 0), 80));

            Objects = CreateListObjects();
            Objects.ForEach(
                o =>
                tree.Insert(o));

            return tree;
        }

        private List<GameObject> CreateListObjects()
        {
            var result = new List<GameObject>();

            Random rand = new Random(300);
            AddRandomPlane(result, new Vector3(47f, 5, -45), rand, "n11");

            AddRandomPlane(result, new Vector3(46f, 17, -45), rand, "n12");

            AddRandomPlane(result, new Vector3(40f, 35, -45), rand, "n13");

            AddRandomPlane(result, new Vector3(55f, -35, -15), rand, "n14");

            AddRandomPlane(result, new Vector3(60f, 45, -35), rand, "n15");

            AddRandomPlane(result, new Vector3(54f, 12, -5), rand, "n16");

            return result;
        }

        private void AddRandomPlane(List<GameObject> result, Vector3 centre, Random rand, string name)
        {
            var dx = (float)rand.NextDouble() * -0.5f;
            var speed = new Vector3(dx, 0, 0);
            var plane2 = new GameObject(centre)
            {
                Name = name,
                Speed = speed
            };
            plane2.NeedReinsert += HandleLeavingOctant;
            result.Add(plane2);
        }
    }
}