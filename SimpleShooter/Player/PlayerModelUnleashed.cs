﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Input;
using OpenTK;
using SimpleShooter.Player.Events;

namespace SimpleShooter.Player
{
    public class PlayerModelUnleashed : Player
    {

        public PlayerModelUnleashed(Vector3 position, Vector3 target)
        {
            Position = position;
            target = Target;
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
                case InputSignal.UP:

                    AngleVerticalRadians -= 0.01f;
                    Rotate();

                    break;
                case InputSignal.DOWN:
                    AngleVerticalRadians += 0.01f;
                    Rotate();

                    break;
                case InputSignal.ROTATE_CLOCKWISE:
                    AngleHorizontalRadians += 0.01f;
                    Rotate();
                    break;
                case InputSignal.ROTATE_COUNTERCLOCKWISE:
                    AngleHorizontalRadians -= 0.01f;
                    Rotate();
                    break;
                case InputSignal.SHOT:
                    break;
                case InputSignal.LOOK_UP:
                    break;
                case InputSignal.LOOK_DOWN:
                    break;
                case InputSignal.FLY_BY_CLOCKWISE:
                    break;
                case InputSignal.FLY_BY_COUNTERCLOCKWISE:
                    break;
                case InputSignal.NONE:
                    break;
                case InputSignal.DOWN_PARALLEL:
                    StepYZ(StepDown);
                    break;
                case InputSignal.UP_PARALLEL:
                    StepYZ(StepUp);
                    break;
                case InputSignal.MOUSE_CLICK:
                    break;
                case InputSignal.RENDER_MODE:
                    break;
                case InputSignal.CHANGE_CAMERA:
                    break;
                default:
                    break;
            }
        }


    }
}