using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        private static uint id = 0;
        public uint ID { get; private set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
            ID = id++;
        }

        public static Point NEInfinity => new Point(Double.PositiveInfinity, Double.PositiveInfinity);

        public static Point SEInfinity => new Point(Double.PositiveInfinity, Double.NegativeInfinity);

        public static Point SWInfinity => new Point(Double.NegativeInfinity, Double.NegativeInfinity);

        public static Point NWInfinity => new Point(Double.NegativeInfinity, Double.PositiveInfinity);

        public bool LiesToTheLeftOf(LineSegment segment) =>
            segment.DirectionFrom(this).IsGreaterThanAndNotAlmostEqualTo(0.0);

        public bool LiesToTheRightOf(LineSegment segment) =>
            segment.DirectionFrom(this).IsLessThanAndNotAlmostEqualTo(0.0);

        public bool LiesOn(LineSegment segment) =>
            segment.RectBoundContains(this) && segment.DirectionFrom(this).IsAlmostEqualTo(0.0);

        public double SquaredDistanceFrom(Point other) =>
            (other.X - X) * (other.X - X) + (other.Y - Y) * (other.Y - Y);

        public double DistanceFrom(Point other) =>
            Math.Sqrt(SquaredDistanceFrom(other));

        public void Translate(double x, double y)
        {
            X += x;
            Y += y;
        }

        public void Translate(Point vector) =>
            Translate(vector.X, vector.Y);

        public Point GetTranslated(double x, double y) =>
            new Point(X + x, Y + y);

        public Point GetTranslated(Point vector) =>
            GetTranslated(vector.X, vector.Y);

        public void ReflectAboutTheXAxis() =>
            Y = -Y;

        public Point GetReflectedAboutTheXAxis() =>
            new Point(X, -Y); 

        public void ReflectAboutTheYAxis() =>
            X = -X;

        public Point GetReflectedAboutTheYAxis() =>
            new Point(-X, Y);

        public bool CoordinatesEqual(Point other) =>
            X.IsAlmostEqualTo(other.X) && Y.IsAlmostEqualTo(other.Y);

        public override string ToString() => $"({X},{Y}) ID={ID}";


        
        public static explicit operator System.Drawing.Point(ComputationalGeometry2D.Point point) =>
            new System.Drawing.Point((int)point.X, (int)point.Y);

        public static explicit operator System.Drawing.PointF(ComputationalGeometry2D.Point point) =>
            new System.Drawing.PointF((float)point.X, (float)point.Y);
            
        public static implicit operator ComputationalGeometry2D.Point(System.Drawing.Point point) =>
            new ComputationalGeometry2D.Point(point.X, point.Y);

        public static implicit operator ComputationalGeometry2D.Point(System.Drawing.PointF point) =>
            new ComputationalGeometry2D.Point(point.X, point.Y);
    }
}
