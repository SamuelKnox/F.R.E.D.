using Artemis.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Components
{
    class MazeComponent : IComponent
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
    }
}
