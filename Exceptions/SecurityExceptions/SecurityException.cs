using System;

namespace SimpleUtilities.Exceptions.SecurityExceptions{
    public class SecurityException : Exception{
        public SecurityException(string message) : base(message) { }
    }
}
