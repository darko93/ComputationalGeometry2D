﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    class PointsPair : IEquatable<PointsPair>
    {
        public Point First { get; private set; }
        public Point Second { get; private set; }
        public PointsPair(Point first, Point second)
        {
            First = first;
            Second = second;
        }

        bool IEquatable<PointsPair>.Equals(PointsPair other) =>
            (First.Equals(other.First) && Second.Equals(other.Second)) || (First.Equals(other.Second) && Second.Equals(other.First));
        

        public override string ToString() =>
            $"[{First},{Second}]";
    }
}
