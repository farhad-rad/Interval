using Ako.Interval.Exceptions;
using System;
using System.Linq;

namespace Ako.Interval
{
    public struct Interval<T> : IEquatable<Interval<T>> where T : struct, IComparable, IConvertible
    {
        /// <summary>
        /// An interval or sequence of comparable structures.
        /// </summary>
        /// <param name="start">Start edge of interval. Null acts as minus infinity</param>
        /// <param name="end">End edge of interval. Null acts as infinity</param>
        /// <param name="excludeStart">Weather the start edge is included inside interval</param>
        /// <param name="excludeEnd">Weather the end edge is included inside interval</param>
        /// <exception cref="ReversedInervalEdgesException{T}">If start edge is less comparing to end edge</exception>
        /// <exception cref="NullInervalException">If interval contains no value when both edges are excluded and equal</exception>
        public Interval(T? start, T? end, bool excludeStart = false, bool excludeEnd = false)
        {
            ExcludeStart = excludeStart;
            ExcludeEnd = excludeEnd;

            if (start == null)
            {
                ExcludeStart = true;
            }
            if (end == null)
            {
                ExcludeEnd = true;
            }

            if (start != null && end != null && start.Value.CompareTo(end) > 0)
            {
                throw new ReversedInervalEdgesException<T>("Invalid order of interval edges.", start.Value, end.Value);
            }
            else if (start != null && end != null && start.Value.CompareTo(end) == 0 && (ExcludeStart || ExcludeEnd))
            {
                throw new NullInervalException("No value exists between edges.");
            }

            Start = start;
            End = end;
        }
        public T? Start { get; }
        public T? End { get; }
        public bool ExcludeStart { get; }
        public bool ExcludeEnd { get; }
        public bool IsInfinite => Start == null && End == null;

        /// <summary>
        /// Weather or not an item is included inside the interval range
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <returns>True if item is included inside interval range; False otherwise</returns>
        public bool Contains(T item) => ((Start == null || Start.Value.CompareTo(item) < 0) && (End == null || End.Value.CompareTo(item) > 0))
            || (Start != null && Start.Value.CompareTo(item) == 0 && !ExcludeStart)
            || (End != null && End.Value.CompareTo(item) == 0 && !ExcludeEnd);

        /// <summary>
        /// Weather or not this interval (left) has overlap with another interval (right). Order does not matter
        /// </summary>
        /// <param name="right">The second interval to check</param>
        /// <returns>True if there is any overlaps; False otherwise</returns>
        public bool HasOverlapWith(Interval<T> right) => HasOverlap(this, right);
        
        /// <summary>
        /// Weather or not first interval (left) has overlap with second interval (right). Order does not matter.
        /// Both start and end edges are included in both interval ranges
        /// </summary>
        /// <param name="leftStart">Start edge of first interval. Null acts as minus infinity</param>
        /// <param name="leftEnd">End edge of first interval. Null acts as infinity</param>
        /// <param name="rightStart">Start edge of second interval. Null acts as minus infinity</param>
        /// <param name="rightEnd">End edge of second interval. Null acts as infinity</param>
        /// <returns>True if there is any overlaps; False otherwise</returns>
        public static bool HasOverlap(T? leftStart, T? leftEnd, T? rightStart, T? rightEnd) => HasOverlap(new(leftStart, leftEnd), new(rightStart, rightEnd));
        
        /// <summary>
        /// Weather or not first interval (left) has overlap with second interval (right). Order does not matter.
        /// </summary>
        /// <param name="left">The first interval</param>
        /// <param name="right">The second interval</param>
        /// <returns>True if there is any overlaps; False otherwise</returns>
        public static bool HasOverlap(Interval<T> left, Interval<T> right) =>
            !((left.End != null && right.Start != null && left.End.Value.CompareTo(right.Start.Value) < 0)
            || (left.End != null && right.Start != null && left.End.Value.CompareTo(right.Start.Value) == 0 && (left.ExcludeEnd || right.ExcludeStart))
            || (left.Start != null && right.End != null && left.Start.Value.CompareTo(right.End.Value) == 0 && (left.ExcludeStart || right.ExcludeEnd))
            || (left.Start != null && right.End != null && left.Start.Value.CompareTo(right.End.Value) > 0));

