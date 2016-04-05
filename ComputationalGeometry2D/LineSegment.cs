using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    class LineSegment
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public LineSegment() { }

        public LineSegment(Point start, Point end)
        {
            StartPoint = start;
            EndPoint = end;
        }

        public LineSegment(double startX, double startY, double endX, double endY)
            : this(new Point(startX, startY), new Point(endX, endY)) { }

        public double MinX => Math.Min(StartPoint.X, EndPoint.X);

        public double MinY => Math.Min(StartPoint.Y, EndPoint.Y);

        public double MaxX => Math.Max(StartPoint.X, EndPoint.X);

        public double MaxY => Math.Max(StartPoint.Y, EndPoint.Y);

        private double Vector2ProductZValue(Point initial, Point terminal1, Point terminal2) =>
            (terminal1.X - initial.X) * (terminal2.Y - initial.Y) - (terminal2.X - initial.X) * (terminal1.Y - initial.Y);

        public double DirectionFrom(Point point) =>
            Vector2ProductZValue(point, StartPoint, EndPoint);

        public bool RectBoundContains(Point point) =>
            MinX.IsLessThanOrAlmostEqualTo(point.X) && MaxX.IsGreaterThanOrAlmostEqualTo(point.X) &&
            MinY.IsLessThanOrAlmostEqualTo(point.Y) && MaxY.IsGreaterThanOrAlmostEqualTo(point.Y);

        public bool RectBoundIntersectsWithRectBoundOf(LineSegment other) =>
            MaxX.IsGreaterThanOrAlmostEqualTo(other.MinX) && MinX.IsLessThanOrAlmostEqualTo(other.MaxX) &&
            MaxY.IsGreaterThanOrAlmostEqualTo(other.MinY) && MinY.IsLessThanOrAlmostEqualTo(other.MaxY);

        public OrientationTestResult OrientationTest(Point point)
        {
            double direction = DirectionFrom(point);
            if (direction.IsAlmostEqualTo(0.0))
            {
                if (RectBoundContains(point)) return OrientationTestResult.On;
                else return OrientationTestResult.Collinear;
            }
            else if (direction > 0.0) return OrientationTestResult.Left;
            else return OrientationTestResult.Right;
        }

        public bool IntersectsWith(LineSegment other) =>
            RectBoundIntersectsWithRectBoundOf(other) &&
            (DirectionFrom(other.StartPoint) * DirectionFrom(other.EndPoint)).IsLessThanOrAlmostEqualTo(0.0) &&
            (other.DirectionFrom(StartPoint) * other.DirectionFrom(EndPoint)).IsLessThanOrAlmostEqualTo(0.0);

        public override string ToString() => $"|{StartPoint}{EndPoint}|";
    }
}
