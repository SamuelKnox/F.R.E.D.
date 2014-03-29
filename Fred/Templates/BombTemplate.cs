using Artemis;
using Artemis.Attributes;
using Artemis.Interface;
using Fred.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Templates
{
    [ArtemisEntityTemplate(Name)]
    class BombTemplate : IEntityTemplate
    {


        public const string Name = "BombTemplate";

        public Entity BuildEntity(Entity entity, EntityWorld world, params object[] args)
        {
            entity.AddComponentFromPool<TransformComponent>();
            entity.AddComponent(new SpatialFormComponent("Bomb"));
            entity.AddComponent(new FuseComponent(1000));
            entity.AddComponent(new DamageComponent(100));
            entity.AddComponent(new NearbyGridsComponent());

            return entity;
        }
    }
}
