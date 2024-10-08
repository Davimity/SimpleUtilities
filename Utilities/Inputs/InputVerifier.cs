using System.Globalization;
using System.Text.RegularExpressions;
using SecureString = SimpleUtilities.Security.SecureInformation.Types.Texts.SecureString;

namespace SimpleUtilities.Utilities.Inputs
{
    public static class InputVerifier{

        #region Methods

            public static bool VerifyLength(string input, int minLength = 1, int maxLength = 100, bool allowWhiteSpaces = false, bool hasMinLength = true, bool hasMaxLength = true){

                if(minLength < 0 && hasMinLength)
                    throw new ArgumentException("minLength must be greater than or equal to 0");

                if(maxLength < 0 && hasMaxLength)
                    throw new ArgumentException("maxLength must be greater than or equal to 0");

                if (hasMinLength && input.Length < minLength || hasMaxLength && input.Length > maxLength) return false;

                return allowWhiteSpaces || input.IndexOf(' ') == -1;
            }

            public static bool VerifyLength(SecureString input, int minLength = 1, int maxLength = 100, bool allowWhiteSpaces = false, bool hasMinLength = true, bool hasMaxLength = true){

                if (minLength < 0 && hasMinLength)
                    throw new ArgumentException("minLength must be greater than or equal to 0");

                if (maxLength < 0 && hasMaxLength)
                    throw new ArgumentException("maxLength must be greater than or equal to 0");

                if (hasMinLength && input.GetLength() < minLength || hasMaxLength && input.GetLength() > maxLength) return false;

                return allowWhiteSpaces || input.IndexOf(' ') == -1;
            }

            public static bool VerifyEmailFormat(string email){

                if (string.IsNullOrWhiteSpace(email))
                    return false;

                try{

                    email = Regex.Replace(email, "(@)(.+)$", DomainMapper,
                                          RegexOptions.None, TimeSpan.FromMilliseconds(200));


                    string DomainMapper(Match match){
                        var idn = new IdnMapping();


                        string domainName = idn.GetAscii(match.Groups[2].Value);

                        return match.Groups[1].Value + domainName;
                    }
                }
                catch (RegexMatchTimeoutException){
                    return false;
                }
                catch (ArgumentException){
                    return false;
                }

                try{
                    return Regex.IsMatch(email,
                        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                        RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                }
                catch (RegexMatchTimeoutException){
                    return false;
                }
            }

        #endregion

    }
}
