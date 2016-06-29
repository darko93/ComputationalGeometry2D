namespace ComparingDoubles
{
    public static class DoubleExtensionMethods
    {
        private static DoublesComparer doublesComparer = new DoublesComparer();
    
        public static bool IsAlmostEqualTo(this double thisValue, double value, double acceptableRelativeDifference = DoublesComparer.AcceptableRelativeDifference) =>
            doublesComparer.AlmostEqualTo(thisValue, value, acceptableRelativeDifference);

        public static bool IsGreaterThanAndNotAlmostEqualTo(this double thisValue, double value, double acceptableRelativeDifference = DoublesComparer.AcceptableRelativeDifference) =>
            doublesComparer.GreaterThanAndNotAlmostEqualTo(thisValue, value, acceptableRelativeDifference);

        public static bool IsGreaterThanOrAlmostEqualTo(this double thisValue, double value, double acceptableRelativeDifference = DoublesComparer.AcceptableRelativeDifference) =>
            doublesComparer.GreaterThanOrAlmostEqualTo(thisValue, value, acceptableRelativeDifference);

        public static bool IsLessThanAndNotAlmostEqualTo(this double thisValue, double value, double acceptableRelativeDifference = DoublesComparer.AcceptableRelativeDifference) =>
            doublesComparer.LessThanAndNotAlmostEqualTo(thisValue, value, acceptableRelativeDifference);

        public static bool IsLessThanOrAlmostEqualTo(this double thisValue, double value, double acceptableRelativeDifference = DoublesComparer.AcceptableRelativeDifference) =>
            doublesComparer.LessThanOrAlmostEqualTo(thisValue, value, acceptableRelativeDifference);

        //Extra methods for comparing to zero for better performence (it's often needed to compare to zero). 
        public static bool IsAlmostEqualToZero(this double thisValue, double acceptableRelativeDifference = DoublesComparer.AcceptableRelativeDifference) =>
            doublesComparer.AlmostEqualToZero(thisValue, acceptableRelativeDifference);

        public static bool IsGreaterThanAndNotAlmostEqualToZero(this double thisValue, double acceptableRelativeDifference = DoublesComparer.AcceptableRelativeDifference) =>
            doublesComparer.GreaterThanAndNotAlmostEqualToZero(thisValue, acceptableRelativeDifference);

        public static bool IsGreaterThanOrAlmostEqualToZero(this double thisValue, double acceptableRelativeDifference = DoublesComparer.AcceptableRelativeDifference) =>
            doublesComparer.GreaterThanOrAlmostEqualToZero(thisValue, acceptableRelativeDifference);

        public static bool IsLessThanAndNotAlmostEqualToZero(this double thisValue, double acceptableRelativeDifference = DoublesComparer.AcceptableRelativeDifference) =>
            doublesComparer.LessThanAndNotAlmostEqualToZero(thisValue, acceptableRelativeDifference);

        public static bool IsLessThanOrAlmostEqualToZero(this double thisValue, double acceptableRelativeDifference = DoublesComparer.AcceptableRelativeDifference) =>
            doublesComparer.LessThanOrAlmostEqualToZero(thisValue, acceptableRelativeDifference);
    }
}
