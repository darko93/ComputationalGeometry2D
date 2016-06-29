using System.Collections.Generic;

namespace ComputationalGeometry2D.SegmentIntersection
{
    class EventPoint
    {
        public Point PointValue;
   
        public List<LineSegmentNode> UpperFor { get; } = new List<LineSegmentNode>();
        public List<LineSegmentNode> IntersectionFor { get; } = new List<LineSegmentNode>();
        public List<LineSegmentNode> LowerFor { get; } = new List<LineSegmentNode>();

        public EventPoint(Point pointValue)
        {
            PointValue = pointValue;
        }

        public override string ToString() =>
            PointValue.ToString();
    }
}
