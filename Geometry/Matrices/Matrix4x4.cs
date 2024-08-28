using SimpleUtilities.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUtilities.Geometry.Matrices {
    public class Matrix4x4<T> where T : INumber<T> {

        #region Variables

            private T m00;
            private T m01;
            private T m02;
            private T m03;
            private T m10;
            private T m11;
            private T m12;
            private T m13;
            private T m20;
            private T m21;
            private T m22;
            private T m23;
            private T m30;
            private T m31;
            private T m32;
            private T m33;

            private object lockObject;

        #endregion

        #region Properties

            public T[,] Data {
                get {
                    using (new SimpleLock(lockObject)) {
                        return new T[,] { { m00, m01, m02, m03 }, { m10, m11, m12, m13 }, { m20, m21, m22, m23 }, { m30, m31, m32, m33 } };
                    }
                }
                set {
                    using (new SimpleLock(lockObject)) {
                        m00 = value[0, 0];
                        m01 = value[0, 1];
                        m02 = value[0, 2];
                        m03 = value[0, 3];
                        m10 = value[1, 0];
                        m11 = value[1, 1];
                        m12 = value[1, 2];
                        m13 = value[1, 3];
                        m20 = value[2, 0];
                        m21 = value[2, 1];
                        m22 = value[2, 2];
                        m23 = value[2, 3];
                        m30 = value[3, 0];
                        m31 = value[3, 1];
                        m32 = value[3, 2];
                        m33 = value[3, 3];
                    }
                }
            }

        #endregion

        #region Constructors

            public Matrix4x4() {

                m00 = T.Zero;
                m01 = T.Zero;
                m02 = T.Zero;
                m03 = T.Zero;
                m10 = T.Zero;
                m11 = T.Zero;
                m12 = T.Zero;
                m13 = T.Zero;
                m20 = T.Zero;
                m21 = T.Zero;
                m22 = T.Zero;
                m23 = T.Zero;
                m30 = T.Zero;
                m31 = T.Zero;
                m32 = T.Zero;
                m33 = T.Zero;

                lockObject = new object();
            }

            public Matrix4x4(T m00, T m01, T m02, T m03, T m10, T m11, T m12, T m13, T m20, T m21, T m22, T m23, T m30, T m31, T m32, T m33) {

                this.m00 = m00;
                this.m01 = m01;
                this.m02 = m02;
                this.m03 = m03;
                this.m10 = m10;
                this.m11 = m11;
                this.m12 = m12;
                this.m13 = m13;
                this.m20 = m20;
                this.m21 = m21;
                this.m22 = m22;
                this.m23 = m23;
                this.m30 = m30;
                this.m31 = m31;
                this.m32 = m32;
                this.m33 = m33;

                lockObject = new object();
            }

        #endregion

        #region Operators

            ///<summary> Adds two matrices. </summary>
            ///<param name="a"> The first matrix. </param>
            ///<param name="b"> The second matrix. </param>
            ///<returns> The sum of the two matrices. </returns>
            ///<remarks>This operator is only available for Matrix of the same type. If you want to add Matrix of different types use method Add.</remarks>
            public static Matrix4x4<T> operator +(Matrix4x4<T> a, Matrix4x4<T> b) {
                using (new SimpleLock(a.lockObject, b.lockObject)) {
                    return new Matrix4x4<T>(a.m00 + b.m00, a.m01 + b.m01, a.m02 + b.m02, a.m03 + b.m03, a.m10 + b.m10, a.m11 + b.m11, a.m12 + b.m12, a.m13 + b.m13, a.m20 + b.m20, a.m21 + b.m21, a.m22 + b.m22, a.m23 + b.m23, a.m30 + b.m30, a.m31 + b.m31, a.m32 + b.m32, a.m33 + b.m33);
                }
            }

            ///<summary> Subtracts two matrices. </summary>
            ///<param name="a"> The first matrix. </param>
            ///<param name="b"> The second matrix. </param>
            ///<returns> The difference of the two matrices. </returns>
            ///<remarks>This operator is only available for Matrix of the same type. If you want to subtract Matrix of different types use method Subtract.</remarks>
            public static Matrix4x4<T> operator -(Matrix4x4<T> a, Matrix4x4<T> b) {
                using (new SimpleLock(a.lockObject, b.lockObject)) {
                    return new Matrix4x4<T>(a.m00 - b.m00, a.m01 - b.m01, a.m02 - b.m02, a.m03 - b.m03, a.m10 - b.m10, a.m11 - b.m11, a.m12 - b.m12, a.m13 - b.m13, a.m20 - b.m20, a.m21 - b.m21, a.m22 - b.m22, a.m23 - b.m23, a.m30 - b.m30, a.m31 - b.m31, a.m32 - b.m32, a.m33 - b.m33);
                }
            }

            ///<summary> Multiplies two matrices. </summary>
            ///<param name="a"> The first matrix. </param>
            ///<param name="b"> The second matrix. </param>
            ///<returns> The product of the two matrices. </returns>
            ///<remarks>This operator is only available for Matrix of the same type. If you want to multiply Matrix of different types use method Multiply.</remarks>
            public static Matrix4x4<T> operator *(Matrix4x4<T> a, Matrix4x4<T> b) {
                using (new SimpleLock(a.lockObject, b.lockObject)) {
                    return new Matrix4x4<T>(
                        a.m00 * b.m00 + a.m01 * b.m10 + a.m02 * b.m20 + a.m03 * b.m30,
                        a.m00 * b.m01 + a.m01 * b.m11 + a.m02 * b.m21 + a.m03 * b.m31,
                        a.m00 * b.m02 + a.m01 * b.m12 + a.m02 * b.m22 + a.m03 * b.m32,
                        a.m00 * b.m03 + a.m01 * b.m13 + a.m02 * b.m23 + a.m03 * b.m33,
                        a.m10 * b.m00 + a.m11 * b.m10 + a.m12 * b.m20 + a.m13 * b.m30,
                        a.m10 * b.m01 + a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31,
                        a.m10 * b.m02 + a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32,
                        a.m10 * b.m03 + a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33,
                        a.m20 * b.m00 + a.m21 * b.m10 + a.m22 * b.m20 + a.m23 * b.m30,
                        a.m20 * b.m01 + a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31,
                        a.m20 * b.m02 + a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32,
                        a.m20 * b.m03 + a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33,
                        a.m30 * b.m00 + a.m31 * b.m10 + a.m32 * b.m20 + a.m33 * b.m30,
                        a.m30 * b.m01 + a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31,
                        a.m30 * b.m02 + a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32,
                        a.m30 * b.m03 + a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33
                    );
                }
            }

        #endregion

        #region Public Methods

            public double Determinant() {
                using (new SimpleLock(lockObject)) {
                    return double.CreateChecked(
                        m03 * m12 * m21 * m30 - m02 * m13 * m21 * m30 -
                        m03 * m11 * m22 * m30 + m01 * m13 * m22 * m30 +
                        m02 * m11 * m23 * m30 - m01 * m12 * m23 * m30 -
                        m03 * m12 * m20 * m31 + m02 * m13 * m20 * m31 +
                        m03 * m10 * m22 * m31 - m00 * m13 * m22 * m31 -
                        m02 * m10 * m23 * m31 + m00 * m12 * m23 * m31 +
                        m03 * m11 * m20 * m32 - m01 * m13 * m20 * m32 -
                        m03 * m10 * m21 * m32 + m00 * m13 * m21 * m32 +
                        m01 * m10 * m23 * m32 - m00 * m11 * m23 * m32 -
                        m02 * m11 * m20 * m33 + m01 * m12 * m20 * m33 +
                        m02 * m10 * m21 * m33 - m00 * m12 * m21 * m33 -
                        m01 * m10 * m22 * m33 + m00 * m11 * m22 * m33
                    );
                }
            }

            public override string ToString() {
                return $"[{m00}, {m01}, {m02}, {m03}]\n[{m10}, {m11}, {m12}, {m13}]\n[{m20}, {m21}, {m22}, {m23}]\n[{m30}, {m31}, {m32}, {m33}]";
            }

        #endregion

    }
}
