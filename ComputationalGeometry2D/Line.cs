using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComparingDoubles;

namespace ComputationalGeometry2D
{
    internal class Line
    {
        public double A { get; private set; }
        public double B { get; private set; }
        public double C { get; private set; }

        public Line(Point p1, Point p2)
        {
            A = p1.Y - p2.Y;
            B = p2.X - p1.X;
            C = p2.Y * p1.X - p2.X * p1.Y;
        }

        public Line(LineSegment segment) : this(segment.Start, segment.End) { }

        public Point TryIntersection(Line other)
        {
            double detA = A * other.B - other.A * B;
            double detAx = -C * other.B + other.C * B;
            double detAy = -A * other.C + other.A * C;

            if (detA.IsAlmostEqualToZero())
            {
                return null; // No intersection or segments overlap...

                //if (!detAx.IsAlmostEqualToZero() || !detAy.IsAlmostEqualToZero())
                //    return null; // No intersection.
                //return null; // Segments overlap...
            }

            double x = detAx / detA;
            double y = detAy / detA;

            return new Point(x, y);
        }

        public double SubstituteIntoEquation(Point point) =>
            A * point.X + B * point.Y + C;

        public bool Contains(Point point) =>
            SubstituteIntoEquation(point).IsAlmostEqualToZero();

        public void DivideByA()
        {
            if (!A.IsAlmostEqualToZero())
            {
                B = B / A;
                C = C / A;
                A = 1;
            }
        }

        public bool IsHorizontal() =>
            A.IsAlmostEqualToZero();
    }
}