        /// <summary>
        /// Union of first interval (left) with second one (right). Intervals must have overlap or be continuous. Order does not matter.<br/>
        /// Throws exceptions when: <br/>
        ///     - The specified intervals neither have any overlaps nor are continuous.
        /// </summary>
        /// <param name="right">The other interval</param>
        /// <returns>Interval of union</returns>
        /// <exception cref="SeperatedInervalsException">When specified intervals neither have any overlaps nor are continuous</exception>
        public Interval<T> UnionWith(Interval<T> right) => Union(this, right);

        /// <summary>
        /// Union of this interval (left) with another one (right). Intervals must have overlap or be continuous. Order does not matter.
        /// Both start and end edges are included in both interval ranges<br/>
        /// Throws exceptions when: <br/>
        ///     - The specified intervals neither have any overlaps nor are continuous.
        /// </summary>
        /// <param name="leftStart">Start edge of first interval. Null acts as minus infinity</param>
        /// <param name="leftEnd">End edge of first interval. Null acts as infinity</param>
        /// <param name="rightStart">Start edge of second interval. Null acts as minus infinity</param>
        /// <param name="rightEnd">End edge of second interval. Null acts as infinity</param>
        /// <returns>Interval of union</returns>
        /// <exception cref="SeperatedInervalsException">When specified intervals neither have any overlaps nor are continuous</exception>
        public static Interval<T> Union(T? leftStart, T? leftEnd, T? rightStart, T? rightEnd) => Union(new(leftStart, leftEnd), new(rightStart, rightEnd));

        /// <summary>
        /// Union of first interval (left) with second one (right). Intervals must have overlap or be continuous. Order does not matter.<br/>
        /// Throws exceptions when: <br/>
        ///     - The specified intervals neither have any overlaps nor are continuous.
        /// </summary>
        /// <param name="left">The first interval</param>
        /// <param name="right">The second interval</param>
        /// <returns>Interval of union</returns>
        /// <exception cref="SeperatedInervalsException">When specified intervals neither have any overlaps nor are continuous</exception>
        public static Interval<T> Union(Interval<T> left, Interval<T> right)
        {
            if (HasOverlap(left, right) == false)
            {
                if (left.Start != null && right.End != null && left.Start.Value.CompareTo(right.End.Value) == 0 && !left.ExcludeStart && !right.ExcludeEnd)
                {
                    return new(right.Start, left.End, right.ExcludeStart, left.ExcludeEnd);
                }
                if (left.End != null && right.Start != null && left.End.Value.CompareTo(right.Start.Value) == 0 && !left.ExcludeEnd && !right.ExcludeStart)
                {
                    return new(left.Start, right.End, left.ExcludeStart, right.ExcludeEnd);
                }

                throw new SeperatedInervalsException("Specified intervals neither have any overlaps nor are continuous.");
            }

            T? start;
            T? end;
            bool excludeStart;
            bool excludeEnd;

            if (left.Start == null || right.Start == null)
            {
                start = null;
                excludeStart = true;
            }
            else if (left.Start.Value.CompareTo(right.Start.Value) == 0)
            {
                start = left.Start.Value;
                excludeStart = left.ExcludeStart && right.ExcludeStart;
            }
            // left.Start > right.Start
            else if (left.Start.Value.CompareTo(right.Start.Value) > 0)
            {
                start = right.Start.Value;
                excludeStart = right.ExcludeStart;
            }
            // left.Start < right.Start
            else
            {
                start = left.Start.Value;
                excludeStart = left.ExcludeStart;
            }

            if (left.End == null || right.End == null)
            {
                end = null;
                excludeEnd = true;
            }
            else if (left.End.Value.CompareTo(right.End.Value) == 0)
            {
                end = left.End.Value;
                excludeEnd = left.ExcludeEnd && right.ExcludeEnd;
            }
            // left.End > right.End
            else if (left.End.Value.CompareTo(right.End.Value) > 0)
            {
                end = left.End.Value;
                excludeEnd = left.ExcludeEnd;
            }
            // left.End < right.End
            else
            {
                end = right.End.Value;
                excludeEnd = right.ExcludeEnd;
            }

            return new(start, end, excludeStart, excludeEnd);
        }

