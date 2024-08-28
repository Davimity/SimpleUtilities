using System.Text;
using System.Runtime.InteropServices;
using SimpleUtilities.Threading;

namespace SimpleUtilities.Security.SecureInformation.Types
{
    public sealed class SecureString : SecureData
    {

        #region Variables

        private Encoding encoding { get; set; }

        #endregion

        #region Properties

        public override int Length
        {
            get => base.Length / encoding.GetByteCount(new char[] { 'a' });
        }

        #endregion

        #region Constructors

        /// <summary> Creates a new SecureString. Default encoding is UTF-8 </summary>
        /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
        public SecureString(bool isReadOnly = false) : base()
        {
            encoding = Encoding.UTF8;
            if (isReadOnly) MakeReadOnly();
        }

        /// <summary> Creates a new SecureString. </summary>
        /// <param name="encoding"> The encoding of the SecureString </param>
        /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
        public SecureString(Encoding encoding, bool isReadOnly = false)
        {
            this.encoding = encoding;
            if (isReadOnly) MakeReadOnly();
        }

        /// <summary> Creates a new SecureString from a string. Default encoding is UTF-8. </summary>
        /// <param name="str"> The string to convert </param>
        /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
        /// <remarks>The input "string" cannot be destroyed since this could affect the "string pool" by also eliminating the content of other unrelated strings but with the same content. Keep in mind that the string with which this SecureString will be created will remain in memory.</remarks>
        public SecureString(string str, bool isReadOnly = false) : base()
        {
            encoding = Encoding.UTF8;

            Append(str);

            if (isReadOnly) MakeReadOnly();
        }

        /// <summary> Creates a new SecureString from a string. </summary>
        /// <param name="str"> The string to convert </param>
        /// <param name="encoding"> The encoding of the string </param>
        /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
        /// <remarks>The input "string" cannot be destroyed since this could affect the "string pool" by also eliminating the content of other unrelated strings but with the same content. Keep in mind that the string with which this SecureString will be created will remain in memory.</remarks>
        public SecureString(string str, Encoding encoding, bool isReadOnly = false) : base()
        {
            this.encoding = encoding;

            Append(str);

            if (isReadOnly) MakeReadOnly();
        }

        /// <summary> Creates a new SecureString from a char array. Default encoding is UTF-8 </summary>
        /// <param name="chars"> The char array to convert </param>
        /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
        /// <param name="destroyArray"> If true, the char array will be destroyed after the SecureString is created </param>
        /// <remarks> Default behaviour is to destroy the char array after the SecureString is created </remarks>
        public SecureString(char[] chars, bool isReadOnly = false, bool destroyArray = true) : base()
        {
            encoding = Encoding.UTF8;

            Append(Encoding.UTF8.GetBytes(chars));

            if (destroyArray) OverwriteArray(chars);
            if (isReadOnly) MakeReadOnly();
        }

        /// <summary> Creates a new SecureString from a char array. </summary>
        /// <param name="chars"> The char array to convert </param>
        /// <param name="encoding"> The encoding of the char array </param>
        /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
        /// <param name="destroyArray"> If true, the char array will be destroyed after the SecureString is created </param>
        /// <remarks> Default behaviour is to destroy the char array after the SecureString is created </remarks>
        public SecureString(char[] chars, Encoding encoding, bool isReadOnly = false, bool destroyArray = true) : base()
        {
            this.encoding = encoding;

            Append(encoding.GetBytes(chars));

            if (destroyArray) OverwriteArray(chars);
            if (isReadOnly) MakeReadOnly();
        }

        /// <summary> Creates a new SecureString from a byte array. Default encoding is UTF-8 </summary>
        /// <param name="bytes"> The char array to convert </param>
        /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
        /// <param name="destroyArray"> If true, the byte array will be destroyed after the SecureString is created </param>
        /// <remarks> Default behaviour is to destroy the byte array after the SecureString is created </remarks>
        public SecureString(byte[] bytes, bool isReadOnly = false, bool destroyArray = true) : base()
        {
            encoding = Encoding.UTF8;

            FromBytes(bytes, destroyArray);

            if (isReadOnly) MakeReadOnly();
        }

