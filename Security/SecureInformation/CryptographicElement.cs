using System.Security.Cryptography;

namespace SimpleUtilities.Security.SecureInformation {
    public class CryptographicElement {

        #region Variables

            protected static readonly Aes AesInstance;
            protected static readonly SHA512 Sha512Instance;

            protected static readonly byte[] ZeroBuffer;

        #endregion

        #region Constructors

            static CryptographicElement() {
                AesInstance = Aes.Create();
                Sha512Instance = SHA512.Create();

                ZeroBuffer = new byte[16];
            }

        #endregion

    }
}