        /// <summary>
        /// Intersection of this interval (left) with another one (right). Intervals must have overlap. Order does not matter.<br/>
        /// Throws exceptions when: <br/>
        ///     - The specified intervals dont have any overlaps.
        /// </summary>
        /// <param name="right">The other interval</param>
        /// <returns>Interval of intersection</returns>
        /// <exception cref="SeperatedInervalsException">When specified intervals dont have any overlaps</exception>
        public Interval<T> IntersectionWith(Interval<T> right) => Intersection(this, right);

        /// <summary>
        /// Intersection of first interval (left) with second one (right). Intervals must have overlap. Order does not matter.
        /// Both start and end edges are included in both interval ranges<br/>
        /// Throws exceptions when: <br/>
        ///     - The specified intervals dont have any overlaps.
        /// </summary>
        /// <param name="leftStart">Start edge of first interval. Null acts as minus infinity</param>
        /// <param name="leftEnd">End edge of first interval. Null acts as infinity</param>
        /// <param name="rightStart">Start edge of second interval. Null acts as minus infinity</param>
        /// <param name="rightEnd">End edge of second interval. Null acts as infinity</param>
        /// <returns>Interval of intersection</returns>
        /// <exception cref="SeperatedInervalsException">When specified intervals dont have any overlaps</exception>
        public static Interval<T> Intersection(T? leftStart, T? leftEnd, T? rightStart, T? rightEnd) => Intersection(new(leftStart, leftEnd), new(rightStart, rightEnd));

        /// <summary>
        /// Intersection of first interval (left) with second one (right). Intervals must have overlap. Order does not matter.<br/>
        /// Throws exceptions when: <br/>
        ///     - The specified intervals dont have any overlaps.
        /// </summary>
        /// <param name="left">The first interval</param>
        /// <param name="right">The second interval</param>
        /// <returns>Interval of intersection</returns>
        /// <exception cref="SeperatedInervalsException">When specified intervals dont have any overlaps</exception>
        public static Interval<T> Intersection(Interval<T> left, Interval<T> right)
        {
            if (HasOverlap(left, right) == false)
            {
                throw new SeperatedInervalsException("Specified intervals do not have any overlaps.");
            }

            T? start;
            T? end;
            bool excludeStart;
            bool excludeEnd;

            if (left.Start == null && right.Start == null)
            {
                start = null;
                excludeStart = true;
            }
            else if (left.Start == null && right.Start != null)
            {
                start = right.Start.Value;
                excludeStart = right.ExcludeStart;
            }
            else if (left.Start != null && right.Start == null)
            {
                start = left.Start.Value;
                excludeStart = left.ExcludeStart;
            }
            else if (left.Start.Value.CompareTo(right.Start.Value) == 0)
            {
                start = left.Start.Value;
                excludeStart = left.ExcludeStart || right.ExcludeStart;
            }
            // left.Start > right.Start
            else if (left.Start.Value.CompareTo(right.Start.Value) > 0)
            {
                start = left.Start.Value;
                excludeStart = left.ExcludeStart;
            }
            // left.Start < right.Start
            else
            {
                start = right.Start.Value;
                excludeStart = right.ExcludeStart;
            }

            if (left.End == null && right.End == null)
            {
                end = null;
                excludeEnd = true;
            }
            else if (left.End == null && right.End != null)
            {
                end = right.End.Value;
                excludeEnd = right.ExcludeEnd;
            }
            else if (left.End != null && right.End == null)
            {
                end = left.End.Value;
                excludeEnd = left.ExcludeEnd;
            }
            else if (left.End.Value.CompareTo(right.End.Value) == 0)
            {
                end = left.End.Value;
                excludeEnd = left.ExcludeEnd || right.ExcludeEnd;
            }
            // left.End > right.End
            else if (left.End.Value.CompareTo(right.End.Value) > 0)
            {
                end = right.End.Value;
                excludeEnd = right.ExcludeEnd;
            }
            // left.End < right.End
            else
            {
                end = left.End.Value;
                excludeEnd = left.ExcludeEnd;
            }

            return new(start, end, excludeStart, excludeEnd);
        }

