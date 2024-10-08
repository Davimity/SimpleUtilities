using System.Runtime.InteropServices;
using System.Text;
using SimpleUtilities.Security.SecureInformation.Types.Numerics;

using static SimpleUtilities.Threading.SimpleLock;

namespace SimpleUtilities.Security.SecureInformation.Types.Texts{
    ///<summary>Represents a secure string structure.</summary>
    ///<remarks>THREAD SAFE</remarks>
    public class SecureString : SecureData {

        #region Variables

            private readonly Encoding encoding;
            private readonly int charSize;

        #endregion

        #region Constructors

            /// <summary> Creates a new SecureString. Default encoding is UTF-8 </summary>
            /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
            public SecureString(bool isReadOnly = false){
                encoding = Encoding.UTF8;
                charSize = encoding.GetByteCount(['a']);
                if (isReadOnly) MakeReadOnly();
            }

            /// <summary> Creates a new SecureString. </summary>
            /// <param name="encoding"> The encoding of the SecureString </param>
            /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
            public SecureString(Encoding encoding, bool isReadOnly = false){
                this.encoding = encoding;
                charSize = encoding.GetByteCount(['a']);
                if (isReadOnly) MakeReadOnly();
            }

            /// <summary> Creates a new SecureString from a string. Default encoding is UTF-8. </summary>
            /// <param name="str"> The string to convert </param>
            /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
            /// <remarks>The input "string" cannot be destroyed since this could affect the "string pool" by also eliminating the content of other unrelated strings but with the same content. Keep in mind that the string with which this SecureString will be created will remain in memory.</remarks>
            public SecureString(string str, bool isReadOnly = false){
                encoding = Encoding.UTF8;
                charSize = encoding.GetByteCount(['a']);

                FromBytes(encoding.GetBytes(str));

                if (isReadOnly) MakeReadOnly();
            }

            /// <summary> Creates a new SecureString from a string. </summary>
            /// <param name="str"> The string to convert </param>
            /// <param name="encoding"> The encoding of the string </param>
            /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
            /// <remarks>The input "string" cannot be destroyed since this could affect the "string pool" by also eliminating the content of other unrelated strings but with the same content. Keep in mind that the string with which this SecureString will be created will remain in memory.</remarks>
            public SecureString(string str, Encoding encoding, bool isReadOnly = false){
                this.encoding = encoding;
                charSize = encoding.GetByteCount(['a']);

                FromBytes(encoding.GetBytes(str));

                if (isReadOnly) MakeReadOnly();
            }

            /// <summary> Creates a new SecureString from a char array. Default encoding is UTF-8 </summary>
            /// <param name="chars"> The char array to convert </param>
            /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
            /// <param name="destroyArray"> If true, the char array will be destroyed after the SecureString is created </param>
            /// <remarks> Default behaviour is to destroy the char array after the SecureString is created </remarks>
            public SecureString(char[] chars, bool isReadOnly = false, bool destroyArray = false){
                encoding = Encoding.UTF8;
                charSize = encoding.GetByteCount(['a']);

                FromBytes(Encoding.UTF8.GetBytes(chars));

                if (destroyArray) OverwriteArray(chars);
                if (isReadOnly) MakeReadOnly();
            }

            /// <summary> Creates a new SecureString from a char array. </summary>
            /// <param name="chars"> The char array to convert </param>
            /// <param name="encoding"> The encoding of the char array </param>
            /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
            /// <param name="destroyArray"> If true, the char array will be destroyed after the SecureString is created </param>
            /// <remarks> Default behaviour is to destroy the char array after the SecureString is created </remarks>
            public SecureString(char[] chars, Encoding encoding, bool isReadOnly = false, bool destroyArray = false){
                this.encoding = encoding;
                charSize = encoding.GetByteCount(['a']);

                FromBytes(encoding.GetBytes(chars));

                if (destroyArray) OverwriteArray(chars);
                if (isReadOnly) MakeReadOnly();
            }

