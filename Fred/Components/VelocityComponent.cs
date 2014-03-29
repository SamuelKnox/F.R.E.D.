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

        public float XVelocity { get; set; }
        public float YVelocity { get; set; }
        public float Direction { get; set; }

        public VelocityComponent()
            : this(0.0f, 0.0f)
        {
        }


        public VelocityComponent(float x, float y)
        {
            this.XVelocity = x;
            this.YVelocity = y;
            this.Direction = 0;
        }
    }
}
