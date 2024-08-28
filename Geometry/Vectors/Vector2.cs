using SimpleUtilities.Threading;
using System.Numerics;

namespace SimpleUtilities.Geometry.Vectors{
    ///<summary>Class that represents a 2D vector of a numeric T type, is preferred to use this class and not Vector<T> if you know that the vector is 2D.</summary>
    ///<remarks>THREAD SAFE</remarks>
    public class Vector2<T> where T : INumber<T>{

        #region Variables

        private T x;
        private T y;

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

        #endregion

        #region Constructors

        public Vector2()
        {
            x = T.Zero;
            y = T.Zero;

            lockObject = new object();
        }

        public Vector2(T x, T y)
        {
            this.x = x;
            this.y = y;

            lockObject = new object();
        }

        public Vector2(Vector2<T> vector)
        {
            using (new SimpleLock(vector.lockObject))
            {
                x = vector.x;
                y = vector.y;

                lockObject = new object();
            }
        }

        public Vector2(Vector<T> vector)
        {

            T[] components = vector.Components;

            if (components.Length >= 2)
            {
                x = components[0];
                y = components[1];
            }
            else if (components.Length == 1)
            {
                x = components[0];
                y = T.Zero;
            }
            else
            {
                x = T.Zero;
                y = T.Zero;
            }

            lockObject = new object();
        }

        #endregion

        #region Operators

        ///<summary>Adds two vector2 component by component.</summary>
        ///<param name="a">The first vector2 to add.</param>
        ///<param name="b">The second vector2 to add.</param>
        ///<returns>The result of the addition.</returns>
        ///<remarks>This operator is only available for vectors of the same type. If you want to add vector2 of different types use method Add.</remarks>
        public static Vector2<T> operator +(Vector2<T> a, Vector2<T> b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new Vector2<T>(a.x + b.x, a.y + b.y);
            }
        }

