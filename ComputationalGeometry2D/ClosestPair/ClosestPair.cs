using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComparingDoubles;
using C5;
using MoreLinq;
using ComputationalGeometry2D.Common;

namespace ComputationalGeometry2D.ClosestPair
{
    class ClosestPair
    {
        public static ClosestPair Instance { get; } = new ClosestPair();

        private ClosestPair() { }

        private delegate Point getNeighborPoint(Point point);

        private ClosestPointsPairResult NoClosestPair =>
            new ClosestPointsPairResult(new List<UnorderedPointsPair>()
            {
                new UnorderedPointsPair(
                    new Point(Double.NegativeInfinity, Double.NegativeInfinity),
                    new Point(Double.PositiveInfinity, Double.PositiveInfinity))
            }, Double.PositiveInfinity);

        // For large input (probably > 10000) this algorithm is faster than recursive version, although both are O(lg(n)).
        // Note: The same instances of Point will not be considered as closest pair of distance 0.
        // (because tree set will not accept duplicate instances)
        // (tree bag will not help, because Successor() and Predecessor() will not find duplicates)
        // For points of the same coordinates use different instances.
        public ClosestPointsPairResult SweepLine(IEnumerable<Point> points)
        {
            TreeSet<Point> eventQueue = new TreeSet<Point>(new PointsXYIDComparer(PointsIDOrder.Ascending));
            points.ForEach(p => eventQueue.Add(p));

            if (eventQueue.Count <= 1)
                return NoClosestPair;

            TreeSet<Point> sweepLine = new TreeSet<Point>(new PointsYXIDComparer(PointsIDOrder.Ascending))
            {
                new Point(Double.NegativeInfinity, Double.NegativeInfinity) // Needed when MinDist = 0, to always allow find predecessor in sweep line.
            };

            ClosestPointsPairResult result = new ClosestPointsPairResult(new List<UnorderedPointsPair>(), Double.PositiveInfinity);

            Point firstActive = eventQueue.FindMin();
            foreach (Point current in eventQueue) // Iterate over succeeding points.
            {
                if (result.MinDist.IsAlmostEqualToZero())
                {
                    Point previous = current;
                    // Adding all pairs of distance 0, which consist of "current" and predecessors in sweep line.
                    while ((previous = sweepLine.Predecessor(previous)).CoordinatesEqual(current))
                        result.ClosestPairs.Add(new UnorderedPointsPair(previous, current));
                }
                else
                    TryUpdateClosestPair(current, result, sweepLine);
                // Removing from the sweep line all points, which distance from "current" is gretaer than MinDist.
                while ((current.X - firstActive.X).IsGreaterThanAndNotAlmostEqualTo(result.MinDist))
                {
                    sweepLine.Remove(firstActive);
                    firstActive = eventQueue.Successor(firstActive);
                }
                sweepLine.Add(current);
            }
            return result;
        }

        private Point lowerBound = new Point(0.0, 0.0);
        private Point upperBound = new Point(0.0, 0.0);

        private void TryUpdateClosestPair(Point current, ClosestPointsPairResult result, TreeSet<Point> sweepLine)
        {
            double minDist = result.MinDist;

            lowerBound.X = current.X - minDist;
            lowerBound.Y = current.Y - minDist;
            upperBound.X = current.X;
            upperBound.Y = current.Y + minDist;

            double dist;
            foreach (Point neighbor in sweepLine.RangeFromTo(lowerBound, upperBound)) // At most 8 points.
            {
                dist = current.DistanceFrom(neighbor);
                if (dist.IsAlmostEqualTo(minDist))
                    result.ClosestPairs.Add(new UnorderedPointsPair(neighbor, current));
                else if (dist < minDist)
                {
                    result.MinDist = dist;
                    result.ClosestPairs.Clear();
                    result.ClosestPairs.Add(new UnorderedPointsPair(neighbor, current));
                }
            }
        }

        public ClosestPointsPairResult BruteForce(List<Point> points)
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
        // Note: It's recommended to not pass duplicates instances of "Point" within points list,
        // because method may produce incorrect result (not enough 0-dist pairs) or throw an exception (if multiple same instances of Point lies on the middle (probably more than 2)). 
        // If the same instances are contained in the list, specify it in "sameInstancesContainedInList" - they should be removed. 
        // For points of the same coordinates use different instances.
        public ClosestPointsPairResult DivideAndConquer(System.Collections.Generic.ICollection<Point> points, bool sameInstancesContainedInList = false)
        {
            if (points.Count <= 1)
                return NoClosestPair;

            if (sameInstancesContainedInList)
                points = points.Distinct().ToList(); // Remove referenece equal points.

            PointsXYIDComparer pointsXYIDComparer = new PointsXYIDComparer(PointsIDOrder.Ascending);
            List<Point> sortedByX = points.OrderBy(p => p, pointsXYIDComparer).ToList();
            List<Point> sortedByY = points.OrderBy(p => p, new PointsYXIDComparer(PointsIDOrder.Ascending)).ToList();

            return ClosestPairRecursive(sortedByX, sortedByY, pointsXYIDComparer);
        }

