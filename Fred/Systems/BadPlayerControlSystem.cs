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
                velocityComponent.xVelocity -= acceleration;
            }
            if (pressedKey.IsKeyDown(Keys.Right) || controller.ThumbSticks.Left.X > 0)
            {
                velocityComponent.xVelocity += acceleration;
            }

            if (pressedKey.IsKeyDown(Keys.Up) || controller.ThumbSticks.Left.Y > 0)
            {
                velocityComponent.yVelocity -= acceleration;
            }

            if (pressedKey.IsKeyDown(Keys.Down) || controller.ThumbSticks.Left.Y < 0)
            {
                velocityComponent.yVelocity += acceleration;
            }

            if ((pressedKey.IsKeyDown(Keys.RightShift) || controller.Buttons.A == ButtonState.Pressed) && cooldownComponent.IsBuildReady)
            {
                double closestDistance = int.MaxValue;
                Entity closestWall = null;
                foreach (Vector2 coords in entity.GetComponent<NearbyGridsComponent>().NearbyGrids)
                {
                    int index = (int)(coords.X * maze.GetComponent<MazeComponent>().Height + coords.Y);
                    Entity w = null;
                    if (index >= 0 && index < walls.Count)
                    {
                        w = walls[(int)(coords.X * maze.GetComponent<MazeComponent>().Height + coords.Y)];

                        double currentDistance = Math.Sqrt(Math.Pow(w.GetComponent<TransformComponent>().X - transformComponent.X, 2) + Math.Pow(w.GetComponent<TransformComponent>().Y - transformComponent.Y, 2));
                        if (!w.GetComponent<HealthComponent>().IsAlive && currentDistance < closestDistance)
                        {
                            closestDistance = currentDistance;
                            closestWall = w;
                        }
                    }
                }
                if (closestDistance < 50 && closestWall != null)
                {
                    closestWall.GetComponent<HealthComponent>().AddHealth(entity.GetComponent<HealComponent>().Heal);
                cooldownComponent.ResetBuildCooldown();
                }
            }

            // Handle max speed
            float maxTwoMoveSpeed =  maxMoveSpeed * cosFortyFive; // xSqrt(2)

            if (velocityComponent.xVelocity > 0 && velocityComponent.yVelocity > 0)
                {
                    velocityComponent.xVelocity = Math.Min(velocityComponent.xVelocity, maxTwoMoveSpeed);
                    velocityComponent.yVelocity = Math.Min(velocityComponent.yVelocity, maxTwoMoveSpeed);
                }
            if (velocityComponent.xVelocity > 0 && velocityComponent.yVelocity < 0)
                {
                    velocityComponent.xVelocity = Math.Min(velocityComponent.xVelocity, maxTwoMoveSpeed);
                    velocityComponent.yVelocity = Math.Max(velocityComponent.yVelocity, -1 * maxTwoMoveSpeed);
                }
            if (velocityComponent.xVelocity < 0 && velocityComponent.yVelocity > 0)
                {
                    velocityComponent.xVelocity = Math.Max(velocityComponent.xVelocity, -1 * maxTwoMoveSpeed);
                    velocityComponent.yVelocity = Math.Min(velocityComponent.yVelocity, maxTwoMoveSpeed);
                }
            if (velocityComponent.xVelocity < 0 && velocityComponent.yVelocity < 0)
                {
                    velocityComponent.xVelocity = Math.Max(velocityComponent.xVelocity, -1 * maxTwoMoveSpeed);
                    velocityComponent.yVelocity = Math.Max(velocityComponent.yVelocity, -1 * maxTwoMoveSpeed);
                }
            if (velocityComponent.xVelocity == 0 && velocityComponent.yVelocity > 0)
                {
                    velocityComponent.yVelocity = Math.Min(velocityComponent.yVelocity, maxMoveSpeed);
                }
            if (velocityComponent.xVelocity == 0 && velocityComponent.yVelocity < 0)
                {
                    velocityComponent.yVelocity = Math.Max(velocityComponent.yVelocity, -1 * maxMoveSpeed);
                }
            if (velocityComponent.xVelocity > 0 && velocityComponent.yVelocity == 0)
                {
                    velocityComponent.xVelocity = Math.Min(velocityComponent.xVelocity, maxMoveSpeed);
                }
            if (velocityComponent.xVelocity < 0 && velocityComponent.yVelocity == 0)
                {
                    velocityComponent.xVelocity = Math.Max(velocityComponent.xVelocity, -1 * maxMoveSpeed);
                }

            // Apply Friction
            if (velocityComponent.xVelocity > 0)
            {
                velocityComponent.xVelocity -= moveSpeedFriction;
            }
            else if (velocityComponent.xVelocity < 0)
            {
                velocityComponent.xVelocity += moveSpeedFriction;
            }
            if (velocityComponent.yVelocity > 0)
            {
                velocityComponent.yVelocity -= moveSpeedFriction;
            }
            else if (velocityComponent.yVelocity < 0)
            {
                velocityComponent.yVelocity += moveSpeedFriction;
            }

        }
    }
}
