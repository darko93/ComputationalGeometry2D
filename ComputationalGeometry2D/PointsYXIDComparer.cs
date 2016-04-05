using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    class PointsYXIDComparer : Comparer<Point>
    {
        public override int Compare(Point p1, Point p2)
        {
            if (p1.Y.IsAlmostEqualTo(p2.Y))
            {
                if (p1.X.IsAlmostEqualTo(p2.X))
                    return p1.ID.CompareTo(p2.ID);
                else return p1.X.CompareTo(p2.X);
            }
            else return p1.Y.CompareTo(p2.Y);
        }
    }
}
