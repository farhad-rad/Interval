using System;

namespace Ako.IntervalCore.Exceptions
{
    public class InconsistentInervalException : Exception
    {
        public InconsistentInervalException(string message) : base(message) { }
    }
}
