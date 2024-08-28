using SimpleUtilities.Threading;

namespace SimpleUtilities.Security.SecureInformation.Types
{
    public abstract class SecureData : IDisposable
    {

        #region Variables

        protected readonly List<SecureArray<byte>> buffers;
        protected bool isReadOnly;

        protected object lockObject;

        #endregion

        #region Properties

        public virtual int Length
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return buffers.Sum(x => x.Length);
                }
            }
        }

        #endregion

        #region Constructors

        ///<summary> Creates a new SecureData object with a random key, iv and salt. Initializes the data structures. </summary>
        public SecureData()
        {
            buffers = new List<SecureArray<byte>>();
            isReadOnly = false;

            lockObject = new object();
        }

        #endregion

        #region Public methods

        ///<summary> Make the SecureString read-only. This action cannot be undone, the SecureString will be read-only until it is disposed. </summary>
        public void MakeReadOnly()
        {
            using (new SimpleLock(lockObject))
            {
                isReadOnly = true;
                foreach (SecureArray<byte> buffer in buffers) buffer.MakeReadOnly();
            }
        }

        ///<summary> Dispose the SecureString removing all data from memory. </summary>
        public void Dispose()
        {
            using (new SimpleLock(lockObject))
            {
                try
                {
                    foreach (var buffer in buffers) buffer.Dispose();
                    buffers.Clear();
                }
                finally
                {
                    isReadOnly = true;
                }
            }
        }

        /// <summary> Delete all data and create new one with the given byte array. </summary>
        /// <param name="data"> Byte array to create new data. </param>
        /// <param name="destroyArray"> If true, the byte array will be destroyed after the operation. </param>
        public void FromBytes(byte[] data, bool destroyArray = true)
        {
            using (new SimpleLock(lockObject))
            {
                if (isReadOnly) throw new InvalidOperationException("SecureString is read-only");

                ClearBuffers();
                buffers.Add(new SecureArray<byte>(data, false, destroyArray));
            }
        }

        ///<summary> Returns all the data as a byte array. </summary>
        ///<remarks> This is not a secure method, the decrypted data will be stored in memory being vulnerable to attacks, try to avoid using this method. </remarks>
        public virtual byte[] ToBytes()
        {
            using (new SimpleLock(lockObject))
            {
                int length = buffers.Sum(x => x.Length);
                byte[] data = new byte[length];

                int offset = 0;
                foreach (var buffer in buffers)
                {
                    byte[] currentData = buffer.Data;
                    Buffer.BlockCopy(currentData, 0, data, offset, currentData.Length);
                    offset += buffer.Length;

                    OverwriteArray(currentData);
                }

                return data;
            }
        }

        #endregion

        #region Abstract methods     

        public abstract override string ToString();

        #endregion

        #region Protected methods

        ///<summary> Clear all buffers deleting existing data. </summary>
        protected void ClearBuffers()
        {
            using (new SimpleLock(lockObject))
            {
                try
                {
                    foreach (var buffer in buffers) buffer.Dispose();
                }
                finally
                {
                    buffers.Clear();
                }
            }
        }

        ///<summary> Compare two byte arrays. </summary>
        ///<param name="a"> First byte array. </param>
        ///<param name="b"> Second byte array. </param>
        ///<returns> True if the arrays are equal, false otherwise. </returns>
        protected bool ByteArrayEquals(byte[] a, byte[] b)
        {
            using (new SimpleLock(lockObject))
            {
                if (a == null || b == null) return false;
                if (a.Length != b.Length) return false;

                return a.SequenceEqual(b);
            }
        }

        #endregion

        #region static methods

        ///<summary> Overwrite a string deleting all data from memory. </summary>
        ///<param name="str"> The string to overwrite </param>
        ///<param name="forceGC"> If true, the garbage collector will be forced to collect the data. </param>
        ///<remarks> Deleting a string can result in unexpected behaviour because of the "string pool" making other strings with the same value to be deleted. </remarks>
        public static void OverwriteString(string str, bool forceGC = false)
        {
            if (str == null) return;

            unsafe
            {
                fixed (char* p = str)
                {
                    for (int i = 0; i < str.Length; i++)
                    {
                        p[i] = '\0';
                    }
                }
            }

            if (forceGC)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        ///<summary> Overwrite a byte array deleting all data from memory. </summary>
        ///<param name="array"> The array to overwrite </param>
        ///<param name="forceGC"> If true, the garbage collector will be forced to collect the data. </param>
        public static void OverwriteArray(Array? array, bool forceGC = false)
        {
            if (array == null) return;

            Array.Clear(array, 0, array.Length);
            array = null;

            if (forceGC)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        ///<summary> Overwrite multiple arrays deleting all data from memory. </summary>
        ///<param name="arrays"> The arrays to overwrite </param>
        public static void OverwriteArray(params Array[] arrays)
        {
            for (int i = 0; i < arrays.Length; i++)
            {

                Array? array = arrays[i];

                if (array == null) continue;

                Array.Clear(array, 0, array.Length);
                array = null;
            }

            GC.Collect();
        }

        #endregion

    }
}
