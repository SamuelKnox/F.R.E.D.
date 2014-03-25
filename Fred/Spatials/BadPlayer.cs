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
    class BadPlayer
    {
        static Texture2D tex;

        public static void Render(SpriteBatch spriteBatch, ContentManager contentManager, TransformComponent transformComponent)
        {
            if (tex == null)
            {
                tex = contentManager.Load<Texture2D>("badplayer");
            }
            spriteBatch.Begin();
            spriteBatch.Draw(tex, new Rectangle((int)transformComponent.CenterOfRectangle.X, (int)transformComponent.CenterOfRectangle.Y, tex.Width, tex.Height), tex.Bounds, Color.White);
            spriteBatch.End();
            transformComponent.Location = new Rectangle((int)transformComponent.X, (int)transformComponent.Y, tex.Width, tex.Height);
        }

    }
}
