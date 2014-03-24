

using Artemis;
using Artemis.Attributes;
using Artemis.Interface;
using Fred.Components;
namespace Fred.Templates
{
    [ArtemisEntityTemplate(Name)]
    class WallAttackTemplate : IEntityTemplate
    {
        public const string Name = "WallAttackTemplate";

        public Entity BuildEntity(Entity entity, EntityWorld world, params object[] args)
        {
            entity.AddComponentFromPool<TransformComponent>();
            entity.AddComponent(new SpatialFormComponent("WallAttack"));
            entity.AddComponent(new ExpirationComponent(500));

            return entity;
        }

    }
}
