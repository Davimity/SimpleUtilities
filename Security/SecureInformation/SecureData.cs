using static SimpleUtilities.Threading.SimpleLock;

namespace SimpleUtilities.Security.SecureInformation{

    ///<summary>Represents a secure data structure.</summary>
    ///<remarks>THREAD SAFE</remarks>
    public abstract class SecureData : IDisposable{

        #region Delegates

            public delegate byte[] ClassArrayToBytes<in T>(T[] array, bool destroyArray);
            public delegate T[] ClassBytesToArray<out T>(byte[] array, bool destroyArray);

        #endregion

        #region Variables

            protected readonly List<SecurePointer<byte>> Buffers;
            protected bool IsReadOnly;

            private int length;

        #endregion

        #region Constructors

            ///<summary> Creates a new SecureData object with a random key, iv and salt. Initializes the data structures. </summary>
            protected SecureData(){
                Buffers = [];
                IsReadOnly = false;

                length = 0;
            }

        #endregion

        #region Public methods

            ///<summary> Make the SecureString read-only. This action cannot be undone, the SecureString will be read-only until it is disposed. </summary>
            public void MakeReadOnly(){
                Lock(this);
                    IsReadOnly = true;
                    foreach (var buffer in Buffers) buffer.MakeReadOnly();
                Unlock(this);
            }

            ///<summary> Dispose the SecureString removing all data from memory. </summary>
            public void Dispose(){
                Lock(this);

                    try{
                        foreach (var buffer in Buffers) buffer.Dispose();
                        Buffers.Clear();
                    }
                    finally{
                        IsReadOnly = true;
                    }
                
                Unlock(this);

                GC.SuppressFinalize(this);
            }

            /// <summary> Delete all data and create new one with the given byte array. </summary>
            /// <param name="data"> Byte array to create new data. </param>
            /// <param name="destroyArray"> If true, the byte array will be destroyed after the operation. </param>
            public void FromBytes(byte[] data, bool destroyArray = false){
                Lock(this);

                    if (IsReadOnly) throw new InvalidOperationException("SecureString is read-only");

                    ClearBuffers();
                    Buffers.Add(new SecurePointer<byte>(data, destroyArray: destroyArray));
                    length = data.Length;

                Unlock(this);
            }

            ///<summary> Returns all the data as a byte array. </summary>
            ///<remarks> This is not a secure method, the decrypted data will be stored in memory being vulnerable to attacks, try to avoid using this method. </remarks>
            public byte[] ToBytes(){
                Lock(this);

                    var data = new byte[length];

                    var offset = 0;
                    foreach (var buffer in Buffers){
                        Buffer.BlockCopy(buffer.GetData(), 0, data, offset, buffer.GetLength());
                        offset += buffer.GetLength();
                    }

                Unlock(this);

                return data;
            }

        #endregion

        #region Abstract methods     

            public abstract override string ToString();

        #endregion

        #region Protected methods

            ///<summary> Clear all Buffers deleting existing data. </summary>
            protected void ClearBuffers(){
                Lock(this);

                    try{
                        foreach (var buffer in Buffers) buffer.Dispose();
                    }
                    finally{
                        Buffers.Clear();
                    }

                Unlock(this);
            }

            ///<summary> Compare two byte arrays. </summary>
            ///<param name="a"> First byte array. </param>
            ///<param name="b"> Second byte array. </param>
            ///<returns> True if the arrays are equal, false otherwise. </returns>
            protected bool ByteArrayEquals(byte[] a, byte[] b){
                try{
                    Lock(this);
                    return a.Length == b.Length && a.SequenceEqual(b);
                }finally {
                    Unlock(this);
                }
            }

            ///<summary> Add a new buffer to the SecureString. </summary>
            ///<param name="data"> Byte array to add. </param>
            ///<param name="destroyArray"> If true, the byte array will be destroyed after the operation. </param>
            protected void AddBuffer(byte[] data, bool destroyArray = false) {
                Lock(this);

                    if (IsReadOnly) throw new InvalidOperationException("SecureString is read-only");

                    Buffers.Add(new SecurePointer<byte>(data, destroyArray: destroyArray));
                    length += data.Length;

                Unlock(this);
            }

        #endregion

        #region static methods

            ///<summary> Overwrite a string deleting all data from memory. </summary>
            ///<param name="str"> The string to overwrite </param>
            ///<param name="forceGc"> If true, the garbage collector will be forced to collect the data. </param>
            ///<remarks> Deleting a string can result in unexpected behaviour because of the "string pool" making other strings with the same value to be deleted. </remarks>
            public static void OverwriteString(string str, bool forceGc = false){
                unsafe{
                    fixed (char* p = str){
                        for (var i = 0; i < str.Length; i++){
                            p[i] = '\0';
                        }
                    }
                }

                if (!forceGc) return;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            ///<summary> Overwrite a byte array deleting all data from memory. </summary>
            ///<param name="array"> The array to overwrite </param>
            ///<param name="forceGc"> If true, the garbage collector will be forced to collect the data. </param>
            public static void OverwriteArray(Array? array, bool forceGc = false){
                if (array == null) return;

                Array.Clear(array, 0, array.Length);

                if (!forceGc) return;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            ///<summary> Overwrite multiple arrays deleting all data from memory. </summary>
            ///<param name="arrays"> The arrays to overwrite </param>
            public static void OverwriteArray(params Array[] arrays) {
                foreach (var a in arrays) Array.Clear(a, 0, a.Length);

                GC.Collect();
            }

        #endregion

        #region Getters

            public virtual int GetLength() {
                try {
                    Lock(this);

                    return length;
                }
                finally {
                    Unlock(this);
                }
            }   

        #endregion
    }
}
