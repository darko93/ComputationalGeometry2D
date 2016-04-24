using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    public class UnorderedPointsPair : IEquatable<UnorderedPointsPair>
    {
        public Point First { get; private set; }
        public Point Second { get; private set; }
        public UnorderedPointsPair(Point first, Point second)
        {
            First = first;
            Second = second;
        }

        bool IEquatable<UnorderedPointsPair>.Equals(UnorderedPointsPair other) =>
            (First.Equals(other.First) && Second.Equals(other.Second)) || (First.Equals(other.Second) && Second.Equals(other.First));
        

        public override string ToString() =>
            $"[{First},{Second}]";
    }
}
