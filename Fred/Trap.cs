using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred
{
    class Trap
    {
        public double Damage { get; set; }
        public double Delay { get; set; }
        public Texture2D Image { get; set; }
        public Rectangle Rectangle { get; set; }
        public bool Active { get; set; }


        public Trap(double dmg, double delay)
        {
            Damage = dmg;
            Delay = delay;
            Active = true;
        }
    }
}