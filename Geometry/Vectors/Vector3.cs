using System.Globalization;
using System.Numerics;

using static SimpleUtilities.Threading.SimpleLock;

namespace SimpleUtilities.Geometry.Vectors {
    ///<summary>Class that represents a 3D vector of a numeric T type, is preferred to use this class and not Vector if you know that the vector is 3D.</summary>
    ///<remarks>THREAD SAFE</remarks>
    public class Vector3<T> where T : INumber<T>{

        #region Static Variables

            public static readonly Vector3<T> Zero = new();
            public static readonly Vector3<T> One = new(T.One);

        #endregion

        #region Variables

            private T x;
            private T y;
            private T z;

        #endregion

        #region Constructors

            public Vector3() {
                x = T.Zero;
                y = T.Zero;
                z = T.Zero;
            }

            public Vector3(T all) {
                x = all;
                y = all;
                z = all;
            }

            public Vector3(T x, T y, T z) {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public Vector3(T[] values) {
                x = values[0];
                y = values[1];
                z = values[2];
            }

            public Vector3(Vector2<T> vector) {
                var v = vector.GetXy();

                x = v[0];
                y = v[1];

                z = T.Zero;
            }

            public Vector3(Vector2<T> vector, T z) {
                var v = vector.GetXy();

                x = v[0];
                y = v[1];

                this.z = z;
            }

            public Vector3(Vector3<T> vector) {
                Lock(vector);

                    x = vector.x;
                    y = vector.y;
                    z = vector.z;

                Unlock(vector);
            }

            public Vector3(Vector4<T> vector) {
                var array = vector.GetXyz();

                x = array[0];
                y = array[1];
                z = array[2];
            }

        #endregion

        #region Operators

            ///<summary>Adds two vector3 component by component.</summary>
            ///<param name="a">The first vector3 to add.</param>
            ///<param name="b">The second vector3 to add.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This operator is only available for vectors of the same type. If you want to add vector3 of different types use method Add.</remarks>
            public static Vector3<T> operator +(Vector3<T> a, Vector3<T> b) {
                try {
                    Lock(a, b);
                    return new Vector3<T>(a.x + b.x, a.y + b.y, a.z + b.z);
                }
                finally {
                    Unlock(a, b);
                }
            }

            ///<summary>Adds a scalar to each component of a vector3.</summary>
            ///<param name="a">The vector3 to add the scalar to.</param>
            ///<param name="b">The scalar to add to the vector3.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to add a scalar to a vector3 of a different type use method Add.</remarks>
            public static Vector3<T> operator +(Vector3<T> a, T b) {
                try {
                    Lock(a);
                    return new Vector3<T>(a.x + b, a.y + b, a.z + b);
                }
                finally {
                    Unlock(a);
                }
            }

            ///<summary>Subtracts two vector3 component by component.</summary>
            ///<param name="a">The first vector3 to subtract.</param>
            ///<param name="b">The second vector3 to subtract.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This operator is only available for vectors of the same type. If you want to subtract vector3 of different types use method Subtract.</remarks>
            public static Vector3<T> operator -(Vector3<T> a, Vector3<T> b) {
                try {
                    Lock(a, b);
                    return new Vector3<T>(a.x - b.x, a.y - b.y, a.z - b.z);
                }
                finally {
                    Unlock(a, b);
                }
            }

            ///<summary>Subtracts a scalar to each component of a vector3.</summary>
            ///<param name="a">The vector3 to subtract the scalar to.</param>
            ///<param name="b">The scalar to subtract to the vector3.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to subtract a scalar to a vector3 of a different type use method Subtract.</remarks>
            public static Vector3<T> operator -(Vector3<T> a, T b) {
                try {
                    Lock(a);
                    return new Vector3<T>(a.x - b, a.y - b, a.z - b);
                }
                finally {
                    Unlock(a);
                }
            }

            ///<summary>Calculates the dot product of two vector3.</summary>
            ///<param name="a">The first vector3 to calculate the dot product.</param>
            ///<param name="b">The second vector3 to calculate the dot product.</param>
            ///<returns>The result of the dot product.</returns>
            ///<remarks>This operator is only available for vectors of the same type. If you want to calculate the dot product of vector3 of different types use method Dot.</remarks>
            public static T operator *(Vector3<T> a, Vector3<T> b) {
                try {
                    Lock(a, b);
                    return a.x * b.x + a.y * b.y + a.z * b.z;
                }
                finally {
                    Unlock(a, b);
                }
            }

            ///<summary>Multiply each component of a vector3 by a scalar.</summary>
            ///<param name="a">The vector3 to multiply by the scalar.</param>
            ///<param name="b">The scalar to multiply to the vector3.</param>
            ///<returns>The result of the multiplication.</returns>
            ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to multiply a scalar to a vector3 of a different type use method Multiply.</remarks>
            public static Vector3<T> operator *(Vector3<T> a, T b) {
                try {
                    Lock(a);
                    return new Vector3<T>(a.x * b, a.y * b, a.z * b);
                }
                finally {
                    Unlock(a);
                }
            }

            ///<summary>Divides each component of a vector3 by a scalar.</summary>
            ///<param name="a">The vector3 to divide by the scalar.</param>
            ///<param name="b">The scalar to divide to the vector3.</param>
            ///<returns>The result of the division.</returns>
            ///<remarks>This operator is only available for a vector3 and a scalar of the same type. If you want to divide a vector3 by a scalar of a different type use method Divide.</remarks>
            public static Vector3<T> operator /(Vector3<T> a, T b) {
                try {
                    Lock(a);
                    return new Vector3<T>(a.x / b, a.y / b, a.z / b);
                }
                finally {
                    Unlock(a);
                }
            }

            ///<summary>Obtain a System.Numerics.Vector3 from a Vector3.</summary>
            ///<param name="v">The Vector3 to convert.</param>
            ///<returns>The System.Numerics.Vector3 obtained from the Vector3.</returns>
            public static implicit operator Vector3(Vector3<T> v) {
                try {
                    Lock(v);
                    return new Vector3(float.CreateChecked(v.x), float.CreateChecked(v.y), float.CreateChecked(v.z));
                }
                finally {
                    Unlock(v);
                }
            }

            ///<summary>Obtain a Vector3 from a System.Numerics.Vector3.</summary>
            ///<param name="v">The System.Numerics.Vector3 to convert.</param>
            ///<returns>The Vector3 obtained from the System.Numerics.Vector3.</returns>
            public static explicit operator Vector3<T>(Vector3 v) {
                return new Vector3<T>(T.CreateChecked(v.X), T.CreateChecked(v.Y), T.CreateChecked(v.Z));
            }

        #endregion

        #region Public Methods

            ///<summary>Adds two vector3 component by component. If vectors have different sizes, the result will have the size of the biggest vector.</summary>
            ///<param name="b">The second vector3 to add.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This method is available for vector3 of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector3<T> Add<T2>(Vector3<T2> b) where T2 : INumber<T2> {
                try {
                    Lock(this, b);
                    return new Vector3<T>(x + (dynamic)b.x, y + (dynamic)b.y, z + (dynamic)b.z);
                }
                finally {
                    Unlock(this, b);
                }
            }

