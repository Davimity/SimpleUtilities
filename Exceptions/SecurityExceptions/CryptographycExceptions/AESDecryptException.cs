namespace SimpleUtilities.Exceptions.SecurityExceptions.CryptographycExceptions{
    public class AESDecryptException : CryptographycException{
        public AESDecryptException(string message) : base(message) { }
    }
}