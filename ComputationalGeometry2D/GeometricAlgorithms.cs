using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using C5;
using MoreLinq;

namespace ComputationalGeometry2D
{
    class GeometricAlgorithms
    {
        //http://www.cosc.canterbury.ac.nz/mukundan/cgeo/Sweep1.html

        private delegate Point getNeighborPoint(Point point);

        private ClosestPointsPairResult NoClosestPair =>
            new ClosestPointsPairResult(new List<UnorderedPointsPair>()
            {
                new UnorderedPointsPair(
                    new Point(Double.NegativeInfinity, Double.NegativeInfinity),
                    new Point(Double.PositiveInfinity, Double.PositiveInfinity))
            }, Double.PositiveInfinity);

        public ClosestPointsPairResult ClosestPairSweepLine(List<Point> points, PointsCoordDuplicatesMode coordDuplicatesMode)
        {
            if (coordDuplicatesMode == PointsCoordDuplicatesMode.ContainedInListAndAllowedInResult)
                return ClosestPairSweepLine(points, new PointsXYIDComparer(), new PointsYXIDComparer());
            else // if (coordDuplicatesMode == PointsCoordDuplicatesMode.ContainedInListButNotAllowedInResult || coordDuplicatesMode == PointsCoordDuplicatesMode.NotContainedInList)
                return ClosestPairSweepLine(points, new PointsXYComparer(), new PointsYXComparer());
        }

        // For large input (probably > 10000) this algorithm is faster than recursive version, although both are O(lg(n)).
        // Note: The same instances of Point will not be considered as closest pair of distance 0.
        // (because tree set will not accept duplicate instances)
        // (tree bag will not help, because Successor() and Predecessor() will not find duplicates)
        // For points of the same coordinates use different instances.
        private ClosestPointsPairResult ClosestPairSweepLine(List<Point> points, IComparer<Point> pointsXYComparer, IComparer<Point> pointsYXComparer)
        {
            List<UnorderedPointsPair> closestPairs = new List<UnorderedPointsPair>();
            double minDist = Double.PositiveInfinity;

            TreeSet<Point> eventQueue = new TreeSet<Point>(pointsXYComparer);
            points.ForEach(p => eventQueue.Add(p));

            if (eventQueue.Count <= 1)
                return NoClosestPair;

            Point rightBound = new Point(Double.NegativeInfinity, Double.PositiveInfinity);
            Point leftBound = new Point(Double.NegativeInfinity, Double.NegativeInfinity);
            TreeSet<Point> statusStructure = new TreeSet<Point>(pointsYXComparer)
            {
                rightBound,
                leftBound
            };

            Point firstActive = eventQueue.FindMin();
            foreach(Point current in eventQueue) // Iterate over succeeding points.
            {
                if (minDist.IsAlmostEqualToZero()) // Possible when coordinate-duplicates allowed.
                {
                    Point previous = current;
                    // Adding all pairs of distance 0, which consist of "current" and predecessors from "broom".
                    while ((previous = statusStructure.Predecessor(previous)).CoordinatesEqual(current))
                        closestPairs.Add(new UnorderedPointsPair(previous, current));
                }
                else
                {
                    UpdateClosestPairOnSide(current, rightBound, closestPairs, ref minDist, statusStructure.Successor);
                    UpdateClosestPairOnSide(current, leftBound, closestPairs, ref minDist, statusStructure.Predecessor);
                }
                UpdateActivePoints(current, ref firstActive, minDist, eventQueue, statusStructure);
                statusStructure.Add(current);
            }
            return new ClosestPointsPairResult(closestPairs, minDist);
        }

        private void UpdateClosestPairOnSide(Point current, Point boundPoint, List<UnorderedPointsPair> minDistPair, ref double minDist, getNeighborPoint neighborFunc)
        {
            Point neighborPoint = neighborFunc(current);
            double dist;
            for (int i = 0; i < 4 && !neighborPoint.CoordinatesEqual(boundPoint); i++)
            {
                dist = current.DistanceFrom(neighborPoint);
                if (dist.IsAlmostEqualTo(minDist))
                    minDistPair.Add(new UnorderedPointsPair(neighborPoint, current));
                else if (dist < minDist)
                {
                    minDist = dist;
                    minDistPair.Clear();
                    minDistPair.Add(new UnorderedPointsPair(neighborPoint, current));
                }
                neighborPoint = neighborFunc(neighborPoint);
            }
        }
        
