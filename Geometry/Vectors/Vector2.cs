using System.Globalization;
using System.Numerics;

using static SimpleUtilities.Threading.SimpleLock;

namespace SimpleUtilities.Geometry.Vectors {
    ///<summary>Class that represents a 2D vector of a numeric T type, is preferred to use this class and not Vector if you know that the vector is 2D.</summary>
    ///<remarks>THREAD SAFE</remarks>
    public class Vector2<T> where T : INumber<T> {

        #region Static Variables
            
            public static readonly Vector2<T> Zero = new();
            public static readonly Vector2<T> One = new(T.One);

        #endregion

        #region Variables

            private T x;
            private T y;

        #endregion

        #region Constructors

            public Vector2() {
                x = T.Zero;
                y = T.Zero;
            }

            public Vector2(T all) {
                x = all;
                y = all;
            }

            public Vector2(T x, T y) {
                this.x = x;
                this.y = y;
            }

            public Vector2(T[] array) {
                switch (array.Length) {
                    case >= 2:
                        x = array[0];
                        y = array[1];
                        break;
                    case 1:
                        x = array[0];
                        y = T.Zero;
                        break;
                    default:
                        x = T.Zero;
                        y = T.Zero;
                        break;
                }
            }

            public Vector2(Vector2<T> vector) {
                Lock(vector);
                    x = vector.x;
                    y = vector.y;
                Unlock(vector);
            }

            public Vector2(Vector3<T> vector) {
                var array = vector.GetXy();

                x = array[0];
                y = array[1];
            }

            public Vector2(Vector4<T> vector) {
                var array = vector.GetXy();

                x = array[0];
                y = array[1];
            }

            public Vector2(Vector<T> vector) {

                var components = vector.GetComponents();

                switch (components.Length) {
                    case >= 2:
                        x = components[0];
                        y = components[1];
                        break;
                    case 1:
                        x = components[0];
                        y = T.Zero;
                        break;
                    default:
                        x = T.Zero;
                        y = T.Zero;
                        break;
                }
            }

        #endregion

        #region Operators

            ///<summary>Adds two vector2 component by component.</summary>
            ///<param name="a">The first vector2 to add.</param>
            ///<param name="b">The second vector2 to add.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This operator is only available for vectors of the same type. If you want to add vector2 of different types use method Add.</remarks>
            public static Vector2<T> operator +(Vector2<T> a, Vector2<T> b) {
                try {
                    Lock(a, b);
                    return new Vector2<T>(a.x + b.x, a.y + b.y);
                }finally {
                    Unlock(a, b);
                }
            }

            ///<summary>Adds a scalar to each component of a vector2.</summary>
            ///<param name="a">The vector2 to add the scalar to.</param>
            ///<param name="b">The scalar to add to the vector2.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to add a scalar to a vector2 of a different type use method Add.</remarks>
            public static Vector2<T> operator +(Vector2<T> a, T b) {
                try {
                    Lock(a);
                    return new Vector2<T>(a.x + b, a.y + b);
                }
                finally {
                    Unlock(a);
                }
            }

            ///<summary>Subtracts two vector2 component by component.</summary>
            ///<param name="a">The first vector2 to subtract.</param>
            ///<param name="b">The second vector2 to subtract.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This operator is only available for vectors of the same type. If you want to subtract vector2 of different types use method Subtract.</remarks>
            public static Vector2<T> operator -(Vector2<T> a, Vector2<T> b) {
                try {
                    Lock(a, b);
                    return new Vector2<T>(a.x - b.x, a.y - b.y);
                }
                finally {
                    Unlock(a, b);
                }
            }

            ///<summary>Subtracts a scalar to each component of a vector2.</summary>
            ///<param name="a">The vector2 to subtract the scalar to.</param>
            ///<param name="b">The scalar to subtract to the vector2.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to subtract a scalar to a vector2 of a different type use method Subtract.</remarks>
            public static Vector2<T> operator -(Vector2<T> a, T b) {
                try {
                    Lock(a);
                    return new Vector2<T>(a.x - b, a.y - b);
                }
                finally {
                    Unlock(a);
                }
            }

