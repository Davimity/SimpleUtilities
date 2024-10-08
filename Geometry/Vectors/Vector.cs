using System.Numerics;

using static SimpleUtilities.Threading.SimpleLock;

namespace SimpleUtilities.Geometry.Vectors {
    ///<summary>
    /// Class that represents a point in an n-dimensional vector of a numeric type T. 
    /// Only use this class when you do not know the dimension of the vector, 
    /// when the dimension is not fixed 
    /// or when a Vector class with sspecific dimension you need does not exist.
    /// </summary>
    /// <remarks>THREAD SAFE</remarks>
    public class Vector<T> where T : INumber<T> {

        #region Variables

            private readonly T[] components;

        #endregion

        #region Constructors

            public Vector(params T[] components) {
                if (components.Length == 0) throw new ArgumentException("The vector must have at least one component.");
                this.components = new T[components.Length];
                Array.Copy(components, this.components, components.Length);
            }

            public Vector(Vector<T> vector) {
                Lock(vector);

                    components = new T[vector.components.Length];
                    Array.Copy(vector.components, components, vector.components.Length);

                Unlock(vector);
            }

        #endregion

        #region Operators

            ///<summary>Adds two vectors component by component. If vectors have different sizes, the result will have the size of the biggest vector.</summary>
            ///<param name="a">The first vector to add.</param>
            ///<param name="b">The second vector to add.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This operator is only available for vectors of the same type. If you want to add vectors of different types use method Add.</remarks>
            public static Vector<T> operator +(Vector<T> a, Vector<T> b) {
                Lock(a, b);

                    var sA = a.components.Length;
                    var sB = b.components.Length;

                    var size = Math.Max(sA, sB);

                    var components = new T[size];

                    for (var i = 0; i < size; i++) {
                        if (i < sA && i < sB) components[i] = a.components[i] + b.components[i];
                        else if (i < sA) components[i] = a.components[i];
                        else components[i] = b.components[i];
                    }

                Unlock(a, b);

                return new Vector<T>(components);
            }

            ///<summary>Adds a scalar to each component of a vector.</summary>
            ///<param name="a">The vector to add the scalar to.</param>
            ///<param name="b">The scalar to add to the vector.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to add a scalar to a vector of a different type use method Add.</remarks>
            public static Vector<T> operator +(Vector<T> a, T b) {
                Lock(a);

                    var s = a.components.Length;
                    var components = new T[s];

                    for (var i = 0; i < s; i++)
                        components[i] = a.components[i] + b;

                Unlock(a);

                return new Vector<T>(components);
            }

            ///<summary>Subtracts two vectors component by component. If vectors have different sizes, the result will have the size of the biggest vector.</summary>
            ///<param name="a">The first vector to subtract.</param>
            ///<param name="b">The second vector to subtract.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This operator is only available for vectors of the same type. If you want to subtract vectors of different types use method Subtract.</remarks>
            public static Vector<T> operator -(Vector<T> a, Vector<T> b) {
                Lock(a, b);

                    var sA = a.components.Length;
                    var sB = b.components.Length;

                    var size = Math.Max(sA, sB);

                    var components = new T[size];

                    for (var i = 0; i < size; i++) {
                        if (i < sA && i < sB) components[i] = a.components[i] - b.components[i];
                        else if (i < sA) components[i] = a.components[i];
                        else components[i] = b.components[i];
                    }

                Unlock(a, b);
                   
                return new Vector<T>(components);
            }

            ///<summary>Subtracts a scalar to each component of a vector.</summary>
            ///<param name="a">The vector to subtract the scalar to.</param>
            ///<param name="b">The scalar to subtract to the vector.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to subtract a scalar to a vector of a different type use method Subtract.</remarks>
            public static Vector<T> operator -(Vector<T> a, T b) {
                Lock(a);

                    var s = a.components.Length;
                    var components = new T[s];

                    for (var i = 0; i < s; i++)
                        components[i] = a.components[i] - b;

                Unlock(a);

                return new Vector<T>(components);
            }