        private void UpdateActivePoints(Point current, ref Point firstActive, double minDist, TreeSet<Point> eventQueue, TreeSet<Point> statusStructure)
        {
            while ((current.X - firstActive.X).IsGreaterThanAndNotAlmostEqualTo(minDist))
            {
                statusStructure.Remove(firstActive);
                firstActive = eventQueue.Successor(firstActive);
            }
        }
        public int Counter = 0;
        public ClosestPointsPairResult ClosestPairBruteForce(List<Point> points)
        {
            double minDist = Double.PositiveInfinity;
            List<UnorderedPointsPair> closestPairs = new List<UnorderedPointsPair>();
            double pointsCount = points.Count;

            if (pointsCount <= 1)
            return NoClosestPair;

            double sqrdMinDist = Double.PositiveInfinity;
            double sqrdDist;
            for (int i = 0; i < pointsCount; i++)
                for (int j = i + 1; j < pointsCount; j++)
                {
                    Point p1 = points[i];
                    Point p2 = points[j];
                    sqrdDist = p1.SquaredDistanceFrom(p2);

                    if (sqrdDist.IsAlmostEqualTo(sqrdMinDist))
                        closestPairs.Add(new UnorderedPointsPair(p1, p2));
                    else if (sqrdDist < sqrdMinDist)
                    {
                        sqrdMinDist = sqrdDist;
                        closestPairs.Clear();
                        closestPairs.Add(new UnorderedPointsPair(p1, p2));
                    }
                }
            minDist = Math.Sqrt(sqrdMinDist);
            return new ClosestPointsPairResult(closestPairs, minDist);
        }

        // For large input (probably > 10000) this algorithm is slower than sweep-line version, although both are O(nlg(n)).
        // Note: It's recommended to not pass the same instances of "Point" within points list, when coordinate-duplicates in the result are allowed,
        // because method may produce incorrect result (not enough 0-dist pairs) or throw an exception (if multiple same instances of Point lies on the middle (probably more than 2)). 
        // If the same instances are contained in the list, specify it in "sameInstancesContainedInList" - they should be removed. For points of the same coordinates use different instances.
        public ClosestPointsPairResult ClosestPairRecursive(List<Point> points, PointsCoordDuplicatesMode resultCoordDuplicatesMode, bool sameInstancesContainedInList = false)
        {
            if (points.Count <= 1)
                return NoClosestPair;

            if (resultCoordDuplicatesMode == PointsCoordDuplicatesMode.ContainedInListButNotAllowedInResult)
                points = points.Distinct(new PointsXYEqualityComparer()).ToList(); // Remove coordinate and reference equal points.
            else if (sameInstancesContainedInList)
                points = points.Distinct().ToList(); // Remove referenece equal points.

            List<Point> sortedByX = points.OrderBy(p => p, new PointsXYIDComparer()).ToList();
            List<Point> sortedByY = points.OrderBy(p => p, new PointsYXIDComparer()).ToList();

            return ClosestPairRecursive(sortedByX, sortedByY, new PointsXYIDComparer());
        }

        public ClosestPointsPairResult ClosestPairRecursive(List<Point> sortedByX, List<Point> sortedByY, PointsXYIDComparer pointsXYIDComparer)
        {
            int pointsCount = sortedByX.Count;
            if (pointsCount <= 3)
                return ClosestPairBruteForce(sortedByX);

            int middle = pointsCount / 2;
            List<Point> leftSortedByX = sortedByX.Take(middle).ToList();
            List<Point> rightSortedByX = sortedByX.Skip(middle).ToList();

            Point middlePoint = sortedByX[middle];
            List<Point> leftSortedByY = new List<Point>();
            List<Point> rightSortedByY = new List<Point>();
            foreach(Point yPoint in sortedByY)
            {
                if (pointsXYIDComparer.Compare(yPoint, middlePoint) < 0)
                    leftSortedByY.Add(yPoint);
                else rightSortedByY.Add(yPoint);
            }

            ClosestPointsPairResult leftResult = ClosestPairRecursive(leftSortedByX, leftSortedByY, pointsXYIDComparer);
            ClosestPointsPairResult rightResult = ClosestPairRecursive(rightSortedByX, rightSortedByY, pointsXYIDComparer);

            ClosestPointsPairResult result;
            if (leftResult.MinDist.IsAlmostEqualTo(rightResult.MinDist))
            {
                leftResult.PointsPairs.AddRange(rightResult.PointsPairs);
                result = leftResult;
            }
            else if (leftResult.MinDist < rightResult.MinDist)
                result = leftResult;
            else result = rightResult;

            List<Point> middleXNeighborsSortedByY = sortedByY.Where(p => Math.Abs(p.X - middlePoint.X).IsLessThanOrAlmostEqualTo(result.MinDist)).ToList();

            int last = middleXNeighborsSortedByY.Count - 1;
            for (int i = 0; i < last; i++)
            {
                Point iNeighbor = middleXNeighborsSortedByY[i];
                int j = i + 1;
                if (result.MinDist.IsAlmostEqualToZero()) // Possible when coordinate-duplicates allowed.
                {
                    Point iNeighborJSucc;
                    while (j <= last && (iNeighborJSucc = middleXNeighborsSortedByY[j++]).CoordinatesEqual(iNeighbor))
                    {
                        UnorderedPointsPair pair = new UnorderedPointsPair(iNeighbor, iNeighborJSucc);
                        bool contains = result.PointsPairs.Contains(pair);
                        if (!contains) // Need to check it, because in the middle neighborhood can be closest pair, which was already added during "conquer".
                            result.PointsPairs.Add(pair);
                    }
                }
                else
                {
                    int lastSucc = i + 7;
                    if (lastSucc > last)
                        lastSucc = last;
                    for ( ; j <= lastSucc; j++)
                    {
                        Point iNeighborJSucc = middleXNeighborsSortedByY[j];
                        double dist = iNeighbor.DistanceFrom(iNeighborJSucc);
                        if (dist.IsAlmostEqualTo(result.MinDist))
                        {
                            UnorderedPointsPair pair = new UnorderedPointsPair(iNeighbor, iNeighborJSucc);
                            bool contains = result.PointsPairs.Contains(pair);
                            if (!contains) // Need to check it, because in the middle neighborhood can be closest pair, which was already added during "conquer".
                                result.PointsPairs.Add(pair);
                        }
                        else if (dist < result.MinDist)
                        {
                            result.MinDist = dist;
                            result.PointsPairs.Clear();
                            result.PointsPairs.Add(new UnorderedPointsPair(iNeighbor, iNeighborJSucc));
                        }
                    }
                }
            }
            return result;
        }

