using System;

namespace SimpleUtilities.Exceptions.SecurityExceptions.CryptographycExceptions{
    public class CryptographycException : Exception{
        public CryptographycException(string message) : base(message) { }
    }
}
