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
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update)]
    class CooldownSystem : EntityProcessingSystem<CooldownComponent>
    {
        protected override void Process(Entity entity, CooldownComponent cooldownComponent)
        {
            cooldownComponent.CurrentBombCooldown -= TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
            cooldownComponent.CurrentBuildCooldown -= TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
            cooldownComponent.CurrentBounceCooldown -= TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
            cooldownComponent.CurrentMenuSelectCooldown -= TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
        }
    }
}
