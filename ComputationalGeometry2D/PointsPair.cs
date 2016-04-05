using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    class PointsPair
    {
        public Point First { get; private set; }
        public Point Second { get; private set; }
        public PointsPair(Point first, Point second)
        {
            First = first;
            Second = second;
        } 
    }
}
