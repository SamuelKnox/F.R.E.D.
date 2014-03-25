using Artemis.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Components
{
    class TextComponent : IComponent
    {
        public SpriteFont SpriteFont { get; set; }
        public string Font { get; set; }
        public string Message { get; set; }
        public Color Color { get; set; }

        public TextComponent(string f, string msg,Color c)
        {
            this.Font = f;
            this.Message = msg;
            this.Color = c;
        }
    }
}