        /// <summary>
        /// Subtract this subtrahend interval from the minuend interval (left). Order matters. <br/>
        /// Throws exceptions when: <br/>
        ///     - Two intervals are equal thus the subtraction has no values.<br/>
        ///     - The subtraction of two intervals result in two other seperated intervals.
        /// </summary>
        /// <param name="left">The minuend interval</param>
        /// <returns>Interval of subtraction</returns>
        /// <exception cref="NullInervalException">When two intervals are equal the subtraction has no values.</exception>
        /// <exception cref="InconsistentInervalException">When the subtraction of two intervals result in two other seperated intervals</exception>
        public Interval<T> SubtractionFrom(Interval<T> left) => Subtraction(left, this);

        /// <summary>
        /// Subtract the the subtrahend interval (right) from this minuend interval. Order matters. <br/>
        /// Throws exceptions when: <br/>
        ///     - Two intervals are equal thus the subtraction has no values.<br/>
        ///     - The subtraction of two intervals result in two other seperated intervals.
        /// </summary>
        /// <param name="right">The subtrahend interval</param>
        /// <returns>Interval of subtraction</returns>
        /// <exception cref="NullInervalException">When two intervals are equal the subtraction has no values.</exception>
        /// <exception cref="InconsistentInervalException">When the subtraction of two intervals result in two other seperated intervals</exception>
        public Interval<T> SubtractionBy(Interval<T> right) => Subtraction(this, right);

        /// <summary>
        /// Subtract the the first subtrahend interval (left) from the second minuend interval (right). Order matters. <br/>
        /// Throws exceptions when: <br/>
        ///     - Two intervals are equal thus the subtraction has no values.<br/>
        ///     - The subtraction of two intervals result in two other seperated intervals.
        /// </summary>
        /// <param name="leftStart">Start edge of first interval. Null acts as minus infinity</param>
        /// <param name="leftEnd">End edge of first interval. Null acts as infinity</param>
        /// <param name="rightStart">Start edge of second interval. Null acts as minus infinity</param>
        /// <param name="rightEnd">End edge of second interval. Null acts as infinity</param>
        /// <returns>Interval of subtraction</returns>
        /// <exception cref="NullInervalException">When two intervals are equal the subtraction has no values.</exception>
        /// <exception cref="InconsistentInervalException">When the subtraction of two intervals result in two other seperated intervals</exception>
        public static Interval<T> Subtraction(T? leftStart, T? leftEnd, T? rightStart, T? rightEnd) => Subtraction(new(leftStart, leftEnd), new(rightStart, rightEnd));

        /// <summary>
        /// Subtract the the first subtrahend interval (left) from the second minuend interval (right). Order matters. <br/>
        /// Throws exceptions when: <br/>
        ///     - Two intervals are equal thus the subtraction has no values.<br/>
        ///     - The subtraction of two intervals result in two other seperated intervals.
        /// </summary>
        /// <param name="left">The first minuend interval</param>
        /// <param name="right">The second subtrahend interval</param>
        /// <returns>Interval of subtraction</returns>
        /// <exception cref="NullInervalException">When two intervals are equal the subtraction has no values.</exception>
        /// <exception cref="InconsistentInervalException">When the subtraction of two intervals result in two other seperated intervals</exception>
        public static Interval<T> Subtraction(Interval<T> left, Interval<T> right)
        {
            if (left.HasOverlapWith(right) == false)
            {
                return left;
            }

            var intersection = left.IntersectionWith(right);
            if (left == intersection)
            {
                throw new NullInervalException("Specified intervals are equal; Thus, no value remains after deduction.");
            }
            else if ((intersection.Start == null && left.Start == null) || (intersection.Start != null && left.Start != null && intersection.Start.Value.CompareTo(left.Start.Value) == 0))
            {
                return new(intersection.End, left.End, !intersection.ExcludeEnd, left.ExcludeEnd);
            }
            else if ((intersection.End == null && left.End == null) || (intersection.End != null && left.End != null && intersection.End.Value.CompareTo(left.End.Value) == 0))
            {
                return new(left.Start, intersection.Start, left.ExcludeStart, !intersection.ExcludeStart);
            }
            else
            {
                throw new InconsistentInervalException("Deduction of specified intervals is not consistent.");
            }
        }

