using System.Collections.Generic;

using ComputationalGeometry2D;
using ComputationalGeometry2D.Common;
using ComparingDoubles;

namespace AlgorithmsTests
{
    class AllPlanePointsAngularIDComparer : Comparer<Point>
    {
        // By default sorting starts from positive X or positive Y axis in countercloskwise direction.

        private Point pole = null;
        private LineSegment segment = new LineSegment();

        private int angularOrderMultiplier = 1;
        private int quadrantMultiplier = 1; 
        private int halfPlaneMultiplier = 1;
        private int idOrderMultiplier = 1;

        private delegate bool comparePoints(Point p1, Point p2);

        private comparePoints liesInEarlierQuadrant = null;
        private comparePoints liesInLaterQuadrant = null;
        private comparePoints liesInEarlierHalfPlane = null;
        private comparePoints liesInLaterHalfPlane = null;

        private delegate bool processPoint(Point point);
        private processPoint liesInSecondBySortOrderHalfPlane = null;

        public AllPlanePointsAngularIDComparer(Point pole, AngularSortStartLocation startLocation, AngularOrder angularOrder, PointsIDOrder pointsIDOrder = PointsIDOrder.Ascending)
        {
            SetPole(pole);
            SetQuadrantMultiplier(startLocation); 
            SetAngularOrderMultiplier(angularOrder); 
            SetHalfPlaneMultiplier();
            SetQuadrantAndHalfPlaneDelegates(startLocation);

            SetIDOrder(pointsIDOrder);
        }

        public void SetPole(Point pole)
        {
            this.pole = pole;
            segment.Start = pole;
        }

        private void SetQuadrantAndHalfPlaneDelegates(AngularSortStartLocation startLocation)
        {
            if (startLocation == AngularSortStartLocation.PositiveX || startLocation == AngularSortStartLocation.NegativeX)
            {
                // SetHorizontalMethods() - virtual
                liesInEarlierQuadrant = LiesInEarlierPositiveYQuadrant;
                liesInLaterQuadrant = LiesInLaterPositiveYQuadrant;

                liesInEarlierHalfPlane = LiesInEarlierHorizontalHalfPlane;
                liesInLaterHalfPlane = LiesInLaterHorizontalHalfPlane;

                if (halfPlaneMultiplier > 0)
                    liesInSecondBySortOrderHalfPlane = LiesInDownHalfPlane;
                else liesInSecondBySortOrderHalfPlane = LiesInUpperHalfPlane;
            }
            else // if (startLocation == AngularSortStartLocation.PositiveY || startLocation == AngularSortStartLocation.NegativeY)
            {
                // SetVerticalMethods() - virtual
                liesInEarlierQuadrant = LiesInEarlierNegativeXQuadrant;
                liesInLaterQuadrant = LiesInLaterNegativeXQuadrant;

                liesInEarlierHalfPlane = LiesInEarlierVerticalHalfPlane;
                liesInLaterHalfPlane = LiesInLaterVerticalHalfPlane;

                if (halfPlaneMultiplier > 0)
                    liesInSecondBySortOrderHalfPlane = LiesInRightHalfPlane;
                else liesInSecondBySortOrderHalfPlane = LiesInLeftHalfPlane;
            }
        }

        private void SetQuadrantMultiplier(AngularSortStartLocation startLocation)
        {
            if (startLocation == AngularSortStartLocation.PositiveX || startLocation == AngularSortStartLocation.PositiveY)
                quadrantMultiplier = 1;
            else // if (startLocation == AngularSortStartLocation.NegativeX || startLocation == AngularSortStartLocation.NegativeY)
                quadrantMultiplier = -1;
        }

        private void SetAngularOrderMultiplier(AngularOrder angularOrder)
        {
            if (angularOrder == AngularOrder.CounterClockwise)
                angularOrderMultiplier = 1;
            else // if (direction == AngularSortDirection.Clockwise)
                angularOrderMultiplier = -1;
        }

