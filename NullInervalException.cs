using System;

namespace Ako.IntervalCore.Exceptions
{
    public class NullInervalException : Exception
    {
        public NullInervalException(string message) : base(message) { }
    }
}
