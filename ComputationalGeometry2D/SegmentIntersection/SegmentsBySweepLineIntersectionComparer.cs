using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComparingDoubles;

namespace ComputationalGeometry2D.SegmentIntersection
{
    class SegmentsBySweepLineIntersectionComparer : Comparer<LineSegmentNode>
    {
        private double sweepLineY = Double.PositiveInfinity;
        private double sweepLineALittleBelowY = Double.PositiveInfinity;
        private const double difference = 1;

        public double SweepLineY
        {
            get { return sweepLineY; }
            set
            {
                sweepLineY = value;
                sweepLineALittleBelowY = value - difference;
            }
        }

        public SegmentsBySweepLineIntersectionComparer(double sweepLineY)
        {
            SweepLineY = sweepLineY;
        }

        public override int Compare(LineSegmentNode segmentNode1, LineSegmentNode segmentNode2)
        {
            bool segment1IsHorizontal = segmentNode1.Line.IsHorizontal();
            bool segment2IsHorizontal = segmentNode2.Line.IsHorizontal();

            // Handle cases, when segments are horizontal.
            // Horizontal segment is considered as less.
            // Both segments can be horizontal only, if they lay on the sweep line. In that case we need to compare segments min X.
            if (segment1IsHorizontal)
            {
                if (segment2IsHorizontal)
                    return segmentNode1.Segment.MinX.CompareTo(segmentNode2.Segment.MinX);
                return -1;
            }
            if (segment2IsHorizontal)
                return 1;

            double intersection1X = segmentNode1.GetLineX(SweepLineY);
            double intersection2X = segmentNode2.GetLineX(SweepLineY);

            if (intersection1X.IsAlmostEqualTo(intersection2X)) // If segments intersects sweep line in the same point.
            {
                // Compare segments and sweep line intersections X a little below.
                intersection1X = segmentNode1.GetLineX(sweepLineALittleBelowY);
                intersection2X = segmentNode2.GetLineX(sweepLineALittleBelowY);
                // We assume, that line segments don't overlap, so intersections a liitle below cannot be equal.
            }
            return intersection1X.CompareTo(intersection2X);
        }
    }
}
