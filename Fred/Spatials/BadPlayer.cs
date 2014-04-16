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

		public static void Render(SpriteBatch spriteBatch, ContentManager contentManager, TransformComponent transformComponent, VelocityComponent velocityComponent, CooldownComponent cooldownComponent)
		{
			if (Math.Abs(velocityComponent.XVelocity) <= 0.03 && Math.Abs(velocityComponent.YVelocity) <= 0.03) {
				tex = contentManager.Load<Texture2D> ("badplayer3");
			}
			else if (!transformComponent.leftImage && cooldownComponent.IsSpriteReady) {
				tex = contentManager.Load<Texture2D> ("badplayer");
				cooldownComponent.ResetSpriteCooldown ();
				transformComponent.leftImage = true;
			}else if (transformComponent.leftImage && cooldownComponent.IsSpriteReady) {
				tex = contentManager.Load<Texture2D> ("badplayer2");
				cooldownComponent.ResetSpriteCooldown ();
				transformComponent.leftImage = false;
			}else if (transformComponent.leftImage && !cooldownComponent.IsSpriteReady) {
				tex = contentManager.Load<Texture2D> ("badplayer");
			}
			else {
				tex = contentManager.Load<Texture2D> ("badplayer2");
			} 
                spriteBatch.Begin();
                SpriteEffects spriteEffects = new SpriteEffects();
                spriteBatch.Draw(tex, new Rectangle((int)transformComponent.CenterOfRectangle.X, (int)transformComponent.CenterOfRectangle.Y, tex.Width, tex.Height), tex.Bounds, Color.White, velocityComponent.Direction, new Vector2((int)(transformComponent.Location.Width * .5), (int)(transformComponent.Location.Height * 0.5)), spriteEffects, 1.0F);
                spriteBatch.End();

                transformComponent.Location = new Rectangle((int)transformComponent.X, (int)transformComponent.Y, tex.Width, tex.Height);
            
        }
    }
}
