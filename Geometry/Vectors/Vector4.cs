using System.Globalization;
using System.Numerics;

using static SimpleUtilities.Threading.SimpleLock;

namespace SimpleUtilities.Geometry.Vectors {
    ///<summary>Class that represents a 4D vector of a numeric T type, is preferred to use this class and not Vector if you know that the vector is 4D.</summary>
    ///<remarks>THREAD SAFE</remarks>
    public class Vector4<T> where T : INumber<T> {

        #region Static Variables

            public static readonly Vector4<T> Zero = new();
            public static readonly Vector4<T> One = new(T.One);

        #endregion

        #region Variables

            private T x;
            private T y;
            private T z;
            private T w;

        #endregion

        #region Constructors

            public Vector4() {
                x = T.Zero;
                y = T.Zero;
                z = T.Zero;
                w = T.Zero;
            }

            public Vector4(T all) {
                x = all;
                y = all;
                z = all;
                w = all;
            }

            public Vector4(T x, T y, T z, T w) {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

            public Vector4(T[] values) {
                if (values.Length != 4) throw new ArgumentException("Values array must have 4 elements.");

                x = values[0];
                y = values[1];
                z = values[2];
                w = values[3];
            }

            public Vector4(Vector2<T> vector) {

                var values = vector.GetXy();

                x = values[0];
                y = values[1];
                z = T.Zero;
                w = T.Zero;
            }

            public Vector4(Vector2<T> vector, T z, T w) {

                var values = vector.GetXy();

                x = values[0];
                y = values[1];
                this.z = z;
                this.w = w;
            }

            public Vector4(Vector3<T> vector) {

                var values = vector.GetXyz();

                x = values[0];
                y = values[1];
                z = values[2];
                w = T.Zero;
            }

            public Vector4(Vector3<T> vector, T w) {

                var values = vector.GetXyz();

                x = values[0];
                y = values[1];
                z = values[2];
                this.w = w;
            }

            public Vector4(Vector4<T> vector) {
                Lock(vector);

                    x = vector.x;
                    y = vector.y;
                    z = vector.z;
                    w = vector.w;

                Unlock(vector);   
            }

        #endregion

        #region Operators

            ///<summary>Adds two vector4 component by component.</summary>
            ///<param name="a">The first vector4 to add.</param>
            ///<param name="b">The second vector4 to add.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This operator is only available for vectors of the same type. If you want to add vector4 of different types use method Add.</remarks>
            public static Vector4<T> operator +(Vector4<T> a, Vector4<T> b) {
                try {
                    Lock(a, b);
                    return new Vector4<T>(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
                }
                finally {
                    Unlock(a, b);
                }
            }

            ///<summary>Adds a scalar to each component of a vector4.</summary>
            ///<param name="a">The vector3 to add the scalar to.</param>
            ///<param name="b">The scalar to add to the vector4.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to add a scalar to a vector4 of a different type use method Add.</remarks>
            public static Vector4<T> operator +(Vector4<T> a, T b) {
                try {
                    Lock(a);
                    return new Vector4<T>(a.x + b, a.y + b, a.z + b, a.w + b);
                }
                finally {
                    Unlock(a);
                }
            }

            ///<summary>Subtracts two Vector4 component by component.</summary>
            ///<param name="a">The first Vector4 to subtract.</param>
            ///<param name="b">The second Vector4 to subtract.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This operator is only available for vectors of the same type. If you want to subtract Vector4 of different types use method Subtract.</remarks>
            public static Vector4<T> operator -(Vector4<T> a, Vector4<T> b) {
                try {
                    Lock(a, b);
                    return new Vector4<T>(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
                }
                finally {
                    Unlock(a, b);
                }
            }

            ///<summary>Subtracts a scalar to each component of a Vector4.</summary>
            ///<param name="a">The Vector4 to subtract the scalar to.</param>
            ///<param name="b">The scalar to subtract to the Vector4.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to subtract a scalar to a Vector4 of a different type use method Subtract.</remarks>
            public static Vector4<T> operator -(Vector4<T> a, T b) {
                try {
                    Lock(a);
                    return new Vector4<T>(a.x - b, a.y - b, a.z - b, a.w - b);
                }
                finally {
                    Unlock(a);
                }
            }

            ///<summary>Calculates the dot product of two Vector4.</summary>
            ///<param name="a">The first Vector4 to calculate the dot product.</param>
            ///<param name="b">The second Vector4 to calculate the dot product.</param>
            ///<returns>The result of the dot product.</returns>
            ///<remarks>This operator is only available for vectors of the same type. If you want to calculate the dot product of Vector4 of different types use method Dot.</remarks>
            public static T operator *(Vector4<T> a, Vector4<T> b) {
                try {
                    Lock(a, b);
                    return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
                }
                finally {
                    Unlock(a, b);
                }
            }

            ///<summary>Multiply each component of a Vector4 by a scalar.</summary>
            ///<param name="a">The Vector4 to multiply by the scalar.</param>
            ///<param name="b">The scalar to multiply to the Vector4.</param>
            ///<returns>The result of the multiplication.</returns>
            ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to multiply a scalar to a Vector4 of a different type use method Multiply.</remarks>
            public static Vector4<T> operator *(Vector4<T> a, T b) {
                try {
                    Lock(a);
                    return new Vector4<T>(a.x * b, a.y * b, a.z * b, a.w * b);
                }
                finally {
                    Unlock(a);
                }
            }

            ///<summary>Divides each component of a Vector4 by a scalar.</summary>
            ///<param name="a">The Vector4 to divide by the scalar.</param>
            ///<param name="b">The scalar to divide to the Vector4.</param>
            ///<returns>The result of the division.</returns>
            ///<remarks>This operator is only available for a Vector4 and a scalar of the same type. If you want to divide a Vector4 by a scalar of a different type use method Divide.</remarks>
            public static Vector4<T> operator /(Vector4<T> a, T b) {
                try {
                    Lock(a);
                    return new Vector4<T>(a.x / b, a.y / b, a.z / b, a.w / b);
                }
                finally {
                    Unlock(a);
                }
            }

            ///<summary>Obtain a System.Numerics.Vector4 from a Vector4.</summary>
            ///<param name="v">The Vector4 to convert.</param>
            ///<returns>The System.Numerics.Vector4 obtained from the Vector4.</returns>
            public static implicit operator Vector4(Vector4<T> v) {
                try {
                    Lock(v);
                    return new Vector4(float.CreateChecked(v.x), float.CreateChecked(v.y), float.CreateChecked(v.z), float.CreateChecked(v.w));
                }
                finally {
                    Unlock(v);
                }
            }

            ///<summary>Obtain a Vector4 from a System.Numerics.Vector4.</summary>
            ///<param name="v">The System.Numerics.Vector4 to convert.</param>
            ///<returns>The Vector4 obtained from the System.Numerics.Vector4.</returns>
            public static explicit operator Vector4<T>(Vector4 v) {
                return new Vector4<T>(T.CreateChecked(v.X), T.CreateChecked(v.Y), T.CreateChecked(v.Z), T.CreateChecked(v.W));
            }

        #endregion

        #region Public Methods

            ///<summary>Adds two Vector4 component by component. If vectors have different sizes, the result will have the size of the biggest vector.</summary>
            ///<param name="b">The second Vector4 to add.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This method is available for Vector4 of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector4<T> Add<T2>(Vector4<T2> b) where T2 : INumber<T2> {
                try {
                    Lock(this, b);
                    return new Vector4<T>(x + (dynamic)b.x, y + (dynamic)b.y, z + (dynamic)b.z, w + (dynamic)b.w);
                }
                finally {
                    Unlock(this, b);
                }
            }

            ///<summary>Adds a scalar to each component of a Vector4.</summary>
            ///<param name="b">The scalar to add to the Vector4.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This method is available for a Vector4 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector4<T> Add<T2>(T2 b) where T2 : INumber<T2> {
                try {
                    Lock(this);
                    return new Vector4<T>(x + (dynamic)b, y + (dynamic)b, z + (dynamic)b, w + (dynamic)b);
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Subtracts two Vector4 component by component.</summary>
            ///<param name="b">The Vector4 to subtract.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This method is available for Vector4 of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector4<T> Subtract<T2>(Vector4<T2> b) where T2 : INumber<T2> {
                try {
                    Lock(this, b);
                    return new Vector4<T>(x - (dynamic)b.x, y - (dynamic)b.y, z - (dynamic)b.z, w - (dynamic)b.w);
                }
                finally {
                    Unlock(this, b);
                }
            }

            ///<summary>Subtracts a scalar to each component of a Vector4.</summary>
            ///<param name="b">The scalar to subtract to the Vector4.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This method is available for a Vector4 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector4<T> Subtract<T2>(T2 b) where T2 : INumber<T2> {
                try {
                    Lock(this);
                    return new Vector4<T>(x - (dynamic)b, y - (dynamic)b, z - (dynamic)b, w - (dynamic)b);
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Calculates the dot product of two Vector4.</summary>
            ///<param name="b">The Vector4 to calculate the dot product.</param>
            ///<returns>The result of the dot product.</returns>
            ///<remarks>This method is available for vectors of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public T Dot<T2>(Vector4<T2> b) where T2 : INumber<T2> {
                try {
                    Lock(this, b);
                    return x * (dynamic)b.x + y * (dynamic)b.y + z * (dynamic)b.z + w * (dynamic)b.w;
                }
                finally {
                    Unlock(this, b);
                }
            }

            ///<summary>Multiply each component of a Vector4 by a scalar.</summary>
            ///<param name="b">The scalar to multiply to the Vector4.</param>
            ///<returns>The result of the multiplication.</returns>
            ///<remarks>This method is available for a Vector4 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector4<T> Multiply<T2>(T2 b) where T2 : INumber<T2> {
                try {
                    Lock(this);
                    return new Vector4<T>(x * (dynamic)b, y * (dynamic)b, z * (dynamic)b, w * (dynamic)b);
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Divide each component of a Vector4 by a scalar.</summary>
            ///<param name="b">The scalar to divide to the Vector4.</param>
            ///<returns>The result of the division.</returns>
            ///<remarks>This method is available for a Vector4 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector4<T> Divide<T2>(T2 b) where T2 : INumber<T2> {
                try {
                    Lock(this);
                    return new Vector4<T>(x / (dynamic)b, y / (dynamic)b, z / (dynamic)b, w / (dynamic)b);
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Calculates the magnitude of the Vector4.</summary>
            ///<returns>The magnitude of the Vector4.</returns>
            public float Magnitude() {
                try {
                    Lock(this);
                    return MathF.Sqrt(float.CreateChecked(x * x + y * y + z * z + w * w));
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Normalizes the Vector4.</summary>
            ///<returns>The normalized Vector4.</returns>
            ///<remarks>Returned Vector4 is a double vector.</remarks>
            public Vector4<float> Normalize() {
                try {
                    Lock(this);
                    return new Vector4<float>(float.CreateChecked(x), float.CreateChecked(y), float.CreateChecked(z), float.CreateChecked(w)) / Magnitude();
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Checks if an object is equal to the Vector4.</summary>
            ///<param name="obj">The object to compare.</param>
            ///<returns>True if the object is equal to the Vector4, false otherwise.</returns>
            public override bool Equals(object? obj) {
                if (obj == null) return false;
                if (obj.GetType() != GetType()) return false;

                var vector = (Vector4<T>)obj;

                try {
                    Lock(this, vector);
                    return x == vector.x && y == vector.y;
                }
                finally {
                    Unlock(this, vector);
                }
            }

            ///<summary>Gets the hash code of the Vector4.</summary>
            ///<returns>The hash code of the Vector4.</returns>
            public override int GetHashCode() {
                try {
                    Lock(this);
                    return 4757297 + x.GetHashCode() * 12167 + y.GetHashCode() * 529 + z.GetHashCode() * 23 + w.GetHashCode();
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Gets the string representation of the Vector4 with format (x, y, z, w)</summary>
            ///<returns>The string representation of the Vector4.</returns>
            public override string ToString() {
                try {
                    Lock(this);
                    IFormatProvider provider = CultureInfo.InvariantCulture;
                    return $"({x.ToString(null, provider)},{y.ToString(null, provider)},{z.ToString(null, provider)},{w.ToString(null, provider)})";
                }
                finally {
                    Unlock(this);
                }
            }

        #endregion

        #region Static Methods

            public static Vector4<T> Parse(string str) {
                str = str.Replace("(", "").Replace(")", "").Replace("\n", "").Replace(" ", "");
                string[] values = str.Split(',');
                return new Vector4<T>(
                    (T)Convert.ChangeType(values[0], typeof(T), CultureInfo.InvariantCulture),
                    (T)Convert.ChangeType(values[1], typeof(T), CultureInfo.InvariantCulture),
                    (T)Convert.ChangeType(values[2], typeof(T), CultureInfo.InvariantCulture),
                    (T)Convert.ChangeType(values[3], typeof(T), CultureInfo.InvariantCulture)
                );
            }

        #endregion

        #region Getters

            public T GetX() {
                try {
                    Lock(this);
                    return x;
                }
                finally {
                    Unlock(this);
                }
            }

            public T GetY() {
                try {
                    Lock(this);
                    return y;
                }
                finally {
                    Unlock(this);
                }
            }

            public T GetZ() {
                try {
                    Lock(this);
                    return z;
                }
                finally {
                    Unlock(this);
                }
            }

            public T GetW() {
                try {
                    Lock(this);
                    return w;
                }
                finally {
                    Unlock(this);
                }
            }

            public T[] GetXy() {
                try {
                    Lock(this);
                    return [x, y];
                }
                finally {
                    Unlock(this);
                }
            }

            public T[] GetXyz() {
                try {
                    Lock(this);
                    return [x, y, z];
                }
                finally {
                    Unlock(this);
                }
            }

            public T[] GetXyzw() {
                try {
                    Lock(this);
                    return [x, y, z, w];
                }
                finally {
                    Unlock(this);
                }
            }

        #endregion

        #region Setters

            public void SetX(T xval) {
                Lock(this);

                x = xval;

                Unlock(this);
            }

            public void SetY(T yval) {
                Lock(this);

                y = yval;

                Unlock(this);
            }

            public void SetZ(T zval) {
                Lock(this);

                z = zval;

                Unlock(this);
            }

            public void SetW(T wval) {

                Lock(this);

                w = wval;

                Unlock(this);
            }

        #endregion
    }
}