            ///<summary>Adds a scalar to each component of a vector3.</summary>
            ///<param name="b">The scalar to add to the vector3.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This method is available for a vector3 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector3<T> Add<T2>(T2 b) where T2 : INumber<T2> {
                try {
                    Lock(this);
                    return new Vector3<T>(x + (dynamic)b, y + (dynamic)b, z + (dynamic)b);
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Subtracts two vector3 component by component.</summary>
            ///<param name="b">The vector3 to subtract.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This method is available for vector3 of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector3<T> Subtract<T2>(Vector3<T2> b) where T2 : INumber<T2> {
                try {
                    Lock(this, b);
                    return new Vector3<T>(x - (dynamic)b.x, y - (dynamic)b.y, z - (dynamic)b.z);
                }
                finally {
                    Unlock(this, b);
                }
            }

            ///<summary>Subtracts a scalar to each component of a vector3.</summary>
            ///<param name="b">The scalar to subtract to the vector3.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This method is available for a vector3 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector3<T> Subtract<T2>(T2 b) where T2 : INumber<T2> {
                try {
                    Lock(this);
                    return new Vector3<T>(x - (dynamic)b, y - (dynamic)b, z - (dynamic)b);
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Calculates the dot product of two vector3.</summary>
            ///<param name="b">The vector3 to calculate the dot product.</param>
            ///<returns>The result of the dot product.</returns>
            ///<remarks>This method is available for vectors of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public T Dot<T2>(Vector3<T2> b) where T2 : INumber<T2> {
                try {
                    Lock(this, b);
                    return x * (dynamic)b.x + y * (dynamic)b.y + z * (dynamic)b.z;
                }
                finally {
                    Unlock(this, b);
                }
            }

