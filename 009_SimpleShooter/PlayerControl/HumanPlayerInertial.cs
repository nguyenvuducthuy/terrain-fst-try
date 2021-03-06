﻿using Common;
using Common.Input;
using OpenTK;

namespace SimpleShooter.PlayerControl
{
    /// <summary>
    /// uses acceleration for all movements
    /// </summary>
    class HumanPlayerInertial : HumanPlayer
    {
        public HumanPlayerInertial(SimpleModel model, Vector3 position, Vector3 target, float mass) : base(model, position, target, mass)
        {

        }

        public override void Handle(InputSignal signal)
        {
            switch (signal)
            {
                case InputSignal.FORWARD:
                    StepXZ(StepForward);
                    break;
                case InputSignal.BACK:
                    StepXZ(StepBack);
                    break;
                case InputSignal.RIGHT:
                    StepXZ(StepRight);
                    break;
                case InputSignal.LEFT:
                    StepXZ(StepLeft);
                    break;
                case InputSignal.SPACE:
                    TryJump();
                    break;
                case InputSignal.MOUSE_CLICK:
                    TryShoot();
                    break;
                default:
                    break;
            }
        }

        protected override void StepXZ(Vector3 stepDirection)
        {
            var rotation = Matrix4.CreateRotationY(AngleHorizontalRadians);
            Vector3 dPosition = Vector3.Transform(stepDirection, rotation);

            Acceleration += dPosition;
        }
    }
}
