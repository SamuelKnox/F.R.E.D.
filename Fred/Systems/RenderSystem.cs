using Artemis;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using Fred.Components;
using Fred.Spatials;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = 0)]
    class RenderSystem : EntityProcessingSystem<SpatialFormComponent, TransformComponent>
    {
        private ContentManager contentManager;

        private string spatialName;

        private SpriteBatch spriteBatch;

        public override void LoadContent()
        {
            spriteBatch = BlackBoard.GetEntry<SpriteBatch>("SpriteBatch");
            contentManager = BlackBoard.GetEntry<ContentManager>("ContentManager");
        }

        protected override void Process(Entity entity, SpatialFormComponent spatialFormComponent, TransformComponent transformComponent)
        {
            if (spatialFormComponent != null)
            {
                spatialName = spatialFormComponent.SpatialFormFile;

                if (transformComponent.X >= 0 &&
                    transformComponent.Y >= 0 &&
                    transformComponent.X < spriteBatch.GraphicsDevice.Viewport.Width &&
                    transformComponent.Y < spriteBatch.GraphicsDevice.Viewport.Height)
                {
                    if (string.Compare("GoodPlayer", spatialName, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        GoodPlayer.Render(spriteBatch, contentManager, transformComponent, entity.GetComponent<VelocityComponent>(),entity.GetComponent<HealthComponent>());
                    }
                    else if (string.Compare("BadPlayer", spatialName, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        BadPlayer.Render(spriteBatch, contentManager, transformComponent, entity.GetComponent<VelocityComponent>(),entity.GetComponent<HealthComponent>());
                    }
                    else if (string.Compare("Wall", spatialName, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        Wall.Render(spriteBatch, contentManager, transformComponent, entity.GetComponent<HealthComponent>());
                    }
                    else if (string.Compare("WallAttack", spatialName, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        WallAttack.Render(spriteBatch, contentManager, transformComponent);
                    }

                    else if (string.Compare("Bomb", spatialName, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        Bomb.Render(spriteBatch, contentManager, transformComponent);

                    }
                    else if (string.Compare("Text", spatialName, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        Text.Render(spriteBatch, contentManager, transformComponent, entity.GetComponent<TextComponent>());
                    }
                    else if (string.Compare("Level", spatialName, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        Level.Render(spriteBatch, contentManager, transformComponent, entity.GetComponent<MenuSelectionComponent>());
                    }
                    else if (string.Compare("Title", spatialName, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        Title.Render(spriteBatch, contentManager, transformComponent);
                    }
                }
            }
        }
    }
}
