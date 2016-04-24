using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComparingDoubles;

namespace ComputationalGeometry2D
{
    class PointsAngularByOrientationIDComparer : Comparer<Point>
    {
        private Point pole = null;
        private LineSegment segment = new LineSegment();

        private int idOrderMultiplier = 1;
        private int directionMultiplier = 1;

        public PointsAngularByOrientationIDComparer(Point pole, AngularSortDirection direction, PointsIDOrder pointsIDOrder = PointsIDOrder.Ascending)
        {
            SetPole(pole);
            SetSortDirection(direction);
            SetIDOrder(pointsIDOrder);
        }

        public void SetPole(Point pole)
        {
            this.pole = pole;
            segment.Start = pole;
        }


        public void SetSortDirection(AngularSortDirection direction)
        {
            if (direction == AngularSortDirection.CounterClockwise)
                directionMultiplier = 1;
            else // if (direction == AngularSortDirection.Clockwise)
                directionMultiplier = -1;
        }

        public void SetIDOrder(PointsIDOrder pointsIDOrder)
        {
            if (pointsIDOrder == PointsIDOrder.Ascending)
                idOrderMultiplier = 1;
            else // if (pointsIDOrder == PointsIDOrder.Descending)
                idOrderMultiplier = -1;
        }

        public override int Compare(Point p1, Point p2)
        {
            segment.End = p1;
            OrientationTestResult orientation = p2.OrientationTest(segment);
            if (orientation == OrientationTestResult.Right)
                return directionMultiplier; // 1 * directionMultiplier
            if (orientation == OrientationTestResult.Left)
                return -directionMultiplier; // -1 * directionMultiplier

            // points are collinear

            double p1SqrdRadious = pole.SquaredDistanceFrom(p1);
            double p2SqrdRadious = pole.SquaredDistanceFrom(p2);
            if (p1SqrdRadious.IsAlmostEqualTo(p2SqrdRadious))
                return p1.ID.CompareTo(p2.ID) * idOrderMultiplier;
            return p1SqrdRadious.CompareTo(p2SqrdRadious);
        }
    }
}
