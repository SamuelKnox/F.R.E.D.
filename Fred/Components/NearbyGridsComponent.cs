using Artemis.Interface;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Components
{
    class NearbyGridsComponent : IComponent
    {
        public List<Vector2> NearbyGrids { get; set; }
        public Vector2 CurrentGrid { get; set; }
        public Vector2 RightGrid { get; set; }
        public Vector2 BottomRightGrid { get; set; }
        public Vector2 BottomGrid { get; set; }
        public Vector2 BottomLeftGrid { get; set; }
        public Vector2 LeftGrid { get; set; }
        public Vector2 TopLeftGrid { get; set; }
        public Vector2 TopGrid { get; set; }
        public Vector2 TopRightGrid { get; set; }

        public NearbyGridsComponent(Vector2 current)
        {
            this.CurrentGrid = current;
            this.RightGrid = new Vector2(current.X + 1, current.Y);
            this.BottomRightGrid = new Vector2(current.X + 1, current.Y + 1);
            this.BottomGrid = new Vector2(current.X, current.Y + 1);
            this.BottomLeftGrid = new Vector2(current.X - 1, current.Y + 1);
            this.LeftGrid = new Vector2(current.X - 1, current.Y);
            this.TopLeftGrid = new Vector2(current.X - 1, current.Y - 1);
            this.TopGrid = new Vector2(current.X, current.Y - 1);
            this.TopRightGrid = new Vector2(current.X + 1, current.Y - 1);
            this.NearbyGrids = new List<Vector2>();

            this.NearbyGrids.Add(CurrentGrid);
            this.NearbyGrids.Add(RightGrid);
            this.NearbyGrids.Add(BottomRightGrid);
            this.NearbyGrids.Add(BottomGrid);
            this.NearbyGrids.Add(BottomLeftGrid);
            this.NearbyGrids.Add(LeftGrid);
            this.NearbyGrids.Add(TopLeftGrid);
            this.NearbyGrids.Add(TopGrid);
            this.NearbyGrids.Add(TopRightGrid);
        }
    }
}
