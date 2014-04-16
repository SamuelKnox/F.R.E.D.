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
    class GoodPlayer
    {
        static Texture2D tex;

		public static void Render(SpriteBatch spriteBatch, ContentManager contentManager, TransformComponent transformComponent, VelocityComponent velocityComponent, CooldownComponent cooldownComponent)
		{

			if (Math.Abs(velocityComponent.XVelocity) <= 0.09 && Math.Abs(velocityComponent.YVelocity) <= 0.09) {
				tex = contentManager.Load<Texture2D> ("goodplayer3");
			}
			else if (!transformComponent.leftImage && cooldownComponent.IsSpriteReady) {
				tex = contentManager.Load<Texture2D> ("goodplayer");
				cooldownComponent.ResetSpriteCooldown ();
				transformComponent.leftImage = true;
			}else if (transformComponent.leftImage && cooldownComponent.IsSpriteReady) {
				tex = contentManager.Load<Texture2D> ("goodplayer2");
				cooldownComponent.ResetSpriteCooldown ();
				transformComponent.leftImage = false;
			}else if (transformComponent.leftImage && !cooldownComponent.IsSpriteReady) {
				tex = contentManager.Load<Texture2D> ("goodplayer");
			}
			else {
				tex = contentManager.Load<Texture2D> ("goodplayer2");
			}
            spriteBatch.Begin();
            SpriteEffects spriteEffects = new SpriteEffects();
            spriteBatch.Draw(tex, new Rectangle((int)transformComponent.CenterOfRectangle.X, (int)transformComponent.CenterOfRectangle.Y, tex.Width, tex.Height), tex.Bounds, Color.White, velocityComponent.Direction, new Vector2((int)(transformComponent.Location.Width * .5), (int)(transformComponent.Location.Height * 0.5)), spriteEffects, 1.0F);
            spriteBatch.End();
            transformComponent.Location = new Rectangle((int)transformComponent.X, (int)transformComponent.Y, tex.Width, tex.Height);
        }
    }
}