        /// <summary> Creates a new SecureString from a byte array. </summary>
        /// <param name="bytes"> The char array to convert </param>
        /// <param name="encoding"> The encoding of the SecureString </param>
        /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
        /// <param name="destroyArray"> If true, the byte array will be destroyed after the SecureString is created </param>
        /// <remarks> Default behaviour is to destroy the byte array after the SecureString is created </remarks>
        public SecureString(byte[] bytes, Encoding encoding, bool isReadOnly = false, bool destroyArray = true) : base()
        {
            this.encoding = encoding;

            FromBytes(bytes, destroyArray);

            if (isReadOnly) MakeReadOnly();
        }

        /// <summary> Creates a new SecureString from System.Security.SecureString. Default encoding is UTF-8 </summary>
        /// <param name="ssSecureString"> The System.Security.SecureString to convert </param>
        /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
        /// <param name="destroyInput"> If true, the System.Security.SecureString will be destroyed after the SecureString is created </param>
        /// <remarks> Default behaviour is to destroy the System.Security.SecureString after the SecureString is created </remarks>
        public SecureString(System.Security.SecureString ssSecureString, bool isReadOnly = false, bool destroyInput = true) : base()
        {
            encoding = Encoding.UTF8;

            nint bstr = nint.Zero;
            byte[]? workArray = null;
            GCHandle? handle = null;

            try
            {
                bstr = Marshal.SecureStringToBSTR(ssSecureString);
                int lengthInChars = ssSecureString.Length;

                unsafe
                {
                    char* bstrChars = (char*)bstr;
                    workArray = new byte[Encoding.UTF8.GetMaxByteCount(lengthInChars)];

                    fixed (byte* workArrayPtr = workArray)
                    {
                        int byteCount = Encoding.UTF8.GetBytes(bstrChars, lengthInChars, workArrayPtr, workArray.Length);
                        Array.Resize(ref workArray, byteCount);
                        handle = GCHandle.Alloc(workArray, GCHandleType.Pinned);
                    }
                }

                FromBytes(workArray);
            }
            finally
            {
                if (workArray != null)
                    for (int i = 0; i < workArray.Length; i++)
                        workArray[i] = 0;

                OverwriteArray(workArray);

                if (handle.HasValue)
                    handle.Value.Free();

                if (bstr != nint.Zero)
                    Marshal.ZeroFreeBSTR(bstr);
            }

            if (isReadOnly) MakeReadOnly();
            if (destroyInput) ssSecureString.Dispose();
        }

        /// <summary> Creates a new SecureString from System.Security.SecureString</summary>
        /// <param name="ssSecureString"> The System.Security.SecureString to convert </param>
        /// <param name="encoding"> The encoding of the SecureString </param>
        /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
        /// <param name="destroyInput"> If true, the System.Security.SecureString will be destroyed after the SecureString is created </param>
        /// <remarks> Default behaviour is to destroy the System.Security.SecureString after the SecureString is created </remarks>
        public SecureString(System.Security.SecureString ssSecureString, Encoding encoding, bool isReadOnly = false, bool destroyInput = true) : base()
        {

            this.encoding = encoding;

            nint bstr = nint.Zero;
            byte[]? workArray = null;
            GCHandle? handle = null;

            try
            {
                bstr = Marshal.SecureStringToBSTR(ssSecureString);
                int lengthInChars = ssSecureString.Length;

                unsafe
                {
                    char* bstrChars = (char*)bstr;
                    workArray = new byte[encoding.GetMaxByteCount(lengthInChars)];

                    fixed (byte* workArrayPtr = workArray)
                    {
                        int byteCount = encoding.GetBytes(bstrChars, lengthInChars, workArrayPtr, workArray.Length);
                        Array.Resize(ref workArray, byteCount);
                        handle = GCHandle.Alloc(workArray, GCHandleType.Pinned);
                    }
                }

                FromBytes(workArray);
            }
            finally
            {
                if (workArray != null)
                    for (int i = 0; i < workArray.Length; i++)
                        workArray[i] = 0;

                OverwriteArray(workArray);

                if (handle.HasValue)
                    handle.Value.Free();

                if (bstr != nint.Zero)
                    Marshal.ZeroFreeBSTR(bstr);
            }

            if (isReadOnly)
                MakeReadOnly();

            if (destroyInput)
                ssSecureString.Dispose();
        }

