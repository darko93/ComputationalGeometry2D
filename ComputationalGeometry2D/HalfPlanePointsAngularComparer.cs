using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    class HalfPlanePointsAngularComparer : Comparer<Point>
    {
        // Default sorting starts from positive X or positive Y axis in countercloskwise direction

        private LineSegment segment = new LineSegment();
        Point pole;

        private int directionMultiplier = 1;
        private int quadrantMultiplier = 1;

        private delegate bool comparePoints(Point p1, Point p2);

        private comparePoints liesInEarlierQuadrant = null;
        private comparePoints liesInLaterQuadrant = null;

        public HalfPlanePointsAngularComparer(Point pole, AngularSortStartLocation startLocation, AngularSortDirection direction)
        {
            this.pole = pole;
            segment.Start = pole;

            if (startLocation == AngularSortStartLocation.NegativeX || startLocation == AngularSortStartLocation.NegativeY)
                quadrantMultiplier = -1;

            if (direction == AngularSortDirection.Clockwise)
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
