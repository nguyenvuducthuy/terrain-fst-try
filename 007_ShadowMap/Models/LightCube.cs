﻿using System.Linq;
using OpenTK;

namespace ShadowMap
{
    public class LightCube : Cube
    {
        public LightCube(Vector3 center, Vector3 color, float scale = 1) : base(center, color, scale)
        {
            Center = center;
        }

        public Vector3 Center { get; set; }
        public Vector3 Target { get; set; }


        public LightCube Tick()
        {
            var rot = Matrix4.CreateRotationY(0.015f);
            var tr = Matrix4.CreateTranslation(-Target);
            var trBack = Matrix4.CreateTranslation(Target);

            var combined = tr * rot * trBack;
            var v = Vector3.Transform(Center, combined);

            var temp = new LightCube(v, Colors.FirstOrDefault())
            {
                Target = Target
            };
            return temp;            
        }
    }
}