        ///<summary> Creates a new SecureString from another SecureString. </summary>
        ///<param name="secureString"> The SecureString to convert </param>
        ///<param name="isReadOnly"> If true, the SecureString will be read-only </param>
        ///<param name="destroyInput"> If true, the SecureString will be destroyed after the SecureString is created </param>
        ///<remarks> Default behaviour is to not destroy the SecureString after the SecureString is created </remarks>
        public SecureString(SecureString secureString, bool isReadOnly = false, bool destroyInput = false) : base()
        {
            using (new SimpleLock(secureString.lockObject))
            {
                encoding = secureString.encoding;

                byte[]? bufferData = null;

                try
                {
                    for (int i = 0; i < secureString.buffers.Count; i++)
                    {

                        SecureArray<byte> buffer = secureString.buffers[i];
                        bufferData = buffer.Data;

                        SecureArray<byte> newBuffer = new SecureArray<byte>(bufferData, false, true);

                        buffers.Add(newBuffer);
                    }
                }
                finally
                {
                    OverwriteArray(bufferData);
                }

                if (isReadOnly) MakeReadOnly();
                if (destroyInput) secureString.Dispose();
            }
        }

        #endregion

        #region Operators

        public static SecureString operator +(SecureString s1, SecureString s2)
        {
            using (new SimpleLock(s1.lockObject, s2.lockObject))
            {
                SecureString newString = new SecureString(s1);
                newString.Append(s2);
                return newString;
            }
        }

        public static SecureString operator +(SecureString s1, string s2)
        {
            using (new SimpleLock(s1.lockObject))
            {
                SecureString newString = new SecureString(s1);
                newString.Append(s2);
                return newString;
            }
        }

        public static SecureString operator +(string s1, SecureString s2)
        {
            using (new SimpleLock(s2.lockObject))
            {
                SecureString newString = new SecureString(s1, s2.encoding);
                newString.Append(s2);
                return newString;
            }
        }

        public static SecureString operator +(SecureString s1, char c)
        {
            using (new SimpleLock(s1.lockObject))
            {
                SecureString newString = new SecureString(s1);
                newString.Append(c);
                return newString;
            }
        }

        public static SecureString operator +(char c, SecureString s2)
        {
            using (new SimpleLock(s2.lockObject))
            {
                SecureString newString = new SecureString(c.ToString(), s2.encoding);
                newString.Append(s2);
                return newString;
            }
        }

        public static bool operator ==(SecureString s1, SecureString s2)
        {
            using (new SimpleLock(s1.lockObject, s2.lockObject))
            {

                if (ReferenceEquals(s1, null))
                    return ReferenceEquals(s2, null);

                if (s1.Length != s2.Length) return false;

                for (int i = 0; i < s1.Length; i++)
                    if (s1.CharAt(i) != s2.CharAt(i)) return false;

                return true;
            }
        }

        public static bool operator ==(SecureString s1, string s2)
        {
            using (new SimpleLock(s1.lockObject))
            {
                if (s1.Length != s2.Length) return false;

                for (int i = 0; i < s1.Length; i++)
                    if (s1.CharAt(i) != s2[i]) return false;

                return true;
            }
        }