            ///<summary>Calculates the dot product of two vectors. The size of the vectors must be the same.</summary>
            ///<param name="a">The first vector to calculate the dot product.</param>
            ///<param name="b">The second vector to calculate the dot product.</param>
            ///<returns>The result of the dot product.</returns>
            ///<remarks>This operator is only available for vectors of the same type. If you want to calculate the dot product of vectors of different types use method Dot.</remarks>
            public static T operator *(Vector<T> a, Vector<T> b) {
                Lock(a, b);

                    if (a.components.Length != b.components.Length) throw new ArgumentException("The vectors must have the same size.");

                    var s = a.components.Length;

                    var result = T.Zero;

                    for (var i = 0; i < s; i++)
                        result += (dynamic)a.components[i] * (dynamic)b.components[i];

                Unlock(a, b);
                    
                return result;
            }

            ///<summary>Multiply each component of a vector by a scalar.</summary>
            ///<param name="a">The vector to multiply by the scalar.</param>
            ///<param name="b">The scalar to multiply to the vector.</param>
            ///<returns>The result of the multiplication.</returns>
            ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to multiply a scalar to a vector of a different type use method Multiply.</remarks>
            public static Vector<T> operator *(Vector<T> a, T b) {
                Lock(a);

                    var s = a.components.Length;

                    var components = new T[s];

                    for (var i = 0; i < s; i++)
                        components[s] = a.components[i] * b;

                Unlock(a);
                   
                return new Vector<T>(components);
            }

            ///<summary>Divides each component of a vector by a scalar.</summary>
            ///<param name="a">The vector to divide by the scalar.</param>
            ///<param name="b">The scalar to divide to the vector.</param>
            ///<returns>The result of the division.</returns>
            ///<remarks>This operator is only available for a vector and a scalar of the same type. If you want to divide a vector by a scalar of a different type use method Divide.</remarks>
            public static Vector<T> operator /(Vector<T> a, T b) {
                Lock(a);
                    var s = a.components.Length;

                    var components = new T[s];

                    for (var i = 0; i < s; i++)
                        components[s] = a.components[i] / b;

                Unlock(b);

                return new Vector<T>(components);
            }

        #endregion

        #region Methods

            ///<summary>Adds two vectors component by component. If vectors have different sizes, the result will have the size of the biggest vector.</summary>
            ///<param name="b">The second vector to add.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This method is available for vectors of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector<T> Add<T2>(Vector<T2> b) where T2 : INumber<T2> {
                Lock(this, b);
                    var sA = components.Length;
                    var sB = b.components.Length;

                    var size = Math.Max(sA, sB);

                    var componentsArray = new T[size];

                    for (var i = 0; i < size; i++) {

                        if (i < sA && i < sB) componentsArray[i] = components[i] + (dynamic)b.components[i];
                        else if (i < sA) componentsArray[i] = components[i];
                        else componentsArray[i] = (dynamic)b.components[i];

                    }

                Unlock(this, b);

                return new Vector<T>(componentsArray);
            }

            ///<summary>Adds a scalar to each component of a vector.</summary>
            ///<param name="b">The scalar to add to the vector.</param>
            ///<returns>The result of the addition.</returns>
            ///<remarks>This method is available for a vector and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector<T> Add<T2>(T2 b) where T2 : INumber<T2> {
                Lock(this);

                    var s = components.Length;
                    var componentsArray = new T[s];

                    for (var i = 0; i < s; i++)
                        componentsArray[i] = components[i] + (dynamic)b;

                Unlock(this);

