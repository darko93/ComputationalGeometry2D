using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    class AlgorithmsTester
    {
        private static AlgorithmsTester instance = new AlgorithmsTester();
        public static AlgorithmsTester Instance => instance;

        private GeometricAlgorithms geometry = new GeometricAlgorithms();

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
    }
}