        /// <summary>
        /// Union of first interval (left) with second one (right). Intervals must have overlap or be continuous. Order does not matter.<br/>
        /// Throws exceptions when: <br/>
        ///     - The specified intervals neither have any overlaps nor are continuous.
        /// </summary>
        /// <param name="left">The first interval</param>
        /// <param name="right">The second interval</param>
        /// <returns>Interval of union</returns>
        /// <exception cref="SeperatedInervalsException">When specified intervals neither have any overlaps nor are continuous</exception>
        public static Interval<T> operator +(Interval<T> left, Interval<T> right) => Union(left, right);

        /// <summary>
        /// Subtract the the first subtrahend interval (left) from the second minuend interval (right). Order matters. <br/>
        /// Throws exceptions when: <br/>
        ///     - Two intervals are equal thus the subtraction has no values.<br/>
        ///     - The subtraction of two intervals result in two other seperated intervals.
        /// </summary>
        /// <param name="left">The first minuend interval</param>
        /// <param name="right">The second subtrahend interval</param>
        /// <returns>Interval of subtraction</returns>
        /// <exception cref="NullInervalException">When two intervals are equal the subtraction has no values.</exception>
        /// <exception cref="InconsistentInervalException">When the subtraction of two intervals result in two other seperated intervals</exception>
        public static Interval<T> operator -(Interval<T> left, Interval<T> right) => Subtraction(left, right);

        /// <summary>
        /// Two intervals are equal when both has the same type, edges are equal and inclusion of edges are the same.
        /// </summary>
        /// <param name="left">The first interval</param>
        /// <param name="right">The second interval</param>
        /// <returns>True if the first interval is equal to the second interval; False otherwise</returns>
        public static bool operator ==(Interval<T> left, Interval<T> right) => left.Equals(right);

        /// <summary>
        /// Two intervals are equal when both has the same type, edges are equal and inclusion of edges are the same.
        /// </summary>
        /// <param name="left">The first interval</param>
        /// <param name="right">The second interval</param>
        /// <returns>False if the first interval is equal to the second interval; True otherwise</returns>
        public static bool operator !=(Interval<T> left, Interval<T> right) => !left.Equals(right);

        /// <summary>
        /// Two intervals are equal when both has the same type, edges are equal and inclusion of edges are the same.
        /// </summary>
        /// <param name="other">The other interval</param>
        /// <returns>True if object is equal to base interval.</returns>
        public override bool Equals(object other)
            => other != null && other is Interval<T> && Equals((Interval<T>)other);

