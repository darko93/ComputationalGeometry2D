using System.Collections.Generic;

using ComparingDoubles;

namespace ComputationalGeometry2D.SegmentIntersection
{
    class EventPointComparer : Comparer<EventPoint>
    {
        public override int Compare(EventPoint event1, EventPoint event2)
        {
            Point p1 = event1.PointValue;
            Point p2 = event2.PointValue;

            if (!p1.Y.IsAlmostEqualTo(p2.Y))
                return p1.Y.CompareTo(p2.Y) * -1;
            else
            {
                if (p1.X.IsAlmostEqualTo(p2.X))
                    return 0;
                else return p1.X.CompareTo(p2.X);
            }
        }
    }
}
