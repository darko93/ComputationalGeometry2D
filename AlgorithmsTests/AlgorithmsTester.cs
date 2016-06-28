using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using ComparingDoubles;

using ComputationalGeometry2D;

namespace AlgorithmsTests
{
    class AlgorithmsTester
    {
        public static AlgorithmsTester Instance { get; } = new AlgorithmsTester();
        private AlgorithmsTester() { }
        
        private TestonlyAlgorithms testGeometry = new TestonlyAlgorithms();
        private Stopwatch stopWatch = new Stopwatch();

        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        private static double RandomDoubleNumber(double bound)
        {
            lock (syncLock)
            { // synchronize
                return (random.NextDouble() * 2 - 1) * bound;
            }
        }
        private static int RandomIntNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }

        private List<Point> GetRandomIntPoints(int amount, int coordBound)
        {
            List<Point> randomPoints = new List<Point>();
            for (int i = 0; i < amount; i++)
                randomPoints.Add(new Point(RandomIntNumber(-coordBound, coordBound), RandomIntNumber(-coordBound, coordBound)));
            return randomPoints;
        }

        private List<Point> GetRandomDoublePoints(int amount, double coordBound)
        {
            List<Point> randomPoints = new List<Point>();
            for (int i = 0; i < amount; i++)
                randomPoints.Add(new Point(RandomDoubleNumber(coordBound), RandomDoubleNumber(coordBound)));
            return randomPoints;
        }

        public bool HalfPlaneAngularSortConcretePoints()
        {
            Point pole = new Point(0, 5);//new Point(0.0, 0.0);
            List<Point> points = new List<Point>()
            {
                new Point(-5, 5),
                new Point(1, 1),
                new Point(5, 1),
                new Point(-10, 1),
                new Point(4, 2),
                new Point(3, 3),
                new Point(1, 5)

            };
            List<Point> pointsHalfPlaneComparer = testGeometry.HalfPlaneAngularSortHalfPlaneComparer(points, pole, AngularOrder.CounterClockwise, AngularSortStartLocation.NegativeY);
            List<Point> pointsQuadrantComparer = Geometry.HalfPlaneAngularSort(points, pole, AngularOrder.CounterClockwise, AngularSortStartLocation.NegativeY);
            bool resultsAreEqual = CheckPointsListsEquality(pointsHalfPlaneComparer, pointsQuadrantComparer);
            return resultsAreEqual;
        }

        public bool AllPlaneAngularSortConcretePoints()
        {
            Point pole = new Point(0.0, 0.0);
            List<Point> points = new List<Point>()
            {
                new Point(-5, 5),
                new Point(1, 1),
                new Point(5, 1),
                new Point(-7, -1),
                new Point(-4, -5),
                new Point(-10, 1),
                pole,
                new Point(4, 2),
                new Point(3, 3),
                new Point(4, -7),
                new Point(8, -2),
                new Point(1, 5)

            };
            AngularOrder angularOrder = AngularOrder.Clockwise;
            AngularSortStartLocation startLocation = AngularSortStartLocation.PositiveY;
            List<Point> pointsHalfPlaneComparer = testGeometry.AllPlaneAngularSortHalfPlaneComparer(points, pole, angularOrder, startLocation);
            List<Point> pointsAllPlaneComparer = testGeometry.AllPlaneAngularSortAllPlaneComparer(points, pole, angularOrder, startLocation);
            List<Point> pointsQuadrantComparer = Geometry.AllPlaneAngularSort(points, pole, angularOrder, startLocation);

            bool resultsAreEqual = CheckPointsListsEquality(pointsHalfPlaneComparer, pointsAllPlaneComparer);
            if (!resultsAreEqual)
                return false;
            resultsAreEqual = CheckPointsListsEquality(pointsAllPlaneComparer, pointsQuadrantComparer);
            return resultsAreEqual;
        }

