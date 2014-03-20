//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Fred.Systems
//{
//    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update)]
//    class GoodPlayerControlSystem
//    {
//        /// <summary>The missile launch timer.</summary>
//        private readonly Timer missileLaunchTimer;

//        /// <summary>The graphics device.</summary>
//        private GraphicsDevice graphicsDevice;

//        /// <summary>Initializes a new instance of the <see cref="PlayerShipControlSystem" /> class.</summary>
//        public PlayerShipControlSystem()
//            : base("PLAYER")
//        {
//            this.missileLaunchTimer = new Timer(new TimeSpan(0, 0, 0, 0, 150));
//        }

//        /// <summary>Override to implement code that gets executed when systems are initialized.</summary>
//        public override void LoadContent()
//        {
//            this.graphicsDevice = BlackBoard.GetEntry<GraphicsDevice>("GraphicsDevice");
//        }

//        /// <summary>Processes the specified entity.</summary>
//        /// <param name="entity">The entity.</param>
//        public override void Process(Entity entity)
//        {
//            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
//            KeyboardState keyboardState = Keyboard.GetState();
//            float keyMoveSpeed = 0.3f * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
//            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
//            {
//                transformComponent.X -= keyMoveSpeed;
//                if (transformComponent.X < 32)
//                {
//                    transformComponent.X = 32;
//                }
//            }
//            else if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
//            {
//                transformComponent.X += keyMoveSpeed;
//                if (transformComponent.X > this.graphicsDevice.Viewport.Width - 32)
//                {
//                    transformComponent.X = this.graphicsDevice.Viewport.Width - 32;
//                }
//            }

//            if (keyboardState.IsKeyDown(Keys.Space) || keyboardState.IsKeyDown(Keys.Enter))
//            {
//                if (this.missileLaunchTimer.IsReached(this.EntityWorld.Delta))
//                {
//                    this.AddMissile(transformComponent);
//                    this.AddMissile(transformComponent, 89, -9);
//                    this.AddMissile(transformComponent, 91, +9);
//                }
//            }
//        }

//        /// <summary>Adds the missile.</summary>
//        /// <param name="transformComponent">The transform component.</param>
//        /// <param name="angle">The angle.</param>
//        /// <param name="offsetX">The offset X.</param>
//        private void AddMissile(TransformComponent transformComponent, float angle = 90.0f, float offsetX = 0.0f)
//        {
//            Entity missile = this.EntityWorld.CreateEntityFromTemplate(MissileTemplate.Name);

//            missile.GetComponent<TransformComponent>().X = transformComponent.X + 1 + offsetX;
//            missile.GetComponent<TransformComponent>().Y = transformComponent.Y - 20;

//            missile.GetComponent<VelocityComponent>().Speed = -0.5f;
//            missile.GetComponent<VelocityComponent>().Angle = angle;
//        }
//    }
//}
