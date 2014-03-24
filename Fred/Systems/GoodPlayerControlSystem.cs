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
            // TODO: Add your update logic here
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            VelocityComponent velocityComponent = entity.GetComponent<VelocityComponent>();
            CooldownComponent cooldownComponent = entity.GetComponent<CooldownComponent>();

            //float turningOffset = .0001F;
            //float changeInAngle = 5;
            float maxMoveSpeed = .2F;
            float keyMoveSpeed = 0.001F * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
            float moveSpeedFriction = 0.0003f * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;

            KeyboardState pressedKey = Keyboard.GetState();
            GamePadState controller = GamePad.GetState(PlayerIndex.One);

            //oard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A)) && player.Mobile == true)
            //{
            //    float x = player.Vector.X - player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    player.Vector = new Vector2(x, player.Vector.Y);
            //}
            if (pressedKey.IsKeyDown(Keys.A) || controller.ThumbSticks.Left.X < 0)
            {
                velocityComponent.xVelocity -= keyMoveSpeed;
            }
            if (pressedKey.IsKeyDown(Keys.D) || controller.ThumbSticks.Left.X > 0)
            {
                velocityComponent.xVelocity += keyMoveSpeed;
            }

            if (pressedKey.IsKeyDown(Keys.W) || controller.ThumbSticks.Left.Y > 0)
            {

                velocityComponent.yVelocity -= keyMoveSpeed;
            }

            if (pressedKey.IsKeyDown(Keys.S) || controller.ThumbSticks.Left.Y < 0)
            {

                velocityComponent.yVelocity += keyMoveSpeed;
            }
            if ((pressedKey.IsKeyDown(Keys.D1) || controller.Buttons.A == ButtonState.Pressed) && cooldownComponent.IsAttackReady)
            {
                Bag<Entity> walls = this.EntityWorld.GroupManager.GetEntities("Walls");
                double closestDistance = int.MaxValue;
                Entity closestWall = walls[0];
                foreach (Entity w in walls)
                {
                    double currentDistance = Math.Sqrt(Math.Pow(w.GetComponent<TransformComponent>().X - transformComponent.X, 2) + Math.Pow(w.GetComponent<TransformComponent>().Y - transformComponent.Y, 2));
                    if (w.GetComponent<HealthComponent>().IsAlive && currentDistance < closestDistance)
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
            if (Math.Abs(velocityComponent.xVelocity) > maxMoveSpeed)
            {
                if (velocityComponent.xVelocity > 0)
                {
                    velocityComponent.xVelocity = maxMoveSpeed;
                }
                else
                {
                    velocityComponent.xVelocity = -maxMoveSpeed;
                }
            }
            else if (Math.Abs(velocityComponent.yVelocity) > maxMoveSpeed)
            {

                if (velocityComponent.yVelocity > 0)
                {
                    velocityComponent.yVelocity = maxMoveSpeed;
                }
                else
                {
                    velocityComponent.yVelocity = -maxMoveSpeed;
                }
            }
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
