using SimpleUtilities.Security.SecureInformation.Types.Texts;

using static SimpleUtilities.Threading.SimpleLock;

namespace SimpleUtilities.Security.SecureInformation.Types.Numerics {
    ///<summary>Represents a secure integer.</summary>
    ///<remarks>THREAD SAFE</remarks>
    public class SecureInt : SecureData {

        #region Constructors

            public SecureInt() {
            }

            public SecureInt(int value) {
                AddBuffer(BitConverter.GetBytes(value));
            }

            public SecureInt(SecureInt value) {
                AddBuffer(BitConverter.GetBytes(value));
            }

            public SecureInt(string value) : this(int.Parse(value)) {
            }
            
            public SecureInt(SecureString value) : this(value.ParseToInt()) {
            }

        #endregion

        #region Operators

            #region Addition

                public static SecureInt operator +(SecureInt a, SecureInt b) {
                    try {
                        Lock(a, b);
                        return new SecureInt(a.ToInt() + b.ToInt());
                    }finally {
                        Unlock(a, b);
                    }
                }
                public static SecureInt operator +(SecureInt a, int b) {
                   try {
                        Lock(a);
                        return new SecureInt(a.ToInt() + b);
                    }finally {
                        Unlock(a);
                    }
                }
                public static SecureInt operator +(int a, SecureInt b) => b + a;

                public static SecureInt operator ++(SecureInt a) {
                    try {
                        Lock(a);
                        return new SecureInt(a.ToInt() + 1);
                    }finally {
                        Unlock(a);
                    }
                }

            #endregion

            #region Subtraction

                public static SecureInt operator -(SecureInt a, SecureInt b) {
                   try {
                        Lock(a, b);
                        return new SecureInt(a.ToInt() - b.ToInt());
                   }
                   finally {
                       Unlock(a, b);
                   }
                }
                public static SecureInt operator -(SecureInt a, int b) {
                   try { 
                       Lock(a);
                        return new SecureInt(a.ToInt() - b);
                   }finally {
                       Unlock(a);
                   }
                }
                public static SecureInt operator -(int a, SecureInt b) => b - a;

                public static SecureInt operator --(SecureInt a) {
                   try {
                        Lock(a);
                        return new SecureInt(a.ToInt() - 1);
                   }finally {
                       Unlock(a);
                   }
                }

            #endregion

            #region Multiplication

                public static SecureInt operator *(SecureInt a, SecureInt b) {
                   try {
                        Lock(a, b);
                        return new SecureInt(a.ToInt() * b.ToInt());
                   }finally { 
                       Unlock(a, b);
                   }
                }
                public static SecureInt operator *(SecureInt a, int b) {
                   try {
                        Lock(a);
                        return new SecureInt(a.ToInt() * b);
                    }finally {
                       Unlock(a);
                    }
                }
                public static SecureInt operator *(int a, SecureInt b) => b * a;

            #endregion

            #region Division

