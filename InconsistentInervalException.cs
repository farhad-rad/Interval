using System;

namespace Ako.Interval.Exceptions
{
    public class InconsistentInervalException : Exception
    {
        public InconsistentInervalException(string message) : base(message) { }
    }
}
