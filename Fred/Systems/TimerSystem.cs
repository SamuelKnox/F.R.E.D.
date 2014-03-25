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
    [ArtemisEntitySystem]
    class TimerSystem : EntityProcessingSystem<TimerComponent>
    {   protected override void Process(Entity entity, TimerComponent timerComponent)
        {
            if (timerComponent != null)
            {
                float ms = TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
                timerComponent.Time -= ms;
                entity.GetComponent<TextComponent>().Message = "" + (int)(timerComponent.Time / 1000);
                if (timerComponent.IsExpired)
                {
                    entity.Delete();
                }
            }
        }

    }
}
