using SimpleUtilities.Threading;

namespace SimpleUtilities.Security.SecureInformation.Types
{
    public class SecureInt : SecureData
    {

        #region Constructors

        public SecureInt(int value) : base()
        {
            byte[] bytes = BitConverter.GetBytes(value);
            buffers.Add(new SecureArray<byte>(bytes));
        }

        #endregion

        #region Operators

        public static SecureInt operator +(SecureInt a, SecureInt b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new SecureInt(a.ToInt() + b.ToInt());
            }
        }

        public static SecureInt operator +(SecureInt a, int b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureInt(a.ToInt() + b);
            }
        }

        public static SecureInt operator +(int a, SecureInt b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return new SecureInt(a + b.ToInt());
            }
        }

        public static SecureInt operator -(SecureInt a, SecureInt b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new SecureInt(a.ToInt() - b.ToInt());
            }
        }

        public static SecureInt operator -(SecureInt a, int b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureInt(a.ToInt() - b);
            }
        }

        public static SecureInt operator -(int a, SecureInt b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return new SecureInt(a - b.ToInt());
            }
        }

        public static SecureInt operator *(SecureInt a, SecureInt b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new SecureInt(a.ToInt() * b.ToInt());
            }
        }

        public static SecureInt operator *(SecureInt a, int b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureInt(a.ToInt() * b);
            }
        }

        public static SecureInt operator *(int a, SecureInt b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return new SecureInt(a * b.ToInt());
            }
        }

        public static SecureInt operator /(SecureInt a, SecureInt b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new SecureInt(a.ToInt() / b.ToInt());
            }
        }

        public static SecureInt operator /(SecureInt a, int b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureInt(a.ToInt() / b);
            }
        }

        public static SecureInt operator /(int a, SecureInt b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return new SecureInt(a / b.ToInt());
            }
        }

        public static bool operator <(SecureInt a, SecureInt b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return a.ToInt() < b.ToInt();
            }
        }

        public static bool operator <(SecureInt a, int b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return a.ToInt() < b;
            }
        }

        public static bool operator <(int a, SecureInt b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return a < b.ToInt();
            }
        }

        public static bool operator >(SecureInt a, SecureInt b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return a.ToInt() > b.ToInt();
            }
        }

        public static bool operator >(SecureInt a, int b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return a.ToInt() > b;
            }
        }

        public static bool operator >(int a, SecureInt b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return a > b.ToInt();
            }
        }

        public static bool operator <=(SecureInt a, SecureInt b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return a.ToInt() <= b.ToInt();
            }
        }

        public static bool operator <=(SecureInt a, int b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return a.ToInt() <= b;
            }
        }

        public static bool operator <=(int a, SecureInt b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return a <= b.ToInt();
            }
        }

        public static bool operator >=(SecureInt a, SecureInt b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return a.ToInt() >= b.ToInt();
            }
        }

        public static bool operator >=(SecureInt a, int b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return a.ToInt() >= b;
            }
        }

        public static bool operator >=(int a, SecureInt b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return a >= b.ToInt();
            }
        }

        public static bool operator ==(SecureInt a, SecureInt b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return a.ToInt() == b.ToInt();
            }
        }

        public static bool operator ==(SecureInt a, int b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return a.ToInt() == b;
            }
        }

        public static bool operator ==(int a, SecureInt b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return a == b.ToInt();
            }
        }

        public static bool operator !=(SecureInt a, SecureInt b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return a.ToInt() != b.ToInt();
            }
        }

        public static bool operator !=(SecureInt a, int b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return a.ToInt() != b;
            }
        }

        public static bool operator !=(int a, SecureInt b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return a != b.ToInt();
            }
        }

        public static SecureInt operator ++(SecureInt a)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureInt(a.ToInt() + 1);
            }
        }

        public static SecureInt operator --(SecureInt a)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureInt(a.ToInt() - 1);
            }
        }

        public static SecureInt operator &(SecureInt a, SecureInt b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new SecureInt(a.ToInt() & b.ToInt());
            }
        }

        public static SecureInt operator &(SecureInt a, int b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureInt(a.ToInt() & b);
            }
        }

        public static SecureInt operator &(int a, SecureInt b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return new SecureInt(a & b.ToInt());
            }
        }

        public static SecureInt operator |(SecureInt a, SecureInt b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new SecureInt(a.ToInt() | b.ToInt());
            }
        }

        public static SecureInt operator |(SecureInt a, int b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureInt(a.ToInt() | b);
            }
        }

        public static SecureInt operator |(int a, SecureInt b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return new SecureInt(a | b.ToInt());
            }
        }

        public static SecureInt operator ^(SecureInt a, SecureInt b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new SecureInt(a.ToInt() ^ b.ToInt());
            }
        }

        public static SecureInt operator ^(SecureInt a, int b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureInt(a.ToInt() ^ b);
            }
        }

        public static SecureInt operator ^(int a, SecureInt b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return new SecureInt(a ^ b.ToInt());
            }
        }

        public static SecureInt operator <<(SecureInt a, int b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureInt(a.ToInt() << b);
            }
        }

        public static SecureInt operator >>(SecureInt a, int b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureInt(a.ToInt() >> b);
            }
        }

        public static SecureInt operator ~(SecureInt a)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureInt(~a.ToInt());
            }
        }

        public static SecureInt operator +(SecureInt a)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureInt(+a.ToInt());
            }
        }

        public static SecureInt operator -(SecureInt a)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureInt(-a.ToInt());
            }
        }

        public static explicit operator int(SecureInt value)
        {
            return value.ToInt();
        }

        public static implicit operator SecureInt(int value)
        {
            return new SecureInt(value);
        }

        #endregion

        #region Public Methods

        public int ToInt()
        {
            using (new SimpleLock(lockObject))
            {
                return BitConverter.ToInt32(ToBytes(), 0);
            }
        }

        public override string ToString()
        {
            using (new SimpleLock(lockObject))
            {
                return ToInt().ToString();
            }
        }

        public override bool Equals(object? obj)
        {
            using (new SimpleLock(lockObject))
            {
                if (obj == null || GetType() != obj.GetType()) return false;
                return ToInt() == ((SecureInt)obj).ToInt();
            }
        }

        public override int GetHashCode()
        {
            using (new SimpleLock(lockObject))
            {
                return ToInt().GetHashCode();
            }
        }

        #endregion

    }
}