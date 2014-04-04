﻿using Artemis;
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

            float maxMoveSpeed = .15F;
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
                                maxMoveSpeed = .35F;
                                acceleration = (float)((0.0016F * w.GetComponent<HealthComponent>().HealthPercentage) * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds);
                                moveSpeedFriction = 0.0003f * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
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
            if (pressedKey.IsKeyDown(Keys.Q) || controller.Buttons.Y == ButtonState.Pressed)
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer("Sounds/qqq.wav");

                player.Play();
            }
            if ((pressedKey.IsKeyDown(Keys.RightShift) || controller.Buttons.A == ButtonState.Pressed) && cooldownComponent.IsBuildReady)
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer("Sounds/build.wav");

                player.Play();
                Entity toBuild0 = null;
                Entity toBuild1 = null;
                Entity toBuild2 = null;
                float direction = (velocityComponent.Direction * (180 / pi)) - 180;
                while (direction < 0)
                {
                    direction += 360;
                }
                if (direction < 22.5 || direction >= 337.5)
                {
                    toBuild0 = walls[nearbyGridsComponent.TopRightIndex];
                    toBuild1 = walls[nearbyGridsComponent.RightIndex];
                    toBuild2 = walls[nearbyGridsComponent.BottomRightIndex];
                }
                else if (direction >= 22.5 && direction < 67.5)
                {
                    toBuild0 = walls[nearbyGridsComponent.RightIndex];
                    toBuild1 = walls[nearbyGridsComponent.BottomRightIndex];
                    toBuild2 = walls[nearbyGridsComponent.BottomIndex];
                }
                else if (direction >= 67.5 && direction < 112.5)
                {
                    toBuild0 = walls[nearbyGridsComponent.BottomRightIndex];
                    toBuild1 = walls[nearbyGridsComponent.BottomIndex];
                    toBuild2 = walls[nearbyGridsComponent.BottomLeftIndex];
                }
                else if (direction >= 112.5 && direction < 157.5)
                {
                    toBuild0 = walls[nearbyGridsComponent.BottomIndex];
                    toBuild1 = walls[nearbyGridsComponent.BottomLeftIndex];
                    toBuild2 = walls[nearbyGridsComponent.LeftIndex];
                }
                else if (direction >= 157.5 && direction < 202.5)
                {
                    toBuild0 = walls[nearbyGridsComponent.BottomLeftIndex];
                    toBuild1 = walls[nearbyGridsComponent.LeftIndex];
                    toBuild2 = walls[nearbyGridsComponent.TopLeftIndex];
                }
                else if (direction >= 202.5 && direction < 247.5)
                {
                    toBuild0 = walls[nearbyGridsComponent.LeftIndex];
                    toBuild1 = walls[nearbyGridsComponent.TopLeftIndex];
                    toBuild2 = walls[nearbyGridsComponent.TopIndex];
                }
                else if (direction >= 247.5 && direction < 292.5)
                {
                    toBuild0 = walls[nearbyGridsComponent.TopLeftIndex];
                    toBuild1 = walls[nearbyGridsComponent.TopIndex];
                    toBuild2 = walls[nearbyGridsComponent.TopRightIndex];
                }
                else if (direction >= 292.5 && direction < 337.5)
                {
                    toBuild0 = walls[nearbyGridsComponent.TopIndex];
                    toBuild1 = walls[nearbyGridsComponent.TopRightIndex];
                    toBuild2 = walls[nearbyGridsComponent.RightIndex];
                }
                if (toBuild0 != null)
                {
                    toBuild0.GetComponent<HealthComponent>().AddHealth(entity.GetComponent<HealComponent>().Heal);
                }
                if (toBuild1 != null)
                {
                    toBuild1.GetComponent<HealthComponent>().AddHealth(entity.GetComponent<HealComponent>().Heal);
                }
                if (toBuild2 != null)
                {
                    toBuild2.GetComponent<HealthComponent>().AddHealth(entity.GetComponent<HealComponent>().Heal);
                }
                    cooldownComponent.ResetBuildCooldown();
                
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
