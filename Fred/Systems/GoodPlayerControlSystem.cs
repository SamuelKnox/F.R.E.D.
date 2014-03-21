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
            KeyboardState keyboardState = Keyboard.GetState();
            float keyMoveSpeed = 0.3f * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
            string vertical = "none";
            string horizontal = "none"; 
            if (keyboardState.IsKeyDown(Keys.A))
            {
                transformComponent.X -= keyMoveSpeed;
                //if (ProcessCollisions(entity))
                //{
                   // transformComponent.X += keyMoveSpeed;
               // }
                if (transformComponent.X < 32)
                {
                    transformComponent.X = 32;
                }
                horizontal = "left";
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                transformComponent.X += keyMoveSpeed;
                //if (ProcessCollisions(entity))
                //{
                   // transformComponent.X -= keyMoveSpeed;
                //}
                if (transformComponent.X > graphicsDevice.Viewport.Width - 32)
                {
                    transformComponent.X = graphicsDevice.Viewport.Width - 32;
                }
                horizontal = "right";
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                transformComponent.Y -= keyMoveSpeed;
                //if (ProcessCollisions(entity))
                //{
                   // transformComponent.Y += keyMoveSpeed;
                //}
                if (transformComponent.Y < 32)
                {
                    transformComponent.Y = 32;
                }
                vertical = "up";
            }
             else if (keyboardState.IsKeyDown(Keys.S))
            {
                transformComponent.Y += keyMoveSpeed;
                //if (ProcessCollisions(entity))
               // {
                    //transformComponent.Y -= keyMoveSpeed;
                //}
                if (transformComponent.Y > graphicsDevice.Viewport.Height - 32)
                {
                    transformComponent.Y = graphicsDevice.Viewport.Height - 32;
                }
                vertical = "down";
            }
             bool collides = ProcessCollisions(entity);
             while (collides && horizontal == "left")
             {
                 transformComponent.X += keyMoveSpeed;
                 collides = ProcessCollisions(entity);
             }
             while (collides && horizontal == "right")
             {
                 transformComponent.X -= keyMoveSpeed;
                 collides = ProcessCollisions(entity);
             }
             while (collides && vertical == "up")
             {
                 transformComponent.Y += keyMoveSpeed;
                 collides = ProcessCollisions(entity);
             }
             while (collides && vertical == "down")
             {
                 transformComponent.Y -= keyMoveSpeed;
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
            return Vector2.Distance(entity1.GetComponent<TransformComponent>().Position, entity2.GetComponent<TransformComponent>().Position) < 25;
        }
    }
}