            /// <summary> Creates a new SecureString from a byte array. Default encoding is UTF-8 </summary>
            /// <param name="bytes"> The char array to convert </param>
            /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
            /// <param name="destroyArray"> If true, the byte array will be destroyed after the SecureString is created </param>
            /// <remarks> Default behaviour is to destroy the byte array after the SecureString is created </remarks>
            public SecureString(byte[] bytes, bool isReadOnly = false, bool destroyArray = false){
                encoding = Encoding.UTF8;
                charSize = encoding.GetByteCount(['a']);

                FromBytes(bytes, destroyArray);

                if (isReadOnly) MakeReadOnly();
            }

            /// <summary> Creates a new SecureString from a byte array. </summary>
            /// <param name="bytes"> The char array to convert </param>
            /// <param name="encoding"> The encoding of the SecureString </param>
            /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
            /// <param name="destroyArray"> If true, the byte array will be destroyed after the SecureString is created </param>
            /// <remarks> Default behaviour is to destroy the byte array after the SecureString is created </remarks>
            public SecureString(byte[] bytes, Encoding encoding, bool isReadOnly = false, bool destroyArray = false){
                this.encoding = encoding;
                charSize = encoding.GetByteCount(['a']);

                FromBytes(bytes, destroyArray);

                if (isReadOnly) MakeReadOnly();
            }