            ///<summary>Calculates the dot product of two vector2.</summary>
            ///<param name="a">The first vector2 to calculate the dot product.</param>
            ///<param name="b">The second vector2 to calculate the dot product.</param>
            ///<returns>The result of the dot product.</returns>
            ///<remarks>This operator is only available for vectors of the same type. If you want to calculate the dot product of vector2 of different types use method Dot.</remarks>
            public static T operator *(Vector2<T> a, Vector2<T> b) {
                try {
                    Lock(a, b);
                    return a.x * b.x + a.y * b.y;
                }
                finally {
                    Unlock(a, b);
                }
            }

            ///<summary>Multiply each component of a vector2 by a scalar.</summary>
            ///<param name="a">The vector2 to multiply by the scalar.</param>
            ///<param name="b">The scalar to multiply to the vector2.</param>
            ///<returns>The result of the multiplication.</returns>
            ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to multiply a scalar to a vector2 of a different type use method Multiply.</remarks>
            public static Vector2<T> operator *(Vector2<T> a, T b) {
                try {
                    Lock(a);
                    return new Vector2<T>(a.x * b, a.y * b);
                }
                finally {
                    Unlock(a);
                }
            }

            ///<summary>Divides each component of a vector2 by a scalar.</summary>
            ///<param name="a">The vector2 to divide by the scalar.</param>
            ///<param name="b">The scalar to divide to the vector2.</param>
            ///<returns>The result of the division.</returns>
            ///<remarks>This operator is only available for a vector2 and a scalar of the same type. If you want to divide a vector2 by a scalar of a different type use method Divide.</remarks>
            public static Vector2<T> operator /(Vector2<T> a, T b) {
                try {
                    Lock(a);
                    return new Vector2<T>(a.x / b, a.y / b);
                }
                finally {
                    Unlock(a);
                }
            }

            ///<summary>Obtain a System.Numerics.Vector2 from a Vector2.</summary>
            ///<param name="v">The Vector2 to convert.</param>
            ///<returns>The System.Numerics.Vector2 obtained from the Vector2.</returns>
            public static implicit operator Vector2(Vector2<T> v) {
                try {
                    Lock(v);
                    return new Vector2(float.CreateChecked(v.x), float.CreateChecked(v.y));
                }
                finally {
                    Unlock(v);
                }
            }

            ///<summary>Obtain a Vector2 from a System.Numerics.Vector2.</summary>
            ///<param name="v">The System.Numerics.Vector2 to convert.</param>
            ///<returns>The Vector2 obtained from the System.Numerics.Vector2.</returns>
            public static explicit operator Vector2<T>(Vector2 v) {
                return new Vector2<T>(T.CreateChecked(v.X), T.CreateChecked(v.Y));
            }

        #endregion

        #region Public Methods

            ///<summary>Adds two vector2 component by component. If vectors have different sizes, the result will have the size of the biggest vector.</summary>
            ///<param name="b">The second vector2 to add.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This method is available for vector2 of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector2<T> Add<T2>(Vector2<T2> b) where T2 : INumber<T2> {
                try {
                    Lock(this, b);
                    return new Vector2<T>(x + (dynamic)b.x, y + (dynamic)b.y);
                }
                finally {
                    Unlock(this, b);
                }
            }

            ///<summary>Adds a scalar to each component of a vector2.</summary>
            ///<param name="b">The scalar to add to the vector2.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This method is available for a vector2 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector2<T> Add<T2>(T2 b) where T2 : INumber<T2> {
                try {
                    Lock(this);
                    return new Vector2<T>(x + (dynamic)b, y + (dynamic)b);
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Subtracts two vector2 component by component.</summary>
            ///<param name="b">The vector2 to subtract.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This method is available for vector2 of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector2<T> Subtract<T2>(Vector2<T2> b) where T2 : INumber<T2> {
                try {
                    Lock(this, b);
                    return new Vector2<T>(x - (dynamic)b.x, y - (dynamic)b.y);
                }
                finally {
                    Unlock(this, b);
                }
            }

