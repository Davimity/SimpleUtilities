using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

using SimpleUtilities.Exceptions.SecurityExceptions;

using static SimpleUtilities.Security.SecureInformation.SecureData;
using static SimpleUtilities.Threading.SimpleLock;

namespace SimpleUtilities.Security.SecureInformation{
    ///<summary>A pointer that stores data in a secure way.</summary>
    ///<remarks>THREAD SAFE</remarks>
    public class SecurePointer<T> : CryptographicElement, IDisposable{

        #region Variables

            private Memory<byte> data;
            private Memory<byte> hash;

            private Memory<byte> key;
            private Memory<byte> iv;

            private int length;
            private bool isReadOnly;

            private System.Timers.Timer? cacheTimer;
            private T[]? cache;

            private ClassArrayToBytes<T>? arrayToBytesMethod;
            private ClassBytesToArray<T>? bytesToArrayMethod;

        #endregion

        #region Constructors

            ///<summary> Creates a new SecurePointer object.</summary>
            public SecurePointer(){

                GenerateKeyIv();

                length = 0;
                hash = new Memory<byte>(Sha512Instance.ComputeHash(data.ToArray()));
            }

            ///<summary> Creates a new SecurePointer object with the given data.</summary>
            ///<param name="data"> The data to store in the pointer </param>
            ///<param name="arrayToBytesMethod"> The method to convert the array to bytes </param>
            ///<param name="bytesToArrayMethod"> The method to convert the bytes to an array </param>
            ///<remarks> The input data won't be destroyed after the creation of the object </remarks>
            public SecurePointer(T data, ClassArrayToBytes<T>? arrayToBytesMethod = null, ClassBytesToArray<T>? bytesToArrayMethod = null) : this([data], arrayToBytesMethod, bytesToArrayMethod){
            }

            ///<summary> Creates a new SecurePointer object with the given data.</summary>
            ///<param name="data"> The data to store in the pointer </param>
            ///<param name="arrayToBytesMethod"> The method to convert the array to bytes </param>
            ///<param name="bytesToArrayMethod"> The method to convert the bytes to an array </param>
            ///<param name="destroyArray"> If true, the data array will be deleted from memory </param>
            ///<remarks> If the destroyArray parameter is set to true, the array will be destroyed after the creation of the object bute the objects in the array can persist in memory if another reference is pointing to them </remarks>
            public SecurePointer(T[] data, ClassArrayToBytes<T>? arrayToBytesMethod = null, ClassBytesToArray<T>? bytesToArrayMethod = null, bool destroyArray = false) : this(){
                SetData(data);

                this.arrayToBytesMethod = arrayToBytesMethod;
                this.bytesToArrayMethod = bytesToArrayMethod;

                if (destroyArray) OverwriteArray(data);
            }

        #endregion

        #region Public methods

            ///<summary> Make the SecurePointer read-only. This action cannot be undone, the SecurePointer will be read-only until it is disposed. </summary>
            public void MakeReadOnly() {
                Lock(this);
                    isReadOnly = true;
                Unlock(this);
            }

            ///<summary> Append the given d to the pointer </summary>
            ///<param name="d"> The d to append </param>
            ///<param name="destroyArray"> If true, the array will be destroyed after the operation </param>
            public void Append(T[] d, bool destroyArray = false) {
                if (isReadOnly) throw new InvalidOperationException("The array is read-only.");

                Lock(this);

                    var newData = new T[length + d.Length];

                    Buffer.BlockCopy(GetData(), 0, newData, 0, length);

                    #pragma warning disable CA2018

                        Buffer.BlockCopy(d, 0, newData, length, d.Length);

                    #pragma warning restore CA2018

                    SetData(newData);
    
                Unlock(this);

                OverwriteArray(newData);
                
                if(destroyArray) OverwriteArray(d);
            }

            public void Append(T d) {
                if (isReadOnly) throw new InvalidOperationException("The array is read-only.");

                Lock(this);

                    var newData = new T[length + 1];

                    Buffer.BlockCopy(GetData(), 0, newData, 0, length);
                    newData[length] = d;

                    SetData(newData);
            
                Unlock(this);

                OverwriteArray(newData);
            }

            ///<summary> Dispose the SecurePointer removing all data from memory. </summary>
            public void Dispose(){
                Lock(this);

                    data.Span.Clear();

                    key.Span.Clear();
                    iv.Span.Clear();

                    length = 0;

                Unlock(this);

                GC.SuppressFinalize(this);
            }

        #endregion

        #region Private methods
            
            private void GenerateKeyIv() {
                var nkey = new byte[32];
                var niv = new byte[16];

                using (var rng = RandomNumberGenerator.Create()) {
                    rng.GetBytes(nkey);
                    rng.GetBytes(niv);
                }

                key = new Memory<byte>(nkey);
                iv = new Memory<byte>(niv);
            }