        public static bool operator ==(string s1, SecureString s2)
        {
            using (new SimpleLock(s2.lockObject))
            {
                if (s1.Length != s2.Length) return false;

                for (int i = 0; i < s1.Length; i++)
                    if (s1[i] != s2.CharAt(i)) return false;

                return true;
            }
        }

        public static bool operator !=(SecureString s1, SecureString s2)
        {
            using (new SimpleLock(s1.lockObject, s2.lockObject))
            {
                if (s1.Length != s2.Length) return true;

                for (int i = 0; i < s1.Length; i++)
                    if (s1.CharAt(i) != s2.CharAt(i)) return true;

                return false;
            }
        }

        public static bool operator !=(SecureString s1, string s2)
        {
            using (new SimpleLock(s1.lockObject))
            {
                if (s1.Length != s2.Length) return true;

                for (int i = 0; i < s1.Length; i++)
                    if (s1.CharAt(i) != s2[i]) return true;

                return false;
            }
        }

        public static bool operator !=(string s1, SecureString s2)
        {
            using (new SimpleLock(s2.lockObject))
            {
                if (s1.Length != s2.Length) return true;

                for (int i = 0; i < s1.Length; i++)
                    if (s1[i] != s2.CharAt(i)) return true;

                return false;
            }
        }

        public static explicit operator string(SecureString s)
        {
            using (new SimpleLock(s.lockObject))
            {
                return s.ToString();
            }
        }

        public static implicit operator SecureString(string s)
        {
            return new SecureString(s);
        }

        #endregion

        #region Public methods

        /// <summary> Appends a char to the SecureString.</summary>
        /// <param name="c"> The char to append </param>
        /// <param name="encoding"> The encoding of the char </param>
        /// <remarks>The input char won't be destroyed. Keep in mind that the char will remain in memory.</remarks>
        public void Append(char c)
        {
            using (new SimpleLock(lockObject))
            {
                if (isReadOnly) throw new InvalidOperationException("SecureString is read-only");

                byte[]? data = null;

                try
                {
                    data = encoding.GetBytes(new[] { c });

                    if (buffers.Count == 0 || buffers[buffers.Count - 1].Length >= 32)
                    {
                        SecureArray<byte> buffer = new SecureArray<byte>(data, false, true);
                        buffers.Add(buffer);
                    }
                    else
                    {
                        buffers[buffers.Count - 1].Append(data, true);
                    }

                }
                finally
                {
                    OverwriteArray(data);
                    c = '\0';
                }
            }
        }

        ///<summary> Appends a string to the SecureString, string will be destroyed after the operation </summary>
        ///<param name="str"> The string to append </param>
        ///<param name="encoding"> The encoding of the string </param>
        ///<remarks>The input string cannot be destroyed since this could affect the "string pool" by also eliminating the content of other unrelated strings but with the same content. Keep in mind that the string with which this SecureString will be created will remain in memory.</remarks>
        public void Append(string str)
        {
            using (new SimpleLock(lockObject))
            {
                if (isReadOnly) throw new InvalidOperationException("SecureString is read-only");

                byte[]? data = null;

                try
                {
                    data = encoding.GetBytes(str);

                    if (buffers.Count == 0 || buffers[buffers.Count - 1].Length >= 32)
                    {
                        SecureArray<byte> buffer = new SecureArray<byte>(data, false, true);
                        buffers.Add(buffer);
                    }
                    else
                    {
                        buffers[buffers.Count - 1].Append(data, true);
                    }

                }
                finally
                {
                    OverwriteArray(data);
                }
            }
        }

