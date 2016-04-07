using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    class ClosestPointsPairResult
    {
        public List<UnorderedPointsPair> PointsPairs { get; internal set; } = new List<ComputationalGeometry2D.UnorderedPointsPair>();
        public double MinDist { get; internal set; }
        public ClosestPointsPairResult(List<UnorderedPointsPair> pointsPairs, double minDist)
        {
            PointsPairs = pointsPairs;
            MinDist = minDist;
        }

        public override string ToString() => $"Count = {PointsPairs.Count} MinDist = {MinDist}";
    }
}
