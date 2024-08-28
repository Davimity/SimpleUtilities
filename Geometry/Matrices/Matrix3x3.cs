using SimpleUtilities.Threading;
using System.Numerics;

namespace SimpleUtilities.Geometry.Matrices {
    public class Matrix3x3<T> where T : INumber<T> {

        #region Variables

            private T m00;
            private T m01;
            private T m02;
            private T m10;
            private T m11;
            private T m12;
            private T m20;
            private T m21;
            private T m22;

            private object lockObject;

        #endregion

        #region Properties

            public T[,] Data {
                get {
                    using (new SimpleLock(lockObject)) {
                        return new T[,] { { m00, m01, m02 }, { m10, m11, m12 }, { m20, m21, m22 } };
                    }
                }
                set {
                    using(new SimpleLock(lockObject)) {

                        m00 = value[0, 0];
                        m01 = value[0, 1];
                        m02 = value[0, 2];
                        m10 = value[1, 0];
                        m11 = value[1, 1];
                        m12 = value[1, 2];
                        m20 = value[2, 0];
                        m21 = value[2, 1];
                        m22 = value[2, 2];
                    
                    }
                }
            }

        #endregion

        #region Constructors

            public Matrix3x3() {

                m00 = T.Zero;
                m01 = T.Zero;
                m02 = T.Zero;
                m10 = T.Zero;
                m11 = T.Zero;
                m12 = T.Zero;
                m20 = T.Zero;
                m21 = T.Zero;
                m22 = T.Zero;

                lockObject = new object();
            }

            public Matrix3x3(T m00, T m01, T m02, T m10, T m11, T m12, T m20, T m21, T m22) {

                this.m00 = m00;
                this.m01 = m01;
                this.m02 = m02;
                this.m10 = m10;
                this.m11 = m11;
                this.m12 = m12;
                this.m20 = m20;
                this.m21 = m21;
                this.m22 = m22;

                lockObject = new object();
            }

        #endregion

        #region Operators

            ///<summary> Adds two matrices. </summary>
            ///<param name="a"> The first matrix. </param>
            ///<param name="b"> The second matrix. </param>
            ///<returns> The sum of the two matrices. </returns>
            ///<remarks>This operator is only available for Matrix of the same type. If you want to add Matrix of different types use method Add.</remarks>
            public static Matrix3x3<T> operator +(Matrix3x3<T> a, Matrix3x3<T> b) {
                using (new SimpleLock(a.lockObject, b.lockObject)) {
                    return new Matrix3x3<T>(a.m00 + b.m00, a.m01 + b.m01, a.m02 + b.m02, a.m10 + b.m10, a.m11 + b.m11, a.m12 + b.m12, a.m20 + b.m20, a.m21 + b.m21, a.m22 + b.m22);
                }
            }

            ///<summary> Subtracts two matrices. </summary>
            ///<param name="a"> The first matrix. </param>
            ///<param name="b"> The second matrix. </param>
            ///<returns> The difference of the two matrices. </returns>
            ///<remarks>This operator is only available for Matrix of the same type. If you want to subtract Matrix of different types use method Subtract.</remarks>
            public static Matrix3x3<T> operator -(Matrix3x3<T> a, Matrix3x3<T> b) {
                using (new SimpleLock(a.lockObject, b.lockObject)) {
                    return new Matrix3x3<T>(a.m00 - b.m00, a.m01 - b.m01, a.m02 - b.m02, a.m10 - b.m10, a.m11 - b.m11, a.m12 - b.m12, a.m20 - b.m20, a.m21 - b.m21, a.m22 - b.m22);
                }
            }

            ///<summary> Multiplies two matrices. </summary>
            ///<param name="a"> The first matrix. </param>
            ///<param name="b"> The second matrix. </param>
            ///<returns> The product of the two matrices. </returns>
            ///<remarks>This operator is only available for Matrix of the same type. If you want to multiply Matrix of different types use method Multiply.</remarks>
            public static Matrix3x3<T> operator *(Matrix3x3<T> a, Matrix3x3<T> b) {
                using (new SimpleLock(a.lockObject, b.lockObject)) {
                    return new Matrix3x3<T>(
                        a.m00 * b.m00 + a.m01 * b.m10 + a.m02 * b.m20,
                        a.m00 * b.m01 + a.m01 * b.m11 + a.m02 * b.m21,
                        a.m00 * b.m02 + a.m01 * b.m12 + a.m02 * b.m22,
                        a.m10 * b.m00 + a.m11 * b.m10 + a.m12 * b.m20,
                        a.m10 * b.m01 + a.m11 * b.m11 + a.m12 * b.m21,
                        a.m10 * b.m02 + a.m11 * b.m12 + a.m12 * b.m22,
                        a.m20 * b.m00 + a.m21 * b.m10 + a.m22 * b.m20,
                        a.m20 * b.m01 + a.m21 * b.m11 + a.m22 * b.m21,
                        a.m20 * b.m02 + a.m21 * b.m12 + a.m22 * b.m22
                    );
                }
            }

        #endregion

        #region Public Methods

            public double Determinant() {
                using (new SimpleLock(lockObject)) {
                    return double.CreateChecked(m00 * m11 * m22 + m01 * m12 * m20 + m02 * m10 * m21 - m02 * m11 * m20 - m01 * m10 * m22 - m00 * m12 * m21);
                }
            }

            public override string ToString() {
                return $"[{m00}, {m01}, {m02}]\n[{m10}, {m11}, {m12}]\n[{m20}, {m21}, {m22}]";
            }

        #endregion
    }
}
