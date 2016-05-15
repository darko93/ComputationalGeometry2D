using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D.SegmentIntersection
{
    class LineSegmentNode
    {
        public LineSegment Segment { get; set; }
        public Line Line { get; set; }

        public LineSegmentNode(LineSegment segment)
        {
            Segment = segment;
            Line line = new Line(segment);
            line.DivideByA();
            Line = line;
        }

        public double GetLineX(double y) =>
            -Line.B * y - Line.C;

        public override string ToString() =>
            Segment.ToString();
    }
}
