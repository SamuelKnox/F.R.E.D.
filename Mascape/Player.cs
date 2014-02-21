using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mascape
{
    class Player
    {
        public Texture2D Image { get; set; }
        public Rectangle Rectangle { get; set; }
        public Vector2 Vector { get; set; }
        public Attribute HitPoints { get; set; }
        public float Speed { get; set; }
        public Attribute DestroyCooldown { get; set; }
        public Attribute BuildCooldown { get; set; }
        public Attribute BombCooldown { get; set; }
        public float Damage { get; set; }
        public float Heal { get; set; }
        public String Team { get; set; }
        public bool Mobile { get; set; }
        public bool Healable { get; set; }
        public bool Human { get; set; }

        public Player(Attribute hp, float speed, Attribute destroy, Attribute build, Attribute bomb, float dmg, float heal, String team)
        {
            HitPoints = hp;
            Speed = speed;
            DestroyCooldown = destroy;
            BuildCooldown = build;
            BombCooldown = bomb;
            Damage = dmg;
            Heal = heal;
            Mobile = true;
            Healable = true;
            Team = team;
            Human = true;
        }

    }
}
