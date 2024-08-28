using SimpleUtilities.Threading;
using System.Runtime.InteropServices;
using System.Text;
using SimpleUtilities.Security.SecureInformation.Types;

namespace SimpleUtilities.Security.SecureInformation
{
    public class SecureArray<T> : IDisposable{

        #region Variables

            public delegate byte[] ClassArrayToBytes(T[] array, bool destroyArray);
            public delegate T[] ClassBytesToArray(byte[] array, bool destroyArray);

            private SecurePointer<byte> ptr;

            private bool isReadOnly;

            private object lockObject;

            public ClassArrayToBytes? arrayToBytesMethod;
            public ClassBytesToArray? bytesToArrayMethod;

        #endregion

        #region Properties

            ///<summary> This property allows to get and set the data in the array. </summary>
            ///<remarks> The input data won't be destroyed after the operation </remarks>
            public T[] Data{
                get{
                    using (new SimpleLock(lockObject)){
                        return BytesToArray(ptr.Data);
                    }
                }
                set{
                    using (new SimpleLock(lockObject)){
                        ptr.Data = ArrayToBytes(value, false);
                    }
                }
            }

            public int Length{
                get{
                    using (new SimpleLock(lockObject)){
                        return ptr.Length / Marshal.SizeOf<T>();
                    }
                }
            }

        #endregion

        #region Constructors

            ///<summary> Creates a new SecureArray object.</summary>
            ///<param name="isReadOnly"> If true, the array will be read-only </param>
            public SecureArray(bool isReadOnly = false){
                ptr = new SecurePointer<byte>();
                this.isReadOnly = isReadOnly;

                lockObject = new object();
            }
        
            ///<summary> Creates a new SecureArray object with the given data.</summary>
            ///<param name="data"> The data to store in the array </param>
            ///<param name="isReadOnly"> If true, the array will be read-only </param>
            ///<param name="destroyArray"> If true, the array will be destroyed after the operation </param>
            public SecureArray(T[] data, bool isReadOnly = false, bool destroyArray = true) : this(){
                ptr = new SecurePointer<byte>(ArrayToBytes(data, destroyArray), true);

                this.isReadOnly = isReadOnly;

                lockObject = new object();
            }

        #endregion

        #region Public methods

            ///<summary> Append the given data to the array </summary>
            ///<param name="data"> The data to append </param>
            ///<param name="destroyArray"> If true, the array will be destroyed after the operation </param>
            public void Append(T[] data, bool destroyArray = true){

                using (new SimpleLock(lockObject)){
                    if (isReadOnly) throw new InvalidOperationException("The array is read-only.");

                    byte[]? combinedBytes = null;
                    byte[]? newBytes = null;
                    byte[]? actualBytes = null;

                    try{
                        actualBytes = ptr.Data;

                        newBytes = ArrayToBytes(data, destroyArray);

                        combinedBytes = new byte[actualBytes.Length + newBytes.Length];

                        Buffer.BlockCopy(actualBytes, 0, combinedBytes, 0, actualBytes.Length);
                        Buffer.BlockCopy(newBytes, 0, combinedBytes, actualBytes.Length, newBytes.Length);

                        SecureData.OverwriteArray(actualBytes, newBytes);

                        ptr.Data = combinedBytes;
                    }
                    finally{

                        if (combinedBytes != null) SecureData.OverwriteArray(combinedBytes);
                        if (newBytes != null) SecureData.OverwriteArray(newBytes);
                        if (actualBytes != null) SecureData.OverwriteArray(actualBytes);

                    }
                }
            }

            ///<summary> Append the given data to the array </summary>
            ///<param name="data"> The data to append </param>
            public void Append(T data) => Append(new T[]{data}, true);

            ///<summary> Append the given data to the array </summary>
            ///<remarks> Making the array read-only is not a reversible operation </remarks>
            public void MakeReadOnly(){
                using (new SimpleLock(lockObject)){
                    isReadOnly = true;
                }
            }

