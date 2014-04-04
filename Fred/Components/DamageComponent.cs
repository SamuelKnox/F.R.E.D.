using Artemis.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Components
{
    class DamageComponent : IComponent
    {

        public double BaseDamage { get; set; }
        public double CriticalHitChance { get; set; }


        public double Damage
        {
            get
            {
                //Random rand = new Random();
                //if (IsCriticalHit)
                //{
                //    return (this.BaseDamage * rand.Next(2, 4));
                //}
                //return BaseDamage;
                return 4;
            }
        }


        public DamageComponent(double dmg, double crit)
        {
            Random rand = new Random();
            BaseDamage = dmg;
            CriticalHitChance = crit;
        }

        public DamageComponent(double dmg)
        {
            BaseDamage = dmg;
            CriticalHitChance = 0.2;
        }


        public bool IsCriticalHit
        {
            get
            {
                Random rand = new Random();
                return rand.NextDouble() < CriticalHitChance;
            }
        }

    }
}

