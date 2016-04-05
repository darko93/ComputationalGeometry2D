using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using C5;

namespace ComputationalGeometry2D
{
    class Geometry
    {
        public ClosestPointsPairResult ClosestPairIterative(List<Point> points)
        {
            List<PointsPair> closestPairs = new List<PointsPair>();
            double minDist = Double.PositiveInfinity;

            if (points.Count <= 1)
            {
                closestPairs.Add(new PointsPair(Point.SWInfinity, Point.NEInfinity));
                return new ClosestPointsPairResult(closestPairs, minDist);
            }

            IntervalHeap<Point> pointsQueue = new IntervalHeap<Point>(new PointsXYIDComparer());
            points.ForEach(p => pointsQueue.Add(p)); // adds also duplicates, because of bag sementic
            Point rightBound = new Point(Double.NegativeInfinity, Double.PositiveInfinity);
            Point leftBound = new Point(Double.NegativeInfinity, Double.NegativeInfinity);
            TreeSet<Point> pointsBroom = new TreeSet<Point>(new PointsYXIDComparer())
            {
                rightBound,
                leftBound
            };
            Point current;
            Point next = pointsQueue.FindMin(); // deleteMin
            Point firstActive = next;
            double sqrdMinDist = Double.PositiveInfinity;
            bool successorExist;
            do
            {
                current = next;

                pointsQueue.DeleteMin(); // unnecessary
                successorExist = pointsQueue.Count > 0;
                if (successorExist)
                    next = pointsQueue.FindMin(); // deleteMin 

                UpdateMinDist(current, rightBound, closestPairs, ref sqrdMinDist, pointsBroom.Successor);
                UpdateMinDist(current, leftBound, closestPairs, ref sqrdMinDist, pointsBroom.Predecessor);
                UpdateActivePoints(current, ref firstActive, minDist, pointsBroom);
                
                pointsBroom.Add(current);
            }
            while (successorExist);

            minDist = Math.Sqrt(sqrdMinDist);
            return new ClosestPointsPairResult(closestPairs, minDist);
        }

        public ClosestPointsPairResult ClosestPairRightDuplicates(List<Point> points)
        {
            List<PointsPair> closestPairs = new List<PointsPair>();
            double minDist = Double.PositiveInfinity;

            if (points.Count <= 1)
            {
                closestPairs.Add(new PointsPair(Point.SWInfinity, Point.NEInfinity));
                return new ClosestPointsPairResult(closestPairs, minDist);
            }

            IntervalHeap<Point> pointsQueue = new IntervalHeap<Point>(new PointsXYComparer());
            points.ForEach(p => pointsQueue.Add(p)); // adds also duplicates, because of bag sementic
            Point rightBound = new Point(Double.NegativeInfinity, Double.PositiveInfinity);
            Point leftBound = new Point(Double.NegativeInfinity, Double.NegativeInfinity);
            TreeSet<Point> pointsBroom = new TreeSet<Point>(new PointsYXComparer())
            {
                rightBound,
                leftBound
            };
            Point current;
            Point next = pointsQueue.FindMin();
            Point firstActive = next;
            double sqrdMinDist = Double.PositiveInfinity;
            bool successorExist;
            do
            {
                current = next;

                pointsQueue.DeleteMin();
                successorExist = pointsQueue.Count > 0;
                if (successorExist)
                    next = pointsQueue.FindMin();
                
                UpdateMinDist(current, rightBound, closestPairs, ref sqrdMinDist, pointsBroom.Successor);
                UpdateMinDist(current, leftBound, closestPairs, ref sqrdMinDist, pointsBroom.Predecessor);
                UpdateActivePoints(current, ref firstActive, minDist, pointsBroom);

                pointsBroom.Add(current);
            }
            while (successorExist);

            minDist = Math.Sqrt(sqrdMinDist);
            return new ClosestPointsPairResult(closestPairs, minDist);
        }

        public ClosestPointsPairResult ClosestPairNoDuplicates(List<Point> points)
        {
            List<PointsPair> closestPairs = new List<PointsPair>();
            double minDist = Double.PositiveInfinity;

            if (points.Count <= 1)
            {
                closestPairs.Add(new PointsPair(Point.SWInfinity, Point.NEInfinity));
                return new ClosestPointsPairResult(closestPairs, minDist);
            }

            TreeSet<Point> pointsQueue = new TreeSet<Point>(new PointsXYComparer());
            points.ForEach(p => pointsQueue.Add(p)); // adds also duplicates, because of bag sementic
            Point rightBound = new Point(Double.NegativeInfinity, Double.PositiveInfinity);
            Point leftBound = new Point(Double.NegativeInfinity, Double.NegativeInfinity);
            TreeSet<Point> pointsBroom = new TreeSet<Point>(new PointsYXComparer())
            {
                rightBound,
                leftBound
            };
            Point current;
            Point next = pointsQueue.FindMin();
            Point firstActive = next;
            double sqrdMinDist = Double.PositiveInfinity;
            bool successorExist;
            do
            {
                current = next;

                pointsQueue.DeleteMin();
                successorExist = pointsQueue.Count > 0;
                if (successorExist)
                    next = pointsQueue.FindMin();

                UpdateMinDist(current, rightBound, closestPairs, ref sqrdMinDist, pointsBroom.Successor);
                UpdateMinDist(current, leftBound, closestPairs, ref sqrdMinDist, pointsBroom.Predecessor);
                UpdateActivePoints(current, ref firstActive, minDist, pointsBroom);

                pointsBroom.Add(current);
            }
            while (successorExist);
            
            minDist = Math.Sqrt(sqrdMinDist);
            return new ClosestPointsPairResult(closestPairs, minDist);
        }