            ///<summary> Get the element at the given index </summary>
            ///<param name="index"> The index of the element </param>
            ///returns> The element at the given index </returns>
            public T At(int index){
                using (new SimpleLock(lockObject)){
                    if (index < 0 || index >= Length) throw new IndexOutOfRangeException("Index out of range.");

                    return BytesToArray(ptr.Data)[index];
                }
            }

            ///<summary> Dispose the SecurePointer removing all data from memory. </summary>
            public void Dispose(){
                using (new SimpleLock(lockObject)){
                    ptr.Dispose();

                    isReadOnly = true;
                }
            }

        #endregion

        #region Private methods

            ///<summary> Converts an array of elements to a byte array. ONLY WORKS FOR PRIMITIVES AND STRINGS, IF YOU WANT TO CONVERT ANOTHER TYPE YOU MUST SET THE ArrayToBytesMethod DELEGATE </summary>
            ///<param name="array"> The array to convert </param>
            ///<param name="destroyArray"> If true, the array will be deleted from memory </param>
            ///<remarks> If the destroyArray parameter is set to true, the array will be destroyed after the creation of the object bute the objects in the array can persist in memory if another reference is pointing to them </remarks>
            private byte[] ArrayToBytes(T[] array, bool destroyArray = true){
                if (array == null) throw new ArgumentNullException(nameof(array));

                byte[] result;
                Type elementType = typeof(T);

                if (elementType.IsPrimitive){
                    int size = Buffer.ByteLength(array);
                    result = new byte[size];
                    Buffer.BlockCopy(array, 0, result, 0, size);
                }
                else if (elementType == typeof(string)){

                    string[] realArray = array.Cast<string>().ToArray();

                    int charSize = Encoding.UTF32.GetBytes(new[] { 'a' }).Length;
                    int size = realArray.Sum(s => s.Length * charSize) + realArray.Length * charSize;

                    result = new byte[size];

                    int offset = 0;
                    foreach (string s in realArray){
                        byte[] bytes = Encoding.UTF32.GetBytes(s);
                        Buffer.BlockCopy(bytes, 0, result, offset, bytes.Length);
                        offset += bytes.Length;

                        byte[] separatorBytes = Encoding.UTF32.GetBytes(new[] { '\0' });

                        Buffer.BlockCopy(separatorBytes, 0, result, offset, separatorBytes.Length);
                        offset += separatorBytes.Length;
                    }
                }
                else{

                    if (arrayToBytesMethod == null) throw new NullReferenceException("The ArrayToBytesMethod delegate is null. You must set it before using this method.");

                    result = arrayToBytesMethod(array, destroyArray);
                }

                if (destroyArray) SecureData.OverwriteArray(array);

                return result;
            }

            ///<summary> Converts a byte array to an array of elements. ONLY WORKS FOR PRIMITIVES AND STRINGS, IF YOU WANT TO CONVERT ANOTHER TYPE YOU MUST SET THE BytesToArrayMethod DELEGATE </summary>
            ///<param name="array"> The array to convert </param>
            ///<param name="destroyArray"> If true, the array will be deleted from memory </param>
            private T[] BytesToArray(byte[] array, bool destroyArray = true){
                if (array == null) throw new ArgumentNullException(nameof(array));

                Type elementType = typeof(T);
                T[] result;

                if (elementType.IsPrimitive){

                    int length = array.Length / Marshal.SizeOf(elementType);

                    result = new T[length];
                    Buffer.BlockCopy(array, 0, result, 0, array.Length);
                }
                else if (elementType == typeof(string)){
                    result = Encoding.UTF32.GetString(array).Split('\0').Cast<T>().ToArray();
                }
                else{
                    if (bytesToArrayMethod == null) throw new NullReferenceException("The BytesToArrayMethod delegate is null. You must set it before using this method.");

                    result = bytesToArrayMethod(array, destroyArray);
                }

                if (destroyArray) SecureData.OverwriteArray();

                return result;
            }

        #endregion
    }
}
