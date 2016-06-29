using System.Collections.Generic;
using System.Linq;

using MoreLinq;
using ComputationalGeometry2D.Common;

namespace ComputationalGeometry2D.AngularSort
{
    class AngularSort
    {
        public static AngularSort Instance { get; } = new AngularSort();

        private AngularSort() { }

        public List<Point> HalfPlane(IEnumerable<Point> points, Point pole, AngularOrder angularOrder, AngularSortStartLocation startLocation)
        {
            HalfPlanePointsToQuadrantsAdder pointsToQuadrantsAdder = new HalfPlanePointsToQuadrantsAdder(pole, angularOrder, startLocation);
            points.ForEach(p => pointsToQuadrantsAdder.Add(p));
            PointsAngularIDComparer pointsAngularIDComparer = new PointsAngularIDComparer(pole, angularOrder, PointsIDOrder.Ascending);
            List<Point> firstSortedQuadrant = pointsToQuadrantsAdder.FirstBySortOrderQuadrant.OrderBy(p => p, pointsAngularIDComparer).ToList();
            List<Point> secondSortedQuadrant = pointsToQuadrantsAdder.SecondBySortOrderQuadrant.OrderBy(p => p, pointsAngularIDComparer).ToList();
            return firstSortedQuadrant.Concat(secondSortedQuadrant).ToList();
        }

        public List<Point> AllPlane(IEnumerable<Point> points, Point pole, AngularOrder angularOrder, AngularSortStartLocation startLocation)
        {
            PointsToQuadrantsAdder pointsToQuadrantsAdder = new PointsToQuadrantsAdder(pole, angularOrder, startLocation);
            points.ForEach(p => pointsToQuadrantsAdder.Add(p));
            PointsAngularIDComparer pointsAngularIDComparer = new PointsAngularIDComparer(pole, angularOrder, PointsIDOrder.Ascending);
            List<Point> firstSortedQuadrant = pointsToQuadrantsAdder.FirstBySortOrderQuadrant.OrderBy(p => p, pointsAngularIDComparer).ToList();
            List<Point> secondSortedQuadrant = pointsToQuadrantsAdder.SecondBySortOrderQuadrant.OrderBy(p => p, pointsAngularIDComparer).ToList();
            List<Point> thirdSortedQuadrant = pointsToQuadrantsAdder.ThirdBySortOrderQuadrant.OrderBy(p => p, pointsAngularIDComparer).ToList();
            List<Point> fourthSortedQuadrant = pointsToQuadrantsAdder.FourthBySortOrderQuadrant.OrderBy(p => p, pointsAngularIDComparer).ToList();
            return firstSortedQuadrant.Concat(secondSortedQuadrant).Concat(thirdSortedQuadrant).Concat(fourthSortedQuadrant).ToList();
        }
    }
}
