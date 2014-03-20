using Artemis;
using Artemis.Interface;
using Artemis.Attributes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Components
{
    class HealthComponent : IComponent
    {
        public HealthComponent()
            : this(0)
        {
        }

        public HealthComponent(int points)
        {
            this.Points = this.MaximumHealth = points;
        }

        public int Points { get; set; }

        public bool IsAlive
        {
            get
            {
                return this.Points > 0;
            }
        }

        public int MaximumHealth { get; private set; }

        public void AddDamage(int damage)
        {
            this.Points -= damage;
            if (this.Points < 0)
            {
                this.Points = 0;
            }
        }
    }
}
