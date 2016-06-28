using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AlgorithmsTests")]

namespace ComputationalGeometry2D
{
    public static class Geometry
    {
        //http://www.cosc.canterbury.ac.nz/mukundan/cgeo/Sweep1.html

        public static ClosestPointsPairResult ClosestPointsPairSweepLine(List<Point> points) =>
            ClosestPairPoints.ClosestPointsPair.Instance.SweepLine(points);

        public static ClosestPointsPairResult ClosestPointsPairDivideAndConquer(List<Point> points) =>
            ClosestPairPoints.ClosestPointsPair.Instance.DivideAndConquer(points);

        public static ClosestPointsPairResult ClosestPointsPairBruteForce(List<Point> points) =>
            ClosestPairPoints.ClosestPointsPair.Instance.BruteForce(points);

        public static List<Point> HalfPlaneAngularSort(List<Point> points, Point pole, AngularOrder angularOrder, AngularSortStartLocation startLocation) =>
            AngularSort.AngularSort.Instance.HalfPlane(points, pole, angularOrder, startLocation);

        public static List<Point> AllPlaneAngularSort(List<Point> points, Point pole, AngularOrder angularOrder, AngularSortStartLocation startLocation) =>
            AngularSort.AngularSort.Instance.AllPlane(points, pole, angularOrder, startLocation);

        public static Stack<Point> ConvexHullGrahamScan(List<Point> points) =>
            ConvexHull.ConvexHull.Instance.GrahamScan(points);

        public static Stack<Point> ConvexHullJarvis(List<Point> points) =>
            ConvexHull.ConvexHull.Instance.Jarvis(points);

        public static List<Intersection> SegmentIntersectionSweepLine(List<LineSegment> segments) =>
            SegmentIntersection.SegmentIntersection.Instance.SweepLine(segments);
    }
}
