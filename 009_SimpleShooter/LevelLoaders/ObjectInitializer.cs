﻿using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Geometry;
using Common.Utils;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.Core.Enemies;
using SimpleShooter.Graphics;
using SimpleShooter.PlayerControl;

namespace SimpleShooter.LevelLoaders
{
    public class ObjectInitializer : IObjectInitializer
    {
        private Vector3 _lightPos = new Vector3(30, 20, 0);

        public static float Edge = 200;

        public static float TapeWidth = 0.15f;

        public Level CreateLevel()
        {
            var level = new Level();

            InitObjects(level);
            InitPlayer(level);
            level.LightPosition = _lightPos;
            level.Volume = BoundingVolume.CreateVolume(new Vector3(0, 0, 0), Edge);

            return level;
        }

        protected virtual void InitObjects(Level level)
        {
            var green = Vector3.UnitY;
            var red = Vector3.UnitX;
            var white = new Vector3(100, 100, 100);

            var objectList = new List<GameObject>();
            Matrix4 translate = Matrix4.CreateTranslation(30, 4, 0);

            GameObject obj = CreateCube(translate, green, 1, ShadersNeeded.TextureLess);
            objectList.Add(obj);

            translate = Matrix4.CreateTranslation(_lightPos);
            obj = CreateCube(translate, white, 0.5f, ShadersNeeded.TextureLessNoLight);
            objectList.Add(obj);


            obj = CreateWafer();
            objectList.Add(obj);


            translate = Matrix4.CreateTranslation(new Vector3(20, 10, 0));
            obj = CreateCube(translate, red, 1f, ShadersNeeded.TextureLessNoLight);
            var movableObj = new MovableObject(obj.Model, ShadersNeeded.TextureLessNoLight, new Vector3(1, 0, 0), new Vector3());
            objectList.Add(movableObj);


            AddEnemies(objectList);

            level.Objects = objectList;
        }

        private void AddEnemies(List<GameObject> objectList)
        {
            var obj = new SimpleModel(@"Content\Models\Armor Sphere.obj", @"Content\Models\armour03s.jpg");

            var farX = obj.Vertices.Max(v => v.X) * -1;

            Matrix4 translate1 = Matrix4.CreateTranslation(farX + 10, 10, 10);

            obj.Vertices = obj.Vertices.Select(v => Vector3.Transform(v, Matrix4.CreateScale(3) * translate1 )).ToArray();

            var enemy = new Enemy(obj, 0, 10);
            enemy.Weapon = new Core.Weapons.BaseWeapon();
            objectList.Add(enemy);
        }

        protected virtual void InitPlayer(Level level)
        {
            var position = new Vector3(0, 1f, 0);
            Matrix4 translate = Matrix4.CreateTranslation(position);

            var vertices = GeometryHelper.GetVerticesForCube(1);
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = Vector3.Transform(vertices[i], translate);
            }

            var model = new SimpleModel()
            {
                Normals = new Vector3[0],
                Vertices = vertices,
                Colors = new Vector3[0]
            };

            var player = new HumanPlayer(model, position, new Vector3(100, 0.5f, 0), mass:1); 

            level.Player = player;

            /*vertices = new Vector3[]
            {
                new Vector3(100, 1f, 0),
                new Vector3(100, 0.0f, 0),

                new Vector3(100, 0.5f, -0.5f),
                new Vector3(100, 0.5f, 0.5f)
            };

            model = new SimpleModel()
            {
                Vertices = vertices,
                Colors = Enumerable.Repeat(new Vector3(1, 0, 0), vertices.Length).ToArray()
            };*/
        }

        public static GameObject CreateWafer()
        {
            var verticesPlane = new[]
            {
                new Vector3(Edge, 0, -Edge),
                new Vector3(-Edge, 0, Edge),
                new Vector3(-Edge, 0, -Edge),

                new Vector3(Edge, 0, -Edge),
                new Vector3(Edge, 0, Edge),
                new Vector3(-Edge, 0, Edge),
            };

            var verticesOx = new[]
            {
                new Vector3(Edge, 0.2f, -TapeWidth),
                new Vector3(-Edge, 0.2f, TapeWidth),
                new Vector3(-Edge, 0.2f, -TapeWidth),

                new Vector3(Edge, 0.2f, -TapeWidth),
                new Vector3(Edge, 0.2f, TapeWidth),
                new Vector3(-Edge, 0.2f, TapeWidth),
            };

            var verticesOZ = new[]
            {
                new Vector3(TapeWidth, 0.2f, -Edge),
                new Vector3(-TapeWidth, 0.2f, Edge),
                new Vector3(-TapeWidth, 0.2f, -Edge),

                new Vector3(TapeWidth, 0.2f, -Edge),
                new Vector3(TapeWidth, 0.2f, Edge),
                new Vector3(-TapeWidth, 0.2f, Edge),
            };

            var colorsCombined = new[]
            {
                 new Vector3(0, 0, 0.4f),
                 new Vector3(0, 0, 0.4f),
                 new Vector3(0, 0, 0.4f),

                 new Vector3(0, 0, 0.4f),
                 new Vector3(0, 0, 0.4f),
                 new Vector3(0, 0, 0.4f),

                 // 0x
                 new Vector3(0, 0.7f, 0.0f),
                 new Vector3(0, 0.1f, 0.0f),
                 new Vector3(0, 0.1f, 0.0f),

                 new Vector3(0, 0.7f, 0.0f),
                 new Vector3(0, 0.7f, 0.0f),
                 new Vector3(0, 0.1f, 0.0f), 
                 
                 // 0z
                 new Vector3(0.7f, 0.0f, 0.0f),
                 new Vector3(0.1f, 0.0f, 0.0f),
                 new Vector3(0.7f, 0.0f, 0.0f),

                 new Vector3(0.7f, 0.0f, 0.0f),
                 new Vector3(0.1f, 0.0f, 0.0f),
                 new Vector3(0.1f, 0.0f, 0.0f),
            };


            var verticesCombined = new List<Vector3>();
            verticesCombined.AddRange(verticesPlane);
            verticesCombined.AddRange(verticesOx);
            verticesCombined.AddRange(verticesOZ);

            var model = new SimpleModel()
            {
                Vertices = verticesCombined.ToArray(),
                Colors = colorsCombined,
                Normals = Enumerable.Repeat(Vector3.UnitY, verticesCombined.Count).ToArray()
            };
            var obj = new GameObject(model, ShadersNeeded.TextureLess);
            return obj;
        }

        public static GameObject CreateCube(Matrix4 translate, Vector3 color, float size, ShadersNeeded shadersKind)
        {
            var model = new SimpleModel();
            var vertices = GeometryHelper.GetVerticesForCube(size);

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = Vector3.Transform(vertices[i], translate);
            }

            model.Vertices = vertices;
            model.Colors = Enumerable.Repeat(color, vertices.Length).ToArray();

            var obj = new GameObject(model, shadersKind);
            obj.CalcNormals();
            return obj;
        }
    }
}
