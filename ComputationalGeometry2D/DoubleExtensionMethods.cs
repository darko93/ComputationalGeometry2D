using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry2D
{
    static class DoubleExtensionMethods
    {
        private static DoublesComparer doublesComparer = new DoublesComparer();

        public static bool IsAlmostEqualTo(this double thisValue, double value, double acceptableRelativeDifference = DoublesComparer.AcceptableRelativeDifference) =>
            doublesComparer.AlmostEqualTo(thisValue, value, acceptableRelativeDifference);

        public static bool IsAlmostEqualToZero(this double thisValue) =>
            doublesComparer.AlmostEqualToZero(thisValue);

        public static bool IsGreaterThanAndNotAlmostEqualTo(this double thisValue, double value, double acceptableRelativeDifference = DoublesComparer.AcceptableRelativeDifference) =>
            doublesComparer.GreaterThanAndNotAlmostEqualTo(thisValue, value, acceptableRelativeDifference);

        public static bool IsGreaterThanOrAlmostEqualTo(this double thisValue, double value, double acceptableRelativeDifference = DoublesComparer.AcceptableRelativeDifference) =>
            doublesComparer.GreaterThanOrAlmostEqualTo(thisValue, value, acceptableRelativeDifference);

        public static bool IsLessThanAndNotAlmostEqualTo(this double thisValue, double value, double acceptableRelativeDifference = DoublesComparer.AcceptableRelativeDifference) =>
            doublesComparer.LessThanAndNotAlmostEqualTo(thisValue, value, acceptableRelativeDifference);

        public static bool IsLessThanOrAlmostEqualTo(this double thisValue, double value, double acceptableRelativeDifference = DoublesComparer.AcceptableRelativeDifference) =>
            doublesComparer.LessThanOrAlmostEqualTo(thisValue, value, acceptableRelativeDifference);
    }
}
