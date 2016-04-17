using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    class HalfPlanePointsAngularIDComparer : Comparer<Point>
    {
        // By default sorting starts from positive X or positive Y axis in countercloskwise direction.

        private LineSegment segment = new LineSegment();
        Point pole;

        private int idOrderMultiplier = 1;
        private int directionMultiplier = 1;
        private int quadrantMultiplier = 1;

        private delegate bool comparePoints(Point p1, Point p2);

        private comparePoints liesInEarlierQuadrant = null;
        private comparePoints liesInLaterQuadrant = null;

        public HalfPlanePointsAngularIDComparer(Point pole, AngularSortStartLocation startLocation, AngularSortDirection direction, PointsIDOrder pointsIDOrder = PointsIDOrder.Ascending)
        {
            SetPole(pole);
            SetSortStartLocation(startLocation);
            SetSortDirection(direction);
            SetIDOrder(pointsIDOrder);
        }

        public void SetPole(Point pole)
        {
            this.pole = pole;
            segment.Start = pole;
        }

        public void SetSortStartLocation(AngularSortStartLocation startLocation)
        {
            if (startLocation == AngularSortStartLocation.PositiveX || startLocation == AngularSortStartLocation.PositiveY)
                quadrantMultiplier = 1;
            else // if (startLocation == AngularSortStartLocation.NegativeX || startLocation == AngularSortStartLocation.NegativeY)
                quadrantMultiplier = -1;

            if (startLocation == AngularSortStartLocation.PositiveX || startLocation == AngularSortStartLocation.NegativeX)
            {
                liesInEarlierQuadrant = LiesInEarlierPositiveYQuadrant;
                liesInLaterQuadrant = LiesInLaterPositiveYQuadrant;
            }
            else // if (startLocation == AngularSortStartLocation.PositiveY || startLocation == AngularSortStartLocation.NegativeY)
            {
                liesInEarlierQuadrant = LiesInEarlierNegativeXQuadrant;
                liesInLaterQuadrant = LiesInLaterNegativeXQuadrant;
            }
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

        private bool LiesInEarlierPositiveYQuadrant(Point p1, Point p2) =>
            p1.X.IsGreaterThanAndNotAlmostEqualTo(pole.X) && p2.X.IsLessThanAndNotAlmostEqualTo(pole.X);

        private bool LiesInLaterPositiveYQuadrant(Point p1, Point p2) =>
            p1.X.IsLessThanAndNotAlmostEqualTo(pole.X) && p2.X.IsGreaterThanAndNotAlmostEqualTo(pole.X);

        private bool LiesInEarlierNegativeXQuadrant(Point p1, Point p2) =>
            p1.Y.IsGreaterThanAndNotAlmostEqualTo(pole.Y) && p2.Y.IsLessThanAndNotAlmostEqualTo(pole.Y);

        private bool LiesInLaterNegativeXQuadrant(Point p1, Point p2) =>
            p1.Y.IsLessThanAndNotAlmostEqualTo(pole.Y) && p2.Y.IsGreaterThanAndNotAlmostEqualTo(pole.Y);


        public override int Compare(Point p1, Point p2)
        {
            int result;
            if (liesInEarlierQuadrant(p1, p2))
                result = -1 * quadrantMultiplier;
            else if (liesInLaterQuadrant(p1, p2))
                result = 1 * quadrantMultiplier;
            else
            {
                segment.End = p1;
                OrientationTestResult p2Orientation = p2.OrientationTest(segment);
                if (p2Orientation == OrientationTestResult.Right)
                    result = 1;
                else if (p2Orientation == OrientationTestResult.Left)
                    result = -1;
                else
                {
                    double p1SqrdRadious = pole.SquaredDistanceFrom(p1);
                    double p2SqrdRadious = pole.SquaredDistanceFrom(p2);
                    if (p1SqrdRadious.IsAlmostEqualTo(p2SqrdRadious))
                        return p1.ID.CompareTo(p2.ID) * idOrderMultiplier;
                    else result = p1SqrdRadious.CompareTo(p2SqrdRadious);
                }
            }
            result *= directionMultiplier;
            return result;
        }
    }
}
