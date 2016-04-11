using System;using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    class PointsAngularComparer : Comparer<Point>
    {
        // Default sorting starts from positive X or positive Y axis in countercloskwise direction

        private Point pole = null;
        private LineSegment segment = new LineSegment();

        private int directionMultiplier = 1;
        private int quadrantMultiplier = 1;

        private delegate bool comparePoints(Point p1, Point p2);

        private comparePoints liesInEarlierQuadrant = null;
        private comparePoints liesInLaterQuadrant = null;
        private comparePoints liesInEarlierHalfPlane = null;
        private comparePoints liesInLaterHalfPlane = null;

        public PointsAngularComparer(Point pole, AngularSortStartLocation startLocation, AngularSortDirection direction)
        {
            this.pole = pole;
            segment.Start = pole;

            if (startLocation == AngularSortStartLocation.NegativeX || startLocation == AngularSortStartLocation.NegativeY)
                quadrantMultiplier = -1;

            if (direction == AngularSortDirection.CounterClockwise)
                directionMultiplier = -1;

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

            //if ((startLocation == AngularSortStartLocation.PositiveX && direction == AngularSortDirection.Clockwise)
            // || (startLocation == AngularSortStartLocation.NegativeX && direction == AngularSortDirection.CounterClockwise))
            //    liesInEarlierHalfPlane = LiesInEarlierHorizontalHalfPlane;
            // ...
            // also set halfPlanMultiplier here
        }
        
        private bool LiesInEarlierPositiveYQuadrant(Point p1, Point p2) =>
            p1.X.IsGreaterThanAndNotAlmostEqualToZero() && p2.X.IsLessThanAndNotAlmostEqualToZero();

        private bool LiesInLaterPositiveYQuadrant(Point p1, Point p2) =>
            p1.X.IsLessThanAndNotAlmostEqualToZero() && p2.X.IsGreaterThanAndNotAlmostEqualToZero();

        private bool LiesInEarlierNegativeXQuadrant(Point p1, Point p2) =>
            p1.Y.IsGreaterThanAndNotAlmostEqualToZero() && p2.Y.IsLessThanAndNotAlmostEqualToZero();

        private bool LiesInLaterNegativeXQuadrant(Point p1, Point p2) =>
            p1.Y.IsLessThanAndNotAlmostEqualToZero() && p2.Y.IsGreaterThanAndNotAlmostEqualToZero();


        private bool LiesInEarlierHorizontalHalfPlane(Point p1, Point p2) =>
            p1.Y.IsGreaterThanAndNotAlmostEqualToZero() && p2.Y.IsLessThanAndNotAlmostEqualToZero();

        private bool LiesInLaterHorizontalHalfPlane(Point p1, Point p2) =>
            p1.Y.IsLessThanAndNotAlmostEqualToZero() && p2.Y.IsGreaterThanAndNotAlmostEqualToZero();

        private bool LiesInEarlierVerticalHalfPlane(Point p1, Point p2) =>
            p1.X.IsLessThanAndNotAlmostEqualToZero() && p2.X.IsGreaterThanAndNotAlmostEqualToZero();

        private bool LiesInLaterVerticalHalfPlane(Point p1, Point p2) =>
            p1.X.IsGreaterThanAndNotAlmostEqualToZero() && p2.X.IsLessThanAndNotAlmostEqualToZero();

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
                        result = 0;
                    else result = p1SqrdRadious.CompareTo(p2SqrdRadious);
                }
            }
            result *= directionMultiplier;
            return result;
        }
    }
}
