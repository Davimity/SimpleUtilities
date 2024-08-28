using SimpleUtilities.Threading;
using System.Numerics;

namespace SimpleUtilities.Geometry.Matrices {
    public class Matrix2x2<T> where T : INumber<T> {

        #region Variables

            private T m00;
            private T m01;
            private T m10;
            private T m11;

            private object lockObject;

        #endregion

        #region Properties

            public T[,] Data {
                get {
                    using (new SimpleLock(lockObject)) {
                        return new T[,] { { m00, m01 }, { m10, m11 } };
                    }
                }
                set {
                    using (new SimpleLock(lockObject)) {
                        m00 = value[0, 0];
                        m01 = value[0, 1];
                        m10 = value[1, 0];
                        m11 = value[1, 1];
                    }
                }
            }

        #endregion

        #region Constructors

            public Matrix2x2() {

                m00 = T.Zero;
                m01 = T.Zero;
                m10 = T.Zero;
                m11 = T.Zero;

                lockObject = new object();
            }

            public Matrix2x2(T m00, T m01, T m10, T m11) {

                this.m00 = m00;
                this.m01 = m01;
                this.m10 = m10;
                this.m11 = m11;

                lockObject = new object();
            }

        #endregion

        #region Operators

            ///<summary> Adds two matrices. </summary>
            ///<param name="a"> The first matrix. </param>
            ///<param name="b"> The second matrix. </param>
            ///<returns> The sum of the two matrices. </returns>
            ///<remarks>This operator is only available for Matrix of the same type. If you want to add Matrix of different types use method Add.</remarks>
            public static Matrix2x2<T> operator +(Matrix2x2<T> a, Matrix2x2<T> b) {
                using(new SimpleLock(a.lockObject, b.lockObject)) {
                    return new Matrix2x2<T>(a.m00 + b.m00, a.m01 + b.m01, a.m10 + b.m10, a.m11 + b.m11);
                }      
            }

            ///<summary> Subtracts two matrices. </summary>
            ///<param name="a"> The first matrix. </param>
            ///<param name="b"> The second matrix. </param>
            ///<returns> The difference of the two matrices. </returns>
            ///<remarks>This operator is only available for Matrix of the same type. If you want to subtract Matrix of different types use method Subtract.</remarks>
            public static Matrix2x2<T> operator -(Matrix2x2<T> a, Matrix2x2<T> b) {
                using(new SimpleLock(a.lockObject, b.lockObject)) {
                    return new Matrix2x2<T>(a.m00 - b.m00, a.m01 - b.m01, a.m10 - b.m10, a.m11 - b.m11);
                }      
            }

            ///<summary> Multiplies two matrices. </summary>
            ///<param name="a"> The first matrix. </param>
            ///<param name="b"> The second matrix. </param>
            ///<returns> The product of the two matrices. </returns>
            ///<remarks>This operator is only available for Matrix of the same type. If you want to multiply Matrix of different types use method Multiply.</remarks>
            public static Matrix2x2<T> operator *(Matrix2x2<T> a, Matrix2x2<T> b) {
                using(new SimpleLock(a.lockObject, b.lockObject)) {
                    return new Matrix2x2<T>(
                        a.m00 * b.m00 + a.m01 * b.m10,
                        a.m00 * b.m01 + a.m01 * b.m11,
                        a.m10 * b.m00 + a.m11 * b.m10,
                        a.m10 * b.m01 + a.m11 * b.m11
                    );
                }
            }

        #endregion

        #region Public Methods

            public double Determinant() {
                using(new SimpleLock(lockObject)) {
                    return double.CreateChecked(m00 * m11 - m01 * m10);
                }
            }

            public override string ToString() {
                return $"[{m00}, {m01}]\n[{m10}, {m11}]";
            }

        #endregion
    }
}
