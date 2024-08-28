using SimpleUtilities.Threading;

namespace SimpleUtilities.Security.SecureInformation.Types
{
    public class SecureLong : SecureData{

        #region Constructors

        public SecureLong(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            buffers.Add(new SecureArray<byte>(bytes));
        }

        #endregion

        #region Operators

        public static SecureLong operator +(SecureLong a, SecureLong b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new SecureLong(a.ToLong() + b.ToLong());
            }
        }

        public static SecureLong operator +(SecureLong a, long b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureLong(a.ToLong() + b);
            }
        }

        public static SecureLong operator +(long a, SecureLong b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return new SecureLong(a + b.ToLong());
            }
        }

        public static SecureLong operator -(SecureLong a, SecureLong b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new SecureLong(a.ToLong() - b.ToLong());
            }
        }

        public static SecureLong operator -(SecureLong a, long b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureLong(a.ToLong() - b);
            }
        }

        public static SecureLong operator -(long a, SecureLong b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return new SecureLong(a - b.ToLong());
            }
        }

        public static SecureLong operator *(SecureLong a, SecureLong b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new SecureLong(a.ToLong() * b.ToLong());
            }
        }

        public static SecureLong operator *(SecureLong a, long b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureLong(a.ToLong() * b);
            }
        }

        public static SecureLong operator *(long a, SecureLong b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return new SecureLong(a * b.ToLong());
            }
        }

        public static SecureLong operator /(SecureLong a, SecureLong b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new SecureLong(a.ToLong() / b.ToLong());
            }
        }

        public static SecureLong operator /(SecureLong a, long b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureLong(a.ToLong() / b);
            }
        }

        public static SecureLong operator /(long a, SecureLong b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return new SecureLong(a / b.ToLong());
            }
        }

        public static SecureLong operator %(SecureLong a, SecureLong b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new SecureLong(a.ToLong() % b.ToLong());
            }
        }

        public static SecureLong operator %(SecureLong a, long b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureLong(a.ToLong() % b);
            }
        }

        public static SecureLong operator %(long a, SecureLong b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return new SecureLong(a % b.ToLong());
            }
        }

        public static bool operator ==(SecureLong a, SecureLong b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return a.Equals(b);
            }
        }

        public static bool operator ==(SecureLong a, long b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return a.Equals(b);
            }
        }

        public static bool operator ==(long a, SecureLong b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return b.Equals(a);
            }
        }

        public static bool operator !=(SecureLong a, SecureLong b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return !a.Equals(b);
            }
        }

        public static bool operator !=(SecureLong a, long b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return !a.Equals(b);
            }
        }

        public static bool operator !=(long a, SecureLong b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return !b.Equals(a);
            }
        }

        public static bool operator >(SecureLong a, SecureLong b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return a.ToLong() > b.ToLong();
            }
        }

        public static bool operator >(SecureLong a, long b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return a.ToLong() > b;
            }
        }

        public static bool operator >(long a, SecureLong b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return a > b.ToLong();
            }
        }

        public static bool operator <(SecureLong a, SecureLong b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return a.ToLong() < b.ToLong();
            }
        }

        public static bool operator <(SecureLong a, long b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return a.ToLong() < b;
            }
        }

        public static bool operator <(long a, SecureLong b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return a < b.ToLong();
            }
        }

        public static bool operator >=(SecureLong a, SecureLong b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return a.ToLong() >= b.ToLong();
            }
        }

        public static bool operator >=(SecureLong a, long b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return a.ToLong() >= b;
            }
        }

        public static bool operator >=(long a, SecureLong b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return a >= b.ToLong();
            }
        }

        public static bool operator <=(SecureLong a, SecureLong b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return a.ToLong() <= b.ToLong();
            }
        }

        public static bool operator <=(SecureLong a, long b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return a.ToLong() <= b;
            }
        }

        public static bool operator <=(long a, SecureLong b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return a <= b.ToLong();
            }
        }

        public static SecureLong operator ++(SecureLong a)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureLong(a.ToLong() + 1);
            }
        }

        public static SecureLong operator --(SecureLong a)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureLong(a.ToLong() - 1);
            }
        }

        public static SecureLong operator >>(SecureLong a, int b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureLong(a.ToLong() >> b);
            }
        }

        public static SecureLong operator <<(SecureLong a, int b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureLong(a.ToLong() << b);
            }
        }

        public static SecureLong operator &(SecureLong a, SecureLong b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new SecureLong(a.ToLong() & b.ToLong());
            }
        }

        public static SecureLong operator &(SecureLong a, long b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureLong(a.ToLong() & b);
            }
        }

        public static SecureLong operator &(long a, SecureLong b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return new SecureLong(a & b.ToLong());
            }
        }

        public static SecureLong operator |(SecureLong a, SecureLong b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new SecureLong(a.ToLong() | b.ToLong());
            }
        }

        public static SecureLong operator |(SecureLong a, long b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureLong(a.ToLong() | b);
            }
        }

        public static SecureLong operator |(long a, SecureLong b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return new SecureLong(a | b.ToLong());
            }
        }

        public static SecureLong operator ^(SecureLong a, SecureLong b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new SecureLong(a.ToLong() ^ b.ToLong());
            }
        }

        public static SecureLong operator ^(SecureLong a, long b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureLong(a.ToLong() ^ b);
            }
        }

        public static SecureLong operator ^(long a, SecureLong b)
        {
            using (new SimpleLock(b.lockObject))
            {
                return new SecureLong(a ^ b.ToLong());
            }
        }

        public static SecureLong operator ~(SecureLong a)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new SecureLong(~a.ToLong());
            }
        }

        public static SecureLong operator >>(SecureLong a, SecureLong b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new SecureLong(a.ToLong() >> (int)b.ToLong());
            }
        }

        public static SecureLong operator <<(SecureLong a, SecureLong b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new SecureLong(a.ToLong() << (int)b.ToLong());
            }
        }

        public static implicit operator SecureLong(long value)
        {
            return new SecureLong(value);
        }

        public static implicit operator SecureLong(int value)
        {
            return new SecureLong(value);
        }

        public static explicit operator long(SecureLong value)
        {
            return value.ToLong();
        }


        #endregion

        #region Public Methods

            public long ToLong()
            {
                return BitConverter.ToInt64(ToBytes(), 0);
            }

            public override string ToString()
            {
                return ToLong().ToString();
            }

            public override bool Equals(object? obj)
            {
                if (obj is SecureLong secureLong)
                {
                    return ToLong() == secureLong.ToLong();
                }

                if (obj is long l)
                {
                    return ToLong() == l;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return ToLong().GetHashCode();
            }

        #endregion
    }
}