            ///<summary>Subtracts a scalar to each component of a vector2.</summary>
            ///<param name="b">The scalar to subtract to the vector2.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This method is available for a vector2 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector2<T> Subtract<T2>(T2 b) where T2 : INumber<T2> {
                try {
                    Lock(this);
                    return new Vector2<T>(x - (dynamic)b, y - (dynamic)b);
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Calculates the dot product of two vector2.</summary>
            ///<param name="b">The vector2 to calculate the dot product.</param>
            ///<returns>The result of the dot product.</returns>
            ///<remarks>This method is available for vectors of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public T Dot<T2>(Vector2<T2> b) where T2 : INumber<T2> {
                try {
                    Lock(this, b);
                    return x * (dynamic)b.x + y * (dynamic)b.y;
                }
                finally {
                    Unlock(this, b);
                }
            }

            ///<summary>Multiply each component of a vector2 by a scalar.</summary>
            ///<param name="b">The scalar to multiply to the vector2.</param>
            ///<returns>The result of the multiplication.</returns>
            ///<remarks>This method is available for a vector2 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector2<T> Multiply<T2>(T2 b) where T2 : INumber<T2> {
                try {
                    Lock(this);
                    return new Vector2<T>(x * (dynamic)b, y * (dynamic)b);
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Divide each component of a vector2 by a scalar.</summary>
            ///<param name="b">The scalar to divide to the vector2.</param>
            ///<returns>The result of the division.</returns>
            ///<remarks>This method is available for a vector2 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector2<T> Divide<T2>(T2 b) where T2 : INumber<T2> {
                try {
                    Lock(this);
                    return new Vector2<T>(x / (dynamic)b, y / (dynamic)b);
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Calculates the cross product of two vector2.</summary>
            ///<param name="b">The vector2 to calculate the cross product.</param>
            ///<returns>The result of the cross product.</returns>
            ///<remarks>This method is available for vector2 of different types.</remarks>
            public Vector3<T> Cross(Vector2<T> b) {
                try {
                    Lock(this, b);
                    return new Vector3<T>(T.Zero, T.Zero, x * b.y - y * b.x);
                }
                finally {
                    Unlock(this, b);
                }
            }

            ///<summary>Calculates the magnitude of the vector2.</summary>
            ///<returns>The magnitude of the vector2.</returns>
            public float Magnitude() {
                try {
                    Lock(this);
                    return MathF.Sqrt(float.CreateChecked(x * x + y * y));
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Normalizes the vector2.</summary>
            ///<returns>The normalized vector2.</returns>
            ///<remarks>Returned vector2 is a double vector.</remarks>
            public Vector2<float> Normalize() {
                try {
                    Lock(this);
                    return new Vector2<float>(float.CreateChecked(x), float.CreateChecked(y)) / Magnitude();
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Checks if an object is equal to the vector2.</summary>
            ///<param name="obj">The object to compare.</param>
            ///<returns>True if the object is equal to the vector2, false otherwise.</returns>
            public override bool Equals(object? obj) {
                if (obj == null) return false;
                if (obj.GetType() != GetType()) return false;

                var vector = (Vector2<T>)obj;

                try {
                    Lock(this, vector);
                    return x == vector.x && y == vector.y;
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Gets the hash code of the vector2.</summary>
            ///<returns>The hash code of the vector2.</returns>
            public override int GetHashCode() {
                try {
                    Lock(this);
                    return 8993 + x.GetHashCode() * 23 + y.GetHashCode();
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Gets the string representation of the vector2 with format (x, y)</summary>
            ///<returns>The string representation of the vector2.</returns>
            public override string ToString() {
                try {
                    Lock(this);
                    IFormatProvider provider = CultureInfo.InvariantCulture;
                    return $"({x.ToString(null, provider)},{y.ToString(null, provider)})";
                }
                finally {
                    Unlock(this);
                }
            }

        #endregion

        #region Static Methods

            public static Vector2<T> Parse(string str) {
                str = str.Replace("(", "").Replace(")", "").Replace("\n", "").Replace(" ", "");
                var values = str.Split(',');
                return new Vector2<T>(
                    (T)Convert.ChangeType(values[0], typeof(T), CultureInfo.InvariantCulture),
                    (T)Convert.ChangeType(values[1], typeof(T), CultureInfo.InvariantCulture)
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

            public T[] GetXy() {
                try {
                    Lock(this);
                    return [x, y];
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

        #endregion
    }
}