using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComparingDoubles;

namespace ComputationalGeometry2D
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        private static uint id = 0;
        internal uint ID { get; private set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
            ID = id++;
        }

        public bool LiesToTheLeftOf(LineSegment segment) =>
            segment.DirectionFrom(this).IsGreaterThanAndNotAlmostEqualToZero();

        public bool LiesToTheRightOf(LineSegment segment) =>
            segment.DirectionFrom(this).IsLessThanAndNotAlmostEqualToZero();

        public bool LiesOn(LineSegment segment) =>
            segment.RectBoundContains(this) && segment.DirectionFrom(this).IsAlmostEqualToZero();

        public double SquaredDistanceFrom(Point other) =>
            (other.X - X) * (other.X - X) + (other.Y - Y) * (other.Y - Y);

        public double DistanceFrom(Point other) =>
            Math.Sqrt(SquaredDistanceFrom(other));

        public Orientation OrientationTest(LineSegment segment)
        {
            double direction = segment.DirectionFrom(this);
            if (direction.IsAlmostEqualToZero())
            {
                if (segment.RectBoundContains(this)) return Orientation.On;
                return Orientation.Collinear;
            }
            if (direction > 0.0) return Orientation.Left;
            return Orientation.Right;
        }

        public void Translate(double x, double y)
        {
            X += x;
            Y += y;
        }

        public bool CoordinatesEqual(Point other) =>
            X.IsAlmostEqualTo(other.X) && Y.IsAlmostEqualTo(other.Y);

        public override string ToString() =>
            //#if DEBUG
            //$"({X},{Y}) ID={ID}";
            //#else
            $"({X},{Y})";
            //#endif
    }
}