            /// <summary> Creates a new SecureString from System.Security.SecureString. Default encoding is UTF-8 </summary>
            /// <param name="ssSecureString"> The System.Security.SecureString to convert </param>
            /// <param name="isReadOnly"> If true, the SecureString will be read-only </param>
            /// <param name="destroyInput"> If true, the System.Security.SecureString will be destroyed after the SecureString is created </param>
            /// <remarks> Default behaviour is to destroy the System.Security.SecureString after the SecureString is created </remarks>
            public SecureString(System.Security.SecureString ssSecureString, bool isReadOnly = false, bool destroyInput = false){
                encoding = Encoding.UTF8;
                charSize = encoding.GetByteCount(['a']);

                var bstr = nint.Zero;
                byte[]? workArray = null;
                GCHandle? handle = null;

                try{
                    bstr = Marshal.SecureStringToBSTR(ssSecureString);
                    var lengthInChars = ssSecureString.Length;

                    unsafe{
                        var bstrChars = (char*)bstr;
                        workArray = new byte[Encoding.UTF8.GetMaxByteCount(lengthInChars)];

                        fixed (byte* workArrayPtr = workArray){
                            int byteCount = Encoding.UTF8.GetBytes(bstrChars, lengthInChars, workArrayPtr, workArray.Length);
                            Array.Resize(ref workArray, byteCount);
                            handle = GCHandle.Alloc(workArray, GCHandleType.Pinned);
                        }
                    }

                    FromBytes(workArray);
                }
                finally{
                    if (workArray != null)
                        for (var i = 0; i < workArray.Length; i++)
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
            public SecureString(System.Security.SecureString ssSecureString, Encoding encoding, bool isReadOnly = false, bool destroyInput = true){

                this.encoding = encoding;
                charSize = encoding.GetByteCount(['a']);

                var bstr = nint.Zero;
                byte[]? workArray = null;
                GCHandle? handle = null;

                try{
                    bstr = Marshal.SecureStringToBSTR(ssSecureString);
                    var lengthInChars = ssSecureString.Length;

                    unsafe{
                        var bstrChars = (char*)bstr;
                        workArray = new byte[encoding.GetMaxByteCount(lengthInChars)];

                        fixed (byte* workArrayPtr = workArray)
                        {
                            var byteCount = encoding.GetBytes(bstrChars, lengthInChars, workArrayPtr, workArray.Length);
                            Array.Resize(ref workArray, byteCount);
                            handle = GCHandle.Alloc(workArray, GCHandleType.Pinned);
                        }
                    }

                    FromBytes(workArray);
                }
                finally{
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
            public SecureString(SecureString secureString, bool isReadOnly = false, bool destroyInput = false){
                Lock(secureString);

                    encoding = secureString.encoding;
                    charSize = encoding.GetByteCount(['a']);

                    foreach (var buffer in secureString.Buffers)
                        AddBuffer(buffer.GetData());

                    if (isReadOnly) MakeReadOnly();
                    if (destroyInput) secureString.Dispose();

                Unlock(secureString);
            }

        #endregion

        #region Operators

            public static SecureString operator +(SecureString s1, SecureString s2){
                Lock(s1, s2);

                    var newString = new SecureString(s1);
                    newString.Append(s2);

                Unlock(s1, s2);

                return newString;
            }       
            public static SecureString operator +(SecureString s1, string s2){
                Lock(s1);

                    var newString = new SecureString(s1);
                    newString.Append(s2);

                Unlock(s1);

                return newString;
            }
            public static SecureString operator +(string s1, SecureString s2) {
                Lock(s2);

                var newString = new SecureString(s1);
                newString.Append(s2);

                Unlock(s2);

                return newString;
            }
            public static SecureString operator +(SecureString s1, char c){
                Lock(s1);

                    var newString = new SecureString(s1);
                    newString.Append(c);

                Unlock(s1);

                return newString;
            }
            public static SecureString operator +(char c, SecureString s2) {
                Lock(s2);

                var newString = new SecureString($"{c}");
                newString.Append(s2);

                Unlock(s2);

                return newString;
            }


            public static bool operator ==(SecureString s1, SecureString s2){
                try {
                    Lock(s1, s2);

                    if (s1.GetLength() != s2.GetLength()) return false;

                    for (var i = 0; i < s1.GetLength(); i++)
                        if (s1.CharAt(i) != s2.CharAt(i))
                            return false;

                    return true;
                }
                finally {
                    Unlock(s1, s2);
                }
            }
            public static bool operator ==(SecureString s1, string s2){
                try {
                    Lock(s1);

                    if (s1.GetLength() != s2.Length) return false;

                    for (var i = 0; i < s1.GetLength(); i++)
                        if (s1.CharAt(i) != s2[i])
                            return false;

                    return true;
                }
                finally {
                    Unlock(s1);
                }
            }
            public static bool operator ==(string s1, SecureString s2) => s2 == s1;


            public static bool operator !=(SecureString s1, SecureString s2){
                try {
                    Lock(s1, s2);

                    if (s1.GetLength() != s2.GetLength()) return true;

                    for (var i = 0; i < s1.GetLength(); i++)
                        if (s1.CharAt(i) != s2.CharAt(i))
                            return true;

                    return false;
                }
                finally {
                    Unlock(s1, s2);
                }
            }
            public static bool operator !=(SecureString s1, string s2){
                try {
                    Lock(s1);

                    if (s1.GetLength() != s2.Length) return true;

                    for (var i = 0; i < s1.GetLength(); i++)
                        if (s1.CharAt(i) != s2[i])
                            return true;

                    return false;
                }
                finally {
                    Unlock(s1);
                }
            }
            public static bool operator !=(string s1, SecureString s2) => s2 != s1;


            public static implicit operator string(SecureString s){
                try {
                    Lock(s);
                    return s.ToString();
                }
                finally {
                    Unlock(s);
                }
            }
            public static explicit operator SecureString(string s){
                try {
                    Lock(s);
                    return new SecureString(s);
                }
                finally {
                    Unlock(s);
                }
            }

        #endregion

        #region Public methods

            /// <summary> Appends a char to the SecureString.</summary>
            /// <param name="c"> The char to append </param>
            /// <remarks>The input char won't be destroyed. Keep in mind that the char will remain in memory.</remarks>
            public void Append(char c){
                Lock(this);

                    if (IsReadOnly) throw new InvalidOperationException("SecureString is read-only");

                    AddBuffer(encoding.GetBytes([c]));

                Unlock(this);
            }

            ///<summary> Appends a string to the SecureString, string will be destroyed after the operation </summary>
            ///<param name="str"> The string to append </param>
            ///<remarks>The input string cannot be destroyed since this could affect the "string pool" by also eliminating the content of other unrelated strings but with the same content. Keep in mind that the string with which this SecureString will be created will remain in memory.</remarks>
            public void Append(string str){
                Lock(this);

                    if (IsReadOnly) throw new InvalidOperationException("SecureString is read-only");

                    AddBuffer(encoding.GetBytes(str));

                Unlock(this);
            }

            ///<summary> Appends a byte array to the SecureString. </summary>
            ///<param name="bytes"> The byte array to append </param>
            ///<param name="destroyArray"> If true, the byte array will be destroyed after the operation </param>
            ///<remarks> Default behaviour is to destroy the byte array after the SecureString is created </remarks>
            public void Append(byte[] bytes, bool destroyArray = false){
                Lock(this);

                    if (IsReadOnly) throw new InvalidOperationException("SecureString is read-only");

                    AddBuffer(bytes, destroyArray);

                    if (destroyArray) OverwriteArray(bytes);
                
                Unlock(this);
            }

            ///<summary> Appends a SecureString to the SecureString. </summary>
            ///<param name="secureString"> The SecureString to append </param>
            ///<param name="destroySecureString"> If true, the SecureString will be destroyed after the operation </param>
            public void Append(SecureString secureString, bool destroySecureString = false){
                Lock(this, secureString);

                    if (IsReadOnly) throw new InvalidOperationException("SecureString is read-only");

                    AddBuffer(secureString.ToBytes());

                    if (destroySecureString) secureString.Dispose();
                
                Unlock(this, secureString);
            }


            ///<summary> Obtains the index of the first occurrence of a char in the SecureString. </summary>
            ///<param name="c"> The char to search </param>
            ///<returns> The index of the first occurrence of the char, if not found, returns -1 </returns>
            ///<remarks> If a char that is not in the encoding is the SecureString wrong results may be returned </remarks>
            public int IndexOf(char c){
                try {

                    Lock(this);

                    var index = 0;

                    foreach (var buffer in Buffers) {
                        var bdata = buffer.GetData();
                        var chars = encoding.GetChars(bdata);
                        OverwriteArray(bdata);

                        foreach (var ch in chars) {
                            if (ch == c) {
                                OverwriteArray(chars);
                                return index;
                            }

                            index++;
                        }

                        OverwriteArray(chars);
                    }

                    return -1;
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary> Obtains the index of the first occurrence of a SecureChar in the SecureString. </summary>
            ///<param name="c"> The SecureChar to search </param>
            ///<returns> The index of the first occurrence of the SecureChar, if not found, returns -1 </returns>
            public SecureInt IndexOf(SecureChar c) {
                try {

                    Lock(this);

                    var index = (SecureInt)0;

                    foreach (var buffer in Buffers) {
                        var bdata = buffer.GetData();
                        var chars = encoding.GetChars(bdata);
                        OverwriteArray(bdata);

                        foreach (var ch in chars) {
                            if (ch == c) {
                                OverwriteArray(chars);
                                return index;
                            }

                            index++;
                        }

                        OverwriteArray(chars);
                    }

                    return (SecureInt)(-1);
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary> Obtains the index of the first occurrence of a char in the SecureString. </summary>
            ///<param name="s"> The string to search </param>
            ///<returns> The index of the first occurrence of the string, if not found, returns -1 </returns>
            ///<remarks> If a string that is not in the encoding is the SecureString wrong results may be returned </remarks>
            public int IndexOf(string s) {
                try{

                    Lock(this);

                    if (s.Length == 1) return IndexOf(s[0]);

                    var index = 0;

                    foreach (var buffer in Buffers) {
                        var bdata = buffer.GetData();
                        var chars = encoding.GetChars(bdata);
                        OverwriteArray(bdata);

                        var coincidenceChars = 0;

                        foreach (var c in chars) {
                            if (c != s[coincidenceChars]) {
                                index++;
                                coincidenceChars = 0;
                                continue;
                            }

                            if (++coincidenceChars != s.Length) {
                                index++;
                                continue;
                            }

                            OverwriteArray(chars);
                            return index - s.Length + 1;
                        }
                    }

                    return -1;
                }finally{
                    Unlock(this);
                }
            }

            ///<summary> Obtains the index of the first occurrence of a SecureString in the SecureString. </summary>
            /// <param name="s"> The SecureString to search </param>
            /// <returns> The index of the first occurrence of the SecureString, if not found, returns -1 </returns>
            public SecureInt IndexOf(SecureString s) {
                try{

                    Lock(this, s);

                    if (s.GetLength() == 1) return IndexOf(s.SecureCharAt(0));

                    var index = (SecureInt)0;

                    foreach (var buffer in Buffers) {
                        var bdata = buffer.GetData();
                        var chars = encoding.GetChars(bdata);
                        OverwriteArray(bdata);

                        var coincidenceChars = 0;

                        foreach (var c in chars) {
                            if (c != s.CharAt(coincidenceChars)) {
                                index++;
                                coincidenceChars = 0;
                                continue;
                            }

                            if (++coincidenceChars != s.GetLength()) {
                                index++;
                                continue;
                            }

                            OverwriteArray(chars);
                            return index - s.GetLength() + 1;
                        }
                    }

                    return (SecureInt)(-1);
                }finally{
                    Unlock(this, s);
                }
            }


            ///<summary> Obtains the substring of the SecureString. </summary>
            ///<param name="start"> The start index of the substring </param>
            ///<param name="length"> The length of the substring </param>
            ///<returns> The substring of the SecureString </returns>
            ///<remarks> If the length is negative, an ArgumentOutOfRangeException will be thrown. If the length is 0, an empty SecureString will be returned. If the start index is negative, an ArgumentOutOfRangeException will be thrown. If the start index is greater than the length of the SecureString, an ArgumentOutOfRangeException will be thrown. If the start index plus the length is greater than the length of the SecureString, an ArgumentOutOfRangeException will be thrown. </remarks>
            public SecureString Substring(int start, int length){

                ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));
                ArgumentOutOfRangeException.ThrowIfNegative(start, nameof(start));
                ArgumentOutOfRangeException.ThrowIfGreaterThan(start + length, GetLength(), nameof(start) + nameof(length));

                var finalData = new char[length];

                try {

                    Lock(this);

                    if (length == 0) return new SecureString(encoding);

                    var count = 0;

                    foreach (var buffer in Buffers) {

                        var bufferLength = buffer.GetLength() / charSize;

                        if (start >= bufferLength) {
                            start -= bufferLength;
                            continue;
                        }

                        var bdata = buffer.GetData();
                        var chars = encoding.GetChars(bdata);
                        OverwriteArray(bdata);

                        if (start + length <= bufferLength) {

                            Array.Copy(chars, start, finalData, count, length);
                            OverwriteArray(chars);

                            return new SecureString(finalData, encoding);
                        }
                        else {

                            var actLength = bufferLength - start;

                            Array.Copy(chars, start, finalData, count, actLength);

                            start = 0;
                            count += actLength;
                            length -= actLength;

                            OverwriteArray(chars);
                        }
                    }
                }
                finally {
                    OverwriteArray(finalData);
                    Unlock(this);
                }

                throw new InvalidOperationException("Unexpected error");
            }

            ///<summary> Obtains the substring of the SecureString from the start index to the end of the SecureString. </summary>
            ///<param name="start"> The start index of the substring </param>
            ///<returns> The substring of the SecureString </returns>
            public SecureString Substring(int start) {
                try{
                    Lock(this);

                    if (start < 0 || start > GetLength()) throw new ArgumentOutOfRangeException(nameof(start));
                    return Substring(start, GetLength() - start);
                }finally {
                    Unlock(this);
                }
            }


            ///<summary> Removes a substring from the SecureString. </summary>
            ///<param name="start"> The start index of the substring to remove </param>
            ///<param name="length"> The length of the substring to remove </param>
            ///<returns> The SecureString without the removed substring </returns>
            public SecureString Remove(int start, int length) {
                try{

                    ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));
                    ArgumentOutOfRangeException.ThrowIfNegative(start, nameof(start));
                    ArgumentOutOfRangeException.ThrowIfGreaterThan(start + length, GetLength(), nameof(start) + nameof(length));

                    Lock(this);

                    if (length == 0) return new SecureString(encoding);

                    var newString = Substring(0, start);
                    newString.Append(Substring(start + length));

                    return newString;
                }finally{
                    Unlock(this);
                }
            }

            ///<summary> Removes a substring from the SecureString, from the start index to the end of the SecureString </summary>
            ///<param name="start"> The start index of the substring to remove </param>
            ///<returns> The SecureString without the removed substring </returns>
            public SecureString Remove(int start) {
                try{
                    ArgumentOutOfRangeException.ThrowIfNegative(start, nameof(start));
                    ArgumentOutOfRangeException.ThrowIfGreaterThan(start, GetLength(), nameof(start));

                    Lock(this);

                    return Remove(start, GetLength() - start);
                }finally{
                    Unlock(this);
                }
            }


            ///<summary>Splits the SecureString into multiple SecureStrings using the given char as separator. </summary>
            ///<param name="separators"> The char to use as separator </param>
            ///<returns> An array with the split SecureStrings </returns>
            public SecureString[] Split(params char[] separators) {
                try{

                    Lock(this);

                    var indexes = new int[separators.Length];
                    var copy = new SecureString(this);
                    var parts = new List<SecureString>();

                    do {

                        for (var i = 0; i < separators.Length; i++) {
                            if (indexes[i] == int.MaxValue) continue;

                            indexes[i] = copy.IndexOf(separators[i]);

                            if (indexes[i] == -1) indexes[i] = int.MaxValue;
                        }

                        var min = indexes.Min();

                        if (min == int.MaxValue) {
                            parts.Add(copy);
                            break;
                        }

                        parts.Add(copy.Substring(0, min));
                        copy = copy.Remove(0, min + 1);

                    } while (true);

                    return parts.ToArray();
                }finally{
                    Unlock(this);
                }
            }

            ///<summary>Splits the SecureString into multiple SecureStrings using the given string as separator. </summary>
            ///<param name="separators"> The string to use as separator </param>
            ///<returns> An array with the split SecureStrings </returns>
            public SecureString[] Split(params string[] separators) {
                try{
                    Lock(this);

                    var separatorBytes = new byte[separators.Length][];
                    for (var i = 0; i < separators.Length; i++) separatorBytes[i] = encoding.GetBytes(separators[i]);

                    var indexes = new int[separators.Length];
                    var copy = new SecureString(this);

                    var parts = new List<SecureString>();

                    do {

                        for (var i = 0; i < separators.Length; i++) {
                            if (indexes[i] == int.MaxValue) continue;

                            indexes[i] = copy.IndexOf(separators[i][0]);

                            if (indexes[i] == -1) indexes[i] = int.MaxValue;
                        }

                        int min;

                        do {

                            min = indexes.Min();
                            var minIndex = Array.IndexOf(indexes, min);

                            if (min == int.MaxValue) {
                                parts.Add(copy);
                                break;
                            }

                            if (copy.Substring(min, separators[minIndex].Length).ToBytes().SequenceEqual(separatorBytes[minIndex])) {
                                parts.Add(copy.Substring(0, min));
                                copy = copy.Remove(0, min + separators[minIndex].Length);
                                break;
                            } 
                            
                            indexes[minIndex] = int.MaxValue - 1;

                        } while (true);

                        if (min == int.MaxValue) break;

                    } while (true);

                    foreach (var b in separatorBytes) OverwriteArray(b);

                    return parts.ToArray();
                }finally {
                    Unlock(this);
                }
            }

            ///<summary>Splits the SecureString into multiple SecureStrings using the given SecureString as separator. </summary>
            ///<param name="separators"> The SecureString to use as separator </param>
            ///<returns> An array with the split SecureStrings </returns>
            ///<remarks> SecureStrings used as separators encoding should be the same as the split SecureString </remarks>
            public SecureString[] Split(params SecureString[] separators) {
                try {

                    Lock(this);

                    var separatorBytes = new byte[separators.Length][];
                    for (var i = 0; i < separators.Length; i++) separatorBytes[i] = separators[i].ToBytes();

                    var indexes = new int[separators.Length];
                    var copy = new SecureString(this);

                    var parts = new List<SecureString>();

                    do {

                        for (var i = 0; i < separators.Length; i++) {
                            if (indexes[i] == int.MaxValue) continue;

                            indexes[i] = copy.IndexOf(separators[i].CharAt(0));

                            if (indexes[i] == -1) indexes[i] = int.MaxValue;
                        }

                        int min;

                        do {

                            min = indexes.Min();
                            var minIndex = Array.IndexOf(indexes, min);

                            if (min == int.MaxValue) {
                                parts.Add(copy);
                                break;
                            }

                            if (copy.Substring(min, separators[minIndex].GetLength()).ToBytes().SequenceEqual(separatorBytes[minIndex])) {
                                parts.Add(copy.Substring(0, min));
                                copy = copy.Remove(0, min + separators[minIndex].GetLength());
                                break;
                            }

                            indexes[minIndex] = int.MaxValue - 1;

                        } while (true);

                        if (min == int.MaxValue) break;

                    } while (true);

                    foreach (var b in separatorBytes) OverwriteArray(b);

                    return parts.ToArray();
                }finally {
                    Unlock(this);
                }
            }


            ///<summary> Obtains the char at the given index </summary>
            ///<param name="i"> The index of the char </param>
            ///<returns> The char at the given index </returns>
            public char CharAt(int i) {

                byte[]? bytes = null;

                try {
                    Lock(this);

                    if (i < 0 || i >= GetLength()) throw new ArgumentOutOfRangeException(nameof(i));

                    var data = ToBytes();
                    bytes = new byte[charSize];

                    Buffer.BlockCopy(data, i * charSize, bytes, 0, charSize);
                    OverwriteArray(data);
                    return encoding.GetChars(bytes)[0];
                }
                finally {
                    OverwriteArray(bytes);
                    Unlock(this);
                }
            }

            /// <summary> Obtains the SecureChar at the given index </summary>
            /// <param name="i"> The index of the SecureChar </param>
            /// <returns> The SecureChar at the given index </returns>
            public SecureChar SecureCharAt(int i) => CharAt(i);


            ///<summary> Sets a char at the given index </summary>
            ///<param name="i"> The index of the char </param>
            ///<param name="c"> The char to set </param>
            public void SetCharAt(int i, char c) {
                Lock(this);

                    if (i < 0 || i >= GetLength()) throw new ArgumentOutOfRangeException(nameof(i));

                    var data = ToBytes();

                    try {
                        Buffer.BlockCopy(encoding.GetBytes([c]), 0, data, i * charSize, charSize);
                        FromBytes(data);
                    }
                    finally {
                        OverwriteArray(data);
                    }

                Unlock(this);
            }
            

            ///<summary> Obtains an int from the SecureString. </summary>
            ///<returns> The int value of the SecureString, the sum of the numeric values of the chars </returns>
            public int ToInt() {
                try{

                    Lock(this);

                    var sum = 0;

                    foreach (var buffer in Buffers) {
                        var bdata = buffer.GetData();
                        var chars = encoding.GetChars(bdata);
                        OverwriteArray(bdata);

                        sum += chars.Sum(c => c);

                        OverwriteArray(chars);
                    }

                    return sum;
                }finally{
                    Unlock(this);
                }
            }

            ///<summary> Obtains a SecureInt from the SecureString. </summary>
            ///<returns> The SecureInt value of the SecureString, the sum of the numeric values of the chars </returns>
            public SecureInt ToSecureInt(){
                try{
                    
                    Lock(this);

                    var sum = (SecureInt)0;

                    foreach (var buffer in Buffers) {
                        var bdata = buffer.GetData();
                        var chars = encoding.GetChars(bdata);
                        OverwriteArray(bdata);

                        sum += chars.Sum(c => c);

                        OverwriteArray(chars);
                    }

                    return sum;
                }finally{
                    Unlock(this);
                }
            }


            ///<summary> Obtains a long from the SecureString. </summary>
            ///<returns> The long value of the SecureString, the sum of the numeric values of the chars </returns>
            public long ToLong() {
                try{

                    Lock(this);

                    var sum = 0L;

                    foreach (var buffer in Buffers) {
                        var bdata = buffer.GetData();
                        var chars = encoding.GetChars(bdata);
                        OverwriteArray(bdata);

                        sum += chars.Sum(c => c);

                        OverwriteArray(chars);
                    }

                    return sum;
                }finally{
                    Unlock(this);
                }
            }

            ///<summary> Obtains a SecureLong from the SecureString. </summary>
            ///<returns> The SecureLong value of the SecureString, the sum of the numeric values of the chars </returns>
            public SecureLong ToSecureLong() {
                try{

                    Lock(this);

                    var sum = (SecureLong)0;

                    foreach (var buffer in Buffers) {
                        var bdata = buffer.GetData();
                        var chars = encoding.GetChars(bdata);
                        OverwriteArray(bdata);

                        sum += chars.Sum(c => c);

                        OverwriteArray(chars);
                    }

                    return sum;
                }finally{
                    Unlock(this);
                }
            }


            ///<summary> Parses the SecureString to an int. </summary>
            ///<returns> The int value of the SecureString </returns>
            public int ParseToInt() {
                return int.Parse(ToString());
            }

            ///<summary> Parses the SecureString to an SecureInt. </summary>
            ///<returns> The SecureInt value of the SecureString </returns>
            public SecureInt ParseToSecureInt() {
                return new SecureInt(ParseToInt());
            }


            ///<summary> Parses the SecureString to a long. </summary>
            ///<returns> The long value of the SecureString </returns>
            public long ParseToLong() {
                return long.Parse(ToString());
            }

            ///<summary> Parses the SecureString to an SecureLong. </summary>
            ///<returns> The SecureLong value of the SecureString </returns>
            public SecureLong ParseToSecureLong() {
                return new SecureLong(ParseToLong());
            }


            ///<summary> Checks if the SecureString is equal to another SecureString. </summary>
            ///<param name="obj"> The SecureString to compare </param>
            ///<returns> True if the SecureStrings are equal, false otherwise </returns>
            public override bool Equals(object? obj) {

                if (obj == null) return false;

                if (obj is SecureString secureString) {

                    try {
                        Lock(this, secureString);

                        if (secureString.GetLength() != GetLength()) return false;

                        var data1 = ToBytes();
                        var data2 = secureString.ToBytes();

                        var equals = data1.SequenceEqual(data2);

                        OverwriteArray(data1, data2);

                        return equals;
                    }finally{
                        Unlock(this, secureString);
                    }
                }

                return false;
            }


            ///<summary> Obtains the hash code of the SecureString </summary>
            ///<returns> The hash code of the SecureString </returns>
            public override int GetHashCode() {
                try {
                    Lock(this);

                    var data = ToBytes();
                    var hash = data.GetHashCode();
                    OverwriteArray(data);

                    return hash;
                }finally{
                    Unlock(this);
                }
            }

            ///<summary> Obtains the content of the SecureString as a string. WARNING: this may be a security risk, use with caution </summary>
            ///<returns> The content of the SecureString </returns>
            public override string ToString() {
                try {
                    Lock(this);

                    var bytes = ToBytes();
                    var str = encoding.GetString(bytes);
                    OverwriteArray(bytes);

                    return str;
                }
                finally {
                    Unlock(this);
                }
            }

        #endregion

        #region Static methods

            ///<summary> Joins multiple SecureStrings using another SecureString as separator. </summary>
            ///<param name="secureStrings"> The SecureStrings to join </param>
            ///<param name="separator"> The SecureString to use as separator </param>
            ///<returns> The joined SecureString </returns>
            public static SecureString Join(SecureString[] secureStrings, SecureString separator) {
  
                var newString = new SecureString(secureStrings[0]);

                for (var i = 1; i < secureStrings.Length; i++) {
                    newString.Append(separator);
                    newString.Append(secureStrings[i]);
                }

                return newString;
            }

            /// <summary> Cheks if a SecureString is null or empty </summary>
            /// <param name="s"> The SecureString to check </param>
            /// <returns> True if the SecureString is null or empty, false otherwise </returns>
            public static bool IsNullOrEmpty(SecureString? s) {
                if(s is null) return true;

                try {
                    Lock(s);
                    return s.GetLength() == 0;
                }
                finally {
                    Unlock(s);
                }
            }

        #endregion

        #region Getters

        public override int GetLength() {
                var l = base.GetLength() / charSize;
                return l;
            }

            public Encoding GetEncoding() {
                try {
                    Lock(this);
                    return encoding;
                }
                finally {
                    Unlock(this);
                }
            }

        #endregion

    }
}
