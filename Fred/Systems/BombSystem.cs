using Artemis;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using Artemis.Utils;
using Fred.Components;
using Fred.Templates;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Systems
{
    [ArtemisEntitySystem]
    class BombSystem : EntityProcessingSystem<TransformComponent, FuseComponent, NearbyGridsComponent, DamageComponent>
    {


        protected override void Process(Entity entity, TransformComponent transformComponent, FuseComponent fuseComponent, NearbyGridsComponent nearbyGridsComponent, DamageComponent damageComponent)
        {

            if (transformComponent != null & fuseComponent != null && nearbyGridsComponent != null && damageComponent != null)
            {
                float ms = TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
                fuseComponent.ReduceLifeTime(ms);

                if (fuseComponent.IsExpired)
                {
                    Bag<Entity> walls = this.EntityWorld.GroupManager.GetEntities("Walls");
                    foreach (int i in nearbyGridsComponent.NearbyIndices)
                    {
                        //walls[i].GetComponent<HealthComponent>().AddDamage(damageComponent.Damage);
                        walls[i].GetComponent<HealthComponent>().AddDamage(1000);
                        Entity attackAnimation = this.EntityWorld.CreateEntityFromTemplate(WallAttackTemplate.Name);
                        attackAnimation.GetComponent<TransformComponent>().Position = walls[i].GetComponent<TransformComponent>().Position;
                    }
                entity.Delete();
                }
            }

        }
    }
}
