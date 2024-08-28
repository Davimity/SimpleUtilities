using SimpleUtilities.Threading;
using System.Numerics;

namespace SimpleUtilities.Geometry.Vectors{
    ///<summary>Class that represents a point in a n-dimensional vector of a numeric type T. 
    ///Only use this class when you do not know the dimension of the vector, 
    ///when the dimension is not fixed 
    ///or when a Vector class with sspecific dimension you need does not exist.
    /// </summary>
    /// <remarks>THREAD SAFE</remarks>
    public class Vector<T> where T : INumber<T>{

        #region Variables

            private T[] components;
            private readonly object lockObject;

        #endregion

        #region Properties

        public int Size
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return components.Length;
                }
            }
        }

        public T[] Components
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return components;
                }
            }
        }

        public T this[int index]
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return components[index];
                }
            }
            set
            {
                using (new SimpleLock(lockObject))
                {
                    components[index] = value;
                }
            }
        }

        public T X
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return components[0];
                }
            }
            set
            {
                using (new SimpleLock(lockObject))
                {
                    components[0] = value;
                }
            }
        }

        public T Y
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return components[1];
                }
            }
            set
            {
                using (new SimpleLock(lockObject))
                {
                    components[1] = value;
                }
            }
        }

        public T Z
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return components[2];
                }
            }
            set
            {
                using (new SimpleLock(lockObject))
                {
                    components[2] = value;
                }
            }
        }

        public T W
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return components[3];
                }
            }
            set
            {
                using (new SimpleLock(lockObject))
                {
                    components[3] = value;
                }
            }
        }

        #endregion

        #region Constructors

        public Vector(params T[] components)
        {
            using (new SimpleLock(components))
            {

                if (components.Length == 0) throw new ArgumentException("The vector must have at least one component.");

                lockObject = new object();

                this.components = new T[components.Length];
                Array.Copy(components, this.components, components.Length);

            }
        }

        public Vector(Vector<T> vector)
        {
            using (new SimpleLock(vector.lockObject))
            {

                lockObject = new object();

                components = new T[vector.components.Length];
                Array.Copy(vector.components, components, vector.components.Length);

            }
        }

        #endregion

        #region Operators

        ///<summary>Adds two vectors component by component. If vectors have different sizes, the result will have the size of the biggest vector.</summary>
        ///<param name="a">The first vector to add.</param>
        ///<param name="b">The second vector to add.</param>
        ///<returns>The result of the addition.</returns>
        ///<remarks>This operator is only available for vectors of the same type. If you want to add vectors of different types use method Add.</remarks>
        public static Vector<T> operator +(Vector<T> a, Vector<T> b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {

                int sA = a.components.Length;
                int sB = b.components.Length;

                int size = Math.Max(sA, sB);

                T[] components = new T[size];

                for (int i = 0; i < size; i++)
                {

                    if (i < sA && i < sB) components[i] = a.components[i] + b.components[i];
                    else if (i < sA) components[i] = a.components[i];
                    else components[i] = b.components[i];

                }

                return new Vector<T>(components);
            }
        }

        ///<summary>Adds a scalar to each component of a vector.</summary>
        ///<param name="a">The vector to add the scalar to.</param>
        ///<param name="b">The scalar to add to the vector.</param>
        ///<returns>The result of the addition.</returns>
        ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to add a scalar to a vector of a different type use method Add.</remarks>
        public static Vector<T> operator +(Vector<T> a, T b)
        {
            using (new SimpleLock(a.lockObject))
            {

                int s = a.components.Length;
                T[] components = new T[s];

                for (int i = 0; i < s; i++)
                    components[i] = a.components[i] + b;

                return new Vector<T>(components);
            }
        }

        ///<summary>Subtracts two vectors component by component. If vectors have different sizes, the result will have the size of the biggest vector.</summary>
        ///<param name="a">The first vector to subtract.</param>
        ///<param name="b">The second vector to subtract.</param>
        ///<returns>The result of the subtraction.</returns>
        ///<remarks>This operator is only available for vectors of the same type. If you want to subtract vectors of different types use method Subtract.</remarks>
        public static Vector<T> operator -(Vector<T> a, Vector<T> b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {

                int sA = a.components.Length;
                int sB = b.components.Length;

                int size = Math.Max(sA, sB);

                T[] components = new T[size];

                for (int i = 0; i < size; i++)
                {

                    if (i < sA && i < sB) components[i] = a.components[i] - b.components[i];
                    else if (i < sA) components[i] = a.components[i];
                    else components[i] = b.components[i];

                }

                return new Vector<T>(components);
            }
        }

        ///<summary>Subtracts a scalar to each component of a vector.</summary>
        ///<param name="a">The vector to subtract the scalar to.</param>
        ///<param name="b">The scalar to subtract to the vector.</param>
        ///<returns>The result of the subtraction.</returns>
        ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to subtract a scalar to a vector of a different type use method Subtract.</remarks>
        public static Vector<T> operator -(Vector<T> a, T b)
        {
            using (new SimpleLock(a.lockObject))
            {

                int s = a.components.Length;
                T[] components = new T[s];

                for (int i = 0; i < s; i++)
                    components[i] = a.components[i] - b;

                return new Vector<T>(components);
            }
        }

        ///<summary>Calculates the dot product of two vectors. The size of the vectors must be the same.</summary>
        ///<param name="a">The first vector to calculate the dot product.</param>
        ///<param name="b">The second vector to calculate the dot product.</param>
        ///<returns>The result of the dot product.</returns>
        ///<remarks>This operator is only available for vectors of the same type. If you want to calculate the dot product of vectors of different types use method Dot.</remarks>
        public static T operator *(Vector<T> a, Vector<T> b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                if (a.components.Length != b.components.Length) throw new ArgumentException("The vectors must have the same size.");

                int s = a.components.Length;

                T result = T.Zero;

                for (int i = 0; i < s; i++)
                    result += (dynamic)a.components[i] * (dynamic)b.components[i];

                return result;
            }
        }

        ///<summary>Multiply each component of a vector by a scalar.</summary>
        ///<param name="a">The vector to multiply by the scalar.</param>
        ///<param name="b">The scalar to multiply to the vector.</param>
        ///<returns>The result of the multiplication.</returns>
        ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to multiply a scalar to a vector of a different type use method Multiply.</remarks>
        public static Vector<T> operator *(Vector<T> a, T b)
        {
            using (new SimpleLock(a.lockObject))
            {
                int s = a.components.Length;

                T[] components = new T[s];

                for (int i = 0; i < s; i++)
                    components[s] = a.components[i] * b;

                return new Vector<T>(components);
            }
        }

        ///<summary>Divides each component of a vector by a scalar.</summary>
        ///<param name="a">The vector to divide by the scalar.</param>
        ///<param name="b">The scalar to divide to the vector.</param>
        ///<returns>The result of the division.</returns>
        ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to divide a vector by a scalar of a different type use method Divide.</remarks>
        public static Vector<T> operator /(Vector<T> a, T b)
        {
            using (new SimpleLock(a.lockObject))
            {
                int s = a.components.Length;

                T[] components = new T[s];

                for (int i = 0; i < s; i++)
                    components[s] = a.components[i] / b;

                return new Vector<T>(components);
            }
        }

        #endregion

        #region Methods

        ///<summary>Adds two vectors component by component. If vectors have different sizes, the result will have the size of the biggest vector.</summary>
        ///<param name="a">The first vector to add.</param>
        ///<param name="b">The second vector to add.</param>
        ///<returns>The result of the addition.</returns>
        ///<remarks>This method is available for vectors of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public Vector<T> Add<T2>(Vector<T2> b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject, b.lockObject))
            {
                int sA = this.components.Length;
                int sB = b.components.Length;

                int size = Math.Max(sA, sB);

                T[] components = new T[size];

                for (int i = 0; i < size; i++)
                {

                    if (i < sA && i < sB) components[i] = this.components[i] + (dynamic)b.components[i];
                    else if (i < sA) components[i] = this.components[i];
                    else components[i] = (dynamic)b.components[i];

                }

                return new Vector<T>(components);
            }
        }

        ///<summary>Adds a scalar to each component of a vector.</summary>
        ///<param name="b">The scalar to add to the vector.</param>
        ///<returns>The result of the addition.</returns>
        ///<remarks>This method is available for a vector and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public Vector<T> Add<T2>(T2 b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject))
            {
                int s = this.components.Length;
                T[] components = new T[s];

                for (int i = 0; i < s; i++)
                    components[i] = this.components[i] + (dynamic)b;

                return new Vector<T>(components);
            }
        }

        ///<summary>Subtracts two vectors component by component. If vectors have different sizes, the result will have the size of the biggest vector.</summary>
        ///<param name="b">The vector to subtract.</param>
        ///<returns>The result of the subtraction.</returns>
        ///<remarks>This method is available for vectors of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public Vector<T> Subtract<T2>(Vector<T2> b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject, b.lockObject))
            {
                int sA = this.components.Length;
                int sB = b.components.Length;

                int size = Math.Max(sA, sB);

                T[] components = new T[size];

                for (int i = 0; i < size; i++)
                {

                    if (i < sA && i < sB) components[i] = this.components[i] - (dynamic)b.components[i];
                    else if (i < sA) components[i] = this.components[i];
                    else components[i] = (dynamic)b.components[i];

                }

                return new Vector<T>(components);
            }
        }

        ///<summary>Subtracts a scalar to each component of a vector.</summary>
        ///<param name="b">The scalar to subtract to the vector.</param>
        ///<returns>The result of the subtraction.</returns>
        ///<remarks>This method is available for a vector and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public Vector<T> Subtract<T2>(T2 b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject))
            {
                int s = this.components.Length;
                T[] components = new T[s];

                for (int i = 0; i < s; i++)
                    components[i] = this.components[i] - (dynamic)b;

                return new Vector<T>(components);
            }
        }

        ///<summary>Calculates the dot product of two vectors. The size of the vectors must be the same.</summary>
        ///<param name="b">The vector to calculate the dot product.</param>
        ///<returns>The result of the dot product.</returns>
        ///<remarks>This method is available for vectors of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public T Dot<T2>(Vector<T2> b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject, b.lockObject))
            {
                if (components.Length != b.components.Length) throw new ArgumentException("The vectors must have the same size.");

                int s = components.Length;

                dynamic result = T.Zero;

                for (int i = 0; i < s; i++)
                {
                    result += components[i] * (dynamic)b.components[i];
                }

                return result;
            }
        }

        ///<summary>Multiply each component of a vector by a scalar.</summary>
        ///<param name="b">The scalar to multiply to the vector.</param>
        ///<returns>The result of the multiplication.</returns>
        ///<remarks>This method is available for a vector and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public Vector<T> Multiply<T2>(Vector<T2> b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject, b.lockObject))
            {
                int sA = this.components.Length;
                int sB = b.components.Length;

                int size = Math.Max(sA, sB);

                T[] components = new T[size];

                for (int i = 0; i < size; i++)
                {

                    if (i < sA && i < sB) components[i] = this.components[i] * (dynamic)b.components[i];
                    else if (i < sA) components[i] = this.components[i];
                    else components[i] = (dynamic)b.components[i];

                }

                return new Vector<T>(components);
            }
        }

        ///<summary>Multiply each component of a vector by a scalar.</summary>
        ///<param name="b">The scalar to multiply to the vector.</param>
        ///<returns>The result of the multiplication.</returns>
        ///<remarks>This method is available for a vector and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
        public Vector<T> Divide<T2>(Vector<T2> b) where T2 : INumber<T2>
        {
            using (new SimpleLock(lockObject, b.lockObject))
            {
                int sA = this.components.Length;
                int sB = b.components.Length;

                int size = Math.Max(sA, sB);

                T[] components = new T[size];

                for (int i = 0; i < size; i++)
                {

                    if (i < sA && i < sB) components[i] = this.components[i] / (dynamic)b.components[i];
                    else if (i < sA) components[i] = this.components[i];
                    else components[i] = (dynamic)b.components[i];

                }

                return new Vector<T>(components);
            }
        }

        ///<summary>Calculates the cross product of two vectors.</summary>
        ///<param name="b">The vector to calculate the cross product.</param>
        ///<returns>The result of the cross product.</returns>
        ///<remarks>This method is available for vectors of different types. The size of the vectors must be 2 or 3.</remarks>
        public Vector<T> Cross(Vector<T> b)
        {
            using (new SimpleLock(lockObject, b.lockObject))
            {

                if (components.Length != b.components.Length) throw new ArgumentException("The vectors must have the same size.");

                if (components.Length == 2)
                    return new Vector<T>(T.Zero, T.Zero, components[0] * b.components[1] - components[1] * b.components[0]);
                else if (components.Length == 3)
                {
                    T x = components[1] * b.components[2] - components[2] * b.components[1];
                    T y = components[2] * b.components[0] - components[0] * b.components[2];
                    T z = components[0] * b.components[1] - components[1] * b.components[0];

                    return new Vector<T>(x, y, z);
                }
                else throw new InvalidOperationException("The cross product is only available for 2D and 3D vectors.");
            }
        }

        ///<summary>Calculates the magnitude of the vector.</summary>
        ///<returns>The magnitude of the vector.</remarks>
        public float Magnitude()
        {
            using (new SimpleLock(lockObject))
            {

                float result = 0;
                foreach (T component in components)
                    result += float.CreateChecked(component * component);

                return MathF.Sqrt(result);
            }
        }

        ///<summary>Normalizes the vector.</summary>
        ///<returns>The normalized vector.</returns>
        ///<remarks>Returned vector is a double vector.</remarks>
        public Vector<float> Normalize()
        {
            using (new SimpleLock(lockObject))
            {
                float magnitude = Magnitude();

                if (magnitude == 0) throw new InvalidOperationException("The magnitude of the vector is 0.");

                float[] components = new float[this.components.Length];

                for (int i = 0; i < components.Length; i++)
                    components[i] = float.CreateChecked(this.components[i]) / magnitude;

                return new Vector<float>(components);
            }
        }

        ///<summary>Checks if a object is equal to the vector.</summary>
        ///<param name="obj">The object to compare.</param>
        ///<returns>True if the object is equal to the vector, false otherwise.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;

            Vector<T> vector = (Vector<T>)obj;

            using (new SimpleLock(lockObject, vector.lockObject))
            {
                if (components.Length != vector.components.Length) return false;

                for (int i = 0; i < components.Length; i++)
                    if (!components[i].Equals(vector.components[i])) return false;

                return true;
            }
        }

        ///<summary>Gets the hash code of the vector.</summary>
        ///<returns>The hash code of the vector.</returns>
        public override int GetHashCode()
        {
            using (new SimpleLock(lockObject))
            {
                int hash = 17;

                foreach (T component in components)
                    hash = hash * 31 + component.GetHashCode();

                return hash;
            }
        }

        ///<summary>Gets the string representation of the vector with format (x, y, z, w...)</summary>
        ///<returns>The string representation of the vector.</returns>
        public override string ToString()
        {
            using (new SimpleLock(lockObject))
            {
                string result = "(";

                for (int i = 0; i < components.Length; i++)
                {
                    result += components[i].ToString();

                    if (i < components.Length - 1) result += ", ";
                }

                result += ")";

                return result;
            }
        }

        #endregion 
    }
}
