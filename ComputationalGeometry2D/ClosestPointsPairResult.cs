using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    class ClosestPointsPairResult
    {
        public List<PointsPair> PointsPairs { get; internal set; } = new List<ComputationalGeometry2D.PointsPair>();
        public double MinDist { get; internal set; }
        public ClosestPointsPairResult(List<PointsPair> pointsPairs, double minDist)
        {
            PointsPairs = pointsPairs;
            MinDist = minDist;
        }

        public override string ToString() => $"Count = {PointsPairs.Count} MinDist = {MinDist}";
    }
}
