using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mascape
{
    class Attribute
    {
        public double Current { get; set; }
        public double Maximum { get; set; }

                public Attribute(double max)
        {
            Current = max;
            Maximum = max;
        }
    }
}
