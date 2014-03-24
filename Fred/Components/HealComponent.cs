using Artemis.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Components
{
    class HealComponent : IComponent
    {


        public double BaseHeal { get; set; }
        public double CriticalHealChance { get; set; }

        public double Heal
        {
            get
            {
                Random rand = new Random();
                if (IsCriticalHit)
                {
                    return (this.BaseHeal * rand.Next(2, 4)) * (rand.Next(90, 110) / 100);;
                }
                return BaseHeal * (rand.Next(90, 110) / 100);
            }
        }


        public bool IsCriticalHit
        {
            get
            {
                Random rand = new Random();
                if (rand.NextDouble() < CriticalHealChance)
                {
                    return true;
                }
                return false;
            }
        }


        public HealComponent(double heal, double crit)
        {
            Random rand = new Random();
            BaseHeal = heal;
            CriticalHealChance = crit;
        }

        public HealComponent(double heal)
        {
            Random rand = new Random();
            BaseHeal = heal;
            CriticalHealChance = 0.2;
        }

    }
}
