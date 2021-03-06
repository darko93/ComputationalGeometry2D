﻿using System.Collections.Generic;

using ComparingDoubles;

namespace ComputationalGeometry2D.SegmentIntersection
{
    class SegmentsFromTopToBottomComparer : Comparer<LineSegment>
    {
        public override int Compare(LineSegment segment1, LineSegment segment2)
        {
            double segment1MaxY = segment1.MaxY;
            double segment2MaxY = segment2.MaxY;

            if (!segment1MaxY.IsAlmostEqualTo(segment2MaxY))
                return segment1MaxY.CompareTo(segment2MaxY) * -1;
            else
            {
                double segment1MinX = segment1.MinX;
                double segment2MinX = segment2.MinX;

                if (segment1MinX.IsAlmostEqualTo(segment2MinX))
                    return segment1.DeltaY.CompareTo(segment2.DeltaY);
                else return segment1MinX.CompareTo(segment2MinX);
            }
        }
    }
}
