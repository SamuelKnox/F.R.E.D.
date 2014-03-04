using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred
{
    class Wall
    {

        public Texture2D Image { get; set; }
        public Rectangle Rectangle { get; set; }
        public bool Active { get; set; }
        public string Orientation { get; set; }
        //public bool Locked { get; set; }
        //public bool Trapped { get; set; }

        //public override string ToString()
        //{
        //    return base.ToString() + ": " + "Active: " + Active;
        //}
    }
}

