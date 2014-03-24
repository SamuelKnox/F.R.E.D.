using Artemis;
using Artemis.Attributes;
using Artemis.System;
using Fred.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Systems
{
    [ArtemisEntitySystem]
    class ExpirationSystem : EntityProcessingSystem<ExpirationComponent>
    {   protected override void Process(Entity entity, ExpirationComponent expirationComponent)
        {
            if (expirationComponent != null)
            {
                float ms = TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
                expirationComponent.ReduceLifeTime(ms);

                if (expirationComponent.IsExpired)
                {
                    entity.Delete();
                }
            }
        }
    }
}