            private void StartCacheTimer(int interval = 1000) {
                if (cacheTimer != null) {
                    cacheTimer.Stop();
                    cacheTimer.Dispose();
                }

                cacheTimer = new System.Timers.Timer(interval);
                cacheTimer.Elapsed += (_, _) => ClearCache();
                cacheTimer.AutoReset = false;
                cacheTimer.Start();
            }

            private void ClearCache() {
                Lock(this);

                    if (cache == null) return;
                    Array.Clear(cache, 0, cache.Length);
                    cache = null;

                Unlock(this);
            }
         
            private byte[] ArrayToBytes(T[] array, bool destroyArray = true) {
                ArgumentNullException.ThrowIfNull(array, nameof(array));

                if (arrayToBytesMethod != null) return arrayToBytesMethod(array, destroyArray);

                byte[] result;
                var elementType = typeof(T);

                if (elementType.IsPrimitive) {
                    result = new byte[Buffer.ByteLength(array)];
                    Buffer.BlockCopy(array, 0, result, 0, result.Length);
                }
                else if (elementType == typeof(string)) {
                    var realArray = array.Cast<string>().ToArray();

                    using (var ms = new MemoryStream()) {
                        foreach (var s in realArray) {
                            var bytes = Encoding.UTF8.GetBytes(s);
                            ms.Write(bytes, 0, bytes.Length);

                            var separatorBytes = Encoding.UTF8.GetBytes(['\0']);
                            ms.Write(separatorBytes, 0, separatorBytes.Length);
                        }

                        result = ms.ToArray();
                    }
                }
                else {
                    throw new ArgumentNullException(nameof(arrayToBytesMethod), "Type not supported by default. Please provide a custom method for ArrayToBytes.");
                }

                if (destroyArray) OverwriteArray(array);

                return result;
            }

            private T[] BytesToArray(byte[] array, bool destroyArray = true) {
                ArgumentNullException.ThrowIfNull(array, nameof(array));

                if (bytesToArrayMethod != null) return bytesToArrayMethod(array, destroyArray);

                var elementType = typeof(T);
                T[] result;

                if (elementType.IsPrimitive) {
                    var l = array.Length / Marshal.SizeOf(elementType);
                    result = new T[l];
                    Buffer.BlockCopy(array, 0, result, 0, array.Length);
                }
                else if (elementType == typeof(string)) {
                    result = Encoding.UTF32.GetString(array).Split('\0').Cast<T>().ToArray();
                }
                else {
                    throw new ArgumentNullException(nameof(bytesToArrayMethod), "Type not supported by default. Please provide a custom method for BytesToArray.");
                }

                if (destroyArray) OverwriteArray(array);

                return result;
            }

            private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform) {
                using var ms = new MemoryStream(data.Length);

                using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write)) {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                }

                return ms.ToArray();
            }

        #endregion

        #region Getters

            public int GetLength() {
                try {
                    Lock(this);
                    return length;
                }
                finally {
                    Unlock(this);
                }
            }

            public T[] GetData() {
                try {
                    Lock(this);

                    if (cache != null) {
                        StartCacheTimer();
                        return cache.ToArray();
                    }

                    if (!Sha512Instance.ComputeHash(data.ToArray()).SequenceEqual(hash.ToArray()))
                        throw new InvalidDataHash("Data has been modified in a potentially malicious way.");

                    byte[] decData;

                    AesInstance.Key = key.Span.ToArray();
                    AesInstance.IV = iv.Span.ToArray();

                    using (var decryptor = AesInstance.CreateDecryptor(AesInstance.Key, AesInstance.IV)) {
                        decData = PerformCryptography(data.ToArray(), decryptor);
                    }

                    AesInstance.Key = ZeroBuffer;
                    AesInstance.IV = ZeroBuffer;

                    cache = BytesToArray(decData, false);

                    return cache.ToArray();
                }
                finally {
                    Unlock(this);
                }
            }

        #endregion

        #region Setters

            public void SetData(T[] value) {
                Lock(this);

                if (isReadOnly) throw new ReadOnlyException("The SecurePointer is read-only.");

                length = value.Length;

                GenerateKeyIv();

                AesInstance.Key = key.Span.ToArray();
                AesInstance.IV = iv.Span.ToArray();

                using (var encryptor = AesInstance.CreateEncryptor(AesInstance.Key, AesInstance.IV)) {
                    data = new Memory<byte>(PerformCryptography(ArrayToBytes(value, false), encryptor));
                }

                AesInstance.Key = ZeroBuffer;
                AesInstance.IV = ZeroBuffer;

                hash = new Memory<byte>(Sha512Instance.ComputeHash(data.ToArray()));

                Unlock(this);
            }

            public void SetArrayToBytesMethod(ClassArrayToBytes<T> method) {
                Lock(this);

                arrayToBytesMethod = method;

                Unlock(this);
            }

            public void SetBytesToArrayMethod(ClassBytesToArray<T> method) {
                Lock(this);

                bytesToArrayMethod = method;

                Unlock(this);
            }

        #endregion
    }
}
