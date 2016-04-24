using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComparingDoubles;
using C5;
using MoreLinq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AlgorithmsTests")]

namespace ComputationalGeometry2D
{
    public class GeometricAlgorithms
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

        // For large input (probably > 10000) this algorithm is faster than recursive version, although both are O(lg(n)).
        // Note: The same instances of Point will not be considered as closest pair of distance 0.
        // (because tree set will not accept duplicate instances)
        // (tree bag will not help, because Successor() and Predecessor() will not find duplicates)
        // For points of the same coordinates use different instances.
        public ClosestPointsPairResult ClosestPairSweepLine(List<Point> points)
        {
            TreeSet<Point> eventQueue = new TreeSet<Point>(new PointsXYIDComparer(PointsIDOrder.Ascending));
            points.ForEach(p => eventQueue.Add(p));

            if (eventQueue.Count <= 1)
                return NoClosestPair;

            TreeSet<Point> statusStructure = new TreeSet<Point>(new PointsYXIDComparer(PointsIDOrder.Ascending))
            {
                new Point(Double.NegativeInfinity, Double.NegativeInfinity) // Meeded when MinDist = 0, to always allow find predecessor in status structure.
            };

            ClosestPointsPairResult result = new ClosestPointsPairResult(new List<UnorderedPointsPair>(), Double.PositiveInfinity);

            Point firstActive = eventQueue.FindMin();
            foreach(Point current in eventQueue) // Iterate over succeeding points.
            {
                if (result.MinDist.IsAlmostEqualToZero())
                {
                    Point previous = current;
                    // Adding all pairs of distance 0, which consist of "current" and predecessors in "statusStructure".
                    while ((previous = statusStructure.Predecessor(previous)).CoordinatesEqual(current))
                        result.PointsPairs.Add(new UnorderedPointsPair(previous, current));
                }
                else
                    UpdateClosestPair(current, result, statusStructure);
                while ((current.X - firstActive.X).IsGreaterThanAndNotAlmostEqualTo(result.MinDist))
                {
                    statusStructure.Remove(firstActive);
                    firstActive = eventQueue.Successor(firstActive);
                }
                statusStructure.Add(current);
            }
            return result;
        }

        private Point downBound = new Point(0.0, 0.0);
        private Point upperBound = new Point(0.0, 0.0);

