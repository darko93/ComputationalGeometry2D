using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D.SegmentIntersection
{
    class EventPointEqualityComparer : EqualityComparer<EventPoint>
    {
        public override bool Equals(EventPoint event1, EventPoint event2)
        {
            return event1.PointValue.CoordinatesEqual(event2.PointValue);
        }

        public override int GetHashCode(EventPoint eventPoint)
        {
            return eventPoint.PointValue.GetHashCode();
        }
    }
}
