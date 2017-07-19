﻿using System.Linq;
using Common;
using Common.Utils;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.Graphics;
using SimpleShooter.PlayerControl;

namespace SimpleShooter.Helpers
{
    class ProjectilesHelper
    {
        public static GameObject CreateProjectile(IShooterPlayer player)
        {
            var point = player.Position;
            var speed = Vector3.Normalize(player.Target - point) * 40;

            var model = new SimpleModel();
            model.Vertices = GeometryHelper.GetVerticesForCube(0.05f);

            for (int i = 0; i < model.Vertices.Length; i++)
            {
                model.Vertices[i] += player.Position;
            }

            model.Colors = Enumerable.Repeat(new Vector3(1, 0, 0), model.Vertices.Length).ToArray();
            var movableObj = new MovableObject(model, ShadersNeeded.TextureLessNoLight, speed, Vector3.Zero);
            movableObj.CalcNormals();
            return movableObj;
        }
    }
}
