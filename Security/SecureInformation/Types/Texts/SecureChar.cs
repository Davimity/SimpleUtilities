using System.Text;

using static SimpleUtilities.Threading.SimpleLock;

namespace SimpleUtilities.Security.SecureInformation.Types.Texts {
    public class SecureChar : SecureData {

        #region Variables

            private readonly Encoding encoding;

        #endregion

        #region Constructors

            public SecureChar(Encoding encoding) {
                IsReadOnly = false;
                this.encoding = encoding;
            }

            public SecureChar(char data) : this(Encoding.UTF8) {
                AddBuffer(encoding.GetBytes([data]));
            }

            public SecureChar(string data, Encoding encoding) : this(encoding) {
                AddBuffer(encoding.GetBytes(data));
            }

        #endregion

        #region Operators

            public static bool operator ==(SecureChar a, SecureChar b) {
                try{
                    Lock(a, b);

                    char[] chars = [a.ToChar(), b.ToChar()];
                    var result = chars[0] == chars[1];

                    OverwriteArray(chars);

                    return result;
                }finally {
                    Unlock(a, b);
                }
            }

            public static bool operator ==(SecureChar a, char b) {
                try{
                    Lock(a);

                    char[] chars = [a.ToChar(), b];
                    var result = chars[0] == chars[1];

                    OverwriteArray(chars);

                    return result;
                }finally{
                    Unlock(a);
                }
            }
            public static bool operator ==(char a, SecureChar b) => b == a;


            public static bool operator !=(SecureChar a, SecureChar b)  => !(a == b);
            public static bool operator !=(SecureChar a, char b) => !(a == b);
            public static bool operator !=(char a, SecureChar b) => !(a == b);


            public static implicit operator char(SecureChar a) => a.ToChar();
            public static implicit operator SecureChar(char a) => new(a);

        #endregion

        #region Public methods

            public override string ToString() {
                var data = ToBytes();
                var s = encoding.GetString(data);

                OverwriteArray(data);

                return s;
            }
            public SecureString ToSecureString() {
                var data = ToBytes();
                var secureString = new SecureString(data, encoding, isReadOnly: IsReadOnly, destroyArray: true);

                return secureString;
            }

            public char ToChar() {
                var data = ToBytes(); 
                var c = encoding.GetChars(data)[0];

                OverwriteArray(data);

                return c;
            }

            public override bool Equals(object? obj) {
                switch (obj) {
                    case null: return false;

                    case SecureChar secureChar: {
                        try {

                            Lock(this, secureChar);

                            char[] chars = [ToChar(), secureChar.ToChar()];
                            var equals = chars[0] == chars[1];

                            OverwriteArray(chars);

                            return equals;
                        }finally {
                            Unlock(this, secureChar);
                        }
                    }

                    default:
                        return false;
                }
            }

            public override int GetHashCode() {
                try{
                    Lock(this);

                    var data = ToBytes();
                    var hash = data.GetHashCode();
                    OverwriteArray(data);

                    return hash;
                }finally{
                    Unlock(this);
                }
            }

        #endregion

        #region Getters

            public Encoding GetEncoding() {
                try {
                    Lock(this);
                    return encoding;
                }
                finally {
                    Unlock(this);
                }
            }

        #endregion
    }
}
