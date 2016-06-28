using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComparingDoubles;

namespace ComputationalGeometry2D
{
    public class Triangle
    {
        public Point V1 { get; set; }
        public Point V2 { get; set; }
        public Point V3 { get; set; }

        public Triangle(Point v1, Point v2, Point v3)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
        }

        public LineSegment[] Edges() =>
            new[]
                {
                    new LineSegment(V1, V2),
                    new LineSegment(V2, V3),
                    new LineSegment(V3, V1),
                };
        

        private static Point circumcircleCenter = new Point(0.0, 0.0);

        public bool ContainsInCircumcircle(Point point)
        {
            double v1XSqrd = V1.X * V1.X;
            double v1YSqrd = V1.Y * V1.Y;
            double v2XSqrd = V2.X * V2.X;
            double v2YSqrd = V2.Y * V2.Y;
            double v3XSqrd = V3.X * V3.X;
            double v3YSqrd = V3.Y * V3.Y;

            circumcircleCenter.X = 0.5 * ((v2XSqrd * V3.Y + v2YSqrd * V3.Y - v1XSqrd * V3.Y + v1XSqrd * V2.Y - v1YSqrd * V3.Y + v1YSqrd * V2.Y + V1.Y * v3XSqrd + V1.Y * v3YSqrd - V1.Y * v2XSqrd - V1.Y * v2YSqrd - V2.Y * v3XSqrd - V2.Y * v3YSqrd) / (V1.Y * V3.X - V1.Y * V2.X - V2.Y * V3.X - V3.Y * V1.X + V3.Y * V2.X + V2.Y * V1.X));
            circumcircleCenter.Y = 0.5 * ((-V1.X * v3XSqrd - V1.X * v3YSqrd + V1.X * v2XSqrd + V1.X * V2.Y + V2.X * v3XSqrd + V2.X * v3YSqrd - v2XSqrd * V3.X - v2YSqrd * V3.X + v1XSqrd * V3.X - v1XSqrd * V2.X + v1YSqrd * V3.X - v1YSqrd * V2.X) / (V1.Y * V3.X - V1.Y * V2.X - V2.Y * V3.X - V3.Y * V1.X + V3.Y * V2.X + V2.Y * V1.X));

            double radious = circumcircleCenter.DistanceFrom(V1);

            return circumcircleCenter.DistanceFrom(point).IsLessThanOrAlmostEqualTo(radious);
        }

        public bool SharesVertexWith(Triangle other) =>
            V1.CoordinatesEqual(other.V1) || V1.CoordinatesEqual(other.V2) || V1.CoordinatesEqual(other.V3) ||
            V2.CoordinatesEqual(other.V1) || V2.CoordinatesEqual(other.V2) || V2.CoordinatesEqual(other.V3) ||
            V3.CoordinatesEqual(other.V1) || V3.CoordinatesEqual(other.V2) || V3.CoordinatesEqual(other.V3);
    }
}