        private List<Point> PolarSort(List<Point> points, Point pole)
        {
            TreeSet<Point> sortedByPolarCoord = new TreeSet<Point>(new PointsPolarCoordIDComparer(pole));
            points.ForEach(p => sortedByPolarCoord.Add(p));
            points.Clear();
            while (sortedByPolarCoord.Count > 0)
                points.Add(sortedByPolarCoord.DeleteMin());
            return points;
        }

        public List<Point> HalfPlaneAngularSort(List<Point> points, Point pole, AngularSortDirection direction, AngularSortStartLocation startLocation) =>
            points.OrderBy(p => p, new HalfPlanePointsAngularComparer(pole, startLocation, direction)).ToList();

        public List<Point> AllPlaneAngularSort(List<Point> points, Point pole, AngularSortDirection direction, AngularSortStartLocation startLocation)
        {
            List<Point> halfPlane1Points, halfPlane2Points;

            AngularSortStartLocation secondHalfPlaneStartLocation;
            if (startLocation == AngularSortStartLocation.PositiveX)
                secondHalfPlaneStartLocation = AngularSortStartLocation.NegativeX;
            else if (startLocation == AngularSortStartLocation.NegativeX)
                secondHalfPlaneStartLocation = AngularSortStartLocation.PositiveX;
            else if (startLocation == AngularSortStartLocation.PositiveY)
                secondHalfPlaneStartLocation = AngularSortStartLocation.NegativeY;
            else // if (startLocation == AngularSortStartLocation.NegativeY)
                secondHalfPlaneStartLocation = AngularSortStartLocation.PositiveY;

            if (startLocation == AngularSortStartLocation.PositiveX || startLocation == AngularSortStartLocation.NegativeX)
            {
                halfPlane1Points = points.Where(p => p.Y.IsGreaterThanOrAlmostEqualToZero()).ToList();
                halfPlane2Points = points.Where(p => p.Y.IsLessThanAndNotAlmostEqualToZero()).ToList();
            }
            else //if (startLocation == PolarSortStartLocation.PositiveY || startLocation == PolarSortStartLocation.NegativeY)
            {
                halfPlane1Points = points.Where(p => p.X.IsGreaterThanOrAlmostEqualToZero()).ToList();
                halfPlane2Points = points.Where(p => p.X.IsLessThanAndNotAlmostEqualToZero()).ToList();
            }
            halfPlane1Points = halfPlane1Points.OrderBy(p => p, new HalfPlanePointsAngularComparer(pole, startLocation, direction)).ToList();
            halfPlane2Points = halfPlane2Points.OrderBy(p => p, new HalfPlanePointsAngularComparer(pole, secondHalfPlaneStartLocation, direction)).ToList();
            halfPlane1Points.AddRange(halfPlane2Points);
            return halfPlane1Points;
        }

        public Stack<Point> ConvexHullGrahamScan(List<Point> points)
        {
            Point minYPoint = points.MinBy(p => p, new PointsYXComparer());
            List<Point> sortedPoints = HalfPlaneAngularSort(points, minYPoint, AngularSortDirection.CounterClockwise, AngularSortStartLocation.PositiveX);
            Stack<Point> stack = new Stack<Point>();
            stack.Push(minYPoint);
            stack.Push(sortedPoints[1]);
            stack.Push(sortedPoints[2]);

            int pointsCount = sortedPoints.Count;
            LineSegment segment = new LineSegment();
            for (int i = 3; i < pointsCount; i++)
            {
                Point iPoint = sortedPoints[i];
                segment.End = iPoint;
                Point top, nextToTheTop;
                do
                {
                    top = stack.Pop();
                    nextToTheTop = stack.Peek();
                    segment.Start = top;
                }
                while (nextToTheTop.LiesToTheRightOf(segment));
                stack.Push(top); // Top was redundantly popped.
                stack.Push(iPoint);
            }
            return stack;
        }
    }
}
