using System.Collections.Generic;

using ComparingDoubles;
using ComputationalGeometry2D;
using ComputationalGeometry2D.Common;

namespace AlgorithmsTests.Testonly
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

        public HalfPlanePointsAngularIDComparer(Point pole, AngularSortStartLocation startLocation, AngularOrder angularOrder, PointsIDOrder pointsIDOrder = PointsIDOrder.Ascending)
        {
            SetPole(pole);
            SetSortStartLocation(startLocation);
            SetSortDirection(angularOrder);
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

        public void SetSortDirection(AngularOrder angularOrder)
        {
            if (angularOrder == AngularOrder.CounterClockwise)
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
            if (liesInEarlierQuadrant(p1, p2))
                return -quadrantMultiplier; // -1 * quadrantMultiplier
            if (liesInLaterQuadrant(p1, p2))
                return quadrantMultiplier; // 1 * quadrantMultiplier

            // brakuje tu 
            //int halfPlaneQuadrantMultiplier = 1;
            //if (liesInSecondBySortOrderHalfPlane(p1))
            //    halfPlaneQuadrantMultiplier = -1;

            // quadrantMultiplier = 1 wtedy, gdy zaczynamy sortowac od positiveX/Y i jestesmy w pierwszej polowce sortowania
            //     lub gdy zaczynamy sortowac od negativeX/Y i jestemy w drugiej polowce sortowania
            // quadrantMultiplier = -1 wtedy, gdy zaczynamy sortowac od negativeX/Y i jestemy w pierwszej polowce sortowania
            //     lub gdy zaczynamy od positiveX/Y i jestesmy w drugiej polowce sortowania
            // przeslac numer polowki sortowania do konstruktora ??

            // points lay in the same quadrant

            segment.End = p1;
            Orientation orientation = p2.OrientationTest(segment);
            if (orientation == Orientation.Right)
                return directionMultiplier; // 1 * directionMultiplier
            if (orientation == Orientation.Left)
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
