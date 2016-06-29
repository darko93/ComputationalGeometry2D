using System.Collections.Generic;

using ComparingDoubles;

namespace ComputationalGeometry2D.AngularSort
{
    class HalfPlanePointsToQuadrantsAdder
    {
        protected Point pole = null;

        protected delegate void AddPoint(Point point);
        protected AddPoint AddPointToProperQuadrant = null;

        public List<Point> FirstBySortOrderQuadrant { get; private set; } = new List<Point>();
        public List<Point> SecondBySortOrderQuadrant { get; private set; } = new List<Point>();

        public HalfPlanePointsToQuadrantsAdder(Point pole, AngularOrder sortAngularOrder, AngularSortStartLocation sortStartLocation)
        {
            this.pole = pole;
            InitializePointAdder(sortAngularOrder, sortStartLocation);
        }

        protected virtual void InitializePointAdder(AngularOrder sortAngularOrder, AngularSortStartLocation sortStartLocation)
        {
            if (sortStartLocation == AngularSortStartLocation.PositiveX)
                AddPointToProperQuadrant = AddPointToProperFirstSortedHalfPlaneQuadrantWhenSortStartsFromPositiveX;
            else if (sortStartLocation == AngularSortStartLocation.NegativeX)
                AddPointToProperQuadrant = AddPointToProperFirstSortedHalfPlaneQuadrantWhenSortStartsFromNegativeX;
            else if (sortStartLocation == AngularSortStartLocation.PositiveY)
                AddPointToProperQuadrant = AddPointToProperFirstSortedHalfPlaneQuadrantWhenSortStartsFromPositiveY;
            else // if (sortStartLocation == AngularSortStartLocation.NegativeY)
                AddPointToProperQuadrant = AddPointToProperFirstSortedHalfPlaneQuadrantWhenSortStartsFromNegativeY;
        }

        // When we are in the first sorted hafl-plane, it's important whether there is a strict or non-strict inequality,
        // because points of the same coordinates as pole should always be in the first sorted quadrant.

        private void AddPointToFirstBySortOrderHalfPlane(Point point, bool shouldBeAddedToFirstBySortOrderQuadrant)
        {
            if (shouldBeAddedToFirstBySortOrderQuadrant)
                FirstBySortOrderQuadrant.Add(point);
            else SecondBySortOrderQuadrant.Add(point);
        }

        protected void AddPointToProperFirstSortedHalfPlaneQuadrantWhenSortStartsFromPositiveX(Point point) =>
            AddPointToFirstBySortOrderHalfPlane(point, point.X.IsGreaterThanOrAlmostEqualTo(pole.X));

        protected void AddPointToProperFirstSortedHalfPlaneQuadrantWhenSortStartsFromNegativeX(Point point) =>
            AddPointToFirstBySortOrderHalfPlane(point, point.X.IsLessThanOrAlmostEqualTo(pole.X));

        protected void AddPointToProperFirstSortedHalfPlaneQuadrantWhenSortStartsFromPositiveY(Point point) =>
            AddPointToFirstBySortOrderHalfPlane(point, point.Y.IsGreaterThanOrAlmostEqualTo(pole.Y));

        protected void AddPointToProperFirstSortedHalfPlaneQuadrantWhenSortStartsFromNegativeY(Point point) =>
            AddPointToFirstBySortOrderHalfPlane(point, point.Y.IsLessThanOrAlmostEqualTo(pole.Y));

        public void Add(Point point) =>
            AddPointToProperQuadrant(point);
    }
}