        public bool AllPlaneAngularSort(out long halfPlaneTime, out long allPlaneTime, out long quadrantTime, int pointsCount = 1000000)
        {
            List<Point> points = GetRandomDoublePoints(pointsCount, 1000);
            Point pole = new Point(0.0, 0.0);
            AngularOrder angularOrder = AngularOrder.Clockwise;
            AngularSortStartLocation startLocation = AngularSortStartLocation.PositiveX;
            stopWatch.Start();
            List<Point> pointsHalfPlaneComparer = testGeometry.AllPlaneAngularSortHalfPlaneComparer(points, pole, angularOrder, startLocation);
            halfPlaneTime = stopWatch.ElapsedMilliseconds;
            stopWatch.Restart();
            List<Point> pointsAllPlaneComparer = testGeometry.AllPlaneAngularSortAllPlaneComparer(points, pole, angularOrder, startLocation);
            allPlaneTime = stopWatch.ElapsedMilliseconds;
            stopWatch.Restart();
            List<Point> pointsQuadrantComparer = Geometry.AllPlaneAngularSort(points, pole, angularOrder, startLocation);
            quadrantTime = stopWatch.ElapsedMilliseconds;
            stopWatch.Stop();
            bool resultsAreEqual = CheckPointsListsEquality(pointsAllPlaneComparer, pointsQuadrantComparer);
            return resultsAreEqual;
        }

        private bool CheckPointsListsEquality(List<Point> points1, List<Point> points2)
        {
            int pointsCount = points1.Count;
            for (int i = 0; i < pointsCount; i++)
            {
                if (!points1[i].Equals(points2[i]))
                    return false;
            }
            return true;
        }

        private bool AngularSortOneCase(List<Point> points, Point pole, AngularOrder angularOrder, AngularSortStartLocation startLocation)
        {
            List<Point> pointsAllPlaneComperer = testGeometry.AllPlaneAngularSortAllPlaneComparer(points, pole, angularOrder, startLocation);
            List<Point> pointsQuadrantComparer = Geometry.AllPlaneAngularSort(points, pole, angularOrder, startLocation);
            return CheckPointsListsEquality(pointsAllPlaneComperer, pointsQuadrantComparer);
        }

        public bool AllPlaneAngularSortAllCases(int pointsCount = 10000)
        {
            List<Point> points = GetRandomDoublePoints(pointsCount, pointsCount);
            Point pole = points[points.Count / 2];
            if (!AngularSortOneCase(points, pole, AngularOrder.CounterClockwise, AngularSortStartLocation.PositiveX))
                return false;
            if (!AngularSortOneCase(points, pole, AngularOrder.CounterClockwise, AngularSortStartLocation.PositiveY))
                return false;
            if (!AngularSortOneCase(points, pole, AngularOrder.CounterClockwise, AngularSortStartLocation.NegativeX))
                return false;
            if (!AngularSortOneCase(points, pole, AngularOrder.CounterClockwise, AngularSortStartLocation.NegativeY))
                return false;
            if (!AngularSortOneCase(points, pole, AngularOrder.Clockwise, AngularSortStartLocation.PositiveX))
                return false;
            if (!AngularSortOneCase(points, pole, AngularOrder.Clockwise, AngularSortStartLocation.PositiveY))
                return false;
            if (!AngularSortOneCase(points, pole, AngularOrder.Clockwise, AngularSortStartLocation.NegativeX))
                return false;
            if (!AngularSortOneCase(points, pole, AngularOrder.Clockwise, AngularSortStartLocation.NegativeY))
                return false;
            return true;
        }

        public bool HalfPlaneAngularSort(out long halfComparerTime, out long quadrantComparerTime, int pointsCount = 10000000)
        {
            List<Point> points = GetRandomDoublePoints(pointsCount, pointsCount);
            AngularOrder angularOrder = AngularOrder.CounterClockwise;
            AngularSortStartLocation startLocation = AngularSortStartLocation.PositiveX;
            Point pole = new Point(0.0, 0.0);
            points.Add(pole);
            stopWatch.Start();
            List<Point> pointsHalfPlaneComparer = testGeometry.HalfPlaneAngularSortHalfPlaneComparer(points, pole, angularOrder, startLocation);
            halfComparerTime = stopWatch.ElapsedMilliseconds;
            stopWatch.Restart();
            List<Point> pointsQuadrant = Geometry.HalfPlaneAngularSort(points, pole, angularOrder, startLocation);
            quadrantComparerTime = stopWatch.ElapsedMilliseconds;
            bool resultsAreEqual = CheckPointsListsEquality(pointsHalfPlaneComparer, pointsQuadrant);
            return resultsAreEqual;
        }