        ///<summary> Appends a byte array to the SecureString. </summary>
        ///<param name="bytes"> The byte array to append </param>
        ///<param name="destroyArray"> If true, the byte array will be destroyed after the operation </param>
        ///<remarks> Default behaviour is to destroy the byte array after the SecureString is created </remarks>
        public void Append(byte[] bytes, bool destroyArray = true)
        {
            using (new SimpleLock(lockObject))
            {
                if (isReadOnly) throw new InvalidOperationException("SecureString is read-only");

                if (bytes.Length % encoding.GetBytes(new[] { 'a' }).Length != 0) throw new InvalidOperationException("Byte array length is not a multiple of the encoding char size");

                try
                {

                    if (buffers.Count == 0 || buffers[buffers.Count - 1].Length >= 32)
                    {
                        SecureArray<byte> buffer = new SecureArray<byte>(bytes, false, true);
                        buffers.Add(buffer);
                    }
                    else
                    {
                        buffers[buffers.Count - 1].Append(bytes, true);
                    }
                }
                finally
                {
                    if (destroyArray) OverwriteArray(bytes);
                }
            }
        }

        ///<summary> Appends a SecureString to the SecureString. </summary>
        ///<param name="secureString"> The SecureString to append </param>
        ///<param name="destroySecureString"> If true, the SecureString will be destroyed after the operation </param>
        public void Append(SecureString secureString, bool destroySecureString = true)
        {
            using (new SimpleLock(lockObject))
            {
                if (isReadOnly) throw new InvalidOperationException("SecureString is read-only");

                byte[] data = secureString.ToBytes();

                try
                {
                    if (buffers.Count == 0 || buffers[buffers.Count - 1].Length >= 32)
                    {
                        SecureArray<byte> buffer = new SecureArray<byte>(data);
                        buffers.Add(buffer);
                    }
                    else
                    {
                        buffers[buffers.Count - 1].Append(data);
                    }
                }
                finally
                {
                    OverwriteArray(data);
                    if (destroySecureString) secureString.Dispose();
                }
            }
        }

        ///<summary> Obtains the index of the first occurrence of a char in the SecureString. </summary>
        ///<param name="c"> The char to search </param>
        ///<returns> The index of the first occurrence of the char, if not found, returns -1 </returns>
        ///<remarks> If a char that is not in the encoding is in the SecureString wrong results may be returned </remarks>
        public int IndexOf(char c)
        {
            using (new SimpleLock(lockObject))
            {
                byte[] charBytes = encoding.GetBytes(new[] { c });
                byte[]? bufferData = null;
                byte[][]? dividedBytes = null;

                int charLength = charBytes.Length;

                try
                {
                    int index = 0;

                    foreach (SecureArray<byte> buffer in buffers)
                    {

                        bufferData = buffer.Data;
                        dividedBytes = DivideBytesByEncoding(bufferData, true);

                        for (int i = 0; i < dividedBytes.Length; i++)
                        {

                            if (ByteArrayEquals(charBytes, dividedBytes[i])) return index;

                            index++;

                            OverwriteArray(dividedBytes[i]);
                        }

                        OverwriteArray(bufferData);
                    }

                    return -1;

                }
                finally
                {

                    OverwriteArray(charBytes);
                    OverwriteArray(bufferData);

                    if (dividedBytes != null)
                    {
                        for (int i = 0; i < dividedBytes.Length; i++) OverwriteArray(dividedBytes[i]);
                        OverwriteArray(dividedBytes);
                    }
                }
            }
        }

