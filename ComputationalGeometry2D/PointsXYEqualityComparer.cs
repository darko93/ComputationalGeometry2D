using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    class PointsXYEqualityComparer : EqualityComparer<Point>
    {
        public override bool Equals(Point p1, Point p2) =>
            p1.CoordinatesEqual(p2);

        public override int GetHashCode(Point point)
        {
            unchecked // Overflow is fine
            {
                int hash = 17;
                hash = hash * 23 + point.X.GetHashCode();
                hash = hash * 23 + point.Y.GetHashCode();
                return hash;
            }
        }
    }
}