                return new Vector<T>(componentsArray);
            }

            ///<summary>Subtracts two vectors component by component. If vectors have different sizes, the result will have the size of the biggest vector.</summary>
            ///<param name="b">The vector to subtract.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This method is available for vectors of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector<T> Subtract<T2>(Vector<T2> b) where T2 : INumber<T2> {
                Lock(this, b);
                    var sA = components.Length;
                    var sB = b.components.Length;

                    var size = Math.Max(sA, sB);

                    var componentsArray = new T[size];

                    for (var i = 0; i < size; i++) {

                        if (i < sA && i < sB) componentsArray[i] = components[i] - (dynamic)b.components[i];
                        else if (i < sA) componentsArray[i] = components[i];
                        else componentsArray[i] = (dynamic)b.components[i];

                    }

                Unlock(this, b);

                return new Vector<T>(componentsArray);
            }

            ///<summary>Subtracts a scalar to each component of a vector.</summary>
            ///<param name="b">The scalar to subtract to the vector.</param>
            ///<returns>The result of the subtraction.</returns>
            ///<remarks>This method is available for a vector and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector<T> Subtract<T2>(T2 b) where T2 : INumber<T2> {
                Lock(this);

                    var s = this.components.Length;
                    var componentsArray = new T[s];
                 
                    for (var i = 0; i < s; i++) componentsArray[i] = this.components[i] - (dynamic)b;

                Unlock(this);

                return new Vector<T>(componentsArray);
            }

            ///<summary>Calculates the dot product of two vectors. The size of the vectors must be the same.</summary>
            ///<param name="b">The vector to calculate the dot product.</param>
            ///<returns>The result of the dot product.</returns>
            ///<remarks>This method is available for vectors of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public T Dot<T2>(Vector<T2> b) where T2 : INumber<T2> {
                Lock(this, b);

                    if (components.Length != b.components.Length) throw new ArgumentException("The vectors must have the same size.");

                    var s = components.Length;

                    dynamic result = T.Zero;

                    for (var i = 0; i < s; i++) result += components[i] * (dynamic)b.components[i];

                Unlock(this, b);

                return result;
            }

            ///<summary>Multiply each component of a vector by a scalar.</summary>
            ///<param name="b">The scalar to multiply to the vector.</param>
            ///<returns>The result of the multiplication.</returns>
            ///<remarks>This method is available for a vector and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector<T> Multiply<T2>(Vector<T2> b) where T2 : INumber<T2> {
                Lock(this, b);

                    var sA = components.Length;
                    var sB = b.components.Length;

                    var size = Math.Max(sA, sB);

                    var componentsArray = new T[size];

                    for (var i = 0; i < size; i++) {

                        if (i < sA && i < sB) componentsArray[i] = components[i] * (dynamic)b.components[i];
                        else if (i < sA) componentsArray[i] = components[i];
                        else componentsArray[i] = (dynamic)b.components[i];

                    }

                Unlock(this, b);

                return new Vector<T>(componentsArray);
            }

            ///<summary>Multiply each component of a vector by a scalar.</summary>
            ///<param name="b">The scalar to multiply to the vector.</param>
            ///<returns>The result of the multiplication.</returns>
            ///<remarks>This method is available for a vector and a scalar of different types. Uses 'dynamic' so it is slower than the operator.</remarks>
            public Vector<T> Divide<T2>(Vector<T2> b) where T2 : INumber<T2> {
                Lock(this, b);

                    var sA = this.components.Length;
                    var sB = b.components.Length;

                    var size = Math.Max(sA, sB);

                    var componentsArray = new T[size];

                    for (var i = 0; i < size; i++) {

                        if (i < sA && i < sB) componentsArray[i] = this.components[i] / (dynamic)b.components[i];
                        else if (i < sA) componentsArray[i] = this.components[i];
                        else componentsArray[i] = (dynamic)b.components[i];

                    }

                Unlock(this, b);

                return new Vector<T>(componentsArray);
            }

            ///<summary>Calculates the cross product of two vectors.</summary>
            ///<param name="b">The vector to calculate the cross product.</param>
            ///<returns>The result of the cross product.</returns>
            ///<remarks>This method is available for vectors of different types. The size of the vectors must be 2 or 3.</remarks>
            public Vector<T> Cross(Vector<T> b) {
                try {

                    Lock(this, b);

                    if (components.Length != b.components.Length)
                        throw new ArgumentException("The vectors must have the same size.");

                    return components.Length switch
                    {
                        2 => new Vector<T>(T.Zero, T.Zero,
                            components[0] * b.components[1] - components[1] * b.components[0]),
                        3 => new Vector<T>(components[1] * b.components[2] - components[2] * b.components[1],
                            components[2] * b.components[0] - components[0] * b.components[2],
                            components[0] * b.components[1] - components[1] * b.components[0]
                        ),
                        _ => throw new InvalidOperationException(
                            "The cross product is only available for 2D and 3D vectors."),
                    };
                }finally {
                    Unlock(this, b);
                }
            }

            ///<summary>Calculates the magnitude of the vector.</summary>
            ///<returns>The magnitude of the vector.</returns>
            public float Magnitude() {
                try{
                    Lock(this);
                    return MathF.Sqrt(components.Sum(component => float.CreateChecked(component * component)));
                }finally {
                    Unlock(this);
                }
            }

            ///<summary>Normalizes the vector.</summary>
            ///<returns>The normalized vector.</returns>
            ///<remarks>Returned vector is a double vector.</remarks>
            public Vector<float> Normalize() {
                Lock(this);

                    var magnitude = Magnitude();

                    if (magnitude == 0) throw new InvalidOperationException("The magnitude of the vector is 0.");

                    var componentsArray = new float[components.Length];

                    for (var i = 0; i < componentsArray.Length; i++) componentsArray[i] = float.CreateChecked(components[i]) / magnitude;

                Unlock(this);

                return new Vector<float>(componentsArray);
            }

            ///<summary>Checks if an object is equal to the vector.</summary>
            ///<param name="obj">The object to compare.</param>
            ///<returns>True if the object is equal to the vector, false otherwise.</returns>
            public override bool Equals(object? obj) {
                if (obj == null) return false;
                if (obj.GetType() != GetType()) return false;

                var vector = (Vector<T>)obj;

                try {
                    Lock(this, vector);
                    return components.SequenceEqual(vector.components);
                }
                finally {
                    Unlock(this, vector);
                }
            }

            ///<summary>Gets the hash code of the vector.</summary>
            ///<returns>The hash code of the vector.</returns>
            public override int GetHashCode() {
                try {
                    Lock(this);
                    return components.Aggregate(17, (current, component) => current * 31 + component.GetHashCode());
                }
                finally {
                    Unlock(this);
                }
            }

            ///<summary>Gets the string representation of the vector with format (x, y, z, w...)</summary>
            ///<returns>The string representation of the vector.</returns>
            public override string ToString() {
                var result = "("; 
                
                Lock(this);

                    for (var i = 0; i < components.Length; i++) {
                        result += components[i].ToString();

                        if (i < components.Length - 1) result += ", ";
                    }

                Unlock(this);

                result += ")";

                return result;
            }

        #endregion

        #region Getters

            public int GetSize() {
                try {
                    Lock(this);

                    return components.Length;
                }
                finally {
                    Unlock(this);
                }
            }

            public T GetX() {
                try {
                    Lock(this);

                    return components[0];
                }
                finally {
                    Unlock(this);
                }
            }

            public T GetY() {
                try {
                    Lock(this);

                    return components[1];
                }
                finally {
                    Unlock(this);
                }
            }

            public T GetZ() {
                try {
                    Lock(this);

                    return components[2];
                }
                finally {
                    Unlock(this);
                }
            }

            public T GetW() {
                try {
                    Lock(this);

                    return components[3];
                }
                finally {
                    Unlock(this);
                }
            }

            public T Get(int index) {
                try {
                    Lock(this);

                    return components[index];
                }
                finally {
                    Unlock(this);
                }
            }

            public T[] GetComponents() {
                try {
                    Lock(this);

                    return components;
                }
                finally {
                    Unlock(this);
                }
            }

        #endregion

        #region Setters

            public void SetX(T x) {
                Lock(this);

                components[0] = x;

                Unlock(this);
            }

            public void SetY(T y) {
                Lock(this);

                components[1] = y;

                Unlock(this);
            }

            public void SetZ(T z) {
                Lock(this);

                components[2] = z;

                Unlock(this);
            }

            public void SetW(T w) {
                Lock(this);

                components[3] = w;

                Unlock(this);
            }

            public void Set(int index, T value) {
                Lock(this);

                components[index] = value;

                Unlock(this);
            }

        #endregion
    }
}