        private ClosestPointsPairResult ClosestPairRecursive(List<Point> sortedByX, List<Point> sortedByY, PointsXYIDComparer pointsXYIDComparer)
        {
            int pointsCount = sortedByX.Count;
            if (pointsCount <= 3)
                return BruteForce(sortedByX);

            int middle = pointsCount / 2;
            List<Point> leftSortedByX = sortedByX.Take(middle).ToList();
            List<Point> rightSortedByX = sortedByX.Skip(middle).ToList();

            Point middlePoint = sortedByX[middle];
            List<Point> leftSortedByY = new List<Point>();
            List<Point> rightSortedByY = new List<Point>();
            foreach (Point yPoint in sortedByY)
            {
                if (pointsXYIDComparer.Compare(yPoint, middlePoint) < 0)
                    leftSortedByY.Add(yPoint);
                else
                    rightSortedByY.Add(yPoint);
            }

            ClosestPointsPairResult leftResult = ClosestPairRecursive(leftSortedByX, leftSortedByY, pointsXYIDComparer);
            ClosestPointsPairResult rightResult = ClosestPairRecursive(rightSortedByX, rightSortedByY, pointsXYIDComparer);

            ClosestPointsPairResult result = GetBetterResult(leftResult, rightResult);

            List<Point> middleXNeighborsSortedByY = sortedByY.Where(p => Math.Abs(p.X - middlePoint.X).IsLessThanOrAlmostEqualTo(result.MinDist)).ToList();

            TryUpdateClosestPairInTheMiddleNeighborhood(result, middleXNeighborsSortedByY);

            return result;
        }

        private ClosestPointsPairResult GetBetterResult(ClosestPointsPairResult leftResult, ClosestPointsPairResult rightResult)
        {
            if (leftResult.MinDist.IsAlmostEqualTo(rightResult.MinDist))
            {
                leftResult.ClosestPairs.AddRange(rightResult.ClosestPairs);
                return leftResult;
            }
            else if (leftResult.MinDist < rightResult.MinDist)
                return leftResult;
            else return rightResult;
        }

        private void TryUpdateClosestPairInTheMiddleNeighborhood(ClosestPointsPairResult result, List<Point> middleXNeighborsSortedByY)
        {
            int last = middleXNeighborsSortedByY.Count - 1;
            for (int i = 0; i < last; i++)
            {
                Point iNeighbor = middleXNeighborsSortedByY[i];
                int j = i + 1;
                if (result.MinDist.IsAlmostEqualToZero())
                {
                    Point iNeighborJSucc;
                    while (j <= last && (iNeighborJSucc = middleXNeighborsSortedByY[j++]).CoordinatesEqual(iNeighbor))
                    {
                        UnorderedPointsPair pair = new UnorderedPointsPair(iNeighbor, iNeighborJSucc);
                        if (!result.ClosestPairs.Contains(pair)) // Need to check it, because in the middle neighborhood can be closest pair, which was already added during "conquer".
                            result.ClosestPairs.Add(pair);
                    }
                }
                else
                {
                    int lastSucc = i + 7;
                    if (lastSucc > last)
                        lastSucc = last;
                    for (; j <= lastSucc; j++)
                    {
                        Point iNeighborJSucc = middleXNeighborsSortedByY[j];
                        double dist = iNeighbor.DistanceFrom(iNeighborJSucc);
                        if (dist.IsAlmostEqualTo(result.MinDist))
                        {
                            UnorderedPointsPair pair = new UnorderedPointsPair(iNeighbor, iNeighborJSucc);
                            if (!result.ClosestPairs.Contains(pair)) // Need to check it, because in the middle neighborhood can be closest pair, which was already added during "conquer".
                                result.ClosestPairs.Add(pair);
                        }
                        else if (dist < result.MinDist)
                        {
                            result.MinDist = dist;
                            result.ClosestPairs.Clear();
                            result.ClosestPairs.Add(new UnorderedPointsPair(iNeighbor, iNeighborJSucc));
                        }
                    }
                }
            }
        }
    }
}
