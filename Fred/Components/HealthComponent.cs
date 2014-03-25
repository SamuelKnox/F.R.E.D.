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


        public double CurrentHealth { get; set; }
        public double MaximumHealth { get; set; }

        public bool IsAlive
        {
            get
            {
                return this.CurrentHealth > 0;
            }
        }
        public double HealthPercentage
        {
            get
            {
                return this.CurrentHealth / this.MaximumHealth;
            }
        }

        public HealthComponent()
            : this(0)
        {
        }

        public HealthComponent(double points)
        {
            this.CurrentHealth = this.MaximumHealth = points;
        }

        public HealthComponent(double current, double max)
        {
            this.CurrentHealth = current;
            this.MaximumHealth = max;
        }

        public void AddDamage(double damage)
        {
            this.CurrentHealth -= damage;
            if (this.CurrentHealth < 0)
            {
                this.CurrentHealth = 0;
            }
        }
        public void AddHealth(double heal)
        {
            this.CurrentHealth += heal;
            if (this.CurrentHealth > this.MaximumHealth)
            {
                this.CurrentHealth = this.MaximumHealth;
            }
        }
    }
}
