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
    class Wall
    {
        static Texture2D tex;

		public static void Render(SpriteBatch spriteBatch, ContentManager contentManager, TransformComponent transformComponent, HealthComponent healthComponent)
        {
			if (!healthComponent.IsAlive) {
				tex = contentManager.Load<Texture2D> ("floor");
			} else if (healthComponent.HealthPercentage >= .80) {
				tex = contentManager.Load<Texture2D> ("wall");
			} else if (healthComponent.HealthPercentage >= .30) {
				tex = contentManager.Load<Texture2D> ("wall2");
			} else {
				tex = contentManager.Load<Texture2D> ("wall3");
			} 

            spriteBatch.Begin();
            spriteBatch.Draw(tex, new Rectangle((int)transformComponent.X, (int)transformComponent.Y, tex.Width, tex.Height), tex.Bounds, Color.White);
            spriteBatch.End();
            transformComponent.Location = new Rectangle((int)transformComponent.X, (int)transformComponent.Y, tex.Width, tex.Height);
        }

    }
}
