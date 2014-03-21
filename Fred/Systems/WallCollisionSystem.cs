

using Artemis;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using Artemis.Utils;
using Fred.Components;
using Microsoft.Xna.Framework;
using System;
namespace Fred.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 1)]
    class WallCollisionSystem : EntityProcessingSystem<TransformComponent, VelocityComponent>
    {
        protected override void Process(Entity entity, TransformComponent transformComponent, VelocityComponent velocityComponent)
        {

            if (velocityComponent != null && transformComponent != null)
            {
                Bag<Entity> walls = this.EntityWorld.GroupManager.GetEntities("Walls");
                foreach (Entity w in walls)
                {
                    if (transformComponent.Location.Intersects(w.GetComponent<TransformComponent>().Location))
                    {
                        if (w.GetComponent<OrientationComponent>().Orientation.Equals("horizontal"))
                        {

                            if (transformComponent.Y < w.GetComponent<TransformComponent>().Y)
                            {
                                float y = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Height * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
                                transformComponent.Y -= y;
                            }
                            else
                            {
                                float y = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Height * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;

                                transformComponent.Y += y;
                            }
                        }
                        else
                        {

                            {

                                if (transformComponent.X < w.GetComponent<TransformComponent>().Y)
                                {
                                    float x = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Width * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
                                    transformComponent.X -= x;
                                }
                                else
                                {
                                    float x = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Width * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
                                    transformComponent.X += x;
                                }
                            }
                        }

                    }
                }
            }
        }
    }
}