        /// <summary>
        /// Two intervals are equal when both has the same type, edges are equal and inclusion of edges are the same.
        /// </summary>
        /// <param name="other">The other interval</param>
        /// <returns>True if other interval is equal to base interval.</returns>
        public bool Equals(Interval<T> other)
            => ((Start == null && other.Start == null) || (Start != null && other.Start != null && Start.Value.CompareTo(other.Start.Value) == 0))
            && ((End == null && other.End == null) || (End != null && other.End != null && End.Value.CompareTo(other.End.Value) == 0))
            && ExcludeStart == other.ExcludeStart
            && ExcludeEnd == other.ExcludeEnd;
        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End, ExcludeStart, ExcludeEnd);
        }

        public static explicit operator Interval<bool>(Interval<T> interval) => interval.ToBooleanInterval();
        public Interval<bool> ToBooleanInterval()
        {
            var start = Convert.ToBoolean(Start);
            var end = Convert.ToBoolean(End);
            return new Interval<bool>(start, end, ExcludeStart, ExcludeEnd);
        }

        public static explicit operator Interval<byte>(Interval<T> interval) => interval.ToByteInterval();
        public Interval<byte> ToByteInterval()
        {
            var start = Convert.ToByte(Start);
            var end = Convert.ToByte(End);
            return new Interval<byte>(start, end, ExcludeStart, ExcludeEnd);
        }

        public static explicit operator Interval<char>(Interval<T> interval) => interval.ToCharInterval();
        public Interval<char> ToCharInterval()
        {
            var start = Convert.ToChar(Start);
            var end = Convert.ToChar(End);
            return new Interval<char>(start, end, ExcludeStart, ExcludeEnd);
        }

        public static explicit operator Interval<DateTime>(Interval<T> interval) => interval.ToDateTimeInterval();
        public Interval<DateTime> ToDateTimeInterval()
        {
            var start = Convert.ToDateTime(Start);
            var end = Convert.ToDateTime(End);
            return new Interval<DateTime>(start, end, ExcludeStart, ExcludeEnd);
        }

        public static explicit operator Interval<decimal>(Interval<T> interval) => interval.ToDecimalInterval();
        public Interval<decimal> ToDecimalInterval()
        {
            var start = Convert.ToDecimal(Start);
            var end = Convert.ToDecimal(End);
            return new Interval<decimal>(start, end, ExcludeStart, ExcludeEnd);
        }

        public static explicit operator Interval<double>(Interval<T> interval) => interval.ToDoubleInterval();
        public Interval<double> ToDoubleInterval()
        {
            var start = Convert.ToDouble(Start);
            var end = Convert.ToDouble(End);
            return new Interval<double>(start, end, ExcludeStart, ExcludeEnd);
        }

        public static explicit operator Interval<short>(Interval<T> interval) => interval.ToInt16Interval();
        public Interval<short> ToInt16Interval()
        {
            var start = Convert.ToInt16(Start);
            var end = Convert.ToInt16(End);
            return new Interval<short>(start, end, ExcludeStart, ExcludeEnd);
        }

        public static explicit operator Interval<int>(Interval<T> interval) => interval.ToInt32Interval();
        public Interval<int> ToInt32Interval()
        {
            var start = Convert.ToInt32(Start);
            var end = Convert.ToInt32(End);
            return new Interval<int>(start, end, ExcludeStart, ExcludeEnd);
        }

        public static explicit operator Interval<long>(Interval<T> interval) => interval.ToInt64Interval();
        public Interval<long> ToInt64Interval()
        {
            var start = Convert.ToInt64(Start);
            var end = Convert.ToInt64(End);
            return new Interval<long>(start, end, ExcludeStart, ExcludeEnd);
        }

        public static explicit operator Interval<sbyte>(Interval<T> interval) => interval.ToSByteInterval();
        public Interval<sbyte> ToSByteInterval()
        {
            var start = Convert.ToSByte(Start);
            var end = Convert.ToSByte(End);
            return new Interval<sbyte>(start, end, ExcludeStart, ExcludeEnd);
        }

        public static explicit operator Interval<float>(Interval<T> interval) => interval.ToSingleInterval();
        public Interval<float> ToSingleInterval()
        {
            var start = Convert.ToSingle(Start);
            var end = Convert.ToSingle(End);
            return new Interval<float>(start, end, ExcludeStart, ExcludeEnd);
        }

        public static explicit operator Interval<ushort>(Interval<T> interval) => interval.ToUInt16Interval();
        public Interval<ushort> ToUInt16Interval()
        {
            var start = Convert.ToUInt16(Start);
            var end = Convert.ToUInt16(End);
            return new Interval<ushort>(start, end, ExcludeStart, ExcludeEnd);
        }

        public static explicit operator Interval<uint>(Interval<T> interval) => interval.ToUInt32Interval();
        public Interval<uint> ToUInt32Interval()
        {
            var start = Convert.ToUInt32(Start);
            var end = Convert.ToUInt32(End);
            return new Interval<uint>(start, end, ExcludeStart, ExcludeEnd);
        }

        public static explicit operator Interval<ulong>(Interval<T> interval) => interval.ToUInt64Interval();
        public Interval<ulong> ToUInt64Interval()
        {
            var start = Convert.ToUInt64(Start);
            var end = Convert.ToUInt64(End);
            return new Interval<ulong>(start, end, ExcludeStart, ExcludeEnd);
        }
        
        public static bool TryParse(string intervalToParse, out Interval<T>? result)
        {
            try
            {
                result = Parse(intervalToParse);
                return true;
            }
            catch (Exception)
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Requires the standard interval notation. Starting with [ <opening-bracket> or ( <opening-parenthesis>. Ending with ] <closing-bracket> or ( <closing-parenthesis>. ∞ is shown when an edge in infinity.
        /// </summary>
        /// <param name="intervalToParse">Standard interval notation string</param>
        /// <returns>Interval</returns>
        /// <exception cref="ArgumentNullException">When string is empty</exception>
        /// <exception cref="ArgumentException">When given string is not standard interval notation</exception>
        public static Interval<T> Parse(string intervalToParse)
        {
            if (string.IsNullOrWhiteSpace(intervalToParse))
            {
                throw new ArgumentNullException(intervalToParse, "No value to parse.");
            }
            T? start;
            T? end;
            bool excludeStart = false;
            bool excludeEnd = false;
            var parts = intervalToParse.Split(',').Select(x => x?.Trim()).ToArray();
            if (parts.Length != 2)
            {
                throw new ArgumentException(intervalToParse, "Interval string must be a valid notation having a , <comma> in the middle");
            }

            if (string.IsNullOrWhiteSpace(parts[0]))
            {
                throw new ArgumentException(intervalToParse, "Interval string must be a valid notation having two values between parenthesis or brackets seperated by a comma");
            }
            else if (parts[0].StartsWith("("))
            {
                excludeStart = true;
            }
            else if (!parts[0].StartsWith("["))
            {
                throw new ArgumentException(intervalToParse, "Interval string must be a valid notation starting with [ <open-bracket> or ( <open-parenthesis>");
            }

            string startString = parts[0].Remove(0, 1);

            if (startString.ToLower().Contains("infinit")
                || startString.Contains('\u221E')
                || startString.Contains('\u267E'))
            {
                start = null;
            }
            else
            {
                start = (T)Convert.ChangeType(startString, typeof(T));
            }

            if (string.IsNullOrWhiteSpace(parts[1]))
            {
                throw new ArgumentException(intervalToParse, "Interval string must be a valid notation having two values between parenthesis or brackets seperated by a comma");
            }
            else if (parts[1].EndsWith(")"))
            {
                excludeEnd = true;
            }
            else if (!parts[1].EndsWith("]"))
            {
                throw new ArgumentException(intervalToParse, "Interval string must be a valid notation ending with ] <closed-bracket> or ) <closed-parenthesis>");
            }

            string endString = parts[1].Remove(parts[1].Length - 1);

            if (endString.ToLower().Contains("infinit")
                || endString.Contains('\u221E')
                || endString.Contains('\u267E'))
            {
                end = null;
            }
            else
            {
                end = (T)Convert.ChangeType(endString, typeof(T));
            }

            return new Interval<T>(start, end, excludeStart, excludeEnd);
        }

        public static explicit operator string(Interval<T> interval) => interval.ToString();
        public static explicit operator Interval<T>(string str) => Parse(str);
        public static implicit operator Interval<T>?(string str) => TryParse(str, out var result) ? result : null;

        /// <summary>
        /// The standard interval notation. Starting with [ <opening-bracket> or ( <opening-parenthesis>. Ending with ] <closing-bracket> or ( <closing-parenthesis>. ∞ is shown when an edge in infinity.
        /// </summary>
        /// <returns>string of interval notation</returns>
        public override string ToString()
        {
            var str = "";

            if (Start == null || ExcludeStart)
            {
                str += "(";
            }
            else
            {
                str += "[";
            }

            if (Start == null)
            {
                str += "-\u221E";
            }
            else
            {
                str += Start.ToString();
            }

            str += ",";


            if (End == null)
            {
                str += "+\u221E";
            }
            else
            {
                str += End.ToString();
            }

            if (End == null || ExcludeEnd)
            {
                str += ")";
            }
            else
            {
                str += "]";
            }

            return str;
        }
    }
}
