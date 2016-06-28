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
        //http://www.cosc.canterbury.ac.nz/mukundan/cgeo/Sweep1.html

        public static ClosestPointsPairResult ClosestPairSweepLine(List<Point> points) =>
            ClosestPair.ClosestPair.Instance.SweepLine(points);

        public static ClosestPointsPairResult ClosestPairDivideAndConquer(List<Point> points) =>
            ClosestPair.ClosestPair.Instance.DivideAndConquer(points);

        public static ClosestPointsPairResult ClosestPairBruteForce(List<Point> points) =>
            ClosestPair.ClosestPair.Instance.BruteForce(points);

        //private delegate Point getNeighborPoint(Point point);

        //private ClosestPointsPairResult NoClosestPair =>
        //    new ClosestPointsPairResult(new List<UnorderedPointsPair>()
        //    {
        //        new UnorderedPointsPair(
        //            new Point(Double.NegativeInfinity, Double.NegativeInfinity),
        //            new Point(Double.PositiveInfinity, Double.PositiveInfinity))
        //    }, Double.PositiveInfinity);

        //// For large input (probably > 10000) this algorithm is faster than recursive version, although both are O(lg(n)).
        //// Note: The same instances of Point will not be considered as closest pair of distance 0.
        //// (because tree set will not accept duplicate instances)
        //// (tree bag will not help, because Successor() and Predecessor() will not find duplicates)
        //// For points of the same coordinates use different instances.
        //public ClosestPointsPairResult ClosestPairSweepLine(List<Point> points)
        //{
        //    TreeSet<Point> eventQueue = new TreeSet<Point>(new PointsXYIDComparer(PointsIDOrder.Ascending));
        //    points.ForEach(p => eventQueue.Add(p));

        //    if (eventQueue.Count <= 1)
        //        return NoClosestPair;

        //    TreeSet<Point> statusStructure = new TreeSet<Point>(new PointsYXIDComparer(PointsIDOrder.Ascending))
        //    {
        //        new Point(Double.NegativeInfinity, Double.NegativeInfinity) // Needed when MinDist = 0, to always allow find predecessor in status structure.
        //    };

        //    ClosestPointsPairResult result = new ClosestPointsPairResult(new List<UnorderedPointsPair>(), Double.PositiveInfinity);

        //    Point firstActive = eventQueue.FindMin();
        //    foreach (Point current in eventQueue) // Iterate over succeeding points.
        //    {
        //        if (result.MinDist.IsAlmostEqualToZero())
        //        {
        //            Point previous = current;
        //            // Adding all pairs of distance 0, which consist of "current" and predecessors in status structure.
        //            while ((previous = statusStructure.Predecessor(previous)).CoordinatesEqual(current))
        //                result.ClosestPairs.Add(new UnorderedPointsPair(previous, current));
        //        }
        //        else
        //            TryUpdateClosestPair(current, result, statusStructure);
        //        // Removing from status structure all points, which distance from "current" is gretaer than MinDist.
        //        while ((current.X - firstActive.X).IsGreaterThanAndNotAlmostEqualTo(result.MinDist))
        //        {
        //            statusStructure.Remove(firstActive);
        //            firstActive = eventQueue.Successor(firstActive);
        //        }
        //        statusStructure.Add(current);
        //    }
        //    return result;
        //}

        //private Point lowerBound = new Point(0.0, 0.0);
        //private Point upperBound = new Point(0.0, 0.0);

        //private void TryUpdateClosestPair(Point current, ClosestPointsPairResult result, TreeSet<Point> statusStructure)
        //{
        //    double minDist = result.MinDist;

        //    lowerBound.X = current.X - minDist;
        //    lowerBound.Y = current.Y - minDist;
        //    upperBound.X = current.X;
        //    upperBound.Y = current.Y + minDist;

        //    double dist;
        //    foreach (Point neighbor in statusStructure.RangeFromTo(lowerBound, upperBound))
        //    {
        //        dist = current.DistanceFrom(neighbor);
        //        if (dist.IsAlmostEqualTo(minDist))
        //            result.ClosestPairs.Add(new UnorderedPointsPair(neighbor, current));
        //        else if (dist < minDist)
        //        {
        //            result.MinDist = dist;
        //            result.ClosestPairs.Clear();
        //            result.ClosestPairs.Add(new UnorderedPointsPair(neighbor, current));
        //        }
        //    }
        //}

        //public ClosestPointsPairResult ClosestPairBruteForce(List<Point> points)
        //{
        //    double minDist = Double.PositiveInfinity;
        //    List<UnorderedPointsPair> closestPairs = new List<UnorderedPointsPair>();
        //    double pointsCount = points.Count;

        //    if (pointsCount <= 1)
        //    return NoClosestPair;

        //    double sqrdMinDist = Double.PositiveInfinity;
        //    double sqrdDist;
        //    for (int i = 0; i < pointsCount; i++)
        //        for (int j = i + 1; j < pointsCount; j++)
        //        {
        //            Point p1 = points[i];
        //            Point p2 = points[j];
        //            sqrdDist = p1.SquaredDistanceFrom(p2);

        //            if (sqrdDist.IsAlmostEqualTo(sqrdMinDist))
        //                closestPairs.Add(new UnorderedPointsPair(p1, p2));
        //            else if (sqrdDist < sqrdMinDist)
        //            {
        //                sqrdMinDist = sqrdDist;
        //                closestPairs.Clear();
        //                closestPairs.Add(new UnorderedPointsPair(p1, p2));
        //            }
        //        }
        //    minDist = Math.Sqrt(sqrdMinDist);
        //    return new ClosestPointsPairResult(closestPairs, minDist);
        //}

        //// For large input (probably > 10000) this algorithm is slower than sweep-line version, although both are O(nlg(n)).
        //// Note: It's recommended to not pass duplicates instances of "Point" within points list,
        //// because method may produce incorrect result (not enough 0-dist pairs) or throw an exception (if multiple same instances of Point lies on the middle (probably more than 2)). 
        //// If the same instances are contained in the list, specify it in "sameInstancesContainedInList" - they should be removed. For points of the same coordinates use different instances.
        //public ClosestPointsPairResult ClosestPairRecursive(List<Point> points, bool sameInstancesContainedInList = false)
        //{
        //    if (points.Count <= 1)
        //        return NoClosestPair;

        //    if (sameInstancesContainedInList)
        //        points = points.Distinct().ToList(); // Remove referenece equal points.

        //    PointsXYIDComparer pointsXYIDComparer = new PointsXYIDComparer(PointsIDOrder.Ascending);
        //    List<Point> sortedByX = points.OrderBy(p => p, pointsXYIDComparer).ToList();
        //    List<Point> sortedByY = points.OrderBy(p => p, new PointsYXIDComparer(PointsIDOrder.Ascending)).ToList();

        //    return ClosestPairRecursive(sortedByX, sortedByY, pointsXYIDComparer);
        //}

        //private ClosestPointsPairResult ClosestPairRecursive(List<Point> sortedByX, List<Point> sortedByY, PointsXYIDComparer pointsXYIDComparer)
        //{
        //    int pointsCount = sortedByX.Count;
        //    if (pointsCount <= 3)
        //        return ClosestPairBruteForce(sortedByX);

        //    int middle = pointsCount / 2;
        //    List<Point> leftSortedByX = sortedByX.Take(middle).ToList();
        //    List<Point> rightSortedByX = sortedByX.Skip(middle).ToList();

        //    Point middlePoint = sortedByX[middle];
        //    List<Point> leftSortedByY = new List<Point>();
        //    List<Point> rightSortedByY = new List<Point>();
        //    foreach (Point yPoint in sortedByY)
        //    {
        //        if (pointsXYIDComparer.Compare(yPoint, middlePoint) < 0)
        //            leftSortedByY.Add(yPoint);
        //        else rightSortedByY.Add(yPoint);
        //    }

        //    ClosestPointsPairResult leftResult = ClosestPairRecursive(leftSortedByX, leftSortedByY, pointsXYIDComparer);
        //    ClosestPointsPairResult rightResult = ClosestPairRecursive(rightSortedByX, rightSortedByY, pointsXYIDComparer);

        //    ClosestPointsPairResult result;
        //    if (leftResult.MinDist.IsAlmostEqualTo(rightResult.MinDist))
        //    {
        //        leftResult.ClosestPairs.AddRange(rightResult.ClosestPairs);
        //        result = leftResult;
        //    }
        //    else if (leftResult.MinDist < rightResult.MinDist)
        //        result = leftResult;
        //    else result = rightResult;

        //    List<Point> middleXNeighborsSortedByY = sortedByY.Where(p => Math.Abs(p.X - middlePoint.X).IsLessThanOrAlmostEqualTo(result.MinDist)).ToList();

        //    int last = middleXNeighborsSortedByY.Count - 1;
        //    for (int i = 0; i < last; i++)
        //    {
        //        Point iNeighbor = middleXNeighborsSortedByY[i];
        //        int j = i + 1;
        //        if (result.MinDist.IsAlmostEqualToZero())
        //        {
        //            Point iNeighborJSucc;
        //            while (j <= last && (iNeighborJSucc = middleXNeighborsSortedByY[j++]).CoordinatesEqual(iNeighbor))
        //            {
        //                UnorderedPointsPair pair = new UnorderedPointsPair(iNeighbor, iNeighborJSucc);
        //                if (!result.ClosestPairs.Contains(pair)) // Need to check it, because in the middle neighborhood can be closest pair, which was already added during "conquer".
        //                    result.ClosestPairs.Add(pair);
        //            }
        //        }
        //        else
        //        {
        //            int lastSucc = i + 7;
        //            if (lastSucc > last)
        //                lastSucc = last;
        //            for (; j <= lastSucc; j++)
        //            {
        //                Point iNeighborJSucc = middleXNeighborsSortedByY[j];
        //                double dist = iNeighbor.DistanceFrom(iNeighborJSucc);
        //                if (dist.IsAlmostEqualTo(result.MinDist))
        //                {
        //                    UnorderedPointsPair pair = new UnorderedPointsPair(iNeighbor, iNeighborJSucc);
        //                    if (!result.ClosestPairs.Contains(pair)) // Need to check it, because in the middle neighborhood can be closest pair, which was already added during "conquer".
        //                        result.ClosestPairs.Add(pair);
        //                }
        //                else if (dist < result.MinDist)
        //                {
        //                    result.MinDist = dist;
        //                    result.ClosestPairs.Clear();
        //                    result.ClosestPairs.Add(new UnorderedPointsPair(iNeighbor, iNeighborJSucc));
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}

        public static List<Point> HalfPlaneAngularSort(List<Point> points, Point pole, AngularOrder angularOrder, AngularSortStartLocation startLocation) =>
            AngularSort.AngularSort.Instance.HalfPlane(points, pole, angularOrder, startLocation);

        public static List<Point> AllPlaneAngularSort(List<Point> points, Point pole, AngularOrder angularOrder, AngularSortStartLocation startLocation) =>
            AngularSort.AngularSort.Instance.AllPlane(points, pole, angularOrder, startLocation);

        //public List<Point> HalfPlaneAngularSort(List<Point> points, Point pole, PointsAngularOrder direction, AngularSortStartLocation startLocation)
        //{
        //    HalfPlanePointsToQuadrantsAdder pointsToQuadrantsAdder = new HalfPlanePointsToQuadrantsAdder(pole, direction, startLocation);
        //    points.ForEach(p => pointsToQuadrantsAdder.Add(p));
        //    PointsAngularByOrientationIDComparer pointsAngularByOrientationIDComparer = new PointsAngularByOrientationIDComparer(pole, direction, PointsIDOrder.Ascending);
        //    List<Point> firstSortedQuadrant = pointsToQuadrantsAdder.FirstBySortOrderQuadrant.OrderBy(p => p, pointsAngularByOrientationIDComparer).ToList();
        //    List<Point> secondSortedQuadrant = pointsToQuadrantsAdder.SecondBySortOrderQuadrant.OrderBy(p => p, pointsAngularByOrientationIDComparer).ToList();
        //    return firstSortedQuadrant.Concat(secondSortedQuadrant).ToList();
        //}

        //public List<Point> AllPlaneAngularSort(List<Point> points, Point pole, PointsAngularOrder direction, AngularSortStartLocation startLocation)
        //{
        //    PointsToQuadrantsAdder pointsToQuadrantsAdder = new PointsToQuadrantsAdder(pole, direction, startLocation);
        //    points.ForEach(p => pointsToQuadrantsAdder.Add(p));
        //    PointsAngularByOrientationIDComparer pointsAngularByOrientationIDComparer = new PointsAngularByOrientationIDComparer(pole, direction, PointsIDOrder.Ascending);
        //    List<Point> firstSortedQuadrant = pointsToQuadrantsAdder.FirstBySortOrderQuadrant.OrderBy(p => p, pointsAngularByOrientationIDComparer).ToList();
        //    List<Point> secondSortedQuadrant = pointsToQuadrantsAdder.SecondBySortOrderQuadrant.OrderBy(p => p, pointsAngularByOrientationIDComparer).ToList();
        //    List<Point> thirdSortedQuadrant = pointsToQuadrantsAdder.ThirdBySortOrderQuadrant.OrderBy(p => p, pointsAngularByOrientationIDComparer).ToList();
        //    List<Point> fourthSortedQuadrant = pointsToQuadrantsAdder.FourthBySortOrderQuadrant.OrderBy(p => p, pointsAngularByOrientationIDComparer).ToList();
        //    return firstSortedQuadrant.Concat(secondSortedQuadrant).Concat(thirdSortedQuadrant).Concat(fourthSortedQuadrant).ToList();
        //}

        public static Stack<Point> ConvexHullGrahamScan(List<Point> points) =>
            ConvexHull.ConvexHull.Instance.GrahamScan(points);

        public static Stack<Point> ConvexHullJarvis(List<Point> points) =>
            ConvexHull.ConvexHull.Instance.Jarvis(points);

        //public Stack<Point> ConvexHullGrahamScan(List<Point> points)
        //{
        //    Point minYPoint = points.MinBy(p => p, new PointsYXIDComparer(PointsIDOrder.Ascending));
        //    List<Point> sortedPoints = HalfPlaneAngularSort(points, minYPoint, PointsAngularOrder.CounterClockwise, AngularSortStartLocation.PositiveX);
        //    Stack<Point> stack = new Stack<Point>();
        //    stack.Push(minYPoint);
        //    stack.Push(sortedPoints[1]);
        //    stack.Push(sortedPoints[2]);

        //    int pointsCount = sortedPoints.Count;
        //    LineSegment segment = new LineSegment();
        //    for (int i = 3; i < pointsCount; i++)
        //    {
        //        Point iPoint = sortedPoints[i];
        //        segment.End = iPoint;
        //        Point top, nextToTheTop;
        //        do
        //        {
        //            top = stack.Pop();
        //            nextToTheTop = stack.Peek();
        //            segment.Start = top;
        //        }
        //        while (nextToTheTop.LiesToTheRightOf(segment));
        //        stack.Push(top); // Top was redundantly popped.
        //        stack.Push(iPoint);
        //    }
        //    return stack;
        //}

        //public Stack<Point> ConvexHullJarvis(List<Point> points)
        //{
        //    Stack<Point> stack = new Stack<Point>();

        //    PointsYXIDComparer pointsYXIDComparer = new PointsYXIDComparer(PointsIDOrder.Ascending);

        //    Point minYPoint = points.MinBy(p => p, pointsYXIDComparer);
        //    Point maxYPoint = points.MaxBy(p => p, pointsYXIDComparer);
        //    Point current = minYPoint;
        //    stack.Push(current);

        //    PointsAngularByOrientationIDComparer pointsAngularByOrientationIDComparer = new PointsAngularByOrientationIDComparer(minYPoint, PointsAngularOrder.CounterClockwise);

        //    while (!current.Equals(maxYPoint))
        //    {
        //        pointsAngularByOrientationIDComparer.SetPole(current);
        //        current = points
        //            .Where(p => pointsYXIDComparer.Compare(p, current) > 0)
        //            .MinBy(p => p, pointsAngularByOrientationIDComparer);
        //        stack.Push(current);
        //    }
        //    pointsYXIDComparer.SetIDOrder(PointsIDOrder.Descending);
        //    while (!current.Equals(minYPoint))
        //    {
        //        pointsAngularByOrientationIDComparer.SetPole(current);
        //        current = points
        //            .Where(p => pointsYXIDComparer.Compare(p, current) < 0)
        //            .MinBy(p => p, pointsAngularByOrientationIDComparer);
        //        stack.Push(current);
        //    }
        //    stack.Pop();

        //    return stack;
        //}

        public static List<Intersection> SegmentIntersectionSweepLine(List<LineSegment> segments) =>
            SegmentIntersection.SegmentIntersection.Instance.SweepLine(segments);
        //// This algorithm assumes, that segments do not overlap. Overlaping segments will not be considered as intersecting.
        //// If list contains segments of identical endpoints, then algorithm will consider only one of them.
        //public List<Intersection> SegmentIntersectionSweepLine(List<LineSegment> segments)
        //{
        //    List<Intersection> result = new List<Intersection>();

        //    TreeSet<EventPoint> eventQueue = new TreeSet<EventPoint>(new EventPointComparer(), new EventPointEqualityComparer());
        //    foreach (LineSegment segment in segments)
        //    {
        //        Point segmentStart = segment.Start;
        //        Point segmentEnd = segment.End;

        //        EventPoint eventStartPoint = new EventPoint(segmentStart);
        //        EventPoint eventEndPoint = new EventPoint(segmentEnd);

        //        LineSegmentNode segmentNode = new LineSegmentNode(segment);

        //        bool segmentStartPointIsEarlier = false;
        //        if (segmentStart.Y.IsGreaterThanAndNotAlmostEqualTo(segmentEnd.Y) || (segmentStart.Y.IsAlmostEqualTo(segmentEnd.Y) && segmentStart.X.IsLessThanOrAlmostEqualTo(segmentEnd.X)))
        //            segmentStartPointIsEarlier = true;

        //        eventQueue.FindOrAdd(ref eventStartPoint);
        //        eventQueue.FindOrAdd(ref eventEndPoint);

        //        if (segmentStartPointIsEarlier)
        //        {
        //            eventStartPoint.UpperFor.Add(segmentNode);
        //            eventEndPoint.LowerFor.Add(segmentNode);
        //        }
        //        else
        //        {
        //            eventStartPoint.LowerFor.Add(segmentNode);
        //            eventEndPoint.UpperFor.Add(segmentNode);
        //        }
        //    }

        //    SegmentsBySweepLineIntersectionComparer segmentsComparer = new SegmentsBySweepLineIntersectionComparer(Double.PositiveInfinity);
        //    TreeSet<LineSegmentNode> sweepLine = new TreeSet<LineSegmentNode>(segmentsComparer); // Sweep line status structure.
        //    // Below tree will allow to eliminate duplicate segments from the sets of intersecting segments associated with the event point.
        //    TreeSet<LineSegment> distinctEventSegments = new TreeSet<LineSegment>(new SegmentsFromTopToBottomComparer());
        //    List<LineSegmentNode> currentEventUpperForAndIntersectionFor = new List<LineSegmentNode>();


        //    double previousEventPointY = eventQueue.FindMin().PointValue.Y;
        //    double oldSweepLineY = previousEventPointY;
        //    while (eventQueue.Count > 0) 
        //    {
        //        EventPoint eventPoint = eventQueue.FindMin();
        //        // HandleEventPoint()
        //        Point eventPointValue = eventPoint.PointValue;

        //        if (!previousEventPointY.IsAlmostEqualTo(eventPointValue.Y))
        //            oldSweepLineY = previousEventPointY;

        //        segmentsComparer.SweepLineY = eventPointValue.Y; // Move sweep line to the current event point.

        //        int upperCount = eventPoint.UpperFor.Count;
        //        int intersectionCount = eventPoint.IntersectionFor.Count;
        //        int downCount = eventPoint.LowerFor.Count;

        //        // If more than one segment are associated with current event point, then mark those segmnets as intersecting and event point as intersection.
        //        if (upperCount + intersectionCount + downCount > 1)
        //        {
        //            // Adding segments to below tree eliminates duplicate segments from the sets of intersecting segments associated with the event point
        //            // Duplicates can appear in the following cases:
        //            // - Single intersection together with it's intersecting segments can sometimes be found more than once by the algorithm.
        //            // - Single event point can be intersecting point and at the same time upper or lower point for some segment.
        //            // - Input list contains duplicate segments (which violates algorithm assumption, that segments do not overlap).
        //            distinctEventSegments.Clear();

        //            eventPoint.UpperFor.ForEach(s => distinctEventSegments.Add(s.Segment));
        //            eventPoint.IntersectionFor.ForEach(s => distinctEventSegments.Add(s.Segment));
        //            eventPoint.LowerFor.ForEach(s => distinctEventSegments.Add(s.Segment));
        //            if (distinctEventSegments.Count > 1)
        //                result.Add(new Intersection(eventPointValue, distinctEventSegments.ToArray()));
        //        }

        //        eventPoint.LowerFor.ForEach(s => sweepLine.Remove(s));
        //        // Increase sweep line for a moment to provide correct removing (segments with sweep line intersections a little above are needed to 
        //        // save correct order in the tree and to allow remove proper segments)
        //        segmentsComparer.SweepLineY = oldSweepLineY;
        //        eventPoint.IntersectionFor.ForEach(s => sweepLine.Remove(s));
        //        segmentsComparer.SweepLineY = eventPointValue.Y;
                
        //        eventPoint.UpperFor.ForEach(s => sweepLine.Add(s));
        //        eventPoint.IntersectionFor.ForEach(s => sweepLine.Add(s));

        //        if (upperCount + intersectionCount == 0)
        //        {
        //            // Considering only segments associated with current event point, for which current event point is lower point.
        //            // Sweep line actually isn't containing any segments for which this event point is lower or intersection point,
        //            // so we can take first segment, for which current event is lower point and find segments to the left and right 
        //            // of this segment in sweep line - they will be to the left and right of current event.
        //            LineSegmentNode eventSegment = eventPoint.LowerFor[0];
        //            LineSegmentNode leftOfEvent;
        //            bool leftExist = sweepLine.TryPredecessor(eventSegment, out leftOfEvent);
        //            if (leftExist)
        //            {
        //                LineSegmentNode rightOfEvent;
        //                bool rightExist = sweepLine.TrySuccessor(eventSegment, out rightOfEvent);
        //                if (rightExist)
        //                    TryFindNewEvent(leftOfEvent, rightOfEvent, eventPoint.PointValue, eventQueue);
        //            }
        //        }
        //        else
        //        {
        //            currentEventUpperForAndIntersectionFor.Clear();
        //            eventPoint.UpperFor.ForEach(s => currentEventUpperForAndIntersectionFor.Add(s));
        //            eventPoint.IntersectionFor.ForEach(s => currentEventUpperForAndIntersectionFor.Add(s));

        //            // Lower sweep line for a moment to avoid additional calculations during comparing segments.
        //            segmentsComparer.SweepLineY -= 1;
        //            LineSegmentNode eventLeftmost = currentEventUpperForAndIntersectionFor.MinBy(s => s, segmentsComparer);
        //            segmentsComparer.SweepLineY += 1;
        //            LineSegmentNode leftOfEventLeftmost;
        //            bool leftExist = sweepLine.TryPredecessor(eventLeftmost, out leftOfEventLeftmost);
        //            if (leftExist)
        //                TryFindNewEvent(leftOfEventLeftmost, eventLeftmost, eventPoint.PointValue, eventQueue);

        //            // Lower sweep line for a moment to avoid additional calculations during comparing segments.
        //            segmentsComparer.SweepLineY -= 1;
        //            LineSegmentNode eventRightmost = currentEventUpperForAndIntersectionFor.MaxBy(s => s, segmentsComparer);
        //            segmentsComparer.SweepLineY += 1;
        //            LineSegmentNode rightOfEventRightmost;
        //            bool rightExist = sweepLine.TrySuccessor(eventRightmost, out rightOfEventRightmost);
        //            if (rightExist)
        //                TryFindNewEvent(eventRightmost, rightOfEventRightmost, eventPoint.PointValue, eventQueue);
        //        }
        //        previousEventPointY = eventPointValue.Y;
        //        eventQueue.DeleteMin();
        //    }
        //    return result;
        //}

        //private void TryFindNewEvent(LineSegmentNode left, LineSegmentNode right, Point eventPointValue, TreeSet<EventPoint> eventQueue)
        //{
        //    if (left.Segment.IntersectsWith(right.Segment))
        //    {
        //        Point intersection = left.Line.TryIntersection(right.Line);
        //        if (intersection != null) // If segments do not overlap.
        //        {
        //            if (intersection.Y.IsLessThanAndNotAlmostEqualTo(eventPointValue.Y) || (intersection.Y.IsAlmostEqualTo(eventPointValue.Y) && intersection.X.IsGreaterThanOrAlmostEqualTo(eventPointValue.X)))
        //            {
        //                EventPoint newEventPoint = new EventPoint(intersection);
        //                eventQueue.FindOrAdd(ref newEventPoint);
        //                List<LineSegmentNode> intersectionFor = newEventPoint.IntersectionFor;
        //                intersectionFor.Add(left);
        //                intersectionFor.Add(right);
        //            }
        //        }
        //    }
        //}
    }
}
