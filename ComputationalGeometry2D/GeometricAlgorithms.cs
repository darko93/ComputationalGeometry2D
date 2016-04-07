using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using C5;

namespace ComputationalGeometry2D
{
    class GeometricAlgorithms
    {
        private delegate Point getNeighborPoint(Point point);

        public ClosestPointsPairResult ClosestPairIterative(List<Point> points, PointsCoordDuplicatesMode duplicatesMode)
        {
            if (duplicatesMode == PointsCoordDuplicatesMode.Allowed)
                return ClosestPairIterative(points, new PointsXYIDComparer(), new PointsYXIDComparer());
            else // if (duplicatesMode == PointsCoordDuplicatesMode.notAllowed)
                return ClosestPairIterative(points, new PointsXYComparer(), new PointsYXComparer());
        }

        // This algorithm is slower than recursive version, although both are O(nlg(n)).
        // Also this algorithm assumes, that for allowed coordinate-duplicates points list does not contain two or more same instances of Point,
        // (because tree set will not accept duplicate instances)
        // (tree bag will not help, because Successor() and Predecessor() will not find duplicates)
        // so same instances of Point will not be considered as closest pair of distance 0.
        // For duplicates instances use recursive algorithm.
        private ClosestPointsPairResult ClosestPairIterative(List<Point> points, IComparer<Point> pointsXYComparer, IComparer<Point> pointsYXComparer)
        {
            List<UnorderedPointsPair> closestPairs = new List<UnorderedPointsPair>();
            double minDist = Double.PositiveInfinity;

            if (points.Count <= 1)
            {
                closestPairs.Add(new UnorderedPointsPair(Point.SWInfinity, Point.NEInfinity));
                return new ClosestPointsPairResult(closestPairs, minDist);
            }

            TreeSet<Point> pointsQueue = new TreeSet<Point>(pointsXYComparer);
            points.ForEach(p => pointsQueue.Add(p));
            Point rightBound = new Point(Double.NegativeInfinity, Double.PositiveInfinity);
            Point leftBound = new Point(Double.NegativeInfinity, Double.NegativeInfinity);
            TreeSet<Point> pointsBroom = new TreeSet<Point>(pointsYXComparer)
            {
                rightBound,
                leftBound
            };;

            Point firstActive = pointsQueue.DeleteMin();
            Point current = firstActive;
            pointsBroom.Add(current);
            Point next;
            while (pointsQueue.TrySuccessor(current, out next))
            {
                current = next;
                if (minDist.IsAlmostEqualToZero()) // Possible when coordinate-duplicates allowed.
                {
                    Point previous = current;
                    // Adding all pairs of distance 0, which consist of "current" and predecessors from "broom".
                    while ((previous = pointsBroom.Predecessor(previous)).CoordinatesEqual(current))
                        closestPairs.Add(new UnorderedPointsPair(previous, current));
                }
                else
                {
                    UpdateMinDist(current, rightBound, closestPairs, ref minDist, pointsBroom.Successor);
                    UpdateMinDist(current, leftBound, closestPairs, ref minDist, pointsBroom.Predecessor);
                }
                UpdateActivePoints(current, ref firstActive, minDist, pointsQueue, pointsBroom);
                pointsBroom.Add(current);
            }
            return new ClosestPointsPairResult(closestPairs, minDist);
        }
        
        private void UpdateMinDist(Point current, Point boundPoint, List<UnorderedPointsPair> minDistPair, ref double minDist, getNeighborPoint neighborFunc)
        {
            Point neighborPoint = neighborFunc(current);
            int i = 0;
            double dist;
            while (i < 4 && !neighborPoint.CoordinatesEqual(boundPoint))
            {
                dist = current.DistanceFrom(neighborPoint);
                
                if (dist.IsAlmostEqualTo(minDist))
                {
                    minDistPair.Add(new UnorderedPointsPair(neighborPoint, current));
                }
                
                else if (dist < minDist)
                {
                    minDist = dist;
                    minDistPair.Clear();
                    minDistPair.Add(new UnorderedPointsPair(neighborPoint, current));
                }
                neighborPoint = neighborFunc(neighborPoint);
                i++;
            }
        }

        public int Counter { get; set; } = 0;
        private void UpdateActivePoints(Point current, ref Point firstActive, double minDist, TreeSet<Point> pointsQueue, TreeSet<Point> pointsBroom)
        {
            Point candidateToDelete = firstActive;
            while ((current.X - candidateToDelete.X).IsGreaterThanAndNotAlmostEqualTo(minDist))
            {
                Counter++;
                pointsBroom.Remove(candidateToDelete);
                firstActive = pointsQueue.Successor(candidateToDelete);
                candidateToDelete = firstActive;
            }
        }

        public ClosestPointsPairResult ClosestPairBruteForce(List<Point> points)
        {
            double minDist = Double.PositiveInfinity;
            List<UnorderedPointsPair> closestPairs = new List<UnorderedPointsPair>();
            double pointsCount = points.Count;

            if (pointsCount <= 1)
            {
                closestPairs.Add(new UnorderedPointsPair(Point.SWInfinity, Point.NEInfinity));
                return new ClosestPointsPairResult(closestPairs, minDist);
            }

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

        public ClosestPointsPairResult ClosestPairRecursive(List<Point> points)
        {
            double minDist = Double.PositiveInfinity;
            List<UnorderedPointsPair> closestPairs = new List<UnorderedPointsPair>();

            if (points.Count <= 1)
            {
                closestPairs.Add(new UnorderedPointsPair(Point.SWInfinity, Point.NEInfinity));
                return new ClosestPointsPairResult(closestPairs, minDist);
            }

            // if no duplicates mode then delete duplicates from points list here

            List<Point> sortedByX = points.OrderBy(p => p, new PointsXYIDComparer()).ToList();
            List<Point> sortedByY = points.OrderBy(p => p, new PointsYXIDComparer()).ToList();

            return ClosestPairRecursive(points, sortedByX, sortedByY, new PointsXYIDComparer());
        }

        public ClosestPointsPairResult ClosestPairRecursive(List<Point> points, List<Point> sortedByX, List<Point> sortedByY, PointsXYIDComparer pointsXYIDComparer)
        {
            int pointsCount = points.Count;
            if (points.Count <= 3)
                return ClosestPairBruteForce(points);

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

            if (leftSortedByY.Count != leftSortedByX.Count)
            {
                bool p = true;
            }

            List<Point> leftPoints = new List<Point>();
            List<Point> rightPoints = new List<Point>();
            foreach (Point point in points)
            {
                if (pointsXYIDComparer.Compare(point, middlePoint) < 0)
                    leftPoints.Add(point);
                else rightPoints.Add(point);
            }

            ClosestPointsPairResult leftResult = ClosestPairRecursive(leftPoints, leftSortedByX, leftSortedByY, pointsXYIDComparer);
            ClosestPointsPairResult rightResult = ClosestPairRecursive(rightPoints, rightSortedByX, rightSortedByY, pointsXYIDComparer);

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

    }
}