        private delegate TPoint getNeighborPoint<TPoint>(TPoint point);
        
        private void UpdateMinDist(Point current, Point boundPoint, List<PointsPair> minDistPair, ref double sqrdMinDist, getNeighborPoint<Point> neighborFunc)
        {
            Point neighborPoint = neighborFunc(current);
            int i = 0;
            double sqrdDist;
            while (!neighborPoint.CoordinatesEqual(boundPoint) && i < 4)
            {
                sqrdDist = current.SquaredDistanceFrom(neighborPoint);
                
                if (sqrdDist.IsAlmostEqualTo(sqrdMinDist))
                {
                    minDistPair.Add(new PointsPair(neighborPoint, current));
                }
                
                else if (sqrdDist < sqrdMinDist)
                {
                    sqrdMinDist = sqrdDist;
                    minDistPair.Clear();
                    minDistPair.Add(new PointsPair(neighborPoint, current));
                    //TPoint nextLeft;
                    // zawsze fałszywe, bo używam TreeSetu
                    /*
                    while ((nextLeft = neighborFunc(sidePoint)).ContentEquals(sidePoint))
                    {
                        minDistPair.Add(new Tuple<TPoint, TPoint>(current, nextLeft));
                        sidePoint = nextLeft;
                    }
                    */
                }
                neighborPoint = neighborFunc(neighborPoint);
                i++; // zawsze dozwolone, bo miotła nie zawiera duplikatów
            }
        }

        private void UpdateActivePoints(Point current, ref Point firstActive, double minDist, TreeSet<Point> pointsBroom)
        {
            Point candidateToDelete = firstActive;
            while(current.X - candidateToDelete.X > minDist)
            {
                firstActive = pointsBroom.Successor(candidateToDelete);
                pointsBroom.Remove(candidateToDelete);
                candidateToDelete = firstActive;
            }
        }

        /*
        //to też może zwracać listę, kiedy np. dwie różne pary mają tą samą odległość
        public Tuple<TPoint, TPoint> MinDistPairNoDuplicates<TPoint>(List<TPoint> points) where TPoint : Point
        {
            Tuple<TPoint, TPoint> tuple = new Tuple<TPoint, TPoint>(points[0], points[1]);
            TPoint p1 = tuple.Item1;
            return null;
        }

        public List<Tuple<TPoint, TPoint>> MinDistPairAllowDuplicates<TPoint>(List<TPoint> points) where TPoint : UniquePoint
        {
            return new List<Tuple<TPoint, TPoint>>() { Tuple.Create<TPoint, TPoint>(points[0], points[1]) };
        }

        public bool AnySegmentsIntersect<TPoint, TSegment>(TPoint p, TSegment s) where TPoint : Point where TSegment : LineSegment
        {
            bool q = p.LiesToTheLeftOf(s);
            return false;
        }
        */

        public ClosestPointsPairResult ClosestPairBruteForce(List<Point> points)
        {
            double minDist = Double.PositiveInfinity;
            List<PointsPair> closestPairs = new List<PointsPair>();
            double pointsCount = points.Count;

            if (pointsCount <= 1)
            {
                closestPairs.Add(new PointsPair(Point.SWInfinity, Point.NEInfinity));
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
                    //if (!sqrdDist.IsAlmostEqualToZero())
                    //{
                        if (sqrdDist.IsAlmostEqualTo(sqrdMinDist))
                            closestPairs.Add(new PointsPair(p1, p2));
                        else if (sqrdDist < sqrdMinDist)
                        {
                            sqrdMinDist = sqrdDist;
                            closestPairs.Clear();
                            closestPairs.Add(new PointsPair(p1, p2));
                        }
                    //}
                }
            minDist = Math.Sqrt(sqrdMinDist);
            return new ClosestPointsPairResult(closestPairs, minDist);
        }

        public ClosestPointsPairResult ClosestPairRecursive(List<Point> points)
        {
            double minDist = Double.PositiveInfinity;
            List<PointsPair> closestPairs = new List<PointsPair>();

            if (points.Count <= 1)
            {
                closestPairs.Add(new PointsPair(Point.SWInfinity, Point.NEInfinity));
                return new ClosestPointsPairResult(closestPairs, minDist);
            }

            //List<Point> sortedByX = points.OrderBy(p => p.X).ToList();
            //List<Point> sortedByY = points.OrderBy(p => p.Y).ToList();

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

            List<Point> middleNeighborsSortedByY = sortedByX.Where(p => (p.X - middlePoint.X).IsLessThanOrAlmostEqualTo(result.MinDist)).ToList();
            foreach (Point yPoint in middleNeighborsSortedByY)
            {
                int count = middleNeighborsSortedByY.Count;
                for (int i = 0; i < 7 && i < count; i++)
                {
                    Point iNeighbor = middleNeighborsSortedByY[i]; 
                    double dist = yPoint.DistanceFrom(iNeighbor);
                    if (dist.IsAlmostEqualTo(result.MinDist))
                        result.PointsPairs.Add(new PointsPair(yPoint, iNeighbor));
                    else if (dist < result.MinDist)
                    {
                        result.MinDist = dist;
                        result.PointsPairs.Clear();
                        result.PointsPairs.Add(new PointsPair(yPoint, iNeighbor));
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
