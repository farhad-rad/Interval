using System;

namespace Ako.Interval.Exceptions
{
    public class NullInervalException : Exception
    {
        public NullInervalException(string message) : base(message) { }
    }
}
