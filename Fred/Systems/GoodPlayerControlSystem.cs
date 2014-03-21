using Artemis;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using Artemis.Utils;
using Fred.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update)]
    class GoodPlayerControlSystem : TagSystem
    {

        private GraphicsDevice graphicsDevice;

        public GoodPlayerControlSystem()
            : base("GOOD_PLAYER")
        {
        }

        public override void LoadContent()
        {
            graphicsDevice = BlackBoard.GetEntry<GraphicsDevice>("GraphicsDevice");
        }

        public override void Process(Entity entity)
        {
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            VelocityComponent velocityComponent = entity.GetComponent<VelocityComponent>();

            float turningOffset = 1F;
            float changeInAngle = 1;
            float maxMoveSpeed = .2F;
            float keyMoveSpeed = 0.001F * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
            float moveSpeedFriction = 0.0003f * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;

            Keys[] pressed_Key = Keyboard.GetState().GetPressedKeys();

            for (int i = 0; i < pressed_Key.Length; i++)
            {
                switch (pressed_Key[i])
                {

                    case Keys.A:
                        if (Math.Abs(velocityComponent.Speed) < turningOffset)
                        {
                            velocityComponent.Angle = 180;
                        }

                        if (velocityComponent.Angle >= 90 && velocityComponent.Angle < 270)
                        {
                            velocityComponent.Speed += keyMoveSpeed;
                        }
                        else
                        {
                            velocityComponent.Speed -= keyMoveSpeed;
                        }

                        if (velocityComponent.Angle < 180)
                        {
                            velocityComponent.AddAngle(changeInAngle);
                        }
                        else if (velocityComponent.Angle > 180 && velocityComponent.Angle > 0)
                        {
                            velocityComponent.AddAngle(-changeInAngle);
                        }
                        break;

                    case Keys.D:
                        if (Math.Abs(velocityComponent.Speed) < turningOffset)
                        {
                            velocityComponent.Angle = 0;
                        }
                        if (velocityComponent.Angle >= 270 || velocityComponent.Angle < 90)
                        {
                            velocityComponent.Speed += keyMoveSpeed;
                        }
                        else
                        {
                            velocityComponent.Speed -= keyMoveSpeed;
                        }
                        if (velocityComponent.Angle > 180)
                        {
                            velocityComponent.AddAngle(changeInAngle);
                        }
                        else if (velocityComponent.Angle < 180 && velocityComponent.Angle > 0)
                        {
                            velocityComponent.AddAngle(-changeInAngle);
                        }
                        break;

                    case Keys.W:
                        if (Math.Abs(velocityComponent.Speed) < turningOffset)
                        {
                            velocityComponent.Angle = 270;
                        }
                        if (velocityComponent.Angle >= 180)
                        {
                            velocityComponent.Speed += keyMoveSpeed;
                        }
                        else
                        {
                            velocityComponent.Speed -= keyMoveSpeed;
                        }
                        if (velocityComponent.Angle < 270 && velocityComponent.Angle > 90)
                        {
                            velocityComponent.AddAngle(changeInAngle);
                        }
                        else if (velocityComponent.Angle < 90 || velocityComponent.Angle > 270)
                        {
                            velocityComponent.AddAngle(-changeInAngle);
                        }
                        break;

                    case Keys.S:
                        if (Math.Abs(velocityComponent.Speed) < turningOffset)
                        {
                            velocityComponent.Angle = 90;
                        }
                        if (velocityComponent.Angle < 180)
                        {
                            velocityComponent.Speed += keyMoveSpeed;
                        }
                        else
                        {
                            velocityComponent.Speed -= keyMoveSpeed;
                        }
                        if (velocityComponent.Angle < 90 || velocityComponent.Angle > 270)
                        {
                            velocityComponent.AddAngle(changeInAngle);
                        }
                        else if (velocityComponent.Angle < 270 && velocityComponent.Angle > 90)
                        {
                            velocityComponent.AddAngle(-changeInAngle);
                        }
                        break;

                    default:
                        break;
                }
            }

            // Handle max speed
            if (velocityComponent.Speed > 0)
            {
                velocityComponent.Speed -= moveSpeedFriction;
            }
            else if (velocityComponent.Speed < 0)
            {
                velocityComponent.Speed += moveSpeedFriction;
            }
            velocityComponent.Speed = Math.Max(velocityComponent.Speed, -1 * maxMoveSpeed);
            velocityComponent.Speed = Math.Min(velocityComponent.Speed, maxMoveSpeed);
        }
    }
}
