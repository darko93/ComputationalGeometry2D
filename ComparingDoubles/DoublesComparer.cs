using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComparingDoubles
{
    sealed class DoublesComparer
    {
        public const double AcceptableRelativeDifference = 1E-9;

        public bool AlmostEqualTo(double val1, double val2, double acceptableRelativeDifference = AcceptableRelativeDifference)
        {
            if (val1 == val2) return true;

            // Check if the numbers are really close - needed, when comparing numbers near zero.
            double diff = Math.Abs(val1 - val2);
            if (diff <= acceptableRelativeDifference)
                return true;

            val1 = Math.Abs(val1);
            val2 = Math.Abs(val2);
            double larger = (val2 > val1) ? val2 : val1;

            // Check if the numbers are relative close - needed, when numbers are really great.
            // (Cannot be equal to, because it will return true for infinity and finite number.)
            if (diff < larger * acceptableRelativeDifference)
                return true;
            return false;
        }

        public bool GreaterThanAndNotAlmostEqualTo(double val1, double val2, double acceptableRelativeDifference = AcceptableRelativeDifference) =>
            val1 > val2 && !AlmostEqualTo(val1, val2, acceptableRelativeDifference);

        public bool GreaterThanOrAlmostEqualTo(double val1, double val2, double acceptableRelativeDifference = AcceptableRelativeDifference) =>
            val1 >= val2 || AlmostEqualTo(val1, val2, acceptableRelativeDifference);

        public bool LessThanAndNotAlmostEqualTo(double val1, double val2, double acceptableRelativeDifference = AcceptableRelativeDifference) =>
            val1 < val2 && !AlmostEqualTo(val1, val2, acceptableRelativeDifference);

        public bool LessThanOrAlmostEqualTo(double val1, double val2, double acceptableRelativeDifference = AcceptableRelativeDifference) =>
            val1 <= val2 || AlmostEqualTo(val1, val2, acceptableRelativeDifference);


        //Extra methods for comparing to zero for better performence. 
        public bool AlmostEqualToZero(double val, double acceptableRelativeDifference = AcceptableRelativeDifference) =>
            //if (val == 0) return true;
            Math.Abs(val) < acceptableRelativeDifference;

        public bool GreaterThanAndNotAlmostEqualToZero(double val, double acceptableRelativeDifference = AcceptableRelativeDifference) =>
            val > 0 && !AlmostEqualToZero(val, acceptableRelativeDifference);

        public bool GreaterThanOrAlmostEqualToZero(double val, double acceptableRelativeDifference = AcceptableRelativeDifference) =>
            val >= 0 || AlmostEqualToZero(val, acceptableRelativeDifference);

        public bool LessThanAndNotAlmostEqualToZero(double val, double acceptableRelativeDifference = AcceptableRelativeDifference) =>
            val < 0 && !AlmostEqualToZero(val);

        public bool LessThanOrAlmostEqualToZero(double val, double acceptableRelativeDifference = AcceptableRelativeDifference) =>
            val <= 0 || AlmostEqualToZero(val, acceptableRelativeDifference);
    }
}
