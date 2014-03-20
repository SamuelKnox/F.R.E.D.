﻿using Artemis;
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
                        GoodPlayer.Render(spriteBatch, contentManager, transformComponent);
                    }
                }
            }
        }
    }
}