        public bool ClosestPairSweepLineAndRecursiveTest(out long sweepLineTime, out long recursiveTime, int pointsCount = 2000000, double coordBound = 3000000.0)
        {
            List<Point> points = GetRandomDoublePoints(pointsCount, coordBound);
            stopWatch.Start();
            ClosestPointsPairResult sweepLineResult = Geometry.ClosestPairSweepLine(points);
            sweepLineTime = stopWatch.ElapsedMilliseconds;
            stopWatch.Restart();
            ClosestPointsPairResult recursiveResult = Geometry.ClosestPairDivideAndConquer(points);
            recursiveTime = stopWatch.ElapsedMilliseconds;
            stopWatch.Reset();
            bool resultsAreEqual = sweepLineResult.MinDist.IsAlmostEqualTo(recursiveResult.MinDist)
                && sweepLineResult.ClosestPairs.Count == recursiveResult.ClosestPairs.Count;
            return resultsAreEqual;
        }

        public bool ConvexHullGrahamScanAndJarvisTest(out long grahamScanTime, out long jarvisTime, int pointsCount = 500000, double coordBound = 300000.0)
        {
            List<Point> points = GetRandomDoublePoints(pointsCount, coordBound);
            stopWatch.Start();
            List<Point> convexHullGrahamScan = Geometry.ConvexHullGrahamScan(points).ToList();
            grahamScanTime = stopWatch.ElapsedMilliseconds;
            stopWatch.Restart();
            List<Point> convexHullJarvis = Geometry.ConvexHullJarvis(points).ToList();
            jarvisTime = stopWatch.ElapsedMilliseconds;
            stopWatch.Reset();
            int resultLength = convexHullGrahamScan.Count;
            if (resultLength != convexHullJarvis.Count)
                return false;
            bool resultsAreEqual = CheckPointsListsEquality(convexHullGrahamScan, convexHullJarvis);
            return resultsAreEqual;
        }

        public List<Intersection> ConcreteSegmentIntersection()
        {
            List<LineSegment> segments = new List<LineSegment>()
            {
                new LineSegment(new Point(0.0, 0.0), new Point(5.0, 5.0)),
                //new LineSegment(new Point(0.0, 0.0), new Point(5.0, 5.0)), // duplicate
                new LineSegment(new Point(1.0, 4.0), new Point(4.0, 1.0)),
                new LineSegment(new Point(0.0, 3.0), new Point(5.0, 3.0)),
                new LineSegment(new Point(4.0, 5.0), new Point(5.0, 4.0)),
                new LineSegment(new Point(3.0, 5.0), new Point(6.0, 4.0)),
                new LineSegment(new Point(0.0, 2.5), new Point(5.0, 2.5)),
                new LineSegment(new Point(2.5, 3.0), new Point(2.5, 2.0)),
                new LineSegment(new Point(1.0, 3.0), new Point(2.5, 2.5)),
                new LineSegment(new Point(2.5, 2.5), new Point(4.0, 2.0)),

                ////new LineSegment(new Point(2, 4), new Point(5, 1)),
                ////new LineSegment(new Point(2, 2), new Point(5, 2)),
                ////new LineSegment(new Point(5, 2), new Point(4, 3)),
                
                //new LineSegment(new Point(0, 1), new Point(1, 0)),
                //new LineSegment(new Point(0, 1), new Point(5, 1)),
                //new LineSegment(new Point(2, 2), new Point(4, 0))
            };

            List<Intersection> result = Geometry.SegmentIntersectionSweepLine(segments);
            return result;            
        }
    }
}
