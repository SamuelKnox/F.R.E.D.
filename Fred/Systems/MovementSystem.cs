using Artemis;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using Fred.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 1)]
    class MovementSystem : EntityProcessingSystem<TransformComponent, VelocityComponent>
    {
        protected override void Process(Entity entity, TransformComponent transformComponent, VelocityComponent velocityComponent)
        {
            if (velocityComponent != null)
            {
                if (transformComponent != null)
                {
                    float ms = TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;

                    transformComponent.X += (float)(Math.Cos(velocityComponent.AngleAsRadians) * velocityComponent.Speed * ms);
                    transformComponent.Y += (float)(Math.Sin(velocityComponent.AngleAsRadians) * velocityComponent.Speed * ms);
                }
            }
        }
    }
}