            ///<summary>Multiply each component of a vector3 by a scalar.</summary>
            ///<param name="b">The scalar to multiply to the vector3.</param>
            ///<returns>The result of the multiplication.</returns>
            ///<remarks>This method is available for a vector3 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector3<T> Multiply<T2>(T2 b) where T2 : INumber<T2> {
                try {
                    Lock(this);
                    return new Vector3<T>(x * (dynamic)b, y * (dynamic)b, z * (dynamic)b);
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Divide each component of a vector3 by a scalar.</summary>
            ///<param name="b">The scalar to divide to the vector3.</param>
            ///<returns>The result of the division.</returns>
            ///<remarks>This method is available for a vector3 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector3<T> Divide<T2>(T2 b) where T2 : INumber<T2> {
                try {
                    Lock(this);
                    return new Vector3<T>(x / (dynamic)b, y / (dynamic)b, z / (dynamic)b);
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Calculates the cross product of two vector3.</summary>
            ///<param name="b">The vector3 to calculate the cross product.</param>
            ///<returns>The result of the cross product.</returns>
            ///<remarks>This method is available for vector3 of different types.</remarks>
            public Vector3<T> Cross(Vector3<T> b) {
                try {
                    Lock(this, b);
                    return new Vector3<T>(y * b.z - z * b.y, z * b.x - x * b.z, x * b.y - y * b.x);
                }
                finally {
                    Unlock(this, b);
                }
            }

            ///<summary>Calculates the magnitude of the vector3.</summary>
            ///<returns>The magnitude of the vector3.</returns>
            public float Magnitude() {
                try {
                    Lock(this);
                    return MathF.Sqrt(float.CreateChecked(x * x + y * y + z * z));
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Normalizes the vector3.</summary>
            ///<returns>The normalized vector3.</returns>
            ///<remarks>Returned vector3 is a double vector.</remarks>
            public Vector3<float> Normalize() {
                try {
                    Lock(this);
                    return new Vector3<float>(float.CreateChecked(x), float.CreateChecked(y), float.CreateChecked(z)) / Magnitude();
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Checks if an object is equal to the vector3.</summary>
            ///<param name="obj">The object to compare.</param>
            ///<returns>True if the object is equal to the vector3, false otherwise.</returns>
            public override bool Equals(object? obj) {
                if (obj == null) return false;
                if (obj.GetType() != GetType()) return false;

                var vector = (Vector3<T>)obj;

                try {
                    Lock(this, vector);
                    return x == vector.x && y == vector.y;
                }
                finally {
                    Unlock(this, vector);
                }
            }

            ///<summary>Gets the hash code of the vector3.</summary>
            ///<returns>The hash code of the vector3.</returns>
            public override int GetHashCode() {
                try {
                    Lock(this);
                    return 206839 + x.GetHashCode() * 529 + y.GetHashCode() * 23 + z.GetHashCode();
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Gets the string representation of the vector3 with format (x, y, z)</summary>
            ///<returns>The string representation of the vector3.</returns>
            public override string ToString(){
                try {
                    Lock(this);
                    IFormatProvider provider = CultureInfo.InvariantCulture;
                    return $"({x.ToString(null, provider)},{y.ToString(null, provider)},{z.ToString(null, provider)})";
                }
                finally {
                    Unlock(this);
                }
            }

        #endregion

        #region Static Methods

            public static Vector3<T> Parse(string str) {
                str = str.Replace("(", "").Replace(")", "").Replace("\n", "").Replace(" ", "");
                var values = str.Split(',');
                return new Vector3<T>(
                    (T)Convert.ChangeType(values[0], typeof(T), CultureInfo.InvariantCulture),
                    (T)Convert.ChangeType(values[1], typeof(T), CultureInfo.InvariantCulture),
                    (T)Convert.ChangeType(values[2], typeof(T), CultureInfo.InvariantCulture)
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

        #endregion
    }
}