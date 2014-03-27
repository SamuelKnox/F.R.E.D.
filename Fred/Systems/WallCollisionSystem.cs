﻿

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
        private Entity maze;

                        bool tester = false;

        public override void LoadContent()
        {
            maze = BlackBoard.GetEntry<Entity>("Maze");
        }

        protected override void Process(Entity entity, TransformComponent transformComponent, VelocityComponent velocityComponent)
        {
            if (velocityComponent != null && transformComponent != null)
            {
                if (entity.Tag.Equals("GOOD_PLAYER"))
                {
                    float bounce = -0.75F;
                    Bag<Entity> walls = this.EntityWorld.GroupManager.GetEntities("Walls");
                    foreach (Vector2 coords in entity.GetComponent<NearbyGridsComponent>().NearbyGrids)
                    {
                        //for w, x too small, y too big
                        Entity w = walls[(int)(coords.Y * maze.GetComponent<MazeComponent>().Width + coords.X)];
                        if (!tester)
                        {
                            Console.WriteLine("Player:  " +
                            entity.GetComponent<TransformComponent>().X + "  " +
                            entity.GetComponent<TransformComponent>().Y);
                            Console.WriteLine("WALL:  " + w.GetComponent<TransformComponent>().X + "  " + w.GetComponent<TransformComponent>().Y);
                       
                            Console.WriteLine(); }
            
                        if (transformComponent.Location.Intersects(w.GetComponent<TransformComponent>().Location) && w.GetComponent<HealthComponent>().IsAlive)
                        {
                            float xDistance = w.GetComponent<TransformComponent>().CenterOfRectangle.X - transformComponent.CenterOfRectangle.X;
                            float YDistance = w.GetComponent<TransformComponent>().CenterOfRectangle.Y - transformComponent.CenterOfRectangle.Y;
                            if (xDistance < 0 && YDistance <= 0)
                            {
                                if (Math.Abs(xDistance) >= Math.Abs(YDistance))
                                {
                                    //right
                                    float x = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Width;
                                    transformComponent.X += x;
                                    if (entity.GetComponent<CooldownComponent>().CurrentBounceCooldown < 0)
                                    {
                                        velocityComponent.xVelocity *= bounce;
                                        entity.GetComponent<CooldownComponent>().ResetBounceCooldown();
                                    }
                                }
                                else
                                {
                                    //bottom
                                    float y = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Height;
                                    transformComponent.Y += y;
                                    if (entity.GetComponent<CooldownComponent>().CurrentBounceCooldown < 0)
                                    {
                                        velocityComponent.yVelocity *= bounce;
                                        entity.GetComponent<CooldownComponent>().ResetBounceCooldown();
                                    }
                                }
                            }
                            else if (xDistance >= 0 && YDistance < 0)
                            {
                                if (Math.Abs(xDistance) >= Math.Abs(YDistance))
                                {
                                    //left
                                    float x = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Width;
                                    transformComponent.X -= x;
                                    if (entity.GetComponent<CooldownComponent>().CurrentBounceCooldown < 0)
                                    {
                                        velocityComponent.xVelocity *= bounce;
                                        entity.GetComponent<CooldownComponent>().ResetBounceCooldown();
                                    }
                                }
                                else
                                {
                                    //bottom
                                    float y = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Height;
                                    transformComponent.Y += y;
                                    if (entity.GetComponent<CooldownComponent>().CurrentBounceCooldown < 0)
                                    {
                                        velocityComponent.yVelocity *= bounce;
                                        entity.GetComponent<CooldownComponent>().ResetBounceCooldown();
                                    }
                                }
                            }

                            else if (xDistance > 0 && YDistance >= 0)
                            {
                                if (Math.Abs(xDistance) >= Math.Abs(YDistance))
                                {
                                    //left
                                    float x = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Width;
                                    transformComponent.X -= x;
                                    if (entity.GetComponent<CooldownComponent>().CurrentBounceCooldown < 0)
                                    {
                                        velocityComponent.xVelocity *= bounce;
                                        entity.GetComponent<CooldownComponent>().ResetBounceCooldown();
                                    }
                                }
                                else
                                {
                                    //top
                                    float y = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Height;
                                    transformComponent.Y -= y;
                                    if (entity.GetComponent<CooldownComponent>().CurrentBounceCooldown < 0)
                                    {
                                        velocityComponent.yVelocity *= bounce;
                                        entity.GetComponent<CooldownComponent>().ResetBounceCooldown();
                                    }
                                }
                            }
                            else if (xDistance <= 0 && YDistance > 0)
                            {
                                if (Math.Abs(xDistance) >= Math.Abs(YDistance))
                                {
                                    //right
                                    float x = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Width;
                                    transformComponent.X += x;
                                    if (entity.GetComponent<CooldownComponent>().CurrentBounceCooldown < 0)
                                    {
                                        velocityComponent.xVelocity *= bounce;
                                        entity.GetComponent<CooldownComponent>().ResetBounceCooldown();
                                    }
                                }
                                else
                                {
                                    //top
                                    float y = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Height;
                                    transformComponent.Y -= y;
                                    if (entity.GetComponent<CooldownComponent>().CurrentBounceCooldown < 0)
                                    {
                                        velocityComponent.yVelocity *= bounce;
                                        entity.GetComponent<CooldownComponent>().ResetBounceCooldown();
                                    }
                                }

                            }

                        }
                    }
                    tester = true;
                }
                else if (entity.Tag.Equals("BAD_PLAYER"))
                {

                    Bag<Entity> walls = this.EntityWorld.GroupManager.GetEntities("Walls");
                    foreach (Vector2 coords in entity.GetComponent<NearbyGridsComponent>().NearbyGrids)
                    {
                        Entity w = walls[(int)(coords.Y * maze.GetComponent<MazeComponent>().Width + coords.X)];
            
                        if (transformComponent.Location.Intersects(w.GetComponent<TransformComponent>().Location) && w.GetComponent<HealthComponent>().CurrentHealth > 1000000)
                        {
                            float xDistance = w.GetComponent<TransformComponent>().CenterOfRectangle.X - transformComponent.CenterOfRectangle.X;
                            float YDistance = w.GetComponent<TransformComponent>().CenterOfRectangle.Y - transformComponent.CenterOfRectangle.Y;
                            if (xDistance < 0 && YDistance <= 0)
                            {
                                if (Math.Abs(xDistance) >= Math.Abs(YDistance))
                                {
                                    //right
                                    float x = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Width;
                                    transformComponent.X += x;
                                }
                                else
                                {
                                    //bottom
                                    float y = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Height;
                                    transformComponent.Y += y;
                                }
                            }
                            else if (xDistance >= 0 && YDistance < 0)
                            {
                                if (Math.Abs(xDistance) >= Math.Abs(YDistance))
                                {
                                    //left
                                    float x = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Width;
                                    transformComponent.X -= x;
                                }
                                else
                                {
                                    //bottom
                                    float y = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Height;
                                    transformComponent.Y += y;
                                }
                            }

                            else if (xDistance > 0 && YDistance >= 0)
                            {
                                if (Math.Abs(xDistance) >= Math.Abs(YDistance))
                                {
                                    //left
                                    float x = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Width;
                                    transformComponent.X -= x;
                                }
                                else
                                {
                                    //top
                                    float y = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Height;
                                    transformComponent.Y -= y;
                                }
                            }
                            else if (xDistance <= 0 && YDistance > 0)
                            {
                                if (Math.Abs(xDistance) >= Math.Abs(YDistance))
                                {
                                    //right
                                    float x = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Width;
                                    transformComponent.X += x;
                                }
                                else
                                {
                                    //top
                                    float y = Rectangle.Intersect(transformComponent.Location, w.GetComponent<TransformComponent>().Location).Height;
                                    transformComponent.Y -= y;
                                }

                            }

                        }
                    }
                }
            }
        }
    }
}
