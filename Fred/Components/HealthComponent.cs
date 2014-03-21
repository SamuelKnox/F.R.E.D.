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
            this.CurrentHealth = this.MaximumHealth = points;
        }

        public int CurrentHealth { get; set; }

        public bool IsAlive
        {
            get
            {
                return this.CurrentHealth > 0;
            }
        }

        public int MaximumHealth { get; set; }

        public void AddDamage(int damage)
        {
            this.CurrentHealth -= damage;
            if (this.CurrentHealth < 0)
            {
                this.CurrentHealth = 0;
            }
        }
    }
}
