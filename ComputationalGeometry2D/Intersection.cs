using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    public class Intersection
    {
        public Point Point { get; internal set; }
        public LineSegment[] Segments { get; internal set; }

        public Intersection(Point point, LineSegment[] segments)
        {
            Point = point;
            Segments = segments;
        }
    }
}