                public static SecureInt operator /(SecureInt a, SecureInt b) {
                   try {
                       Lock(a, b);
                       return new SecureInt(a.ToInt() / b.ToInt());
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static SecureInt operator /(SecureInt a, int b) {
                   try {  
                       Lock(a);
                       return new SecureInt(a.ToInt() / b);
                   }
                   finally {
                       Unlock(a);
                   }
                }
                public static SecureInt operator /(int a, SecureInt b) => b / a;

            #endregion

            #region Comparison 

                public static bool operator <(SecureInt a, SecureInt b) {
                   try {
                        Lock(a, b);
                        return a.ToInt() < b.ToInt();
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static bool operator <(SecureInt a, int b) {
                   try { 
                       Lock(a);
                       return a.ToInt() < b;
                   }finally {
                       Unlock(a);
                   }
                }
                public static bool operator <(int a, SecureInt b) => b >= a;

                public static bool operator >(SecureInt a, SecureInt b) {
                   try { 
                       Lock(a, b);
                       return a.ToInt() > b.ToInt();
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static bool operator >(SecureInt a, int b) {
                   try { 
                       Lock(a);
                        return a.ToInt() > b;
                   }finally {
                       Unlock(a);
                   }
                }
                public static bool operator >(int a, SecureInt b) => b <= a;

                public static bool operator <=(SecureInt a, SecureInt b) {
                   try {
                        Lock(a, b);
                        return a.ToInt() <= b.ToInt();
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static bool operator <=(SecureInt a, int b) {
                   try { 
                       Lock(a);
                       return a.ToInt() <= b;
                   }finally {
                       Unlock(a);
                   }
                }
                public static bool operator <=(int a, SecureInt b) => b > a;

                public static bool operator >=(SecureInt a, SecureInt b) {
                   try { 
                       Lock(a, b);
                       return a.ToInt() >= b.ToInt();
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static bool operator >=(SecureInt a, int b) {
                   try { 
                       Lock(a);
                       return a.ToInt() >= b;
                   }finally {
                       Unlock(a);
                   }
                }
                public static bool operator >=(int a, SecureInt b) => b < a;

                public static bool operator ==(SecureInt a, SecureInt b) {
                   try { 
                       Lock(a, b);
                       return a.ToInt() == b.ToInt();
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static bool operator ==(SecureInt a, int b) {
                   try { 
                       Lock(a);
                       return a.ToInt() == b;
                   }finally {
                       Unlock(a);
                   }
                }
                public static bool operator ==(int a, SecureInt b) => b == a;

                public static bool operator !=(SecureInt a, SecureInt b) {
                   try { 
                       Lock(a, b);
                        return a.ToInt() != b.ToInt();
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static bool operator !=(SecureInt a, int b) {
                   try { 
                       Lock(a);
                       return a.ToInt() != b;
                   }finally {
                       Unlock(a);
                   }
                }
                public static bool operator !=(int a, SecureInt b) => b != a;

        #endregion

            #region Bitwise

                public static SecureInt operator &(SecureInt a, SecureInt b) {
                   try { 
                       Lock(a, b);
                       return new SecureInt(a.ToInt() & b.ToInt());
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static SecureInt operator &(SecureInt a, int b) {
                   try { 
                       Lock(a);
                       return new SecureInt(a.ToInt() & b);
                   }finally {
                       Unlock(a);
                   }
                }
                public static SecureInt operator &(int a, SecureInt b) => b & a;

                public static SecureInt operator |(SecureInt a, SecureInt b) {
                   try { 
                       Lock(a, b);
                       return new SecureInt(a.ToInt() | b.ToInt());
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static SecureInt operator |(SecureInt a, int b) {
                   try { 
                       Lock(a);
                       return new SecureInt(a.ToInt() | b);
                   }finally {
                       Unlock(a);
                   }
                }
                public static SecureInt operator |(int a, SecureInt b) => b | a;

                public static SecureInt operator ^(SecureInt a, SecureInt b) {
                   try { 
                       Lock(a, b);
                       return new SecureInt(a.ToInt() ^ b.ToInt());
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static SecureInt operator ^(SecureInt a, int b) {
                   try { 
                       Lock(a);
                       return new SecureInt(a.ToInt() ^ b);
                   }finally {
                       Unlock(a);
                   }
                }
                public static SecureInt operator ^(int a, SecureInt b) => b ^ a;

                public static SecureInt operator <<(SecureInt a, SecureInt b) {
                   try { 
                       Lock(a, b);
                       return new SecureInt(a.ToInt() << b.ToInt());
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static SecureInt operator <<(SecureInt a, int b) {
                   try { 
                       Lock(a);
                       return new SecureInt(a.ToInt() << b);
                   }finally {
                       Unlock(a);
                   }
                }

                public static SecureInt operator >>(SecureInt a, SecureInt b) {
                   try { 
                       Lock(a, b);
                       return new SecureInt(a.ToInt() >> b.ToInt());
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static SecureInt operator >>(SecureInt a, int b) {
                   try { 
                       Lock(a);
                       return new SecureInt(a.ToInt() >> b);
                   }finally {
                       Unlock(a);
                   }
                }

                public static SecureInt operator ~(SecureInt a) {
                   try { 
                       Lock(a);
                       return new SecureInt(~a.ToInt());
                   }finally {
                       Unlock(a);
                   }
                }

        #endregion

            #region Modulus

                public static SecureInt operator %(SecureInt a, SecureInt b) {
                   try { 
                       Lock(a, b);
                        return new SecureInt(a.ToInt() % b.ToInt());
                   }finally {
                       Unlock(a, b);
                   }
                }
                public static SecureInt operator %(SecureInt a, int b) {
                   try { 
                       Lock(a);
                       return new SecureInt(a.ToInt() % b);
                   }finally {
                       Unlock(a);
                   }
                }
                public static SecureInt operator %(int a, SecureInt b) => b % a;

            #endregion

            public static SecureInt operator +(SecureInt a) {
               try { 
                   Lock(a);
                   return new SecureInt(+a.ToInt());
               }finally {
                   Unlock(a);
               }
            }
            public static SecureInt operator -(SecureInt a) {
               try { 
                   Lock(a);
                   return new SecureInt(-a.ToInt());
               }finally {
                   Unlock(a);
               }
            }

            public static implicit operator int(SecureInt value) {
                return value.ToInt();
            }

            public static explicit operator SecureInt(int value) {
                return new SecureInt(value);
            }

        #endregion

        #region Public Methods

            public int ToInt() {
               try { 
                   Lock(this);
                    return BitConverter.ToInt32(ToBytes(), 0);
               }finally {
                   Unlock(this);
               }
            }

            public SecureString ToSecureString() {
               try { 
                   Lock(this);
                   return new SecureString(ToInt().ToString());
               }finally {
                   Unlock(this);
               }
            }

            public override string ToString() {
               try { 
                   Lock(this);
                   return ToInt().ToString();
               }finally {
                   Unlock(this);
               }
            }

            public override bool Equals(object? obj) {
               try { 
                   Lock(this);
                   if (obj == null || GetType() != obj.GetType()) return false;
                   return ToInt() == ((SecureInt)obj).ToInt();
               }finally {
                   Unlock(this);
               }
            }

            public override int GetHashCode() {
               try { 
                   Lock(this);
                   return ToInt().GetHashCode();
               }finally {
                   Unlock(this);
               }
            }

        #endregion

    }
}