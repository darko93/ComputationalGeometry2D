﻿using System.Collections.Generic;

namespace ComputationalGeometry2D
{
    public class ClosestPointsPairResult
    {
        public List<UnorderedPointsPair> ClosestPairs { get; internal set; } = new List<ComputationalGeometry2D.UnorderedPointsPair>();
        public double MinDist { get; internal set; }
        public ClosestPointsPairResult(List<UnorderedPointsPair> closestPairs, double minDist)
        {
            ClosestPairs = closestPairs;
            MinDist = minDist;
        }
        public override string ToString() => $"Count = {ClosestPairs.Count} MinDist = {MinDist}";
    }
}
