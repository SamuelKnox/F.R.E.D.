using Artemis;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using Artemis.Utils;
using Fred.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update)]
    class BadPlayerControlSystem : TagSystem
    {

        private GraphicsDevice graphicsDevice;

        public BadPlayerControlSystem()
            : base("BAD_PLAYER")
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
            CooldownComponent cooldownComponent = entity.GetComponent<CooldownComponent>();

            float turningOffset = 1F;
            float changeInAngle = 1;
            float maxMoveSpeed = .2F;
            float keyMoveSpeed = 0.001F * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
            float moveSpeedFriction = 0.0005f * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;

            Bag<Entity> walls = this.EntityWorld.GroupManager.GetEntities("Walls");
            foreach (Entity w in walls)
            {
                if (transformComponent.Location.Intersects(w.GetComponent<TransformComponent>().Location) && w.GetComponent<HealthComponent>().IsAlive)
                {
                    maxMoveSpeed = .04F;
                    keyMoveSpeed = 0.0005F * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
                    moveSpeedFriction = 0.0001f * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
                }
            }

            KeyboardState pressedKey = Keyboard.GetState();
            GamePadState controller = GamePad.GetState(PlayerIndex.Two);

            if (pressedKey.IsKeyDown(Keys.Left) || controller.ThumbSticks.Left.X < 0)
            {
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
            }
            if (pressedKey.IsKeyDown(Keys.Right) || controller.ThumbSticks.Left.X > 0)
            {
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
            }

            if (pressedKey.IsKeyDown(Keys.Up) || controller.ThumbSticks.Left.Y > 0)
            {
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
            }

            if (pressedKey.IsKeyDown(Keys.Down) || controller.ThumbSticks.Left.Y < 0)
            {
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
            }

            if ((pressedKey.IsKeyDown(Keys.RightShift) || controller.Buttons.A == ButtonState.Pressed) && cooldownComponent.IsBuildReady)
            {
                double closestDistance = int.MaxValue;
                Entity closestWall = null;
                foreach (Entity w in walls)
                {
                    double currentDistance = Math.Sqrt(Math.Pow(w.GetComponent<TransformComponent>().X - transformComponent.X, 2) + Math.Pow(w.GetComponent<TransformComponent>().Y - transformComponent.Y, 2));
                    if (!w.GetComponent<HealthComponent>().IsAlive && currentDistance < closestDistance)
                    {
                        closestDistance = currentDistance;
                        closestWall = w;
                    }
                }
                if (closestDistance < 50 && closestWall != null)
                {
                    closestWall.GetComponent<HealthComponent>().AddHealth(entity.GetComponent<HealComponent>().Heal);
                cooldownComponent.ResetBuildCooldown();
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
