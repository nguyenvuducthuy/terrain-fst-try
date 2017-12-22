﻿using System;
using System.Collections.Generic;
using Common;
using DeferredRender.Graphics.FrameBuffer;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace DeferredRender.Graphics
{
    internal class GraphicsSystem
    {
        private int _height;
        private int _width;
        private Player _player;

        public Matrix4 Projection;
        public Matrix4 ModelView;
        public Matrix4 ModelViewProjection;

        public FrameBufferManager FrameBuf { get; set; }

        public GraphicsSystem(int width, int height, Player player)
        {
            _width = width;
            _height = height;
            _player = player;
            InitGraphics();
        }

        private void InitGraphics()
        {
            float aspect = (float)_width / _height;
            GL.Viewport(0, 0, _width, _height);
            Projection = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.1f, 400);

            Shaders.InitTexturelessNoLight();
            Shaders.InitTexturedNoLight();
            Shaders.InitSecondGBufferPassProgram();

            FrameBuf = new FrameBufferManager(_width, _height);
        }

        internal void Render(List<SimpleModel> models)
        {
            FrameBuf.EnableMainFrameBuffer();
            RenderToCurrentTarget(models);
            FrameBuf.DisableMainFrameBuffer();
            DrawUsingGBuffer();
        }


        private void RenderToCurrentTarget(List<SimpleModel> models)
        {
            RebuildMatrices();

            GL.ClearColor(0, 0f, 0.1f, 1f);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            foreach (var model in models)
            {
                model.Bind(ModelView, ModelViewProjection, Projection);

                GL.DrawArrays(PrimitiveType.Triangles, 0, model.Vertices.Length);
            }

            GL.Flush();

        }


        public void DrawUsingGBuffer()
        {
            GL.Disable(EnableCap.DepthTest);
            GL.ClearColor(0, 0f, 0f, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            Shaders.BindOneQuadScreenAndDraw(FrameBuf);

            GL.Flush();
        }


        private void RebuildMatrices()
        {
            ModelView = Matrix4.LookAt(_player.Position, _player.Target, Vector3.UnitY);
            ModelViewProjection = Matrix4.Mult(ModelView, Projection);
        }
    }
}