        private void SetHalfPlaneMultiplier()
        {
            // Sort starts from earlier (upper or left) halfplane ?
            halfPlaneMultiplier = quadrantMultiplier == angularOrderMultiplier ? 1 : -1;
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
        //                                                                  OrAlmostEqualTo ??

        private bool LiesInLaterPositiveYQuadrant(Point p1, Point p2) =>
            p1.X.IsLessThanAndNotAlmostEqualTo(pole.X) && p2.X.IsGreaterThanAndNotAlmostEqualTo(pole.X);

        private bool LiesInEarlierNegativeXQuadrant(Point p1, Point p2) =>
            p1.Y.IsGreaterThanAndNotAlmostEqualTo(pole.Y) && p2.Y.IsLessThanAndNotAlmostEqualTo(pole.Y);

        private bool LiesInLaterNegativeXQuadrant(Point p1, Point p2) =>
            p1.Y.IsLessThanAndNotAlmostEqualTo(pole.Y) && p2.Y.IsGreaterThanAndNotAlmostEqualTo(pole.Y);


        private bool LiesInEarlierHorizontalHalfPlane(Point p1, Point p2) =>
            p1.Y.IsGreaterThanAndNotAlmostEqualTo(pole.Y) && p2.Y.IsLessThanAndNotAlmostEqualTo(pole.Y);

        private bool LiesInLaterHorizontalHalfPlane(Point p1, Point p2) =>
            p1.Y.IsLessThanAndNotAlmostEqualTo(pole.Y) && p2.Y.IsGreaterThanAndNotAlmostEqualTo(pole.Y);

        private bool LiesInEarlierVerticalHalfPlane(Point p1, Point p2) =>
            p1.X.IsLessThanAndNotAlmostEqualTo(pole.X) && p2.X.IsGreaterThanAndNotAlmostEqualTo(pole.X);

        private bool LiesInLaterVerticalHalfPlane(Point p1, Point p2) =>
            p1.X.IsGreaterThanAndNotAlmostEqualTo(pole.X) && p2.X.IsLessThanAndNotAlmostEqualTo(pole.X);

        private bool LiesInUpperHalfPlane(Point point) =>
            point.Y.IsGreaterThanAndNotAlmostEqualTo(pole.Y);

        private bool LiesInDownHalfPlane(Point point) =>
            point.Y.IsLessThanAndNotAlmostEqualTo(pole.Y);

        private bool LiesInLeftHalfPlane(Point point) =>
            point.X.IsLessThanAndNotAlmostEqualTo(pole.X);

        private bool LiesInRightHalfPlane(Point point) =>
            point.X.IsGreaterThanAndNotAlmostEqualTo(pole.X);

        public override int Compare(Point p1, Point p2)
        {
            if (liesInEarlierHalfPlane(p1, p2))
                return -halfPlaneMultiplier; // -1 * halfPlaneMultiplier;
            else if (liesInLaterHalfPlane(p1, p2))
                return halfPlaneMultiplier; // 1 * halfPlaneMultiplier;

            // points lay in the same half-plane

            int halfPlaneQuadrantMultiplier = 1;
            if (liesInSecondBySortOrderHalfPlane(p1))
                halfPlaneQuadrantMultiplier = -1;

            if (liesInEarlierQuadrant(p1, p2))
                return -quadrantMultiplier * halfPlaneQuadrantMultiplier; // -1 * quadrantMultiplier * halfPlaneQuadrantMultiplier
            else if (liesInLaterQuadrant(p1, p2))
                return quadrantMultiplier * halfPlaneQuadrantMultiplier; // 1 * quadrantMultiplier * halfPlaneQuadrantMultiplier

            // points lay in the same quadrant

            segment.End = p1;
            Orientation orientation = p2.OrientationTest(segment);
            if (orientation == Orientation.Right)
                return angularOrderMultiplier; // 1 * directionMultiplier
            else if (orientation == Orientation.Left)
                return -angularOrderMultiplier; // -1 * directionMultiplier

            // points are collinear

            double p1SqrdRadious = pole.SquaredDistanceFrom(p1);
            double p2SqrdRadious = pole.SquaredDistanceFrom(p2);
            if (p1SqrdRadious.IsAlmostEqualTo(p2SqrdRadious))
                return p1.ID.CompareTo(p2.ID) * idOrderMultiplier;
            else return p1SqrdRadious.CompareTo(p2SqrdRadious);
        }
    }
}
