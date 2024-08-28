using SimpleUtilities.Threading;
using System.Numerics;

namespace SimpleUtilities.Geometry.Vectors
{
    ///<summary>Class that represents a 3D vector of a numeric T type, is preferred to use this class and not Vector<T> if you know that the vector is 3D.</summary>
    ///<remarks>THREAD SAFE</remarks>
    public class Vector3<T> where T : INumber<T>{

        #region Variables

        private T x;
        private T y;
        private T z;

        private object lockObject;

        #endregion

        #region Properties

        public T X
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return x;
                }
            }
            set
            {
                using (new SimpleLock(lockObject))
                {
                    x = value;
                }
            }
        }
        public T Y
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return y;
                }
            }
            set
            {
                using (new SimpleLock(lockObject))
                {
                    y = value;
                }
            }
        }
        public T Z
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return z;
                }
            }
            set
            {
                using (new SimpleLock(lockObject))
                {
                    z = value;
                }
            }
        }

        public T[] XY
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return [x, y];
                }
            }
        }
        public T[] YZ
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return [y, z];
                }
            }
        }
        public T[] XYZ
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return [x, y, z];
                }
            }
        }

        #endregion

        #region Constructors

        public Vector3()
        {
            x = T.Zero;
            y = T.Zero;
            z = T.Zero;

            lockObject = new object();
        }

        public Vector3(T x, T y, T z)
        {
            this.x = x;
            this.y = y;
            this.z = z;

            lockObject = new object();
        }

        public Vector3(Vector3<T> vector)
        {
            using (new SimpleLock(vector.lockObject))
            {
                x = vector.x;
                y = vector.y;
                z = vector.z;

                lockObject = new object();
            }
        }

        #endregion

        #region Operators

        ///<summary>Adds two vector3 component by component.</summary>
        ///<param name="a">The first vector3 to add.</param>
        ///<param name="b">The second vector3 to add.</param>
        ///<returns>The result of the addition.</returns>
        ///<remarks>This operator is only available for vectors of the same type. If you want to add vector3 of different types use method Add.</remarks>
        public static Vector3<T> operator +(Vector3<T> a, Vector3<T> b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new Vector3<T>(a.x + b.x, a.y + b.y, a.z + b.z);
            }
        }

        ///<summary>Adds a scalar to each component of a vector3.</summary>
        ///<param name="a">The vector3 to add the scalar to.</param>
        ///<param name="b">The scalar to add to the vector3.</param>
        ///<returns>The result of the addition.</returns>
        ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to add a scalar to a vector3 of a different type use method Add.</remarks>
        public static Vector3<T> operator +(Vector3<T> a, T b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new Vector3<T>(a.x + b, a.y + b, a.z + b);
            }
        }

        ///<summary>Subtracts two vector3 component by component.</summary>
        ///<param name="a">The first vector3 to subtract.</param>
        ///<param name="b">The second vector3 to subtract.</param>
        ///<returns>The result of the subtraction.</returns>
        ///<remarks>This operator is only available for vectors of the same type. If you want to subtract vector3 of different types use method Subtract.</remarks>
        public static Vector3<T> operator -(Vector3<T> a, Vector3<T> b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new Vector3<T>(a.x - b.x, a.y - b.y, a.z - b.z);
            }
        }

        ///<summary>Subtracts a scalar to each component of a vector3.</summary>
        ///<param name="a">The vector3 to subtract the scalar to.</param>
        ///<param name="b">The scalar to subtract to the vector3.</param>
        ///<returns>The result of the subtraction.</returns>
        ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to subtract a scalar to a vector3 of a different type use method Subtract.</remarks>
        public static Vector3<T> operator -(Vector3<T> a, T b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new Vector3<T>(a.x - b, a.y - b, a.z - b);
            }
        }

        ///<summary>Calculates the dot product of two vector3.</summary>
        ///<param name="a">The first vector3 to calculate the dot product.</param>
        ///<param name="b">The second vector3 to calculate the dot product.</param>
        ///<returns>The result of the dot product.</returns>
        ///<remarks>This operator is only available for vectors of the same type. If you want to calculate the dot product of vector3 of different types use method Dot.</remarks>
        public static T operator *(Vector3<T> a, Vector3<T> b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return a.x * b.x + a.y * b.y + a.z * b.z;
            }
        }

        ///<summary>Multiply each component of a vector3 by a scalar.</summary>
        ///<param name="a">The vector3 to multiply by the scalar.</param>
        ///<param name="b">The scalar to multiply to the vector3.</param>
        ///<returns>The result of the multiplication.</returns>
        ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to multiply a scalar to a vector3 of a different type use method Multiply.</remarks>
        public static Vector3<T> operator *(Vector3<T> a, T b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new Vector3<T>(a.x * b, a.y * b, a.z * b);
            }
        }

        ///<summary>Divides each component of a vector3 by a scalar.</summary>
        ///<param name="a">The vector3 to divide by the scalar.</param>
        ///<param name="b">The scalar to divide to the vector3.</param>
        ///<returns>The result of the division.</returns>
        ///<remarks>This operator is only available for a vector3 and a scalar of the same type. If you want to divide a vector3 by a scalar of a different type use method Divide.</remarks>
        public static Vector3<T> operator /(Vector3<T> a, T b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new Vector3<T>(a.x / b, a.y / b, a.z / b);
            }
        }

        ///<summary>Obtain a System.Numerics.Vector3 from a Vector3.</summary>
        ///<param name="v">The Vector3 to convert.</param>
        ///<returns>The System.Numerics.Vector3 obtained from the Vector3.</returns>
        public static implicit operator System.Numerics.Vector3(Vector3<T> v)
        {
            using (new SimpleLock(v.lockObject))
            {
                return new Vector3(float.CreateChecked(v.X), float.CreateChecked(v.Y), float.CreateChecked(v.Z));
            }
        }

        ///<summary>Obtain a Vector3 from a System.Numerics.Vector3.</summary>
        ///<param name="v">The System.Numerics.Vector3 to convert.</param>
        ///<returns>The Vector3 obtained from the System.Numerics.Vector3.</returns>
        public static explicit operator Vector3<T>(System.Numerics.Vector3 v)
        {
            return new Vector3<T>(T.CreateChecked(v.X), T.CreateChecked(v.Y), T.CreateChecked(v.Z));
        }

        #endregion

        #region Methods

        ///<summary>Adds two vector3 component by component. If vectors have different sizes, the result will have the size of the biggest vector.</summary>
        ///<param name="a">The first vector3 to add.</param>
        ///<param name="b">The second vector3 to add.</param>
        ///<returns>The result of the addition.</returns>
        ///<remarks>This method is available for vector3 of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public Vector3<T> Add<T2>(Vector3<T2> b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject, b.lockObject))
            {
                return new Vector3<T>(x + (dynamic)b.x, y + (dynamic)b.y, z + (dynamic)b.z);
            }
        }

        ///<summary>Adds a scalar to each component of a vector3.</summary>
        ///<param name="b">The scalar to add to the vector3.</param>
        ///<returns>The result of the addition.</returns>
        ///<remarks>This method is available for a vector3 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public Vector3<T> Add<T2>(T2 b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject))
            {
                return new Vector3<T>(x + (dynamic)b, y + (dynamic)b, z + (dynamic)b);
            }
        }

        ///<summary>Subtracts two vector3 component by component.</summary>
        ///<param name="b">The vector3 to subtract.</param>
        ///<returns>The result of the subtraction.</returns>
        ///<remarks>This method is available for vector3 of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public Vector3<T> Subtract<T2>(Vector3<T2> b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject, b.lockObject))
            {
                return new Vector3<T>(x - (dynamic)b.x, y - (dynamic)b.y, z - (dynamic)b.z);
            }
        }

        ///<summary>Subtracts a scalar to each component of a vector3.</summary>
        ///<param name="b">The scalar to subtract to the vector3.</param>
        ///<returns>The result of the subtraction.</returns>
        ///<remarks>This method is available for a vector3 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public Vector3<T> Subtract<T2>(T2 b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject))
            {
                return new Vector3<T>(x - (dynamic)b, y - (dynamic)b, z - (dynamic)b);
            }
        }

        ///<summary>Calculates the dot product of two vector3.</summary>
        ///<param name="b">The vector3 to calculate the dot product.</param>
        ///<returns>The result of the dot product.</returns>
        ///<remarks>This method is available for vectors of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public T Dot<T2>(Vector3<T2> b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject, b.lockObject))
            {
                return x * (dynamic)b.x + y * (dynamic)b.y + z * (dynamic)b.z;
            }
        }

        ///<summary>Multiply each component of a vector3 by a scalar.</summary>
        ///<param name="b">The scalar to multiply to the vector3.</param>
        ///<returns>The result of the multiplication.</returns>
        ///<remarks>This method is available for a vector3 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public Vector3<T> Multiply<T2>(T2 b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject))
            {
                return new Vector3<T>(x * (dynamic)b, y * (dynamic)b, z * (dynamic)b);
            }
        }

        ///<summary>Divide each component of a vector3 by a scalar.</summary>
        ///<param name="b">The scalar to divide to the vector3.</param>
        ///<returns>The result of the division.</returns>
        ///<remarks>This method is available for a vector3 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public Vector3<T> Divide<T2>(T2 b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject))
            {
                return new Vector3<T>(x / (dynamic)b, y / (dynamic)b, z / (dynamic)b);
            }
        }

        ///<summary>Calculates the cross product of two vector3.</summary>
        ///<param name="b">The vector3 to calculate the cross product.</param>
        ///<returns>The result of the cross product.</returns>
        ///<remarks>This method is available for vector3 of different types.</remarks>
        public Vector3<T> Cross(Vector3<T> b)
        {
            using (new SimpleLock(lockObject, b.lockObject))
            {
                return new Vector3<T>(y * b.z - z * b.y, z * b.x - x * b.z, x * b.y - y * b.x);
            }
        }

        ///<summary>Calculates the magnitude of the vector3.</summary>
        ///<returns>The magnitude of the vector3.</remarks>
        public float Magnitude()
        {
            using (new SimpleLock(lockObject))
            {
                return MathF.Sqrt(float.CreateChecked(x * x + y * y + z * z));
            }
        }

        ///<summary>Normalizes the vector3.</summary>
        ///<returns>The normalized vector3.</returns>
        ///<remarks>Returned vector3 is a double vector.</remarks>
        public Vector3<float> Normalize()
        {
            using (new SimpleLock(lockObject))
            {
                return new Vector3<float>(float.CreateChecked(x), float.CreateChecked(y), float.CreateChecked(z)) / Magnitude();
            }
        }

        ///<summary>Checks if a object is equal to the vector3.</summary>
        ///<param name="obj">The object to compare.</param>
        ///<returns>True if the object is equal to the vector3, false otherwise.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;

            Vector3<T> vector = (Vector3<T>)obj;

            using (new SimpleLock(lockObject, vector.lockObject))
            {

                if (x == vector.x && y == vector.y) return true;

                return false;
            }
        }

        ///<summary>Gets the hash code of the vector3.</summary>
        ///<returns>The hash code of the vector3.</returns>
        public override int GetHashCode()
        {
            using (new SimpleLock(lockObject))
            {
                int hash = 17;

                hash = hash * 23 + x.GetHashCode();
                hash = hash * 23 + y.GetHashCode();
                hash = hash * 23 + z.GetHashCode();

                return hash;
            }
        }

        ///<summary>Gets the string representation of the vector3 with format (x, y, z)</summary>
        ///<returns>The string representation of the vector3.</returns>
        public override string ToString()
        {
            using (new SimpleLock(lockObject))
            {
                return $"({x},{y},{z})";
            }
        }

        #endregion
    }
}
