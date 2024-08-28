using SimpleUtilities.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUtilities.Geometry.Vectors {
    ///<summary>Class that represents a 4D vector of a numeric T type, is preferred to use this class and not Vector<T> if you know that the vector is 4D.</summary>
    ///<remarks>THREAD SAFE</remarks>
    public class Vector4<T> where T : INumber<T> {

        #region Variables

            private T x;
            private T y;
            private T z;
            private T w;

            private object lockObject;

        #endregion

        #region Properties

            public T X {
                get {
                    using (new SimpleLock(lockObject)) {
                        return x;
                    }
                }
                set {
                    using (new SimpleLock(lockObject)) {
                        x = value;
                    }
                }
            }
            public T Y {
                get {
                    using (new SimpleLock(lockObject)) {
                        return y;
                    }
                }
                set {
                    using (new SimpleLock(lockObject)) {
                        y = value;
                    }
                }
            }
            public T Z {
                get {
                    using (new SimpleLock(lockObject)) {
                        return z;
                    }
                }
                set {
                    using (new SimpleLock(lockObject)) {
                        z = value;
                    }
                }
            }
            public T W {
                get {
                    using (new SimpleLock(lockObject)) {
                        return w;
                    }
                }
                set {
                    using (new SimpleLock(lockObject)) {
                        w = value;
                    }
                }
            }

            public Vector2<T> XY {
                get {
                    using (new SimpleLock(lockObject)) {
                        return new Vector2<T>(x, y);
                    }
                }
            }
            public Vector2<T> YZ {
                get {
                    using (new SimpleLock(lockObject)) {
                        return new Vector2<T>(y, z);
                    }
                }
            }
            public Vector2<T> ZW {
                get {
                    using (new SimpleLock(lockObject)) {
                        return new Vector2<T>(z, w);
                    }
                }
            }
            public Vector2<T> XW {
                get {
                    using (new SimpleLock(lockObject)) {
                        return new Vector2<T>(x, w);
                    }
                }
            }
            public Vector2<T> YW {
                get {
                    using (new SimpleLock(lockObject)) {
                        return new Vector2<T>(y, w);
                    }
                }
            }

            public Vector3<T> XYZ {
                get {
                    using (new SimpleLock(lockObject)) {
                        return new Vector3<T>(x, y, z);
                    }
                }
            }
            public Vector3<T> YZW {
                get {
                    using (new SimpleLock(lockObject)) {
                        return new Vector3<T>(y, z, w);
                    }
                }
            }

        #endregion

        #region Constructors

            public Vector4() {
                x = T.Zero;
                y = T.Zero;
                z = T.Zero;
                w = T.Zero;

                lockObject = new object();
            }

            public Vector4(T x, T y, T z, T w) {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;

                lockObject = new object();
            }

            public Vector4(Vector4<T> vector) {
                using (new SimpleLock(vector.lockObject)) {
                    x = vector.x;
                    y = vector.y;
                    z = vector.z;
                    w = vector.w;

                    lockObject = new object();
                }
            }

        #endregion

        #region Operators

            ///<summary>Adds two vector4 component by component.</summary>
            ///<param name="a">The first vector4 to add.</param>
            ///<param name="b">The second vector4 to add.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This operator is only available for vectors of the same type. If you want to add vector4 of different types use method Add.</remarks>
            public static Vector4<T> operator +(Vector4<T> a, Vector4<T> b) {
                using (new SimpleLock(a.lockObject, b.lockObject)) {
                    return new Vector4<T>(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
                }
            }

            ///<summary>Adds a scalar to each component of a vector4.</summary>
            ///<param name="a">The vector3 to add the scalar to.</param>
            ///<param name="b">The scalar to add to the vector4.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to add a scalar to a vector4 of a different type use method Add.</remarks>
            public static Vector4<T> operator +(Vector4<T> a, T b) {
                using (new SimpleLock(a.lockObject)) {
                    return new Vector4<T>(a.x + b, a.y + b, a.z + b, a.w + b);
                }
            }

            ///<summary>Subtracts two Vector4 component by component.</summary>
            ///<param name="a">The first Vector4 to subtract.</param>
            ///<param name="b">The second Vector4 to subtract.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This operator is only available for vectors of the same type. If you want to subtract Vector4 of different types use method Subtract.</remarks>
            public static Vector4<T> operator -(Vector4<T> a, Vector4<T> b) {
                using (new SimpleLock(a.lockObject, b.lockObject)) {
                    return new Vector4<T>(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
                }
            }

            ///<summary>Subtracts a scalar to each component of a Vector4.</summary>
            ///<param name="a">The Vector4 to subtract the scalar to.</param>
            ///<param name="b">The scalar to subtract to the Vector4.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to subtract a scalar to a Vector4 of a different type use method Subtract.</remarks>
            public static Vector4<T> operator -(Vector4<T> a, T b) {
                using (new SimpleLock(a.lockObject)) {
                    return new Vector4<T>(a.x - b, a.y - b, a.z - b, a.w - b);
                }
            }

            ///<summary>Calculates the dot product of two Vector4.</summary>
            ///<param name="a">The first Vector4 to calculate the dot product.</param>
            ///<param name="b">The second Vector4 to calculate the dot product.</param>
            ///<returns>The result of the dot product.</returns>
            ///<remarks>This operator is only available for vectors of the same type. If you want to calculate the dot product of Vector4 of different types use method Dot.</remarks>
            public static T operator *(Vector4<T> a, Vector4<T> b) {
                using (new SimpleLock(a.lockObject, b.lockObject)) {
                    return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
                }
            }

            ///<summary>Multiply each component of a Vector4 by a scalar.</summary>
            ///<param name="a">The Vector4 to multiply by the scalar.</param>
            ///<param name="b">The scalar to multiply to the Vector4.</param>
            ///<returns>The result of the multiplication.</returns>
            ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to multiply a scalar to a Vector4 of a different type use method Multiply.</remarks>
            public static Vector4<T> operator *(Vector4<T> a, T b) {
                using (new SimpleLock(a.lockObject)) {
                    return new Vector4<T>(a.x * b, a.y * b, a.z * b, a.w * b);
                }
            }

            ///<summary>Divides each component of a Vector4 by a scalar.</summary>
            ///<param name="a">The Vector4 to divide by the scalar.</param>
            ///<param name="b">The scalar to divide to the Vector4.</param>
            ///<returns>The result of the division.</returns>
            ///<remarks>This operator is only available for a Vector4 and a scalar of the same type. If you want to divide a Vector4 by a scalar of a different type use method Divide.</remarks>
            public static Vector4<T> operator /(Vector4<T> a, T b) {
                using (new SimpleLock(a.lockObject)) {
                    return new Vector4<T>(a.x / b, a.y / b, a.z / b, a.w / b);
                }
            }

            ///<summary>Obtain a System.Numerics.Vector4 from a Vector4.</summary>
            ///<param name="v">The Vector4 to convert.</param>
            ///<returns>The System.Numerics.Vector4 obtained from the Vector4.</returns>
            public static implicit operator System.Numerics.Vector4(Vector4<T> v) {
                using (new SimpleLock(v.lockObject)) {
                    return new Vector4(float.CreateChecked(v.x), float.CreateChecked(v.y), float.CreateChecked(v.z), float.CreateChecked(v.w));
                }
            }

            ///<summary>Obtain a Vector4 from a System.Numerics.Vector4.</summary>
            ///<param name="v">The System.Numerics.Vector4 to convert.</param>
            ///<returns>The Vector4 obtained from the System.Numerics.Vector4.</returns>
            public static explicit operator Vector4<T>(System.Numerics.Vector4 v) {
                return new Vector4<T>(T.CreateChecked(v.X), T.CreateChecked(v.Y), T.CreateChecked(v.Z), T.CreateChecked(v.W));
            }

        #endregion

        #region Methods

            ///<summary>Adds two Vector4 component by component. If vectors have different sizes, the result will have the size of the biggest vector.</summary>
            ///<param name="a">The first Vector4 to add.</param>
            ///<param name="b">The second Vector4 to add.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This method is available for Vector4 of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector4<T> Add<T2>(Vector4<T2> b) where T2 : INumber<T2> {
                using (new SimpleLock(lockObject, b.lockObject)) {
                    return new Vector4<T>(x + (dynamic)b.x, y + (dynamic)b.y, z + (dynamic)b.z, w + (dynamic)b.w);
                }
            }

            ///<summary>Adds a scalar to each component of a Vector4.</summary>
            ///<param name="b">The scalar to add to the Vector4.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This method is available for a Vector4 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector4<T> Add<T2>(T2 b) where T2 : INumber<T2> {
                using (new SimpleLock(lockObject)) {
                    return new Vector4<T>(x + (dynamic)b, y + (dynamic)b, z + (dynamic)b, w + (dynamic)b);
                }
            }

            ///<summary>Subtracts two Vector4 component by component.</summary>
            ///<param name="b">The Vector4 to subtract.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This method is available for Vector4 of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector4<T> Subtract<T2>(Vector4<T2> b) where T2 : INumber<T2> {
                using (new SimpleLock(lockObject, b.lockObject)) {
                    return new Vector4<T>(x - (dynamic)b.x, y - (dynamic)b.y, z - (dynamic)b.z, w - (dynamic)b.w);
                }
            }

            ///<summary>Subtracts a scalar to each component of a Vector4.</summary>
            ///<param name="b">The scalar to subtract to the Vector4.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This method is available for a Vector4 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector4<T> Subtract<T2>(T2 b) where T2 : INumber<T2> {
                using (new SimpleLock(lockObject)) {
                    return new Vector4<T>(x - (dynamic)b, y - (dynamic)b, z - (dynamic)b, w - (dynamic)b);
                }
            }

            ///<summary>Calculates the dot product of two Vector4.</summary>
            ///<param name="b">The Vector4 to calculate the dot product.</param>
            ///<returns>The result of the dot product.</returns>
            ///<remarks>This method is available for vectors of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public T Dot<T2>(Vector4<T2> b) where T2 : INumber<T2> {
                using (new SimpleLock(lockObject, b.lockObject)) {
                    return x * (dynamic)b.x + y * (dynamic)b.y + z * (dynamic)b.z + w * (dynamic)b.w;
                }
            }

            ///<summary>Multiply each component of a Vector4 by a scalar.</summary>
            ///<param name="b">The scalar to multiply to the Vector4.</param>
            ///<returns>The result of the multiplication.</returns>
            ///<remarks>This method is available for a Vector4 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector4<T> Multiply<T2>(T2 b) where T2 : INumber<T2> {
                using (new SimpleLock(lockObject)) {
                    return new Vector4<T>(x * (dynamic)b, y * (dynamic)b, z * (dynamic)b, w * (dynamic)b);
                }
            }

            ///<summary>Divide each component of a Vector4 by a scalar.</summary>
            ///<param name="b">The scalar to divide to the Vector4.</param>
            ///<returns>The result of the division.</returns>
            ///<remarks>This method is available for a Vector4 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector4<T> Divide<T2>(T2 b) where T2 : INumber<T2> {
                using (new SimpleLock(lockObject)) {
                    return new Vector4<T>(x / (dynamic)b, y / (dynamic)b, z / (dynamic)b, w / (dynamic)b);
                }
            }

            ///<summary>Calculates the magnitude of the Vector4.</summary>
            ///<returns>The magnitude of the Vector4.</remarks>
            public float Magnitude() {
                using (new SimpleLock(lockObject)) {
                    return MathF.Sqrt(float.CreateChecked(x * x + y * y + z * z + w * w));
                }
            }

            ///<summary>Normalizes the Vector4.</summary>
            ///<returns>The normalized Vector4.</returns>
            ///<remarks>Returned Vector4 is a double vector.</remarks>
            public Vector4<float> Normalize() {
                using (new SimpleLock(lockObject)) {
                    return new Vector4<float>(float.CreateChecked(x), float.CreateChecked(y), float.CreateChecked(z), float.CreateChecked(w)) / Magnitude();
                }
            }

            ///<summary>Checks if a object is equal to the Vector4.</summary>
            ///<param name="obj">The object to compare.</param>
            ///<returns>True if the object is equal to the Vector4, false otherwise.</returns>
            public override bool Equals(object? obj) {
                if (obj == null) return false;
                if (obj.GetType() != GetType()) return false;

                Vector4<T> vector = (Vector4<T>)obj;

                using (new SimpleLock(lockObject, vector.lockObject)) {

                    if (x == vector.x && y == vector.y) return true;

                    return false;
                }
            }

            ///<summary>Gets the hash code of the Vector4.</summary>
            ///<returns>The hash code of the Vector4.</returns>
            public override int GetHashCode() {
                using (new SimpleLock(lockObject)) {
                    int hash = 17;

                    hash = hash * 23 + x.GetHashCode();
                    hash = hash * 23 + y.GetHashCode();
                    hash = hash * 23 + z.GetHashCode();

                    return hash;
                }
            }

            ///<summary>Gets the string representation of the Vector4 with format (x, y, z, w)</summary>
            ///<returns>The string representation of the Vector4.</returns>
            public override string ToString() {
                using (new SimpleLock(lockObject)) {
                    return $"({x},{y},{z},{w})";
                }
            }

        #endregion
    }
}
