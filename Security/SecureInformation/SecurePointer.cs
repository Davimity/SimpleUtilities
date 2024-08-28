using SimpleUtilities.Exceptions.SecurityExceptions;
using SimpleUtilities.Security.Cryptography;
using SimpleUtilities.Security.SecureInformation.Types;
using SimpleUtilities.Threading;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;


namespace SimpleUtilities.Security.SecureInformation
{
    public class SecurePointer<T> : IDisposable{

        #region Variables

            public delegate byte[] ClassArrayToBytes(T[] array, bool destroyArray);
            public delegate T[] ClassBytesToArray(byte[] array, bool destroyArray);

            private IntPtr ptr;
            private IntPtr hash;

            private int encryptedLength;
            private int realLength;

            private IntPtr keyPtr;
            private IntPtr ivPtr;

            private readonly object lockObject;

            public ClassArrayToBytes? ArrayToBytesMethod;
            public ClassBytesToArray? BytesToArrayMethod;

        #endregion

        #region Properties

            public int Length{
                get{
                    using(new SimpleLock(lockObject)){
                        return realLength;
                    }
                }
            }

            ///<summary> This property allows to get and set the data in the pointer. </summary>
            ///<remarks> The input data won't be destroyed after the operation </remarks>
            public T[] Data{
                get{
                    using (new SimpleLock(lockObject)){
                        if (!CheckHash()) throw new InvalidDataHash("Data has been modified in a potentially malicious way.");
                        return BytesToArray(ObtainData(ptr, encryptedLength));
                    }
                }

                set{
                    using (new SimpleLock(lockObject)){
                        if (!CheckHash()) throw new InvalidDataHash("Data has been modified in a potentially malicious way.");

                        AllocateMainData(ArrayToBytes(value, false));
                    }
                }
            }

            private byte[] Key{
                get => ObtainData(keyPtr, 32, false);
            }

            private byte[] IV{
                get => ObtainData(ivPtr, 16, false);
            }

            private byte[] Hash{
                get{
                    using(new SimpleLock(lockObject)){
                        return ObtainData(hash, 64, false);
                    }
                }
            }

        #endregion

        #region Constructors

            ///<summary> Creates a new SecurePointer object.</summary>
            public SecurePointer(){

                byte[] key = new byte[32];
                byte[] iv = new byte[16];

                lockObject = new object();

                using (var rng = RandomNumberGenerator.Create()){
                    rng.GetBytes(key);
                    rng.GetBytes(iv);
                }

                try{
                    AllocateData(ref keyPtr, 0, key, encrypt: false, destroyArray: true);
                    AllocateData(ref ivPtr, 0, iv, encrypt: false, destroyArray: true);
                }
                finally{
                    SecureData.OverwriteArray(key, iv);
                }

                ptr = IntPtr.Zero;
                hash = IntPtr.Zero;

                encryptedLength = 0;
                realLength = 0;
            }

            ///<summary> Creates a new SecurePointer object with the given data.</summary>
            ///<param name="data"> The data to store in the pointer </param>
            ///<remarks> The input data won't be destroyed after the creation of the object </remarks>
            public SecurePointer(T data) : this(){
                AllocateMainData(ArrayToBytes(new[] { data }));
            }

            ///<summary> Creates a new SecurePointer object with the given data.</summary>
            ///<param name="data"> The data to store in the pointer </param>
            ///<param name="destroyArray"> If true, the data array will be deleted from memory </param>
            ///<remarks> If the destroyArray parameter is set to true, the array will be destroyed after the creation of the object bute the objects in the array can persist in memory if another reference is pointing to them </remarks>
            public SecurePointer(T[] data, bool destroyArray = true) : this(){
                AllocateMainData(ArrayToBytes(data, destroyArray));
            }

        #endregion

        #region Public methods

            ///<summary> Dispose the SecurePointer removing all data from memory. </summary>
            public void Dispose(){
                using (new SimpleLock(lockObject)){
                    DeleteData(ref ptr, encryptedLength);
                    DeleteData(ref hash, 64);
                    DeleteData(ref keyPtr, 32);
                    DeleteData(ref ivPtr, 16);

                    encryptedLength = 0;
                    realLength = 0;
                }
            }

        #endregion

        #region Private methods

            ///<summary> Modifies the data in the pointer deleting the old data from memory. </summary>
            ///<param name="data"> The new data to store in the pointer </param>
            ///<param name="encrypt"> If true, the data will be encrypted using the object Key and IV </param>
            ///<param name="destroyArray"> If true, the data array will be deleted from memory </param>
            private void AllocateMainData(byte[] data, bool encrypt = true, bool destroyArray = true){
                using (new SimpleLock(lockObject)){
                    if (!CheckHash()) throw new InvalidDataHash("Data has been modified in a potentially malicious way.");

                    realLength = data.Length;
                    encryptedLength = AllocateData(ref ptr, encryptedLength, data, encrypt, destroyArray);
                    AllocateData(ref hash, 64, CalculateHash(), false);
                }
            }

