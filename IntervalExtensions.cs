using System;

namespace Ako.IntervalCore.Extensions
{
    public static class IntervalExtensions
    {
        public static Interval<T> IntervalUntil<T>(this T start, T? end, bool excludeStart = false, bool excludeEnd = false)
            where T : struct, IComparable, IConvertible => new(start, end, excludeStart, excludeEnd);

        public static Interval<T> IntervalUntil<T>(this T? start, T? end, bool excludeStart = false, bool excludeEnd = false)
            where T : struct, IComparable, IConvertible => new(start, end, excludeStart, excludeEnd);

        public static Interval<T> IntervalFrom<T>(this T end, T? start, bool excludeStart = false, bool excludeEnd = false)
            where T : struct, IComparable, IConvertible => new(start, end, excludeStart, excludeEnd);

        public static Interval<T> IntervalFrom<T>(this T? end, T? start, bool excludeStart = false, bool excludeEnd = false)
            where T : struct, IComparable, IConvertible => new(start, end, excludeStart, excludeEnd);
    }
}
