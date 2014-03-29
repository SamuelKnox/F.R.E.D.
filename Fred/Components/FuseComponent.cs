using Artemis.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Components
{
    class FuseComponent : IComponent
    {   public FuseComponent() 
            : this(0.0f)
        {
        }

        public FuseComponent(float lifeTime)
        {
            this.LifeTime = lifeTime;
        }

        public bool IsExpired
        {
            get
            {
                return this.LifeTime <= 0;
            }
        }

        public float LifeTime { get; set; }

        public void ReduceLifeTime(float lifeTimeDelta)
        {
            this.LifeTime -= lifeTimeDelta;
            if (this.LifeTime < 0)
            {
                this.LifeTime = 0;
            }
        }
    }
}
