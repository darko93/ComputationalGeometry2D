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
    public static class Geometry
    {
        public static ClosestPointsPairResult ClosestPointsPairSweepLine(List<Point> points) =>
            ClosestPairPoints.ClosestPair.Instance.SweepLine(points);

        public static ClosestPointsPairResult ClosestPointsPairDivideAndConquer(List<Point> points) =>
            ClosestPairPoints.ClosestPair.Instance.DivideAndConquer(points);

        public static ClosestPointsPairResult ClosestPointsPairBruteForce(List<Point> points) =>
            ClosestPairPoints.ClosestPair.Instance.BruteForce(points);

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
