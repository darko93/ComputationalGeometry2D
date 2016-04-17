using System;using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    class PointsAngularIDComparer : Comparer<Point>
    {
        // By default sorting starts from positive X or positive Y axis in countercloskwise direction.

        private Point pole = null;
        private LineSegment segment = new LineSegment();

        private int directionMultiplier = 1;
        private int quadrantMultiplier = 1; // Needed only to calculate halfPlaneMultiplier 
        private int halfPlaneMultiplier = 1;
        private int idOrderMultiplier = 1;

        private delegate bool comparePoints(Point p1, Point p2);

        private comparePoints liesInEarlierQuadrant = null;
        private comparePoints liesInLaterQuadrant = null;
        private comparePoints liesInEarlierHalfPlane = null;
        private comparePoints liesInLaterHalfPlane = null;

        public PointsAngularIDComparer(Point pole, AngularSortStartLocation startLocation, AngularSortDirection direction, PointsIDOrder pointsIDOrder = PointsIDOrder.Ascending)
        {
            SetPole(pole);
            SetOnlySortStartLocation(startLocation); // base.SetSortStartLocation(startLocation);
            SetOnlySortDirection(direction) ;// base.SetOnlySortDirection(direction);
            SetHalfPlaneMultiplier();
            SetIDOrder(pointsIDOrder);

            // or just base(pole, startLocation, direction, pointsIDOrder); SetHalfPlaneMultiplier();
            // if it will call reight method...
        }

        public void SetPole(Point pole)
        {
            this.pole = pole;
            segment.Start = pole;
        }

        private void SetOnlySortStartLocation(AngularSortStartLocation startLocation)
        {
            if (startLocation == AngularSortStartLocation.PositiveX || startLocation == AngularSortStartLocation.PositiveY)
                quadrantMultiplier = 1;
            else // if (startLocation == AngularSortStartLocation.NegativeX || startLocation == AngularSortStartLocation.NegativeY)
                quadrantMultiplier = -1;

            if (startLocation == AngularSortStartLocation.PositiveX || startLocation == AngularSortStartLocation.NegativeX)
            {
                // SetHorizontalMethods() - virtual
                liesInEarlierQuadrant = LiesInEarlierPositiveYQuadrant;
                liesInLaterQuadrant = LiesInLaterPositiveYQuadrant;

                liesInEarlierHalfPlane = LiesInEarlierHorizontalHalfPlane;
                liesInLaterHalfPlane = LiesInLaterHorizontalHalfPlane;
            }
            else // if (startLocation == AngularSortStartLocation.PositiveY || startLocation == AngularSortStartLocation.NegativeY)
            {
                // SetVerticalMethods() - virtual
                liesInEarlierQuadrant = LiesInEarlierNegativeXQuadrant;
                liesInLaterQuadrant = LiesInLaterNegativeXQuadrant;

                liesInEarlierHalfPlane = LiesInEarlierVerticalHalfPlane;
                liesInLaterHalfPlane = LiesInLaterHorizontalHalfPlane;
            }
        }

        //private override void SetHorizontalMethods()
        //{
        //    base.SetHorizontalMethods();
        //    liesInEarlierHalfPlane = LiesInEarlierHorizontalHalfPlane;
        //    liesInLaterHalfPlane = LiesInLaterHorizontalHalfPlane;
        //}
        
        private void SetOnlySortDirection(AngularSortDirection direction)
        {
            if (direction == AngularSortDirection.CounterClockwise)
                directionMultiplier = 1;
            else // if (direction == AngularSortDirection.Clockwise)
                directionMultiplier = -1;
        }

        private void SetHalfPlaneMultiplier()
        {
            // If sort starts from earlier (upper or left) halfplane...
            halfPlaneMultiplier = quadrantMultiplier == directionMultiplier ? 1 : -1;
        }

        public /*override*/ void SetSortStartLocation(AngularSortStartLocation startLocation)
        {
            SetOnlySortStartLocation(startLocation); // base.SetSortStartLocation(startLocation);
            SetHalfPlaneMultiplier();
        }

        public /*override*/ void SetSortDirection(AngularSortDirection direction)
        {
            SetOnlySortDirection(direction); // base.SetOnlySortDirection(direction);
            SetHalfPlaneMultiplier();
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


        private bool LiesInEarlierHorizontalHalfPlane(Point p1, Point p2) =>
            p1.Y.IsGreaterThanAndNotAlmostEqualTo(pole.Y) && p2.Y.IsLessThanAndNotAlmostEqualTo(pole.Y);

        private bool LiesInLaterHorizontalHalfPlane(Point p1, Point p2) =>
            p1.Y.IsLessThanAndNotAlmostEqualTo(pole.Y) && p2.Y.IsGreaterThanAndNotAlmostEqualTo(pole.Y);

        private bool LiesInEarlierVerticalHalfPlane(Point p1, Point p2) =>
            p1.X.IsLessThanAndNotAlmostEqualTo(pole.X) && p2.X.IsGreaterThanAndNotAlmostEqualTo(pole.X);

        private bool LiesInLaterVerticalHalfPlane(Point p1, Point p2) =>
            p1.X.IsGreaterThanAndNotAlmostEqualTo(pole.X) && p2.X.IsLessThanAndNotAlmostEqualTo(pole.X);

        public override int Compare(Point p1, Point p2)
        {
            int result;

            // if LiesInEarlierHalfPlane(p1, p2)
            //     result = -1 * halfPlaneMultiplier;
            // else if LiesInLaterHalfPlane(p1, p2)
            //     result = 1 * halfPlaneMultiplier;
            // else
            // {
            //     // points lay in the same half-plane
            //     ...

            if (liesInEarlierQuadrant(p1, p2))
                result = -1 * quadrantMultiplier;
            else if (liesInLaterQuadrant(p1, p2))
                result = 1 * quadrantMultiplier;

            else
            {
                segment.End = p1;
                OrientationTestResult orientation = p2.OrientationTest(segment);
                if (orientation == OrientationTestResult.Right)
                    result = 1;
                else if (orientation == OrientationTestResult.Left)
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
