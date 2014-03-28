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
using Fred.Spatials;
using Fred.Templates;

namespace Fred.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update)]
    class GoodPlayerControlSystem : TagSystem
    {
        private Entity maze;

        public GoodPlayerControlSystem()
            : base("GOOD_PLAYER")
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


            velocityComponent.Direction = (float)(-Math.Atan2(velocityComponent.XVelocity, velocityComponent.YVelocity) + (-Math.PI * 0.45));
            //velocityComponent.Direction++;

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
                velocityComponent.XVelocity -= acceleration;
            }
            if (pressedKey.IsKeyDown(Keys.D) || controller.ThumbSticks.Left.X > 0)
            {
                velocityComponent.XVelocity += acceleration;
            }

            if (pressedKey.IsKeyDown(Keys.W) || controller.ThumbSticks.Left.Y > 0)
            {
                velocityComponent.YVelocity -= acceleration;
            }

            if (pressedKey.IsKeyDown(Keys.S) || controller.ThumbSticks.Left.Y < 0)
            {
                velocityComponent.YVelocity += acceleration;
            }
            if ((pressedKey.IsKeyDown(Keys.D1) || controller.Buttons.A == ButtonState.Pressed) && cooldownComponent.IsAttackReady)
            {
                Bag<Entity> walls = this.EntityWorld.GroupManager.GetEntities("Walls");
                double closestDistance = int.MaxValue;
                Entity closestWall = walls[0];

                foreach (Vector2 coords in entity.GetComponent<NearbyGridsComponent>().NearbyGrids)
                {
                    int index = (int)(coords.X * maze.GetComponent<MazeComponent>().Height + coords.Y);
                    Entity w = null;
                    if (index >= 0 && index < walls.Count)
                    {
                        w = walls[(int)(coords.X * maze.GetComponent<MazeComponent>().Height + coords.Y)];

                        double currentDistance = Math.Sqrt(Math.Pow(w.GetComponent<TransformComponent>().X - transformComponent.X, 2) + Math.Pow(w.GetComponent<TransformComponent>().Y - transformComponent.Y, 2));
                        if (w.GetComponent<HealthComponent>().IsAlive && currentDistance < closestDistance && w.GetComponent<HealthComponent>().CurrentHealth < 1000000)
                        {
                            closestDistance = currentDistance;
                            closestWall = w;
                        }
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
            float maxTwoMoveSpeed = maxMoveSpeed * cosFortyFive; // cos(45)

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