        ///<summary> Obtains the substring of the SecureString. </summary>
        ///<param name="start"> The start index of the substring </param>
        ///<param name="length"> The length of the substring </param>
        ///<returns> The substring of the SecureString </returns>
        ///<remarks> If the length is negative, an ArgumentOutOfRangeException will be thrown. If the length is 0, an empty SecureString will be returned. If the start index is negative, an ArgumentOutOfRangeException will be thrown. If the start index is greater than the length of the SecureString, an ArgumentOutOfRangeException will be thrown. If the start index plus the length is greater than the length of the SecureString, an ArgumentOutOfRangeException will be thrown. </remarks>
        public SecureString Substring(int start, int length)
        {

            using (new SimpleLock(lockObject))
            {

                if (length < 0) throw new ArgumentOutOfRangeException("Length cannot be negative or zero");
                if (length == 0) return new SecureString(encoding);
                if (start < 0 || start + length > Length) throw new ArgumentOutOfRangeException("Start index and length are out of range");

                int charSize = encoding.GetBytes(new[] { 'a' }).Length;

                int count = 0;
                start *= charSize;
                length *= charSize;

                byte[] finalData = new byte[length];
                byte[]? bufferData = null;

                try
                {
                    foreach (SecureArray<byte> buffer in buffers)
                    {

                        int bufferLength = buffer.Length;

                        if (start >= bufferLength)
                        {
                            start -= bufferLength;
                            OverwriteArray(bufferData);
                            continue;
                        }

                        bufferData = buffer.Data;

                        if (start + length - charSize > bufferLength - charSize + 1)
                        {

                            int actLength = bufferLength - start;

                            Buffer.BlockCopy(bufferData, start, finalData, count, actLength);

                            start = 0;
                            count += actLength;
                            length -= actLength;

                            OverwriteArray(bufferData);
                        }
                        else
                        {
                            Buffer.BlockCopy(bufferData, start, finalData, count, length);

                            OverwriteArray(bufferData);
                            return new SecureString(finalData, encoding);
                        }
                    }

                    throw new Exception("Unexpected error. Check if all used chars are in the selected encoding for the SecureString");

                }
                finally
                {
                    OverwriteArray(finalData);
                    OverwriteArray(bufferData);
                }
            }
        }

        ///<summary> Obtains the substring of the SecureString from the start index to the end of the SecureString. </summary>
        ///<param name="start"> The start index of the substring </param>
        ///<returns> The substring of the SecureString </returns>
        public SecureString Substring(int start)
        {
            using (new SimpleLock(lockObject))
            {
                if (start < 0 || start > Length) throw new ArgumentOutOfRangeException("Start index are out of range");
                return Substring(start, Length - start);
            }
        }

        ///<summary> Removes a substring from the SecureString. </summary>
        ///<param name="start"> The start index of the substring to remove </param>
        ///<param name="length"> The length of the substring to remove </param>
        ///<returns> The SecureString without the removed substring </returns>
        public SecureString Remove(int start, int length)
        {
            using (new SimpleLock(lockObject))
            {
                if (length < 0) throw new ArgumentOutOfRangeException("Length cannot be negative or zero");
                if (length == 0) return new SecureString(encoding);

                if (start < 0 || start + length > Length) throw new ArgumentOutOfRangeException("Start index is out of range");

                SecureString newString = Substring(0, start);
                newString.Append(Substring(start + length));

                return newString;
            }
        }

        ///<summary> Removes a substring from the SecureString, from the start index to the end of the SecureString </summary>
        ///<param name="start"> The start index of the substring to remove </param>
        ///<returns> The SecureString without the removed substring </returns>
        public SecureString Remove(int start)
        {
            using (new SimpleLock(lockObject))
            {
                if (start < 0 || start > Length) throw new ArgumentOutOfRangeException("Start index is out of range");
                return Remove(start, Length - start);
            }
        }

        ///<summary>Splits the SecureString into multiple SecureStrings using the given char as separator. </summary>
        ///<param name="separators"> The char to use as separator </param>
        ///<returns> An array with the splitted SecureStrings </returns>
        public SecureString[] Split(params char[] separators)
        {
            using (new SimpleLock(lockObject))
            {
                int[] indexes = new int[separators.Length];
                SecureString copy = new SecureString(this);

                List<SecureString> parts = new List<SecureString>();

                do
                {

                    for (int i = 0; i < separators.Length; i++)
                    {
                        if (indexes[i] == int.MaxValue) continue;

                        indexes[i] = copy.IndexOf(separators[i]);

                        if (indexes[i] == -1) indexes[i] = int.MaxValue;
                    }

                    int min = indexes.Min();

                    if (min == int.MaxValue)
                    {
                        parts.Add(copy);
                        break;
                    }

                    int minIndex = Array.IndexOf(indexes, min);

                    parts.Add(copy.Substring(0, min));
                    copy = copy.Remove(0, min + 1);

                } while (true);

                return parts.ToArray();
            }
        }

