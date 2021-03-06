﻿using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

namespace OcTreeRevisited
{
    class MainForm : GameWindow
    {

        public OcTreeRevisitedEngine   Engine { get; set; }
        public long Start { get; set; }
        public Stopwatch Watch { get; set; }

        public MainForm() : base(1920, 900, GraphicsMode.Default, "octreev2", GameWindowFlags.Default,
            DisplayDevice.Default, major: 4, minor: 0, flags: GraphicsContextFlags.ForwardCompatible)
        {
            CursorVisible = false;
            Location = new System.Drawing.Point()
            {
                X = 0,
                Y = 0
            };
            Watch = new Stopwatch();
            Engine = new OcTreeRevisitedEngine(Width, Height);
            ResetMouse();
            Watch.Start();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //base.OnRenderFrame(e);

            if (!Watch.IsRunning)
            {
                Watch.Start();
            }

            long end = Watch.ElapsedMilliseconds;
            Vector2 dxdy = GetChanges();
            Engine.Tick(end - Start, dxdy);
            ResetMouse();

            SwapBuffers();
            Start = end;

        }

        private Vector2 GetChanges()
        {
            var mouseState = System.Windows.Forms.Cursor.Position;

            var res = new Vector2()
            {
                X = (Location.X + Width / 2 - mouseState.X),
                Y = (Location.Y + Height / 2 - mouseState.Y)
            };      

            return res;
        }

        private void ResetMouse()
        {
            OpenTK.Input.Mouse.SetPosition(Location.X + Width / 2, Location.Y + Height / 2);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            OpenTK.Graphics.OpenGL4.GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F4:
                    if (e.Alt)
                    {
                        Exit();
                    }
                    break;

                default:
                    break;
            }
        }

    }
}
