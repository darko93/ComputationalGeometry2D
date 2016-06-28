using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComparingDoubles;

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
