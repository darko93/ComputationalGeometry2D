using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComparingDoubles;

namespace ComputationalGeometry2D
{
    internal sealed class PointsToQuadrantsAdder : HalfPlanePointsToQuadrantsAdder
    {
        public List<Point> ThirdBySortOrderQuadrant { get; private set; } = new List<Point>();
        public List<Point> FourthBySortOrderQuadrant { get; private set; } = new List<Point>();

        public PointsToQuadrantsAdder(Point pole, AngularSortDirection sortDirection, AngularSortStartLocation sortStartLocation)
            : base(pole, sortDirection, sortStartLocation)
        { }

        protected override void InitializePointAdder(AngularSortDirection sortDirection, AngularSortStartLocation sortStartLocation)
        {
            if (sortStartLocation == AngularSortStartLocation.PositiveX)
            {
                if (sortDirection == AngularSortDirection.CounterClockwise)
                    AddPointToProperQuadrant = AddPointToProperQuadrantsWhenSortIsHorizontalAndStartsFromFirstQuadrant;
                else AddPointToProperQuadrant = AddPointToProperQuadrantsWhenSortIsHorizontalAndStartsFromFourthQuadrant;
            }
            else if (sortStartLocation == AngularSortStartLocation.NegativeX)
            {
                if (sortDirection == AngularSortDirection.Clockwise)
                    AddPointToProperQuadrant = AddPointToProperQuadrantsWhenSortIsHorizontalAndStartsFromSecondQuadrant;
                else AddPointToProperQuadrant = AddPointToProperQuadrantsWhenSortIsHorizontalAndStartsFromThirdQuadrant;
            }
            else if (sortStartLocation == AngularSortStartLocation.PositiveY)
            {
                if (sortDirection == AngularSortDirection.CounterClockwise)
                    AddPointToProperQuadrant = AddPointToProperQuadrantsWhenSortIsVerticalAndStartsFromSecondQuadrant;
                else AddPointToProperQuadrant = AddPointToProperQuadrantsWhenSortIsVerticalAndStartsFromFirstQuadrant;
            }
            else // if (sortStartLocation == AngularSortStartLocation.NegativeY)
            {
                if (sortDirection == AngularSortDirection.Clockwise)
                    AddPointToProperQuadrant = AddPointToProperQuadrantsWhenSortIsVerticalAndStartsFromThirdQuadrant;
                else AddPointToProperQuadrant = AddPointToProperQuadrantsWhenSortIsVerticalAndStartsFromFourthQuadrant;
            }
        }

        // When we are in the second sorted hafl-plane, it doesn't matter whether there is a strict or non-strict inequality.

        private void AddPointToSecondSortedHalfPlane(Point point, bool shouldBeAddedToThirdBySortOrderQuadrant)
        {
            if (shouldBeAddedToThirdBySortOrderQuadrant)
                ThirdBySortOrderQuadrant.Add(point);
            else FourthBySortOrderQuadrant.Add(point);
        }

        private void AddPointToProperQuadrantsWhenSortIsHorizontalAndStartsFromFirstQuadrant(Point point)
        {
            if (point.Y.IsGreaterThanOrAlmostEqualTo(pole.Y))
                base.AddPointToProperFirstSortedHalfPlaneQuadrantWhenSortStartsFromPositiveX(point);
            else AddPointToSecondSortedHalfPlane(point, point.X.IsLessThanOrAlmostEqualTo(pole.X));
        }

        private void AddPointToProperQuadrantsWhenSortIsHorizontalAndStartsFromSecondQuadrant(Point point)
        {
            if (point.Y.IsGreaterThanOrAlmostEqualTo(pole.Y))
                base.AddPointToProperFirstSortedHalfPlaneQuadrantWhenSortStartsFromNegativeX(point);
            else AddPointToSecondSortedHalfPlane(point, point.X.IsGreaterThanOrAlmostEqualTo(pole.X));
        }

        private void AddPointToProperQuadrantsWhenSortIsHorizontalAndStartsFromThirdQuadrant(Point point)
        {
            if (point.Y.IsLessThanOrAlmostEqualTo(pole.Y))
                base.AddPointToProperFirstSortedHalfPlaneQuadrantWhenSortStartsFromNegativeX(point);
            else AddPointToSecondSortedHalfPlane(point, point.X.IsGreaterThanOrAlmostEqualTo(pole.X));
        }

        private void AddPointToProperQuadrantsWhenSortIsHorizontalAndStartsFromFourthQuadrant(Point point)
        {
            if (point.Y.IsLessThanOrAlmostEqualTo(pole.Y))
                base.AddPointToProperFirstSortedHalfPlaneQuadrantWhenSortStartsFromPositiveX(point);
            else AddPointToSecondSortedHalfPlane(point, point.X.IsLessThanOrAlmostEqualTo(pole.X));
        }

        private void AddPointToProperQuadrantsWhenSortIsVerticalAndStartsFromSecondQuadrant(Point point)
        {
            if (point.X.IsLessThanOrAlmostEqualTo(pole.X))
                base.AddPointToProperFirstSortedHalfPlaneQuadrantWhenSortStartsFromPositiveY(point);
            else AddPointToSecondSortedHalfPlane(point, point.Y.IsLessThanOrAlmostEqualTo(pole.Y));
        }

        private void AddPointToProperQuadrantsWhenSortIsVerticalAndStartsFromThirdQuadrant(Point point)
        {
            if (point.X.IsLessThanOrAlmostEqualTo(pole.X))
                base.AddPointToProperFirstSortedHalfPlaneQuadrantWhenSortStartsFromNegativeY(point);
            else AddPointToSecondSortedHalfPlane(point, point.Y.IsGreaterThanOrAlmostEqualTo(pole.Y));
        }

        private void AddPointToProperQuadrantsWhenSortIsVerticalAndStartsFromFirstQuadrant(Point point)
        {
            if (point.X.IsGreaterThanOrAlmostEqualTo(pole.X))
                base.AddPointToProperFirstSortedHalfPlaneQuadrantWhenSortStartsFromPositiveY(point);
            else AddPointToSecondSortedHalfPlane(point, point.Y.IsLessThanOrAlmostEqualTo(pole.Y));
        }

        private void AddPointToProperQuadrantsWhenSortIsVerticalAndStartsFromFourthQuadrant(Point point)
        {
            if (point.X.IsGreaterThanOrAlmostEqualTo(pole.X))
                base.AddPointToProperFirstSortedHalfPlaneQuadrantWhenSortStartsFromNegativeY(point);
            else AddPointToSecondSortedHalfPlane(point, point.Y.IsGreaterThanOrAlmostEqualTo(pole.Y));
        }
    }
}
