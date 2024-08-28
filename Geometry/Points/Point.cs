using SimpleUtilities.Geometry.Vectors;
using SimpleUtilities.Threading;
using System.Numerics;
using System.Text;

namespace SimpleUtilities.Geometry.Points
{
    public class Point<T> where T : INumber<T>
    {

        #region Variables

        private T[] components;
        private readonly object lockObject;

        #endregion

        #region Properties

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

        public int Dimensions
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return components.Length;
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

        public Point(params T[] components)
        {

            this.components = new T[components.Length];
            Array.Copy(components, this.components, components.Length);

            lockObject = new object();

        }

        public Point(Point<T> point)
        {

            using (new SimpleLock(point.lockObject))
            {
                components = new T[point.components.Length];
                Array.Copy(point.components, components, point.components.Length);

                lockObject = new object();
            }

        }

        #endregion

        #region Operators

        ///<summary>Adds two points together</summary>
        ///<param name="point1"> The first point </param>
        ///<param name="point2"> The second point </param>
        ///<returns> The sum of the two points </returns>
        public static Point<T> operator +(Point<T> point1, Point<T> point2)
        {

            using (new SimpleLock(point1.lockObject, point2.lockObject))
            {
                T[] values = new T[point1.components.Length];

                for (int i = 0; i < point1.components.Length; i++)
                {
                    values[i] = point1.components[i] + point2.components[i];
                }

                return new Point<T>(values);
            }

        }

        ///<summary>Subtracts two points</summary>
        ///<param name="point1"> The first point </param>
        ///<param name="point2"> The second point </param>
        ///<returns> The difference between the two points </returns>
        public static Point<T> operator -(Point<T> point1, Point<T> point2)
        {

            using (new SimpleLock(point1.lockObject, point2.lockObject))
            {
                T[] values = new T[point1.components.Length];

                for (int i = 0; i < point1.components.Length; i++)
                {
                    values[i] = point1.components[i] - point2.components[i];
                }

                return new Point<T>(values);
            }
        }

        ///<summary>Multiplies a point by a scalar</summary>
        ///<param name="point"> The point </param>
        ///<param name="scalar"> The scalar </param>
        ///<returns> The point multiplied by the scalar </returns>
        public static Point<T> operator *(Point<T> point, T scalar)
        {

            using (new SimpleLock(point.lockObject))
            {
                T[] values = new T[point.components.Length];

                for (int i = 0; i < point.components.Length; i++)
                {
                    values[i] = point.components[i] * scalar;
                }

                return new Point<T>(values);
            }
        }

        #endregion

        #region Methods

        ///<summary> Calculates the distance between two points </summary>
        ///<param name="point"> The point to calculate the distance to </param>
        ///<returns> The distance between the two points </returns>
        public double Distance(Point<T> point)
        {

            using (new SimpleLock(lockObject, point.lockObject))
            {
                double sum = 0;

                for (int i = 0; i < components.Length; i++)
                {
                    double diff = double.CreateChecked(components[i] - point.components[i]);
                    sum = sum + diff * diff;
                }

                return Math.Sqrt(sum);
            }
        }

        ///<summary> Calculates the vector between two points </summary>
        ///<param name="point"> The point to calculate the vector to </param>
        ///<returns> The vector between the two points </returns>
        ///<remarks>The vector points from the current point to the given point</remarks>
        public SimpleUtilities.Geometry.Vectors.Vector<double> Vector(Point<T> point)
        {

            using (new SimpleLock(lockObject, point.lockObject))
            {
                double[] values = new double[components.Length];

                for (int i = 0; i < components.Length; i++)
                {
                    values[i] = double.CreateChecked(components[i] - point.components[i]);
                }

                return new SimpleUtilities.Geometry.Vectors.Vector<double>(values);
            }

        }

        ///<summary>Convert the point to a string with the format (x, y, z, w...)</summary>
        ///<returns> The point as a string </returns>
        public override string ToString()
        {

            using (new SimpleLock(lockObject))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");

                for (int i = 0; i < components.Length; i++)
                {
                    sb.Append(components[i].ToString());
                    if (i != components.Length - 1) sb.Append(", ");
                }

                sb.Append(")");

                return sb.ToString();
            }

        }

        ///<summary> Calculates the hash code of the point </summary>
        ///<returns> The hash code of the point </returns>
        public override int GetHashCode()
        {

            using (new SimpleLock(lockObject))
            {
                int hash = 17;

                for (int i = 0; i < components.Length; i++)
                {
                    hash = hash * 23 + components[i].GetHashCode();
                }

                return hash;
            }

        }

        ///<summary> Checks if two points are equal </summary>
        ///<param name="obj"> The object to compare </param>
        ///<returns> True if the points are equal, false otherwise </returns>
        public override bool Equals(object? obj)
        {

            if (obj == null) return false;
            if (obj.GetType() != typeof(Point<T>)) return false;

            Point<T> point = (Point<T>)obj;

            using (new SimpleLock(lockObject, point.lockObject))
            {
                if (components.Length != point.components.Length) return false;

                for (int i = 0; i < components.Length; i++)
                {
                    if (!components[i].Equals(point.components[i])) return false;
                }

                return true;
            }

        }

        #endregion
    }
}
