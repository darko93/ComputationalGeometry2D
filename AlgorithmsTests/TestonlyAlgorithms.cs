using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComputationalGeometry2D;
using ComparingDoubles;
using MoreLinq;

namespace AlgorithmsTests
{
    class TestonlyAlgorithms
    {
        public List<Point> HalfPlaneAngularSortHalfPlaneComparer(List<Point> points, Point pole, AngularSortDirection direction, AngularSortStartLocation startLocation) =>
            points.OrderBy(p => p, new HalfPlanePointsAngularIDComparer(pole, startLocation, direction)).ToList();

        public List<Point> AllPlaneAngularSortAllPlaneComparer(List<Point> points, Point pole, AngularSortDirection direction, AngularSortStartLocation startLocation) =>
            points.OrderBy(p => p, new PointsAngularIDComparer(pole, startLocation, direction)).ToList();

        public List<Point> AllPlaneAngularSortHalfPlaneComparer(List<Point> points, Point pole, AngularSortDirection direction, AngularSortStartLocation startLocation)
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

            // poprawic bo zero ma nalezec do dej polowki co jest pierwsza sortowana
            if (startLocation == AngularSortStartLocation.PositiveX || startLocation == AngularSortStartLocation.NegativeX)
            {
                halfPlane1Points = points.Where(p => p.Y.IsGreaterThanOrAlmostEqualTo(pole.Y)).ToList();
                halfPlane2Points = points.Where(p => p.Y.IsLessThanAndNotAlmostEqualTo(pole.Y)).ToList();
            }
            else //if (startLocation == PolarSortStartLocation.PositiveY || startLocation == PolarSortStartLocation.NegativeY)
            {
                halfPlane1Points = points.Where(p => p.X.IsGreaterThanOrAlmostEqualTo(pole.X)).ToList();
                halfPlane2Points = points.Where(p => p.X.IsLessThanAndNotAlmostEqualTo(pole.X)).ToList();
            }
            halfPlane1Points = halfPlane1Points.OrderBy(p => p, new HalfPlanePointsAngularIDComparer(pole, startLocation, direction)).ToList();
            halfPlane2Points = halfPlane2Points.OrderBy(p => p, new HalfPlanePointsAngularIDComparer(pole, secondHalfPlaneStartLocation, direction)).ToList();
            halfPlane1Points.AddRange(halfPlane2Points); // kolejnosc laczenia trzebaby sprawdzac, bo dolna polowa moze byc pierwsza...
            return halfPlane1Points;
        }

        //public void MinByAngleTest(List<Point> points, AngularSortDirection direction, AngularSortStartLocation startLocation, out long quadrantTime, out long halfPlaneTime)
        //{
        //    System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
        //    Point pole = points[points.Count / 2];
        //    PointsAngularByOrientationIDComparer quadrantComparer = new PointsAngularByOrientationIDComparer(pole, direction);
        //    HalfPlanePointsAngularIDComparer halfPlaneComparer = new HalfPlanePointsAngularIDComparer(pole, startLocation, direction);
        //    Point pp;
        //    foreach (Point point in points)
        //        pp = points.MinBy(p => p, quadrantComparer);
        //    quadrantTime = sw.ElapsedMilliseconds;
        //    sw.Restart();
        //    foreach (Point point in points)
        //        pp = points.MinBy(p => p, halfPlaneComparer);
        //    halfPlaneTime = sw.ElapsedMilliseconds;
        //}
    }
}