        ///<summary>Splits the SecureString into multiple SecureStrings using the given string as separator. </summary>
        ///<param name="separators"> The string to use as separator </param>
        ///<returns> An array with the splitted SecureStrings </returns>
        public SecureString[] Split(params string[] separators)
        {
            using (new SimpleLock(lockObject))
            {
                byte[][] separatorBytes = new byte[separators.Length][];
                for (int i = 0; i < separators.Length; i++) separatorBytes[i] = encoding.GetBytes(separators[i]);

                int[] indexes = new int[separators.Length];
                SecureString copy = new SecureString(this);

                List<SecureString> parts = new List<SecureString>();

                do
                {

                    for (int i = 0; i < separators.Length; i++)
                    {
                        if (indexes[i] == int.MaxValue) continue;

                        indexes[i] = copy.IndexOf(separators[i][0]);

                        if (indexes[i] == -1) indexes[i] = int.MaxValue;
                    }

                    int min;
                    int minIndex;

                    do
                    {

                        min = indexes.Min();
                        minIndex = Array.IndexOf(indexes, min);

                        if (min == int.MaxValue)
                        {
                            parts.Add(copy);
                            break;
                        }

                        if (copy.Substring(min, separators[minIndex].Length).ToBytes().SequenceEqual(separatorBytes[minIndex]))
                        {
                            parts.Add(copy.Substring(0, min));
                            copy = copy.Remove(0, min + separators[minIndex].Length);
                            break;
                        }
                        else indexes[minIndex] = int.MaxValue - 1;

                    } while (true);

                    if (min == int.MaxValue) break;

                } while (true);

                foreach (byte[] b in separatorBytes) OverwriteArray(b);
                OverwriteArray(separatorBytes);

                return parts.ToArray();
            }
        }

        ///<summary>Splits the SecureString into multiple SecureStrings using the given SecureString as separator. </summary>
        ///<param name="separators"> The SecureString to use as separator </param>
        ///<returns> An array with the splitted SecureStrings </returns>
        ///<remarks> SecureStrings used as separators encoding should be the same as the splitted SecureString </remarks>
        public SecureString[] Split(params SecureString[] separators)
        {
            using (new SimpleLock(lockObject))
            {
                byte[][] separatorBytes = new byte[separators.Length][];
                for (int i = 0; i < separators.Length; i++) separatorBytes[i] = separators[i].ToBytes();

                int[] indexes = new int[separators.Length];
                SecureString copy = new SecureString(this);

                List<SecureString> parts = new List<SecureString>();

                do
                {

                    for (int i = 0; i < separators.Length; i++)
                    {
                        if (indexes[i] == int.MaxValue) continue;

                        indexes[i] = copy.IndexOf(separators[i].CharAt(0));

                        if (indexes[i] == -1) indexes[i] = int.MaxValue;
                    }

                    int min;
                    int minIndex;

                    do
                    {

                        min = indexes.Min();
                        minIndex = Array.IndexOf(indexes, min);

                        if (min == int.MaxValue)
                        {
                            parts.Add(copy);
                            break;
                        }

                        if (copy.Substring(min, separators[minIndex].Length).ToBytes().SequenceEqual(separatorBytes[minIndex]))
                        {
                            parts.Add(copy.Substring(0, min));
                            copy = copy.Remove(0, min + separators[minIndex].Length);
                            break;
                        }
                        else indexes[minIndex] = int.MaxValue - 1;

                    } while (true);

                    if (min == int.MaxValue) break;

                } while (true);

                foreach (byte[] b in separatorBytes) OverwriteArray(b);
                OverwriteArray(separatorBytes);

                return parts.ToArray();
            }
        }

