using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ComputationalGeometry2D
{
    class AlgorithmsTester
    {
        private static AlgorithmsTester instance = new AlgorithmsTester();
        public static AlgorithmsTester Instance => instance;

        private GeometricAlgorithms geometry = new GeometricAlgorithms();
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

        public List<Point> HalfPlaneAngularSort()
        {
            Point pole = new Point(0.0, 0.0);
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
            points = geometry.HalfPlaneAngularSort(points, pole, AngularSortDirection.CounterClockwise, AngularSortStartLocation.PositiveX);
            return points;
        }

        public List<Point> AllPlaneAngularSort()
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
                new Point(4, 2),
                new Point(3, 3),
                new Point(4, -7),
                new Point(8, -2),
                new Point(1, 5)

            };
            points = geometry.AllPlaneAngularSort(points, pole, AngularSortDirection.CounterClockwise, AngularSortStartLocation.PositiveX);
            return points;
        }

        public bool ClosestPairSweepLineAndRecursiveTest(int pointsCount = 2000000, double coordBound = 300000.0)
        {
            List<Point> points = GetRandomDoublePoints(pointsCount, coordBound);
            stopWatch.Start();
            ClosestPointsPairResult sweepLineResult = geometry.ClosestPairSweepLine(points);
            long sweepLineTime = stopWatch.ElapsedMilliseconds;
            stopWatch.Restart();
            ClosestPointsPairResult recursiveResult = geometry.ClosestPairRecursive(points);
            long recursiveTime = stopWatch.ElapsedMilliseconds;
            stopWatch.Reset();
            bool resultsAreEqual = sweepLineResult.MinDist.IsAlmostEqualTo(recursiveResult.MinDist)
                && sweepLineResult.PointsPairs.Count == recursiveResult.PointsPairs.Count;
            return resultsAreEqual;
        }

        public bool ConvexHullGrahamScanAndJarvisTest(int pointsCount = 500000, double coordBound = 300000.0)
        {
            List<Point> points = GetRandomDoublePoints(pointsCount, coordBound);
            stopWatch.Start();
            Point[] convexHullGrahamScan = geometry.ConvexHullGrahamScan(points).ToArray();
            long grahamScanTime = stopWatch.ElapsedMilliseconds;
            stopWatch.Restart();
            Point[] convexHullJarvis = geometry.ConvexHullJarvis(points).ToArray();
            long jarvisTime = stopWatch.ElapsedMilliseconds;
            stopWatch.Reset();
            bool resultsAreEqual = true;
            int resultLength = convexHullGrahamScan.Length;
            if (resultLength != convexHullJarvis.Length)
                return false;
            for (int i = 0; i < resultLength; i++)
            {
                if (!convexHullGrahamScan[i].Equals(convexHullJarvis[i]))
                {
                    resultsAreEqual = false;
                    break;
                }
            }

            return resultsAreEqual;          
        }
    }
}
