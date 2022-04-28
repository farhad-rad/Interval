using Ako.IntervalCore.Exceptions;
using System;

namespace Ako.IntervalCore
{
    public struct Interval
    {
        public static bool TryParse<T>(string intervalToParse, out Interval<T>? result)
            where T : struct, IComparable, IConvertible => Interval<T>.TryParse(intervalToParse, out result);

        /// <summary>
        /// Requires the standard interval notation. Starting with [ <opening-bracket> or ( <opening-parenthesis>. Ending with ] <closing-bracket> or ( <closing-parenthesis>. ∞ is shown when an edge in infinity.
        /// </summary>
        /// <param name="intervalToParse">Standard interval notation string</param>
        /// <returns>Interval</returns>
        /// <exception cref="ArgumentNullException">When string is empty</exception>
        /// <exception cref="ArgumentException">When given string is not standard interval notation</exception>
        public static Interval<T> Parse<T>(string intervalToParse)
            where T : struct, IComparable, IConvertible => Interval<T>.Parse(intervalToParse);

        /// <summary>
        /// Weather or not first interval (left) has overlap with second interval (right). Order does not matter.
        /// Both start and end edges are included in both interval ranges
        /// </summary>
        /// <param name="leftStart">Start edge of first interval. Null acts as minus infinity</param>
        /// <param name="leftEnd">End edge of first interval. Null acts as infinity</param>
        /// <param name="rightStart">Start edge of second interval. Null acts as minus infinity</param>
        /// <param name="rightEnd">End edge of second interval. Null acts as infinity</param>
        /// <returns>True if there is any overlaps; False otherwise</returns>
        public static bool HasOverlap<T>(T? leftStart, T? leftEnd, T? rightStart, T? rightEnd) where T : struct, IComparable, IConvertible => Interval<T>.HasOverlap(leftStart, leftEnd, rightStart, rightEnd);
        
        /// <summary>
        /// Weather or not first interval (left) has overlap with second interval (right). Order does not matter.
        /// Both start and end edges are included in both interval ranges
        /// </summary>
        /// <param name="leftStart">Start edge of first interval</param>
        /// <param name="leftEnd">End edge of first interval</param>
        /// <param name="rightStart">Start edge of second interval</param>
        /// <param name="rightEnd">End edge of second interval</param>
        /// <returns>True if there is any overlaps; False otherwise</returns>
        public static bool HasOverlap<T>(T leftStart, T leftEnd, T rightStart, T rightEnd) where T : struct, IComparable, IConvertible => Interval<T>.HasOverlap(leftStart, leftEnd, rightStart, rightEnd);

        /// <summary>
        /// Weather or not first interval (left) has overlap with second interval (right). Order does not matter.
        /// </summary>
        /// <param name="left">The first interval</param>
        /// <param name="right">The second interval</param>
        /// <returns>True if there is any overlaps; False otherwise</returns>
        public static bool HasOverlap<T>(Interval<T> left, Interval<T> right) where T : struct, IComparable, IConvertible => Interval<T>.HasOverlap(left, right);

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
        public static Interval<T> Union<T>(T? leftStart, T? leftEnd, T? rightStart, T? rightEnd) where T : struct, IComparable, IConvertible => Interval<T>.Union(leftStart, leftEnd, rightStart, rightEnd);

        /// <summary>
        /// Union of this interval (left) with another one (right). Intervals must have overlap or be continuous. Order does not matter.
        /// Both start and end edges are included in both interval ranges<br/>
        /// Throws exceptions when: <br/>
        ///     - The specified intervals neither have any overlaps nor are continuous.
        /// </summary>
        /// <param name="leftStart">Start edge of first interval</param>
        /// <param name="leftEnd">End edge of first interval</param>
        /// <param name="rightStart">Start edge of second interval</param>
        /// <param name="rightEnd">End edge of second interval</param>
        /// <returns>Interval of union</returns>
        /// <exception cref="SeperatedInervalsException">When specified intervals neither have any overlaps nor are continuous</exception>
        public static Interval<T> Union<T>(T leftStart, T leftEnd, T rightStart, T rightEnd) where T : struct, IComparable, IConvertible => Interval<T>.Union(leftStart, leftEnd, rightStart, rightEnd);
        
