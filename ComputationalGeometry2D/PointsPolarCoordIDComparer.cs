using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    class PointsPolarCoordIDComparer : Comparer<Point>
    {
        private Point pole = null;
        private LineSegment segment = new LineSegment();

        public PointsPolarCoordIDComparer() : this(new Point(0.0, 0.0))
        { }

        public PointsPolarCoordIDComparer(Point pole)
        {
            this.pole = pole;
        }

        public override int Compare(Point p1, Point p2)
        {
            segment.Start = pole;
            segment.End = p1;
            OrientationTestResult orientation = p2.OrientationTest(segment);
            if (orientation == OrientationTestResult.Right)
                return 1;
            else if (orientation == OrientationTestResult.Left)
                return -1;
            else
            {
                double p1SqrdRadious = pole.SquaredDistanceFrom(p1);
                double p2SqrdRadious = pole.SquaredDistanceFrom(p2);
                if (p1SqrdRadious.IsAlmostEqualTo(p2SqrdRadious))
                    return p1.ID.CompareTo(p2.ID);
                else return p1SqrdRadious.CompareTo(p2SqrdRadious);
            }
        }
    }
}
