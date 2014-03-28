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
				tex = contentManager.Load<Texture2D>("goodplayerright");
			}
			if (direction.Equals("left")) {
				tex = contentManager.Load<Texture2D> ("goodplayerleft");
			}
			if (direction.Equals ("down")) {
				tex = contentManager.Load<Texture2D> ("goodplayerdown");
			}
			if (direction.Equals ("up")) {
				tex = contentManager.Load<Texture2D> ("goodplayerup");
			}
			if (direction.Equals ("upright")) {
				tex = contentManager.Load<Texture2D> ("goodplayerright");
				rotation = (float)(Math.Atan2 (transformComponent.X, transformComponent.Y) + (-Math.PI * 0.45));
				//rotation = (float)(270 - Math.Atan2(transformComponent.X, transformComponent.Y));
			}
			if (direction.Equals ("upleft")) {
				tex = contentManager.Load<Texture2D> ("goodplayerup");
				rotation = (float)(Math.Atan2 (transformComponent.X, transformComponent.Y) + (-Math.PI * 0.75));
				//rotation = (float)(270 - Math.Atan2(transformComponent.X, transformComponent.Y));
			}
			if (direction.Equals ("downright")) {
				tex = contentManager.Load<Texture2D> ("goodplayerdown");
				rotation = (float)(Math.Atan2 (transformComponent.X, transformComponent.Y) + (-Math.PI * 0.75));
				//rotation = (float)(270 - Math.Atan2(transformComponent.X, transformComponent.Y));
			}
			if (direction.Equals ("downleft")) {
				tex = contentManager.Load<Texture2D> ("goodplayerleft");
				rotation = (float)(Math.Atan2 (transformComponent.X, transformComponent.Y) + (-Math.PI * 0.45));
				//rotation = (float)(270 - Math.Atan2(transformComponent.X, transformComponent.Y));
			}
			spriteBatch.Begin();
			spriteBatch.Draw(tex, new Vector2((int)transformComponent.X, (int)transformComponent.Y), tex.Bounds, Color.White,rotation,new Vector2(tex.Width/2,tex.Height/2), 1.0f, SpriteEffects.None, 1);
			spriteBatch.End();
			transformComponent.Location = new Rectangle((int)transformComponent.X, (int)transformComponent.Y, tex.Width, tex.Height);
		}

	}
}