            ///<summary> Modifies the data in the pointer deleting the old data from memory. </summary>
            ///<param name="ptr"> The pointer to modify </param>
            ///<param name="length"> The length of the data on the pointer before modifications</param>
            ///<param name="data"> The new data to store in the pointer </param>
            ///<param name="encrypt"> If true, the data will be encrypted using the object Key and IV </param>
            ///<param name="destroyArray"> If true, the data array will be deleted from memory </param>
            ///<returns> The length of the data </returns>
            private int AllocateData(ref IntPtr ptr, int length, byte[] data, bool encrypt = true, bool destroyArray = true){

                if (ptr != IntPtr.Zero) DeleteData(ref ptr, length);

                if (encrypt){
                    byte[] key = Key;
                    byte[] iv = IV;

                    byte[]? encrypted = null;

                    try{
                        encrypted = AES.AESEncrypt(data, key, iv);

                        ptr = Marshal.AllocHGlobal(encrypted.Length);
                        Marshal.Copy(encrypted, 0, ptr, encrypted.Length);

                        return encrypted.Length;
                    }
                    finally{
                        if(encrypted != null) SecureData.OverwriteArray(encrypted);
                        SecureData.OverwriteArray(key, iv, data);
                    }
                }
                else{
                    try{
                        ptr = Marshal.AllocHGlobal(data.Length);
                        Marshal.Copy(data, 0, ptr, data.Length);

                        return data.Length;
                    }
                    finally{
                       SecureData.OverwriteArray(data);
                    }
 
                }
            }

            ///<summary> Deletes the data in the pointer and sets the pointer to IntPtr.Zero </summary>
            ///<param name="ptr"> The pointer to delete </param>
            private void DeleteData(ref IntPtr ptr, int length){
                using (new SimpleLock(lockObject)){
                    if (ptr == IntPtr.Zero) return;

                    Marshal.Copy(new byte[length], 0, ptr, length);
                    Marshal.FreeHGlobal(ptr);

                    ptr = IntPtr.Zero;
                }
            }

            ///<summary> Obtains the data from the pointer </summary>
            ///<param name="ptr"> The pointer to obtain the data from </param>
            ///<param name="length"> The length of the data </param>
            ///<param name="isEncrypted"> If true, the data will be decrypted </param>
            private byte[] ObtainData(IntPtr ptr, int length, bool isEncrypted = true){
                using (new SimpleLock(lockObject)){
                    if (ptr == IntPtr.Zero) throw new NullReferenceException("Pointer is null");
                    if (length <= 0) throw new ArgumentOutOfRangeException("Length must be greater than 0");

                    byte[] data = new byte[length];
                    Marshal.Copy(ptr, data, 0, length);

                    if (isEncrypted){
                        byte[] key = ObtainData(keyPtr, 32, false);
                        byte[] iv = ObtainData(ivPtr, 16, false);

                        try{
                            byte[] decrypted = AES.AESDecrypt(data, key, iv);
                            return decrypted;
                        }
                        finally{
                            SecureData.OverwriteArray(key, iv, data);
                        }
                    }

                    return data;
                }
            }

            ///<summary> Check if the saved hash is equal to the hash of the data </summary>
            ///<returns> True if the hashes are equal, false otherwise </returns>
            private bool CheckHash(){

                if (ptr == IntPtr.Zero && hash == IntPtr.Zero) return true;

                byte[] savedHash = Hash;
                byte[] calculatedHash = CalculateHash();

                bool result = savedHash.SequenceEqual(calculatedHash);

                SecureData.OverwriteArray(savedHash, calculatedHash);

                return result;
            }

            ///<summary> Calculates the hash of the data </summary>
            ///<returns> The hash of the data </returns>
            private byte[] CalculateHash(){
                byte[] bytes = ObtainData(ptr, encryptedLength, false);
                byte[]? hash = null;
                byte[] key = Key;

                try{
                    hash = SHA.SHA512HashSalt(bytes, key, true);
                    return hash;
                }finally{
                    SecureData.OverwriteArray(bytes, key);
                }
            }

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

                    if (ArrayToBytesMethod == null) throw new NullReferenceException("The ArrayToBytesMethod delegate is null. You must set it before using this method.");

                    result = ArrayToBytesMethod(array, destroyArray);
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
                    if (BytesToArrayMethod == null) throw new NullReferenceException("The BytesToArrayMethod delegate is null. You must set it before using this method.");

                    result = BytesToArrayMethod(array, destroyArray);
                }

                if (destroyArray) SecureData.OverwriteArray();

                return result;
            }

        #endregion

    }
}
