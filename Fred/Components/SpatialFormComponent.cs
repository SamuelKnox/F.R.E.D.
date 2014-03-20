using Artemis.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Components
{
    class SpatialFormComponent : IComponent
    {
        public string SpatialFormFile { get; set; }

        public SpatialFormComponent()
            : this(string.Empty)
        {
        }

        public SpatialFormComponent(string spatialFormFile)
        {
            this.SpatialFormFile = spatialFormFile;
        }
    }
}
