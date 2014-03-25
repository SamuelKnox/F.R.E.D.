using Fred.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Spatials
{
    class Text
    {
        public static void Render(SpriteBatch spriteBatch, ContentManager contentManager, TransformComponent transformComponent, TextComponent textComponent)
        {
            if (textComponent.SpriteFont == null)
            {
                textComponent.SpriteFont = contentManager.Load<SpriteFont>("Arial");
            }
            spriteBatch.Begin();
           spriteBatch.DrawString(textComponent.SpriteFont, textComponent.Message, new Vector2(transformComponent.CenterOfRectangle.X - textComponent.SpriteFont.MeasureString(textComponent.Message).X / 2, transformComponent.CenterOfRectangle.Y - textComponent.SpriteFont.MeasureString(textComponent.Message).Y / 2), textComponent.Color);
            spriteBatch.End();
          }
    }
}
