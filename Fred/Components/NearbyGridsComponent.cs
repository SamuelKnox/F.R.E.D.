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
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
        public int CurrentIndex
        {
            get{
                return (int) (this.CurrentGrid.X * this.MapHeight + this.CurrentGrid.Y);
            }
        }
        public int RightIndex{
            get
            {
                return (int)(this.RightGrid.X * this.MapHeight + this.RightGrid.Y);
            }
        }
        public int BottomRightIndex{
            get
            {
                return (int)(this.BottomRightGrid.X * this.MapHeight + this.BottomRightGrid.Y);
            }
        }
        public int BottomIndex{
            get
            {
                return (int)(this.BottomGrid.X * this.MapHeight + this.BottomGrid.Y);
            }
        }
        public int BottomLeftIndex{
            get
            {
                return (int)(this.BottomLeftGrid.X * this.MapHeight + this.BottomLeftGrid.Y);
            }
        }
        public int LeftIndex{
            get
            {
                return (int)(this.LeftGrid.X * this.MapHeight + this.LeftGrid.Y);
            }
        }
        public int TopLeftIndex{
            get
            {
                return (int)(this.TopLeftGrid.X * this.MapHeight + this.TopLeftGrid.Y);
            }
        }
        public int TopIndex{
            get
            {
                return (int)(this.TopGrid.X * this.MapHeight + this.TopGrid.Y);
            }
        }
        public int TopRightIndex{
            get
            {
                return (int)(this.TopRightGrid.X * this.MapHeight + this.TopRightGrid.Y);
            }
        }

        public List<int> NearbyIndices
        {
            get
            {
                List<int> indices = new List<int>();
                indices.Add(this.CurrentIndex);
                indices.Add(this.RightIndex);
                indices.Add(this.BottomRightIndex);
                indices.Add(this.BottomIndex);
                indices.Add(this.BottomLeftIndex);
                indices.Add(this.LeftIndex);
                indices.Add(this.TopLeftIndex);
                indices.Add(this.TopIndex);
                indices.Add(this.TopRightIndex);
                return indices;
            }
        }
        public NearbyGridsComponent()
        {
            
        }
        
        public NearbyGridsComponent(Vector2 current, int w, int h)
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

            this.MapWidth = w;
            this.MapHeight = h;
        }
    }
}
