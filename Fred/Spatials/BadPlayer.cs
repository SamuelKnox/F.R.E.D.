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
		static string direction;
		static float rotation;
		public static void Render(SpriteBatch spriteBatch, ContentManager contentManager, TransformComponent transformComponent)
		{
			if (transformComponent.direction == null) {
				direction = "right";
			} else {
				direction = transformComponent.direction;
			}
			rotation = 0;
			if (direction.Equals("right")){
				tex = contentManager.Load<Texture2D>("badplayerright");
			}
			if (direction.Equals("left")) {
				tex = contentManager.Load<Texture2D> ("badplayerleft");
			}
			if (direction.Equals ("down")) {
				tex = contentManager.Load<Texture2D> ("badplayerdown");
			}
			if (direction.Equals ("up")) {
				tex = contentManager.Load<Texture2D> ("badplayerup");
			}
			if (direction.Equals ("upright")) {
				tex = contentManager.Load<Texture2D> ("badplayerright");
				rotation = (float)(Math.Atan2 (transformComponent.X, transformComponent.Y) + (-Math.PI * 0.65));
			}
			if (direction.Equals ("upleft")) {
				tex = contentManager.Load<Texture2D> ("badplayerup");
				rotation = (float)(Math.Atan2 (transformComponent.X, transformComponent.Y) + (-Math.PI * 0.65));
			}
			if (direction.Equals ("downright")) {
				tex = contentManager.Load<Texture2D> ("badplayerdown");
				rotation = (float)(Math.Atan2 (transformComponent.X, transformComponent.Y) + (-Math.PI * 0.65));
			}
			if (direction.Equals ("downleft")) {
				tex = contentManager.Load<Texture2D> ("badplayerleft");
				rotation = (float)(Math.Atan2 (transformComponent.X, transformComponent.Y) + (-Math.PI * 0.65));
			}
			spriteBatch.Begin();
			spriteBatch.Draw(tex, new Vector2((int)transformComponent.X, (int)transformComponent.Y), tex.Bounds, Color.White,rotation,new Vector2(tex.Width/2,tex.Height/2), 1.0f, SpriteEffects.None, 1);
			spriteBatch.End();
			transformComponent.Location = new Rectangle((int)transformComponent.X, (int)transformComponent.Y, tex.Width, tex.Height);
		}

        public static void Render(SpriteBatch spriteBatch, ContentManager contentManager, TransformComponent transformComponent, VelocityComponent velocityComponent)
        {
            if (tex == null)
            {
                tex = contentManager.Load<Texture2D>("badplayer");
            }
            spriteBatch.Begin();
                        SpriteEffects spriteEffects = new SpriteEffects();
            spriteBatch.Draw(tex, new Rectangle((int)transformComponent.CenterOfRectangle.X, (int)transformComponent.CenterOfRectangle.Y, tex.Width, tex.Height), tex.Bounds, Color.White, velocityComponent.Direction, new Vector2((int)(transformComponent.Location.Width * .5), (int)(transformComponent.Location.Height * 0.5)), spriteEffects, 1.0F);
            spriteBatch.End();
            transformComponent.Location = new Rectangle((int)transformComponent.X, (int)transformComponent.Y, tex.Width, tex.Height);
        }

    }
}