        private void UpdateClosestPair(Point current, ClosestPointsPairResult result, TreeSet<Point> statusStructure)
        {
            double minDist = result.MinDist;

            downBound.X = current.X - minDist;
            downBound.Y = current.Y - minDist;
            upperBound.X = current.X;
            upperBound.Y = current.Y + minDist;

            double dist;
            foreach (Point neighbor in statusStructure.RangeFromTo(downBound, upperBound))
            {
                dist = current.DistanceFrom(neighbor);
                if (dist.IsAlmostEqualTo(minDist))
                    result.PointsPairs.Add(new UnorderedPointsPair(neighbor, current));
                else if (dist < minDist)
                {
                    result.MinDist = dist;
                    result.PointsPairs.Clear();
                    result.PointsPairs.Add(new UnorderedPointsPair(neighbor, current));
                }
            }
        }

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
        // Note: It's recommended to not pass duplicates instances of "Point" within points list,
        // because method may produce incorrect result (not enough 0-dist pairs) or throw an exception (if multiple same instances of Point lies on the middle (probably more than 2)). 
        // If the same instances are contained in the list, specify it in "sameInstancesContainedInList" - they should be removed. For points of the same coordinates use different instances.
        public ClosestPointsPairResult ClosestPairRecursive(List<Point> points, bool sameInstancesContainedInList = false)
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
                return ClosestPairBruteForce(sortedByX);

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
                if (result.MinDist.IsAlmostEqualToZero())
                {
                    Point iNeighborJSucc;
                    while (j <= last && (iNeighborJSucc = middleXNeighborsSortedByY[j++]).CoordinatesEqual(iNeighbor))
                    {
                        UnorderedPointsPair pair = new UnorderedPointsPair(iNeighbor, iNeighborJSucc);
                        if (!result.PointsPairs.Contains(pair)) // Need to check it, because in the middle neighborhood can be closest pair, which was already added during "conquer".
                            result.PointsPairs.Add(pair);
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
                            if (!result.PointsPairs.Contains(pair)) // Need to check it, because in the middle neighborhood can be closest pair, which was already added during "conquer".
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

        public List<Point> HalfPlaneAngularSort(List<Point> points, Point pole, AngularSortDirection direction, AngularSortStartLocation startLocation)
        {
            HalfPlanePointsToQuadrantsAdder pointsToQuadrantsAdder = new HalfPlanePointsToQuadrantsAdder(pole, direction, startLocation);
            points.ForEach(p => pointsToQuadrantsAdder.Add(p));
            PointsAngularByOrientationIDComparer pointsAngularByOrientationIDComparer = new PointsAngularByOrientationIDComparer(pole, direction, PointsIDOrder.Ascending);
            List<Point> firstSortedQuadrant = pointsToQuadrantsAdder.FirstBySortOrderQuadrant.OrderBy(p => p, pointsAngularByOrientationIDComparer).ToList();
            List<Point> secondSortedQuadrant = pointsToQuadrantsAdder.SecondBySortOrderQuadrant.OrderBy(p => p, pointsAngularByOrientationIDComparer).ToList();
            return firstSortedQuadrant.Concat(secondSortedQuadrant).ToList();
        }

        public List<Point> AllPlaneAngularSort(List<Point> points, Point pole, AngularSortDirection direction, AngularSortStartLocation startLocation)
        {
            PointsToQuadrantsAdder pointsToQuadrantsAdder = new PointsToQuadrantsAdder(pole, direction, startLocation);
            points.ForEach(p => pointsToQuadrantsAdder.Add(p));
            PointsAngularByOrientationIDComparer pointsAngularByOrientationIDComparer = new PointsAngularByOrientationIDComparer(pole, direction, PointsIDOrder.Ascending);
            List<Point> firstSortedQuadrant = pointsToQuadrantsAdder.FirstBySortOrderQuadrant.OrderBy(p => p, pointsAngularByOrientationIDComparer).ToList();
            List<Point> secondSortedQuadrant = pointsToQuadrantsAdder.SecondBySortOrderQuadrant.OrderBy(p => p, pointsAngularByOrientationIDComparer).ToList();
            List<Point> thirdSortedQuadrant = pointsToQuadrantsAdder.ThirdBySortOrderQuadrant.OrderBy(p => p, pointsAngularByOrientationIDComparer).ToList();
            List<Point> fourthSortedQuadrant = pointsToQuadrantsAdder.FourthBySortOrderQuadrant.OrderBy(p => p, pointsAngularByOrientationIDComparer).ToList();
            return firstSortedQuadrant.Concat(secondSortedQuadrant).Concat(thirdSortedQuadrant).Concat(fourthSortedQuadrant).ToList();
        }

        public Stack<Point> ConvexHullGrahamScan(List<Point> points)
        {
            Point minYPoint = points.MinBy(p => p, new PointsYXIDComparer(PointsIDOrder.Ascending));
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

        public Stack<Point> ConvexHullJarvis(List<Point> points)
        {
            Stack<Point> stack = new Stack<Point>();

            PointsYXIDComparer pointsYXIDComparer = new PointsYXIDComparer(PointsIDOrder.Ascending);

            Point minYPoint = points.MinBy(p => p, pointsYXIDComparer);
            Point maxYPoint = points.MaxBy(p => p, pointsYXIDComparer);
            Point current = minYPoint;
            stack.Push(current);

            PointsAngularByOrientationIDComparer pointsAngularByOrientationIDComparer = new PointsAngularByOrientationIDComparer(minYPoint, AngularSortDirection.CounterClockwise);

            while (!current.Equals(maxYPoint))
            {
                pointsAngularByOrientationIDComparer.SetPole(current);
                current = points
                    .Where(p => pointsYXIDComparer.Compare(p, current) > 0)
                    .MinBy(p => p, pointsAngularByOrientationIDComparer);
                stack.Push(current);
            }
            pointsYXIDComparer.SetIDOrder(PointsIDOrder.Descending);
            while (!current.Equals(minYPoint))
            {
                pointsAngularByOrientationIDComparer.SetPole(current);
                current = points
                    .Where(p => pointsYXIDComparer.Compare(p, current) < 0)
                    .MinBy(p => p, pointsAngularByOrientationIDComparer);
                stack.Push(current);
            }
            stack.Pop();

            return stack;
        }
    }
}
