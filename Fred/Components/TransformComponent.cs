using Artemis;
using Artemis.Attributes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Components
{
    [ArtemisComponentPool(InitialSize = 5, IsResizable = true, ResizeSize = 20, IsSupportMultiThread = false)]
    class TransformComponent : ComponentPoolable
    {

        public float X { get; set; }
        public float Y { get; set; }
        public Rectangle Location { get; set; }
        public Vector2 CenterOfRectangle 
        {
            get
            {
                float x = X + Location.Width / 2;
                float y = Y + Location.Height / 2;
                return new Vector2(x, y);
            }
        }
        public Vector2 Position
        {
            get
            {
                return new Vector2(this.X, this.Y);
            }

            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        public TransformComponent()
            : this(Vector2.Zero)
        {
        }
        public TransformComponent(float x, float y)
            : this(new Vector2(x, y))
        {
        }
        public TransformComponent(Vector2 position)
        {
            this.Position = position;
        }



        public override void CleanUp()
        {
            this.Position = Vector2.Zero;
        }
    }
}
