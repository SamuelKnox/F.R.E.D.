using Artemis;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using Fred.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update)]
    class NearbyGridsSystem : EntityProcessingSystem<NearbyGridsComponent>
    {
        private Entity maze;
        private ContentManager content;


        public override void LoadContent()
        {
            maze = BlackBoard.GetEntry<Entity>("Maze");
            content = BlackBoard.GetEntry<ContentManager>("ContentManager");
        }

        protected override void Process(Entity entity, NearbyGridsComponent nearbyGridsComponent)
        {
            int width = content.Load<Texture2D>("wall").Width;
            int height = content.Load<Texture2D>("wall").Height;
            entity.GetComponent<NearbyGridsComponent>().NearbyGrids = NearbyGridsUpdate(nearbyGridsComponent, new Vector2((int)(entity.GetComponent<TransformComponent>().X / width), (int)(entity.GetComponent<TransformComponent>().Y / height)), maze.GetComponent<MazeComponent>().Width, maze.GetComponent<MazeComponent>().Height);


        }

        public List<Vector2> NearbyGridsUpdate(NearbyGridsComponent nearbyGridsComponent, Vector2 current, int w, int h)
        {
            nearbyGridsComponent.CurrentGrid = current;
            nearbyGridsComponent.RightGrid = new Vector2(current.X + 1, current.Y);
            nearbyGridsComponent.BottomRightGrid = new Vector2(current.X + 1, current.Y + 1);
            nearbyGridsComponent.BottomGrid = new Vector2(current.X, current.Y + 1);
            nearbyGridsComponent.BottomLeftGrid = new Vector2(current.X - 1, current.Y + 1);
            nearbyGridsComponent.LeftGrid = new Vector2(current.X - 1, current.Y);
            nearbyGridsComponent.TopLeftGrid = new Vector2(current.X - 1, current.Y - 1);
            nearbyGridsComponent.TopGrid = new Vector2(current.X, current.Y - 1);
            nearbyGridsComponent.TopRightGrid = new Vector2(current.X + 1, current.Y - 1);
            nearbyGridsComponent.NearbyGrids = new List<Vector2>();

            nearbyGridsComponent.NearbyGrids.Add(nearbyGridsComponent.CurrentGrid);
            nearbyGridsComponent.NearbyGrids.Add(nearbyGridsComponent.RightGrid);
            nearbyGridsComponent.NearbyGrids.Add(nearbyGridsComponent.BottomRightGrid);
            nearbyGridsComponent.NearbyGrids.Add(nearbyGridsComponent.BottomGrid);
            nearbyGridsComponent.NearbyGrids.Add(nearbyGridsComponent.BottomLeftGrid);
            nearbyGridsComponent.NearbyGrids.Add(nearbyGridsComponent.LeftGrid);
            nearbyGridsComponent.NearbyGrids.Add(nearbyGridsComponent.TopLeftGrid);
            nearbyGridsComponent.NearbyGrids.Add(nearbyGridsComponent.TopGrid);
            nearbyGridsComponent.NearbyGrids.Add(nearbyGridsComponent.TopRightGrid);

            nearbyGridsComponent.MapWidth = w;
            nearbyGridsComponent.MapHeight = h;
            return nearbyGridsComponent.NearbyGrids;
        }
    }
}
