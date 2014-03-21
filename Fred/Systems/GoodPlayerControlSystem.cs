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
            float maxMoveSpeed = .3F;
            float keyMoveSpeed = 0.001F * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
            float moveSpeedFriction = 0.0003f * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;

            string vertical = "none";
            string horizontal = "none";

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
                        horizontal = "left";
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
                        horizontal = "right";
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
                        vertical = "up";
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
                        vertical = "down";
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

            // Handle collisions
            bool collides = ProcessCollisions(entity);
            if (collides)
            {
                velocityComponent.Speed = 0;
            }
            while (collides && horizontal == "left")
            {
                transformComponent.X += keyMoveSpeed;
                velocityComponent.Speed = 0;
                collides = ProcessCollisions(entity);
            }
            while (collides && horizontal == "right")
            {
                transformComponent.X -= keyMoveSpeed;
                velocityComponent.Speed = 0;
                collides = ProcessCollisions(entity);
            }
            while (collides && vertical == "up")
            {
                transformComponent.Y += keyMoveSpeed;
                velocityComponent.Speed = 0;
                collides = ProcessCollisions(entity);
            }
            while (collides && vertical == "down")
            {
                transformComponent.Y -= keyMoveSpeed;
                velocityComponent.Speed = 0;
                collides = ProcessCollisions(entity);
            }
        }
        private bool ProcessCollisions(Entity entity)
        {
            Bag<Entity> walls = this.EntityWorld.GroupManager.GetEntities("Walls");
            for (int wallIndex = 0; walls.Count > wallIndex; ++wallIndex)
            {
                Entity wall = walls.Get(wallIndex);
                if (this.CollisionExists(wall, entity))
                {
                    return true;
                }
            }
            return false;
        }
        private bool CollisionExists(Entity entity1, Entity entity2)
        {
            return Vector2.Distance(entity1.GetComponent<TransformComponent>().Position, entity2.GetComponent<TransformComponent>().Position) < 20;
        }
    }
}
