using SimpleUtilities.Security.SecureInformation.Types.Texts;

using static SimpleUtilities.Threading.SimpleLock;

namespace SimpleUtilities.Security.SecureInformation.Types.Numerics {
    public class SecureLong : SecureData {

        #region Constructors

            public SecureLong() {
            }

            public SecureLong(long value) {
                AddBuffer(BitConverter.GetBytes(value));
            }

            public SecureLong(SecureLong value) {
                AddBuffer(value.ToBytes());
            }

            public SecureLong(string value) : this(long.Parse(value)) {
            }

            public SecureLong(SecureString value) : this(value.ParseToInt()) {
            }

        #endregion

        #region Operators

            #region Addition

                public static SecureLong operator +(SecureLong a, SecureLong b) {
                   try { 
                       Lock(a, b);
                       return new SecureLong(a.ToLong() + b.ToLong());
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static SecureLong operator +(SecureLong a, long b) {
                   try { 
                       Lock(a);
                       return new SecureLong(a.ToLong() + b);
                   }
                   finally {
                       Unlock(a);
                   }
                }
                public static SecureLong operator +(long a, SecureLong b) => b + a;

                public static SecureLong operator ++(SecureLong a) {
                   try { 
                       Lock(a);
                       return new SecureLong(a.ToLong() + 1);
                   }finally {
                       Unlock(a);
                   }
                }

        #endregion

            #region Subtraction

                public static SecureLong operator -(SecureLong a, SecureLong b) {
                   try { 
                       Lock(a, b);
                       return new SecureLong(a.ToLong() - b.ToLong());
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static SecureLong operator -(SecureLong a, long b) {
                   try { 
                       Lock(a);
                       return new SecureLong(a.ToLong() - b);
                   }finally {
                       Unlock(a);
                   }
                }
                public static SecureLong operator -(long a, SecureLong b) => b - a;

                public static SecureLong operator --(SecureLong a) {
                   try { 
                       Lock(a);
                       return new SecureLong(a.ToLong() - 1);
                   }finally {
                       Unlock(a);
                   }
                }

        #endregion

            #region Multiplication

                public static SecureLong operator *(SecureLong a, SecureLong b) {
                   try { 
                       Lock(a, b);
                       return new SecureLong(a.ToLong() * b.ToLong());
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static SecureLong operator *(SecureLong a, long b) {
                   try { 
                       Lock(a);
                       return new SecureLong(a.ToLong() * b);
                   }finally {
                       Unlock(a);
                   }
                }
                public static SecureLong operator *(long a, SecureLong b) => b * a;

        #endregion

            #region Division

                public static SecureLong operator /(SecureLong a, SecureLong b) {
                   try { 
                       Lock(a, b);
                       return new SecureLong(a.ToLong() / b.ToLong());
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static SecureLong operator /(SecureLong a, long b) {
                   try { 
                       Lock(a);
                       return new SecureLong(a.ToLong() / b);
                   }finally {
                       Unlock(a);
                   }
                }
                public static SecureLong operator /(long a, SecureLong b) => b / a;

            #endregion

            #region Comparison

                public static bool operator ==(SecureLong a, SecureLong b) {
                   try { 
                       Lock(a, b);
                       return a.Equals(b);
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static bool operator ==(SecureLong a, long b) {
                   try { 
                       Lock(a);
                       return a.ToLong().Equals(b);
                   }finally {
                       Unlock(a);
                   }
                }
                public static bool operator ==(long a, SecureLong b) => b == a;

                public static bool operator !=(SecureLong a, SecureLong b) {
                   try { 
                       Lock(a, b);
                       return !a.Equals(b);
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static bool operator !=(SecureLong a, long b) {
                   try { 
                       Lock(a);
                       return !a.ToLong().Equals(b);
                   }finally {
                       Unlock(a);
                   }
                }
                public static bool operator !=(long a, SecureLong b) => b != a;

                public static bool operator >(SecureLong a, SecureLong b) {
                   try { 
                       Lock(a, b);
                       return a.ToLong() > b.ToLong();
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static bool operator >(SecureLong a, long b) {
                   try { 
                       Lock(a);
                       return a.ToLong() > b;
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static bool operator >(long a, SecureLong b) => b > a;

                public static bool operator <(SecureLong a, SecureLong b) {
                   try { 
                       Lock(a, b);
                       return a.ToLong() < b.ToLong();
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static bool operator <(SecureLong a, long b) {
                   try { 
                       Lock(a);
                       return a.ToLong() < b;
                   }finally {
                       Unlock(a);
                   }
                }
                public static bool operator <(long a, SecureLong b) => b < a;

                public static bool operator >=(SecureLong a, SecureLong b) {
                   try { 
                       Lock(a, b);
                       return a.ToLong() >= b.ToLong();
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static bool operator >=(SecureLong a, long b) {
                   try { 
                       Lock(a);
                       return a.ToLong() >= b;
                   }finally {
                       Unlock(a);
                   }
                }
                public static bool operator >=(long a, SecureLong b) => b >= a;

                public static bool operator <=(SecureLong a, SecureLong b) {
                   try { 
                       Lock(a, b);
                       return a.ToLong() <= b.ToLong();
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static bool operator <=(SecureLong a, long b) {
                   try { 
                       Lock(a);
                       return a.ToLong() <= b;
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static bool operator <=(long a, SecureLong b) => b <= a;

        #endregion

            #region Bitwise

                public static SecureLong operator >>(SecureLong a, SecureLong b) {
                    try {
                        Lock(a, b);
                        return new SecureLong(a.ToLong() >> (int)b.ToLong());
                    }
                    finally {
                        Unlock(a, b);
                    }
                }
                public static SecureLong operator >>(SecureLong a, int b) {
                   try { 
                       Lock(a);
                       return new SecureLong(a.ToLong() >> b);
                   }finally {
                       Unlock(a, b);
                   }
                }

                public static SecureLong operator <<(SecureLong a, SecureLong b) {
                   try { 
                       Lock(a, b);
                       return new SecureLong(a.ToLong() << (int)b.ToLong());
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static SecureLong operator <<(SecureLong a, int b) {
                    try {
                        Lock(a);
                        return new SecureLong(a.ToLong() << b);
                    }
                    finally {
                        Unlock(a);
                    }
                }

                public static SecureLong operator &(SecureLong a, SecureLong b) {
                   try { 
                       Lock(a, b);
                       return new SecureLong(a.ToLong() & b.ToLong());
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static SecureLong operator &(SecureLong a, long b) {
                   try { 
                       Lock(a);
                       return new SecureLong(a.ToLong() & b);
                   }finally {
                       Unlock(a);
                   }
                }
                public static SecureLong operator &(long a, SecureLong b) => b & a;

                public static SecureLong operator |(SecureLong a, SecureLong b) {
                   try { 
                       Lock(a, b);
                       return new SecureLong(a.ToLong() | b.ToLong());
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static SecureLong operator |(SecureLong a, long b) {
                   try { 
                       Lock(a);
                       return new SecureLong(a.ToLong() | b);
                   }finally {
                       Unlock(a);
                   }
                }
                public static SecureLong operator |(long a, SecureLong b) => b | a;

                public static SecureLong operator ^(SecureLong a, SecureLong b) {
                   try { 
                       Lock(a, b);
                       return new SecureLong(a.ToLong() ^ b.ToLong());
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static SecureLong operator ^(SecureLong a, long b) {
                   try { 
                       Lock(a);
                       return new SecureLong(a.ToLong() ^ b);
                   }finally {
                       Unlock(a);
                   }
                }
                public static SecureLong operator ^(long a, SecureLong b) => b ^ a;

                public static SecureLong operator ~(SecureLong a) {
                   try { 
                       Lock(a);
                       return new SecureLong(~a.ToLong());
                   }finally {
                       Unlock(a);
                   }
                }

        #endregion

            #region Modulus

                public static SecureLong operator %(SecureLong a, SecureLong b) {
                   try { 
                       Lock(a, b);
                       return new SecureLong(a.ToLong() % b.ToLong());
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static SecureLong operator %(SecureLong a, long b) {
                   try { 
                       Lock(a);
                       return new SecureLong(a.ToLong() % b);
                   }finally {
                       Unlock(a);
                   }
                }
                public static SecureLong operator %(long a, SecureLong b) => b % a;
        
            #endregion

            public static implicit operator long(SecureLong value) {
                try {
                    Lock(value);
                    return value.ToLong();
                }
                finally {
                    Unlock(value);
                }
            }

            public static explicit operator SecureLong(long value) {
                return new SecureLong(value);
            }

        #endregion

        #region Public Methods

            public long ToLong() {
                return BitConverter.ToInt64(ToBytes(), 0);
            }

            public override string ToString() {
                return ToLong().ToString();
            }

            public override bool Equals(object? obj) {
                if (obj is SecureLong secureLong)
                    return ToLong() == secureLong.ToLong();

                if (obj is long l)
                    return ToLong() == l;

                return false;
            }

            public override int GetHashCode() {
                return ToLong().GetHashCode();
            }

        #endregion
    }
}
