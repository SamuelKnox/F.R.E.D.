﻿using Artemis;
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
using Fred.Spatials;
using Fred.Templates;

namespace Fred.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update)]
    class GoodPlayerControlSystem : TagSystem
    {

        public GoodPlayerControlSystem()
            : base("GOOD_PLAYER")
        {
        }

        public override void Process(Entity entity)
        {
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            VelocityComponent velocityComponent = entity.GetComponent<VelocityComponent>();
            CooldownComponent cooldownComponent = entity.GetComponent<CooldownComponent>();

            float maxMoveSpeed = .3F;
            float acceleration = 0.001F * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
            float moveSpeedFriction = 0.0003f * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
            int keysDown = 0;
            float cosFortyFive = 0.707F;

            KeyboardState pressedKey = Keyboard.GetState();
            GamePadState controller = GamePad.GetState(PlayerIndex.One);

            if (pressedKey.IsKeyDown(Keys.A) || controller.ThumbSticks.Left.X < 0)
            {
                keysDown++;
            }
            if (pressedKey.IsKeyDown(Keys.D) || controller.ThumbSticks.Left.X > 0)
            {
                keysDown++;
            }

            if (pressedKey.IsKeyDown(Keys.W) || controller.ThumbSticks.Left.Y > 0)
            {
                keysDown++;
            }

            if (pressedKey.IsKeyDown(Keys.S) || controller.ThumbSticks.Left.Y < 0)
            {
                keysDown++;
            }
            if (keysDown > 1)
            {
                acceleration *= cosFortyFive;
            }
            if (pressedKey.IsKeyDown(Keys.A) || controller.ThumbSticks.Left.X < 0)
            {
                velocityComponent.xVelocity -= acceleration;
            }
            if (pressedKey.IsKeyDown(Keys.D) || controller.ThumbSticks.Left.X > 0)
            {
                velocityComponent.xVelocity += acceleration;
            }

            if (pressedKey.IsKeyDown(Keys.W) || controller.ThumbSticks.Left.Y > 0)
            {
                velocityComponent.yVelocity -= acceleration;
            }

            if (pressedKey.IsKeyDown(Keys.S) || controller.ThumbSticks.Left.Y < 0)
            {
                velocityComponent.yVelocity += acceleration;
            }
            if ((pressedKey.IsKeyDown(Keys.D1) || controller.Buttons.A == ButtonState.Pressed) && cooldownComponent.IsAttackReady)
            {
                Bag<Entity> walls = this.EntityWorld.GroupManager.GetEntities("Walls");
                double closestDistance = int.MaxValue;
                Entity closestWall = walls[0];
                foreach (Entity w in walls)
                {
                    double currentDistance = Math.Sqrt(Math.Pow(w.GetComponent<TransformComponent>().X - transformComponent.X, 2) + Math.Pow(w.GetComponent<TransformComponent>().Y - transformComponent.Y, 2));
                    if (w.GetComponent<HealthComponent>().IsAlive && currentDistance < closestDistance && w.GetComponent<HealthComponent>().CurrentHealth < 1000000)
                    {
                        closestDistance = currentDistance;
                        closestWall = w;
                    }
                }
                if (closestDistance < 50)
                {
                    closestWall.GetComponent<HealthComponent>().AddDamage(entity.GetComponent<DamageComponent>().Damage);
                cooldownComponent.ResetAttackCooldown();
                Entity attack = this.EntityWorld.CreateEntityFromTemplate(WallAttackTemplate.Name);
                attack.GetComponent<TransformComponent>().Position = closestWall.GetComponent<TransformComponent>().Position;

                }
            }

            // Handle max speed
            float maxTwoMoveSpeed =  maxMoveSpeed * cosFortyFive; // cos(45)

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
