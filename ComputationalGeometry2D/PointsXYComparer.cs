using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    class PointsXYComparer : Comparer<Point>
    {
        public override int Compare(Point p1, Point p2)
        {
            if (p1.X.IsAlmostEqualTo(p2.X))
            {
                if (p1.Y.IsAlmostEqualTo(p2.Y))
                    return 0;
                else return p1.Y.CompareTo(p2.Y);
            }
            else return p1.X.CompareTo(p2.X);
        }
    }
}
