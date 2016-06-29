using System.Collections.Generic;
using System.Linq;

using MoreLinq;
using ComputationalGeometry2D.Common;

namespace ComputationalGeometry2D.ConvexHull
{
    class ConvexHull
    {
        public static ConvexHull Instance { get; } = new ConvexHull();

        private ConvexHull() { }

        public Stack<Point> GrahamScan(ICollection<Point> points)
        {
            if (points.Count < 3)
                return GetSmallStack(points);

            Point minYPoint = points.MinBy(p => p, new PointsYXIDComparer(PointsIDOrder.Ascending));
            List<Point> sortedPoints = AngularSort.AngularSort.Instance.HalfPlane(points, minYPoint, AngularOrder.CounterClockwise, AngularSortStartLocation.PositiveX);
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
                while (nextToTheTop.LiesRightOf(segment));
                stack.Push(top); // Top was redundantly popped.
                stack.Push(iPoint);
            }
            return stack;
        }

        public Stack<Point> Jarvis(ICollection<Point> points)
        {
            if (points.Count < 3)
                return GetSmallStack(points);

            Stack<Point> stack = new Stack<Point>();
            PointsYXIDComparer pointsYXIDComparer = new PointsYXIDComparer(PointsIDOrder.Ascending);

            Point minYPoint = points.MinBy(p => p, pointsYXIDComparer);
            Point maxYPoint = points.MaxBy(p => p, pointsYXIDComparer);
            Point current = minYPoint;
            stack.Push(current);

            PointsAngularIDComparer pointsAngularIDComparer = new PointsAngularIDComparer(minYPoint, AngularOrder.CounterClockwise);

            while (!current.Equals(maxYPoint))
            {
                pointsAngularIDComparer.SetPole(current);
                current = points
                    .Where(p => pointsYXIDComparer.Compare(p, current) > 0)
                    .MinBy(p => p, pointsAngularIDComparer);
                stack.Push(current);
            }
            pointsYXIDComparer.SetIDOrder(PointsIDOrder.Descending);
            while (!current.Equals(minYPoint))
            {
                pointsAngularIDComparer.SetPole(current);
                current = points
                    .Where(p => pointsYXIDComparer.Compare(p, current) < 0)
                    .MinBy(p => p, pointsAngularIDComparer);
                stack.Push(current);
            }
            stack.Pop();

            return stack;
        }

        private Stack<Point> GetSmallStack(IEnumerable<Point> points)
        {
            Stack<Point> stack = new Stack<Point>();
            points.ForEach(p => stack.Push(p));
            return stack;
        }
    }
}
