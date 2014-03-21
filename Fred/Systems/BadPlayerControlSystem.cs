using Artemis;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using Artemis.Utils;
using Fred.Components;
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
            KeyboardState keyboardState = Keyboard.GetState();
            float keyMoveSpeed = 0.3f * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                transformComponent.X -= keyMoveSpeed;
                if (transformComponent.X < 32)
                {
                    transformComponent.X = 32;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                transformComponent.X += keyMoveSpeed;
                if (transformComponent.X > graphicsDevice.Viewport.Width - 32)
                {
                    transformComponent.X = graphicsDevice.Viewport.Width - 32;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                transformComponent.Y -= keyMoveSpeed;
                if (transformComponent.Y < 32)
                {
                    transformComponent.Y = 32;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                transformComponent.Y += keyMoveSpeed;
                if (transformComponent.Y > graphicsDevice.Viewport.Height - 32)
                {
                    transformComponent.Y = graphicsDevice.Viewport.Height - 32;
                }
            }
        }
    }
}
