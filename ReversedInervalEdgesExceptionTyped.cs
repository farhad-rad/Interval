using System;

namespace Ako.IntervalCore.Exceptions
{
    public class ReversedInervalEdgesException<T> : Exception where T : struct, IComparable
    {
        T Start;
        T End;
        public ReversedInervalEdgesException(string message) : base(message) { }
        public ReversedInervalEdgesException(string message, T start, T end) : base(message)
        {
            Start = start;
            End = end;
        }
    }
}
