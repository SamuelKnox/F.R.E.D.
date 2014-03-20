using Artemis.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Components
{
    class VelocityComponent : IComponent
    {
        private const float ToRadians = (float)(Math.PI / 180.0);

        public float Speed { get; set; }
        public float Angle { get; set; }

        public float AngleAsRadians
        {
            get
            {
                return this.Angle * ToRadians;
            }
        }

        public VelocityComponent()
            : this(0.0f, 0.0f)
        {
        }

        public VelocityComponent(float velocity)
            : this(velocity, 0.0f)
        {
        }

        public VelocityComponent(float velocity, float angle)
        {
            this.Speed = velocity;
            this.Angle = angle;
        }
        public void AddAngle(float angle)
        {
            this.Angle = (this.Angle + angle) % 360;
        }
    }
}