        ///<summary>Adds a scalar to each component of a vector2.</summary>
        ///<param name="a">The vector2 to add the scalar to.</param>
        ///<param name="b">The scalar to add to the vector2.</param>
        ///<returns>The result of the addition.</returns>
        ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to add a scalar to a vector2 of a different type use method Add.</remarks>
        public static Vector2<T> operator +(Vector2<T> a, T b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new Vector2<T>(a.x + b, a.y + b);
            }
        }

        ///<summary>Subtracts two vector2 component by component.</summary>
        ///<param name="a">The first vector2 to subtract.</param>
        ///<param name="b">The second vector2 to subtract.</param>
        ///<returns>The result of the subtraction.</returns>
        ///<remarks>This operator is only available for vectors of the same type. If you want to subtract vector2 of different types use method Subtract.</remarks>
        public static Vector2<T> operator -(Vector2<T> a, Vector2<T> b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new Vector2<T>(a.x - b.x, a.y - b.y);
            }
        }

        ///<summary>Subtracts a scalar to each component of a vector2.</summary>
        ///<param name="a">The vector2 to subtract the scalar to.</param>
        ///<param name="b">The scalar to subtract to the vector2.</param>
        ///<returns>The result of the subtraction.</returns>
        ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to subtract a scalar to a vector2 of a different type use method Subtract.</remarks>
        public static Vector2<T> operator -(Vector2<T> a, T b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new Vector2<T>(a.x - b, a.y - b);
            }
        }

        ///<summary>Calculates the dot product of two vector2.</summary>
        ///<param name="a">The first vector2 to calculate the dot product.</param>
        ///<param name="b">The second vector2 to calculate the dot product.</param>
        ///<returns>The result of the dot product.</returns>
        ///<remarks>This operator is only available for vectors of the same type. If you want to calculate the dot product of vector2 of different types use method Dot.</remarks>
        public static T operator *(Vector2<T> a, Vector2<T> b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return a.x * b.x + a.y * b.y;
            }
        }

        ///<summary>Multiply each component of a vector2 by a scalar.</summary>
        ///<param name="a">The vector2 to multiply by the scalar.</param>
        ///<param name="b">The scalar to multiply to the vector2.</param>
        ///<returns>The result of the multiplication.</returns>
        ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to multiply a scalar to a vector2 of a different type use method Multiply.</remarks>
        public static Vector2<T> operator *(Vector2<T> a, T b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new Vector2<T>(a.x * b, a.y * b);
            }
        }

        ///<summary>Divides each component of a vector2 by a scalar.</summary>
        ///<param name="a">The vector2 to divide by the scalar.</param>
        ///<param name="b">The scalar to divide to the vector2.</param>
        ///<returns>The result of the division.</returns>
        ///<remarks>This operator is only available for a vector2 and a scalar of the same type. If you want to divide a vector2 by a scalar of a different type use method Divide.</remarks>
        public static Vector2<T> operator /(Vector2<T> a, T b)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new Vector2<T>(a.x / b, a.y / b);
            }
        }

        ///<summary>Obtain a System.Numerics.Vector2 from a Vector2.</summary>
        ///<param name="v">The Vector2 to convert.</param>
        ///<returns>The System.Numerics.Vector2 obtained from the Vector2.</returns>
        public static implicit operator System.Numerics.Vector2(Vector2<T> v)
        {
            using (new SimpleLock(v.lockObject))
            {
                return new System.Numerics.Vector2(float.CreateChecked(v.X), float.CreateChecked(v.Y));
            }
        }

        ///<summary>Obtain a Vector2 from a System.Numerics.Vector2.</summary>
        ///<param name="v">The System.Numerics.Vector2 to convert.</param>
        ///<returns>The Vector2 obtained from the System.Numerics.Vector2.</returns>
        public static explicit operator Vector2<T>(Vector2 v)
        {
            return new Vector2<T>(T.CreateChecked(v.X), T.CreateChecked(v.Y));
        }

        #endregion

        #region Methods

        ///<summary>Adds two vector2 component by component. If vectors have different sizes, the result will have the size of the biggest vector.</summary>
        ///<param name="a">The first vector2 to add.</param>
        ///<param name="b">The second vector2 to add.</param>
        ///<returns>The result of the addition.</returns>
        ///<remarks>This method is available for vector2 of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public Vector2<T> Add<T2>(Vector2<T2> b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject, b.lockObject))
            {
                return new Vector2<T>(x + (dynamic)b.x, y + (dynamic)b.y);
            }
        }

        ///<summary>Adds a scalar to each component of a vector2.</summary>
        ///<param name="b">The scalar to add to the vector2.</param>
        ///<returns>The result of the addition.</returns>
        ///<remarks>This method is available for a vector2 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public Vector2<T> Add<T2>(T2 b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject))
            {
                return new Vector2<T>(x + (dynamic)b, y + (dynamic)b);
            }
        }

        ///<summary>Subtracts two vector2 component by component.</summary>
        ///<param name="b">The vector2 to subtract.</param>
        ///<returns>The result of the subtraction.</returns>
        ///<remarks>This method is available for vector2 of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public Vector2<T> Subtract<T2>(Vector2<T2> b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject, b.lockObject))
            {
                return new Vector2<T>(x - (dynamic)b.x, y - (dynamic)b.y);
            }
        }

        ///<summary>Subtracts a scalar to each component of a vector2.</summary>
        ///<param name="b">The scalar to subtract to the vector2.</param>
        ///<returns>The result of the subtraction.</returns>
        ///<remarks>This method is available for a vector2 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public Vector2<T> Subtract<T2>(T2 b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject))
            {
                return new Vector2<T>(x - (dynamic)b, y - (dynamic)b);
            }
        }

        ///<summary>Calculates the dot product of two vector2.</summary>
        ///<param name="b">The vector2 to calculate the dot product.</param>
        ///<returns>The result of the dot product.</returns>
        ///<remarks>This method is available for vectors of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public T Dot<T2>(Vector2<T2> b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject, b.lockObject))
            {
                return x * (dynamic)b.x + y * (dynamic)b.y;
            }
        }

        ///<summary>Multiply each component of a vector2 by a scalar.</summary>
        ///<param name="b">The scalar to multiply to the vector2.</param>
        ///<returns>The result of the multiplication.</returns>
        ///<remarks>This method is available for a vector2 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public Vector2<T> Multiply<T2>(T2 b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject))
            {
                return new Vector2<T>(x * (dynamic)b, y * (dynamic)b);
            }
        }

        ///<summary>Divide each component of a vector2 by a scalar.</summary>
        ///<param name="b">The scalar to divide to the vector2.</param>
        ///<returns>The result of the division.</returns>
        ///<remarks>This method is available for a vector2 and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public Vector2<T> Divide<T2>(T2 b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject))
            {
                return new Vector2<T>(x / (dynamic)b, y / (dynamic)b);
            }
        }

        ///<summary>Calculates the cross product of two vector2.</summary>
        ///<param name="b">The vector2 to calculate the cross product.</param>
        ///<returns>The result of the cross product.</returns>
        ///<remarks>This method is available for vector2 of different types.</remarks>
        public Vector3<T> Cross(Vector2<T> b)
        {
            using (new SimpleLock(lockObject, b.lockObject))
            {
                return new Vector3<T>(T.Zero, T.Zero, x * b.y - y * b.x);
            }
        }

        ///<summary>Calculates the magnitude of the vector2.</summary>
        ///<returns>The magnitude of the vector2.</remarks>
        public float Magnitude()
        {
            using (new SimpleLock(lockObject))
            {
                return MathF.Sqrt(float.CreateChecked(x * x + y * y));
            }
        }

        ///<summary>Normalizes the vector2.</summary>
        ///<returns>The normalized vector2.</returns>
        ///<remarks>Returned vector2 is a double vector.</remarks>
        public Vector2<float> Normalize()
        {
            using (new SimpleLock(lockObject))
            {
                return new Vector2<float>(float.CreateChecked(x), float.CreateChecked(y)) / Magnitude();
            }
        }

        ///<summary>Checks if a object is equal to the vector2.</summary>
        ///<param name="obj">The object to compare.</param>
        ///<returns>True if the object is equal to the vector2, false otherwise.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;

            Vector2<T> vector = (Vector2<T>)obj;

            using (new SimpleLock(lockObject, vector.lockObject))
            {

                if (x == vector.x && y == vector.y) return true;

                return false;
            }
        }

        ///<summary>Gets the hash code of the vector2.</summary>
        ///<returns>The hash code of the vector2.</returns>
        public override int GetHashCode()
        {
            using (new SimpleLock(lockObject))
            {
                int hash = 17;

                hash = hash * 23 + x.GetHashCode();
                hash = hash * 23 + y.GetHashCode();

                return hash;
            }
        }

        ///<summary>Gets the string representation of the vector2 with format (x, y)</summary>
        ///<returns>The string representation of the vector2.</returns>
        public override string ToString()
        {
            using (new SimpleLock(lockObject))
            {
                return $"({x},{y})";
            }
        }

        #endregion

    }
}