        /// <summary>
        /// Union of first interval (left) with second one (right). Intervals must have overlap or be continuous. Order does not matter.<br/>
        /// Throws exceptions when: <br/>
        ///     - The specified intervals neither have any overlaps nor are continuous.
        /// </summary>
        /// <param name="left">The first interval</param>
        /// <param name="right">The second interval</param>
        /// <returns>Interval of union</returns>
        /// <exception cref="SeperatedInervalsException">When specified intervals neither have any overlaps nor are continuous</exception>
        public static Interval<T> Union<T>(Interval<T> left, Interval<T> right) where T : struct, IComparable, IConvertible => Interval<T>.Union(left, right);


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
        public static Interval<T> Intersection<T>(T? leftStart, T? leftEnd, T? rightStart, T? rightEnd) where T : struct, IComparable, IConvertible => Interval<T>.Intersection(leftStart, leftEnd, rightStart, rightEnd);

        /// <summary>
        /// Intersection of first interval (left) with second one (right). Intervals must have overlap. Order does not matter.
        /// Both start and end edges are included in both interval ranges<br/>
        /// Throws exceptions when: <br/>
        ///     - The specified intervals dont have any overlaps.
        /// </summary>
        /// <param name="leftStart">Start edge of first interval</param>
        /// <param name="leftEnd">End edge of first interval</param>
        /// <param name="rightStart">Start edge of second interval</param>
        /// <param name="rightEnd">End edge of second interval</param>
        /// <returns>Interval of intersection</returns>
        /// <exception cref="SeperatedInervalsException">When specified intervals dont have any overlaps</exception>
        public static Interval<T> Intersection<T>(T leftStart, T leftEnd, T rightStart, T rightEnd) where T : struct, IComparable, IConvertible => Interval<T>.Intersection(leftStart, leftEnd, rightStart, rightEnd);
        
        /// <summary>
        /// Intersection of first interval (left) with second one (right). Intervals must have overlap. Order does not matter.<br/>
        /// Throws exceptions when: <br/>
        ///     - The specified intervals dont have any overlaps.
        /// </summary>
        /// <param name="left">The first interval</param>
        /// <param name="right">The second interval</param>
        /// <returns>Interval of intersection</returns>
        /// <exception cref="SeperatedInervalsException">When specified intervals dont have any overlaps</exception>
        public static Interval<T> Intersection<T>(Interval<T> left, Interval<T> right) where T : struct, IComparable, IConvertible => Interval<T>.Intersection(left, right);

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
        public static Interval<T> Subtraction<T>(T? leftStart, T? leftEnd, T? rightStart, T? rightEnd) where T : struct, IComparable, IConvertible => Interval<T>.Subtraction(leftStart, leftEnd, rightStart, rightEnd);

        /// <summary>
        /// Subtract the the first subtrahend interval (left) from the second minuend interval (right). Order matters. <br/>
        /// Throws exceptions when: <br/>
        ///     - Two intervals are equal thus the subtraction has no values.<br/>
        ///     - The subtraction of two intervals result in two other seperated intervals.
        /// </summary>
        /// <param name="leftStart">Start edge of first interval</param>
        /// <param name="leftEnd">End edge of first interval</param>
        /// <param name="rightStart">Start edge of second interval</param>
        /// <param name="rightEnd">End edge of second interval</param>
        /// <returns>Interval of subtraction</returns>
        /// <exception cref="NullInervalException">When two intervals are equal the subtraction has no values.</exception>
        /// <exception cref="InconsistentInervalException">When the subtraction of two intervals result in two other seperated intervals</exception>
        public static Interval<T> Subtraction<T>(T leftStart, T leftEnd, T rightStart, T rightEnd) where T : struct, IComparable, IConvertible => Interval<T>.Subtraction(leftStart, leftEnd, rightStart, rightEnd);

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
        public static Interval<T> Subtraction<T>(Interval<T> left, Interval<T> right) where T : struct, IComparable, IConvertible => Interval<T>.Subtraction(left, right);
    }
}
