using Artemis.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Components
{
    class ExpirationComponent : IComponent
    {   public ExpirationComponent() 
            : this(0.0f)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ExpiresComponent" /> class.</summary>
        /// <param name="lifeTime">The life time.</param>
        public ExpirationComponent(float lifeTime)
        {
            this.LifeTime = lifeTime;
        }

        /// <summary>Gets a value indicating whether is expired.</summary>
        /// <value><see langword="true" /> if this instance is expired; otherwise, <see langword="false" />.</value>
        public bool IsExpired
        {
            get
            {
                return this.LifeTime <= 0;
            }
        }

        /// <summary>Gets or sets the life time.</summary>
        /// <value>The life time.</value>
        public float LifeTime { get; set; }

        /// <summary>The reduce life time.</summary>
        /// <param name="lifeTimeDelta">The life time.</param>
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
