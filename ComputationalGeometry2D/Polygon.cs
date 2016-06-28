using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComparingDoubles;
using MoreLinq;

namespace ComputationalGeometry2D
{
    public class Polygon
    {
        public List<Point> Vertices { get; private set; }

        public double MinX() => Vertices.MinBy(p => p.X).X;

        public double MaxX() => Vertices.MaxBy(p => p.X).X;

        public double MinY() => Vertices.MinBy(p => p.Y).Y;

        public double MaxY() => Vertices.MaxBy(p => p.Y).Y;

        public Polygon(IEnumerable<Point> vertices)
        {
            Vertices = new List<Point>();
            AddVertices(vertices);
        }

        public Polygon(IEnumerable<Point> vertices, int verticesCount)
        {
            Vertices = new List<Point>(verticesCount);
            AddVertices(vertices);
        }

        private void AddVertices(IEnumerable<Point> vertices)
        {
            foreach (Point vertex in vertices)
                this.Vertices.Add(vertex);
        }


        public LineSegment[] Edges()
        {
            LineSegment[] edges = new LineSegment[Vertices.Count];
            int verticesCountLessTwo = Vertices.Count - 2;
            for (int i = 0; i <= verticesCountLessTwo; i++)
                edges[i] = new LineSegment(Vertices[i], Vertices[i + 1]);
            edges[edges.Length - 1] = new LineSegment(Vertices[Vertices.Count - 1], Vertices[0]);
            return edges;
        }

        public bool Contains(Point point) // Point in polygon algorithm.
        {
            Line horizontalLine = new Line(0, 1, -point.Y); // Horizontal line passing through the 'point'.
            LineSegment[] edges = Edges();
            int leftIntersectionsAmount = 0;
            int verticesCount = Vertices.Count;
            Line iEdgeLine = new Line(0, 0, 0);
            for (int i = 0; i < verticesCount; i++)
            {
                LineSegment iEdge = edges[i];
                if (iEdge.IntersectsWith(horizontalLine))
                {
                    iEdgeLine.Reinitialize(iEdge);
                    Point intersection = horizontalLine.TryIntersection(iEdgeLine);
                    if (intersection != null) // If iEdgeLine is not horizontal (if lines do not overlap and are not parallel).
                    {
                        if (intersection.X.IsLessThanAndNotAlmostEqualTo(point.X) && !intersection.Y.IsAlmostEqualTo(iEdge.MinY))
                            leftIntersectionsAmount++;
                    }
                }
            }
            return leftIntersectionsAmount % 2 == 1;
        }
    }
}
