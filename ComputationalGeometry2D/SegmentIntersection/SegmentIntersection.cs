using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComparingDoubles;
using C5;
using MoreLinq;

namespace ComputationalGeometry2D.SegmentIntersection
{
    class SegmentIntersection
    {
        public static SegmentIntersection Instance { get; } = new SegmentIntersection();

        private SegmentIntersection() { }

        // This algorithm assumes, that segments do not overlap and doesn't work good with horizontal segments.
        // If list contains segments of identical endpoints, then algorithm will consider only one of them.
        public List<Intersection> SweepLine(IEnumerable<LineSegment> segments)
        {
            List<Intersection> result = new List<Intersection>();

            TreeSet<EventPoint> eventQueue = new TreeSet<EventPoint>(new EventPointComparer(), new EventPointEqualityComparer());
            foreach (LineSegment segment in segments)
            {
                Point segmentStart = segment.Start;
                Point segmentEnd = segment.End;

                EventPoint eventStartPoint = new EventPoint(segmentStart);
                EventPoint eventEndPoint = new EventPoint(segmentEnd);

                LineSegmentNode segmentNode = new LineSegmentNode(segment);

                bool segmentStartPointIsEarlier = false;
                if (segmentStart.Y.IsGreaterThanAndNotAlmostEqualTo(segmentEnd.Y) || (segmentStart.Y.IsAlmostEqualTo(segmentEnd.Y) && segmentStart.X.IsLessThanOrAlmostEqualTo(segmentEnd.X)))
                    segmentStartPointIsEarlier = true;

                eventQueue.FindOrAdd(ref eventStartPoint);
                eventQueue.FindOrAdd(ref eventEndPoint);

                if (segmentStartPointIsEarlier)
                {
                    eventStartPoint.UpperFor.Add(segmentNode);
                    eventEndPoint.LowerFor.Add(segmentNode);
                }
                else
                {
                    eventStartPoint.LowerFor.Add(segmentNode);
                    eventEndPoint.UpperFor.Add(segmentNode);
                }
            }

            if (eventQueue.Count <= 2)
                return result;

            SegmentsBySweepLineIntersectionComparer segmentsComparer = new SegmentsBySweepLineIntersectionComparer(Double.PositiveInfinity);
            TreeSet<LineSegmentNode> sweepLine = new TreeSet<LineSegmentNode>(segmentsComparer); // Sweep line status structure.
            // Below tree will allow to eliminate duplicate segments from the sets of intersecting segments associated with the event point.
            TreeSet<LineSegment> distinctEventSegments = new TreeSet<LineSegment>(new SegmentsFromTopToBottomComparer());
            List<LineSegmentNode> currentEventUpperForAndIntersectionFor = new List<LineSegmentNode>();

            double previousEventPointY = eventQueue.FindMin().PointValue.Y;
            double oldSweepLineY = previousEventPointY;
            while (eventQueue.Count > 0)
            {
                EventPoint eventPoint = eventQueue.FindMin();
                // HandleEventPoint()
                Point eventPointValue = eventPoint.PointValue;

                if (!previousEventPointY.IsAlmostEqualTo(eventPointValue.Y))
                    oldSweepLineY = previousEventPointY;

                segmentsComparer.SweepLineY = eventPointValue.Y; // Move sweep line to the current event point.

                int upperCount = eventPoint.UpperFor.Count;
                int intersectionCount = eventPoint.IntersectionFor.Count;
                int downCount = eventPoint.LowerFor.Count;

                // If more than one segment are associated with current event point, then mark those segmnets as intersecting and event point as intersection.
                if (upperCount + intersectionCount + downCount > 1)
                {
                    // Adding segments to below tree eliminates duplicate segments from the sets of intersecting segments associated with the event point
                    // Duplicates can appear in the following cases:
                    // - Single intersection together with it's intersecting segments can sometimes be found more than once by the algorithm.
                    // - Single event point can be intersecting point and at the same time upper or lower point for some segment.
                    // - Input list contains duplicate segments (which violates algorithm assumption, that segments do not overlap).
                    distinctEventSegments.Clear();

                    eventPoint.UpperFor.ForEach(s => distinctEventSegments.Add(s.Segment));
                    eventPoint.IntersectionFor.ForEach(s => distinctEventSegments.Add(s.Segment));
                    eventPoint.LowerFor.ForEach(s => distinctEventSegments.Add(s.Segment));
                    if (distinctEventSegments.Count > 1)
                        result.Add(new Intersection(eventPointValue, distinctEventSegments.ToArray()));
                }

                eventPoint.LowerFor.ForEach(s => sweepLine.Remove(s));
                // Increase sweep line for a moment to provide correct removing (segments with sweep line intersections a little above are needed to 
                // save correct order in the tree and to allow remove proper segments)
                segmentsComparer.SweepLineY = oldSweepLineY;
                eventPoint.IntersectionFor.ForEach(s => sweepLine.Remove(s));
                segmentsComparer.SweepLineY = eventPointValue.Y;

                eventPoint.UpperFor.ForEach(s => sweepLine.Add(s));
                eventPoint.IntersectionFor.ForEach(s => sweepLine.Add(s));

                if (upperCount + intersectionCount == 0)
                {
                    // Considering only segments associated with current event point, for which current event point is lower point.
                    // Sweep line actually isn't containing any segments for which this event point is lower or intersection point,
                    // so we can take first segment, for which current event is lower point and find segments to the left and right 
                    // of this segment in sweep line - they will be to the left and right of current event.
                    LineSegmentNode eventSegment = eventPoint.LowerFor[0];
                    LineSegmentNode leftOfEvent;
                    bool leftExist = sweepLine.TryPredecessor(eventSegment, out leftOfEvent);
                    if (leftExist)
                    {
                        LineSegmentNode rightOfEvent;
                        bool rightExist = sweepLine.TrySuccessor(eventSegment, out rightOfEvent);
                        if (rightExist)
                            TryFindNewEvent(leftOfEvent, rightOfEvent, eventPoint.PointValue, eventQueue);
                    }
                }
                else
                {
                    currentEventUpperForAndIntersectionFor.Clear();
                    eventPoint.UpperFor.ForEach(s => currentEventUpperForAndIntersectionFor.Add(s));
                    eventPoint.IntersectionFor.ForEach(s => currentEventUpperForAndIntersectionFor.Add(s));

                    // Lower sweep line for a moment to avoid additional calculations during comparing segments.
                    segmentsComparer.SweepLineY -= 1;
                    LineSegmentNode eventLeftmost = currentEventUpperForAndIntersectionFor.MinBy(s => s, segmentsComparer);
                    segmentsComparer.SweepLineY += 1;
                    LineSegmentNode leftOfEventLeftmost;
                    bool leftExist = sweepLine.TryPredecessor(eventLeftmost, out leftOfEventLeftmost);
                    if (leftExist)
                        TryFindNewEvent(leftOfEventLeftmost, eventLeftmost, eventPoint.PointValue, eventQueue);

                    // Lower sweep line for a moment to avoid additional calculations during comparing segments.
                    segmentsComparer.SweepLineY -= 1;
                    LineSegmentNode eventRightmost = currentEventUpperForAndIntersectionFor.MaxBy(s => s, segmentsComparer);
                    segmentsComparer.SweepLineY += 1;
                    LineSegmentNode rightOfEventRightmost;
                    bool rightExist = sweepLine.TrySuccessor(eventRightmost, out rightOfEventRightmost);
                    if (rightExist)
                        TryFindNewEvent(eventRightmost, rightOfEventRightmost, eventPoint.PointValue, eventQueue);
                }
                previousEventPointY = eventPointValue.Y;
                eventQueue.DeleteMin();
            }
            return result;
        }

        private void TryFindNewEvent(LineSegmentNode left, LineSegmentNode right, Point eventPointValue, TreeSet<EventPoint> eventQueue)
        {
            if (left.Segment.IntersectsWith(right.Segment))
            {
                Point intersection = left.Line.TryIntersection(right.Line);
                if (intersection != null) // If segments do not overlap.
                {
                    if (intersection.Y.IsLessThanAndNotAlmostEqualTo(eventPointValue.Y) || (intersection.Y.IsAlmostEqualTo(eventPointValue.Y) && intersection.X.IsGreaterThanOrAlmostEqualTo(eventPointValue.X)))
                    {
                        EventPoint newEventPoint = new EventPoint(intersection);
                        eventQueue.FindOrAdd(ref newEventPoint);
                        List<LineSegmentNode> intersectionFor = newEventPoint.IntersectionFor;
                        intersectionFor.Add(left);
                        intersectionFor.Add(right);
                    }
                }
            }
        }
    }
}
