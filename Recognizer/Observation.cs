using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp
{
    public class Observation
    {
        public string Label { get; private set; }
        public int[] Pixels { get; private set; }

        public Observation(string label, int[] pixels)
        {
            Label = label;
            Pixels = pixels;
        }
    }
}
