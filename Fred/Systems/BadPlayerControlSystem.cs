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

        private Entity maze;

        public BadPlayerControlSystem()
            : base("BAD_PLAYER")
        {
        }

        public override void LoadContent()
        {
            maze = BlackBoard.GetEntry<Entity>("Maze");
        }

        public override void Process(Entity entity)
        {
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            VelocityComponent velocityComponent = entity.GetComponent<VelocityComponent>();
            CooldownComponent cooldownComponent = entity.GetComponent<CooldownComponent>();
            NearbyGridsComponent nearbyGridsComponent = entity.GetComponent<NearbyGridsComponent>();

            float pi = 3.14159265359F;

            if (Math.Abs(velocityComponent.XVelocity) + Math.Abs(velocityComponent.YVelocity) > 0.02F)
            {
                velocityComponent.Direction = (float)(-Math.Atan2(velocityComponent.XVelocity, velocityComponent.YVelocity) + (-pi * 0.45));
            }

            float maxMoveSpeed = .25F;
            float acceleration = 0.0008F * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
            float moveSpeedFriction = 0.0005f * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
            int keysDown = 0;
            //float cosFortyFive = 0.707F;
            float cosFortyFive = 0.9F;

            Bag<Entity> walls = this.EntityWorld.GroupManager.GetEntities("Walls");
            foreach (Vector2 coords in entity.GetComponent<NearbyGridsComponent>().NearbyGrids)
            {
                        int index = (int)(coords.X * maze.GetComponent<MazeComponent>().Height + coords.Y);
                        Entity w = null;
                        if (index >= 0 && index < walls.Count)
                        {
                            w = walls[(int)(coords.X * maze.GetComponent<MazeComponent>().Height + coords.Y)];
                            if (transformComponent.Location.Intersects(w.GetComponent<TransformComponent>().Location) && w.GetComponent<HealthComponent>().IsAlive)
                            {
                                maxMoveSpeed = .04F;
                                acceleration = (float)((0.0004F * w.GetComponent<HealthComponent>().HealthPercentage) * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds);
                                moveSpeedFriction = 0.0001f * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
                            }
                        }
            }

            KeyboardState pressedKey = Keyboard.GetState();
            GamePadState controller = GamePad.GetState(PlayerIndex.Two);
            if (pressedKey.IsKeyDown(Keys.Left) || controller.ThumbSticks.Left.X < 0)
            {
                keysDown++;
            }
            if (pressedKey.IsKeyDown(Keys.Right) || controller.ThumbSticks.Left.X > 0)
            {
                keysDown++;
            }

            if (pressedKey.IsKeyDown(Keys.Up) || controller.ThumbSticks.Left.Y > 0)
            {
                keysDown++;
            }

            if (pressedKey.IsKeyDown(Keys.Down) || controller.ThumbSticks.Left.Y < 0)
            {
                keysDown++;
            }
            if(keysDown >1){
            acceleration *= cosFortyFive;}
            if (pressedKey.IsKeyDown(Keys.Left) || controller.ThumbSticks.Left.X < 0)
            {
                velocityComponent.XVelocity -= acceleration;
            }
            if (pressedKey.IsKeyDown(Keys.Right) || controller.ThumbSticks.Left.X > 0)
            {
                velocityComponent.XVelocity += acceleration;
            }

            if (pressedKey.IsKeyDown(Keys.Up) || controller.ThumbSticks.Left.Y > 0)
            {
                velocityComponent.YVelocity -= acceleration;
            }

            if (pressedKey.IsKeyDown(Keys.Down) || controller.ThumbSticks.Left.Y < 0)
            {
                velocityComponent.YVelocity += acceleration;
            }
            if ((pressedKey.IsKeyDown(Keys.RightShift) || controller.Buttons.A == ButtonState.Pressed) && cooldownComponent.IsBuildReady)
            {
                Entity toBuild = null;
                float direction = (velocityComponent.Direction * (180 / pi)) - 180;
                while (direction < 0)
                {
                    direction += 360;
                }
                Console.WriteLine(direction + " UHFEKHFKJREGRE");
                if (direction < 22.5 || direction >= 337.5)
                {
                    toBuild = walls[nearbyGridsComponent.RightIndex];
                }
                else if (direction >= 22.5 && direction < 67.5)
                {
                    toBuild = walls[nearbyGridsComponent.BottomRightIndex];
                }
                else if (direction >= 67.5 && direction < 112.5)
                {
                    toBuild = walls[nearbyGridsComponent.BottomIndex];
                }
                else if (direction >= 112.5 && direction < 157.5)
                {
                    toBuild = walls[nearbyGridsComponent.BottomLeftIndex];
                }
                else if (direction >= 157.5 && direction < 202.5)
                {
                    toBuild = walls[nearbyGridsComponent.LeftIndex];
                }
                else if (direction >= 202.5 && direction < 247.5)
                {
                    toBuild = walls[nearbyGridsComponent.TopLeftIndex];
                }
                else if (direction >= 247.5 && direction < 292.5)
                {
                    toBuild = walls[nearbyGridsComponent.TopIndex];
                }
                else if (direction >= 292.5 && direction < 337.5)
                {
                    toBuild = walls[nearbyGridsComponent.TopRightIndex];
                }
                if (!toBuild.GetComponent<HealthComponent>().IsAlive)
                {
                    toBuild.GetComponent<HealthComponent>().AddHealth(entity.GetComponent<HealComponent>().Heal);
                    cooldownComponent.ResetBuildCooldown();
                }
            }

            // Handle max speed
            float maxTwoMoveSpeed =  maxMoveSpeed * cosFortyFive; // xSqrt(2)

            if (velocityComponent.XVelocity > 0 && velocityComponent.YVelocity > 0)
                {
                    velocityComponent.XVelocity = Math.Min(velocityComponent.XVelocity, maxTwoMoveSpeed);
                    velocityComponent.YVelocity = Math.Min(velocityComponent.YVelocity, maxTwoMoveSpeed);
                }
            if (velocityComponent.XVelocity > 0 && velocityComponent.YVelocity < 0)
                {
                    velocityComponent.XVelocity = Math.Min(velocityComponent.XVelocity, maxTwoMoveSpeed);
                    velocityComponent.YVelocity = Math.Max(velocityComponent.YVelocity, -1 * maxTwoMoveSpeed);
                }
            if (velocityComponent.XVelocity < 0 && velocityComponent.YVelocity > 0)
                {
                    velocityComponent.XVelocity = Math.Max(velocityComponent.XVelocity, -1 * maxTwoMoveSpeed);
                    velocityComponent.YVelocity = Math.Min(velocityComponent.YVelocity, maxTwoMoveSpeed);
                }
            if (velocityComponent.XVelocity < 0 && velocityComponent.YVelocity < 0)
                {
                    velocityComponent.XVelocity = Math.Max(velocityComponent.XVelocity, -1 * maxTwoMoveSpeed);
                    velocityComponent.YVelocity = Math.Max(velocityComponent.YVelocity, -1 * maxTwoMoveSpeed);
                }
            if (velocityComponent.XVelocity == 0 && velocityComponent.YVelocity > 0)
                {
                    velocityComponent.YVelocity = Math.Min(velocityComponent.YVelocity, maxMoveSpeed);
                }
            if (velocityComponent.XVelocity == 0 && velocityComponent.YVelocity < 0)
                {
                    velocityComponent.YVelocity = Math.Max(velocityComponent.YVelocity, -1 * maxMoveSpeed);
                }
            if (velocityComponent.XVelocity > 0 && velocityComponent.YVelocity == 0)
                {
                    velocityComponent.XVelocity = Math.Min(velocityComponent.XVelocity, maxMoveSpeed);
                }
            if (velocityComponent.XVelocity < 0 && velocityComponent.YVelocity == 0)
                {
                    velocityComponent.XVelocity = Math.Max(velocityComponent.XVelocity, -1 * maxMoveSpeed);
                }

            // Apply Friction
            if (velocityComponent.XVelocity > 0)
            {
                velocityComponent.XVelocity -= moveSpeedFriction;
            }
            else if (velocityComponent.XVelocity < 0)
            {
                velocityComponent.XVelocity += moveSpeedFriction;
            }
            if (velocityComponent.YVelocity > 0)
            {
                velocityComponent.YVelocity -= moveSpeedFriction;
            }
            else if (velocityComponent.YVelocity < 0)
            {
                velocityComponent.YVelocity += moveSpeedFriction;
            }

        }
    }
}