        ///<summary> Obtains the char at the given index </summary>
        ///<param name="i"> The index of the char </param>
        ///<returns> The char at the given index </returns>
        public char CharAt(int i)
        {
            using (new SimpleLock(lockObject))
            {
                if (i < 0 || i >= Length) throw new ArgumentOutOfRangeException("Index out of range");

                int charSize = encoding.GetByteCount(new char[] { 'a' });

                byte[] data = ToBytes();
                byte[] bytes = new byte[charSize];

                try
                {
                    Buffer.BlockCopy(data, i * charSize, bytes, 0, charSize);
                    OverwriteArray(data);
                    return encoding.GetChars(bytes)[0];
                }
                finally
                {
                    OverwriteArray(data);
                    OverwriteArray(bytes);
                }
            }
        }

        ///<summary> Obtains a int from the SecureString. </summary>
        ///<returns> The int value of the SecureString, the sum of the numeric values of the chars </returns>
        public int ToInt()
        {
            using (new SimpleLock(lockObject))
            {

                int sum = 0;

                foreach (SecureArray<byte> buffer in buffers)
                {
                    byte[] data = buffer.Data;
                    byte[][] dividedBytes = DivideBytesByEncoding(data, true);

                    foreach (byte[] bytes in dividedBytes)
                    {
                        sum += encoding.GetString(bytes)[0];
                        OverwriteArray(bytes);
                    }

                    OverwriteArray(data);
                }

                return sum;
            }
        }

        ///<summary> Parses the SecureString to an int. </summary>
        ///<returns> The int value of the SecureString </returns>
        public int ParseToInt()
        {
            return int.Parse(ToString());
        }

        ///<summary> Checks if the SecureString is equal to another SecureString. </summary>
        ///<param name="obj"> The SecureString to compare </param>
        ///<returns> True if the SecureStrings are equal, false otherwise </returns>
        public override bool Equals(object? obj)
        {

            if (obj == null) return false;

            if (obj is SecureString secureString)
            {

                using (new SimpleLock(lockObject, secureString.lockObject))
                {
                    if (secureString.Length != Length) return false;

                    byte[] data1 = ToBytes();
                    byte[] data2 = secureString.ToBytes();

                    bool equals = data1.SequenceEqual(data2);

                    OverwriteArray(data1, data2);

                    return equals;
                }
            }

            return false;
        }

        ///<summary> Obtains the hash code of the SecureString </summary>
        ///<returns> The hash code of the SecureString </returns>
        public override int GetHashCode()
        {
            using (new SimpleLock(lockObject))
            {
                byte[] data = ToBytes();
                int hash = data.GetHashCode();
                OverwriteArray(data);

                return hash;
            }
        }

        ///<summary> Obtains the content of the SecureString as a string. WARNING: this may be a security risk, use with caution </summary>
        ///<returns> The content of the SecureString </returns>
        public override string ToString()
        {
            using (new SimpleLock(lockObject))
            {
                byte[] bytes = ToBytes();
                string str = encoding.GetString(bytes);
                OverwriteArray(bytes);

                return str;
            }
        }

        #endregion

        #region Private methods

        ///<summary> Divides a byte array into multiple byte arrays, each one with the size of a char </summary>
        ///<param name="bytes"> The byte array to divide </param>
        ///<returns> A 2D array with the divided bytes </returns>
        private byte[][] DivideBytesByEncoding(byte[] bytes, bool destroyArray = true)
        {

            int charSize = encoding.GetByteCount(new char[] { 'a' });

            if (bytes.Length % charSize != 0) throw new Exception("Length of the byte array is not a multiple of the char size");

            byte[][] dividedBytes = new byte[bytes.Length / charSize][];

            for (int i = 0; i < dividedBytes.Length; i++) dividedBytes[i] = new byte[charSize];

            for (int i = 0; i < bytes.Length; i += charSize)
            {

                int div = i / charSize;

                Buffer.BlockCopy(bytes, i, dividedBytes[div], 0, charSize);
            }

            if (destroyArray) OverwriteArray(bytes);

            return dividedBytes;
        }

        #endregion
    }
}
