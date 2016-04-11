using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    class LineSegment
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public LineSegment() { }

        public LineSegment(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public LineSegment(double startX, double startY, double endX, double endY)
            : this(new Point(startX, startY), new Point(endX, endY)) { }

        public double MinX => Math.Min(Start.X, End.X);

        public double MinY => Math.Min(Start.Y, End.Y);

        public double MaxX => Math.Max(Start.X, End.X);

        public double MaxY => Math.Max(Start.Y, End.Y);

        private double Vector2ProductZValue(Point initial, Point terminal1, Point terminal2) =>
            (terminal1.X - initial.X) * (terminal2.Y - initial.Y) - (terminal2.X - initial.X) * (terminal1.Y - initial.Y);

        public double DirectionFrom(Point point) =>
            Vector2ProductZValue(point, Start, End);

        public bool RectBoundContains(Point point) =>
            MinX.IsLessThanOrAlmostEqualTo(point.X) && MaxX.IsGreaterThanOrAlmostEqualTo(point.X) &&
            MinY.IsLessThanOrAlmostEqualTo(point.Y) && MaxY.IsGreaterThanOrAlmostEqualTo(point.Y);

        public bool RectBoundIntersectsWithRectBoundOf(LineSegment other) =>
            MaxX.IsGreaterThanOrAlmostEqualTo(other.MinX) && MinX.IsLessThanOrAlmostEqualTo(other.MaxX) &&
            MaxY.IsGreaterThanOrAlmostEqualTo(other.MinY) && MinY.IsLessThanOrAlmostEqualTo(other.MaxY);

        public bool IntersectsWith(LineSegment other) =>
            RectBoundIntersectsWithRectBoundOf(other) &&
            (DirectionFrom(other.Start) * DirectionFrom(other.End)).IsLessThanOrAlmostEqualToZero() &&
            (other.DirectionFrom(Start) * other.DirectionFrom(End)).IsLessThanOrAlmostEqualToZero();

        public override string ToString() => $"|{Start}{End}|";
    }
}
