using System;

using ComparingDoubles;


namespace ComputationalGeometry2D
{
    public class LineSegment
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        internal LineSegment() { }

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

        public double DeltaX => End.X - Start.X;

        public double DeltaY => End.Y - Start.Y;

        public bool IsHorizontal() =>
            DeltaY.IsAlmostEqualToZero();

        public bool IsVertical() =>
            DeltaX.IsAlmostEqualToZero();

        public void SwapEnds()
        {
            Point temp = Start;
            Start = End;
            End = temp;
        }

        public static double Vector2ProductZValue(Point initial, Point terminal1, Point terminal2) =>
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

        public Point TryIntersection(LineSegment other)
        {
            if (!this.IntersectsWith(other))
                return null;

            Line l1 = new Line(this);
            Line l2 = new Line(other);

            return l1.TryIntersection(l2);
        }

        public bool IntersectsWith(Line line) =>
            (line.SubstituteIntoEquation(Start) * line.SubstituteIntoEquation(End)).IsLessThanOrAlmostEqualToZero();

        private static double Length(double deltaX, double deltaY) =>
            Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

        public double Length() =>
            Length(DeltaX, DeltaY);

        private static double ScalarProduct(double deltaX1, double deltaY1, double deltaX2, double deltaY2) =>
            deltaX1 * deltaX2 + deltaY1 * deltaY2;

        public double ScalarProduct(LineSegment other) =>
            ScalarProduct(DeltaX, DeltaY, other.DeltaX, other.DeltaY);

        public double AngleRadians(LineSegment other)
        {
            double deltaX = DeltaX;
            double deltaY = DeltaY;

            double otherDeltaX = other.DeltaX;
            double otherDeltaY = other.DeltaY;

            double scalarProduct = ScalarProduct(deltaX, deltaY, otherDeltaX, otherDeltaY);

            double length = Length(deltaX, deltaY);
            double otherLength = Length(otherDeltaX, otherDeltaY);

            double angleCosine = scalarProduct / (length * otherLength);

            return Math.Acos(angleCosine);
        }

        public bool CoordinatesEqual(LineSegment other) =>
            Start.CoordinatesEqual(other.Start) && End.CoordinatesEqual(other.End);

        public override string ToString() => $"|{Start}{End}|";
    }
}
