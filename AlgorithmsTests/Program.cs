using System;

using ComputationalGeometry2D;

namespace AlgorithmsTests
{
    class Program
    {
        static void Main(string[] args)
        {
            //ClosestPairsAlgorithms();
            //AllPlaneAngularSort();
            //AlgorithmsTester.Instance.AllPlaneAngularSortConcretePoints();
            //HalfPlaneAngularSort();
            //AlgorithmsTester.Instance.HalfPlaneAngularSortConcretePoints();
            //AlgorithmsTester.Instance.AllPlaneAngularSortAllCases(100000);
            //MinByAngleTest();
            //TreeSetRangeFromTo();
            ConcreteSegmentIntersection();

            Console.ReadKey();
        }

        private static void ConvexHullAlgorithms()
        {
            long grahamScanTime, jarvisTime;
            int pointsCount = 100000;
            bool resultsAreEqual = AlgorithmsTester.Instance.ConvexHullGrahamScanAndJarvisTest(out grahamScanTime, out jarvisTime, pointsCount);
            Console.WriteLine($"Convex hull\npoints count = {pointsCount}\ngraham scan time = {grahamScanTime}\njarvis time = {jarvisTime}\nresults are equal = {resultsAreEqual}");
        }

        private static void ClosestPairsAlgorithms()
        {
            long sweepTime, recursiveTime;
            int pointsCount = 2000000;
            bool resultsAreEqual = AlgorithmsTester.Instance.ClosestPairSweepLineAndRecursiveTest(out sweepTime, out recursiveTime, pointsCount, 5000000);
            Console.WriteLine($"Closest pairs\npointsCount = {pointsCount}\nsweep time = {sweepTime}\nrecursiveTime = {recursiveTime}\nresults are equal = {resultsAreEqual}");
        }

        private static void AllPlaneAngularSort()
        {
            int pointsCount = 20000000;
            long halfPlaneTime, allPlaneTime, quadrantTime; 
            bool resultsAreEqual = AlgorithmsTester.Instance.AllPlaneAngularSort(out halfPlaneTime, out allPlaneTime, out quadrantTime, pointsCount);
            Console.WriteLine($"All plane angular sort\npoints count = {pointsCount}\nhalf plane comparer time = {halfPlaneTime}\nall plane comparer time = {allPlaneTime}\nquadrant comparer time = {quadrantTime}\nresults are equal = {resultsAreEqual}");
        }

        private static void HalfPlaneAngularSort()
        {
            int pointsCount = 20000000;
            long halfPlaneTime, quadrantTime;
            bool resultsAreEqual = AlgorithmsTester.Instance.HalfPlaneAngularSort(out halfPlaneTime, out quadrantTime, pointsCount);
            Console.WriteLine($"Half plane angular sort\npoints count = {pointsCount}\nhalf plane comparer time = {halfPlaneTime}\nquadrant comparer time = {quadrantTime}\nresults are equal = {resultsAreEqual}");
        }

        //private static void MinByAngleTest()
        //{
        //    long quadrantTime, halfPlaneTime;
        //    int pointsCount = 10000;
        //    AlgorithmsTester.Instance.MinByAngle(out quadrantTime, out halfPlaneTime, pointsCount);
        //    Console.WriteLine($"MinByAngle\npointsCount = {pointsCount}\nquadrantTime = {quadrantTime}\nhalf plane time = {halfPlaneTime}");
        //}

        private static void ConcreteSegmentIntersection()
        {
            foreach (Intersection intersection in AlgorithmsTester.Instance.ConcreteSegmentIntersection())
            {
                Console.WriteLine($"\nIntersection point: {intersection.Point}");
                Console.WriteLine("Intersecting segments:");
                foreach (LineSegment segment in intersection.Segments)
                    Console.WriteLine(segment);
            }
        }
    }
}
