using System.Numerics;

using static SimpleUtilities.Threading.SimpleLock;

namespace SimpleUtilities.Geometry.Matrices {
    ///<summary>Represents a 3x3 matrix.</summary>
    ///<remarks>THREAD SAFE</remarks>
    public class Matrix3X3<T> where T : INumber<T> {

        #region Static Variables

            public static readonly Matrix3X3<T> Identity = new(T.One, T.Zero, T.Zero, T.Zero, T.One, T.Zero, T.Zero, T.Zero, T.One);
            public static readonly Matrix3X3<T> Zero = new();
            public static readonly Matrix3X3<T> One = new(T.One);

        #endregion

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

        #endregion

        #region Constructors

            public Matrix3X3() {

                m00 = T.Zero;
                m01 = T.Zero;
                m02 = T.Zero;
                m10 = T.Zero;
                m11 = T.Zero;
                m12 = T.Zero;
                m20 = T.Zero;
                m21 = T.Zero;
                m22 = T.Zero;
            }

            public Matrix3X3(T all) {
                m00 = all;
                m01 = all;
                m02 = all;
                m10 = all;
                m11 = all;
                m12 = all;
                m20 = all;
                m21 = all;
                m22 = all;
            }

            public Matrix3X3(T m00, T m01, T m02, T m10, T m11, T m12, T m20, T m21, T m22) {

                this.m00 = m00;
                this.m01 = m01;
                this.m02 = m02;
                this.m10 = m10;
                this.m11 = m11;
                this.m12 = m12;
                this.m20 = m20;
                this.m21 = m21;
                this.m22 = m22;
            }

            public Matrix3X3(Matrix3X3<T> matrix) {
                Lock(matrix);

                    m00 = matrix.m00;
                    m01 = matrix.m01;
                    m02 = matrix.m02;
                    m10 = matrix.m10;
                    m11 = matrix.m11;
                    m12 = matrix.m12;
                    m20 = matrix.m20;
                    m21 = matrix.m21;
                    m22 = matrix.m22;

                Unlock(matrix);
            }

        #endregion

        #region Operators

            ///<summary> Adds two matrices. </summary>
            ///<param name="a"> The first matrix. </param>
            ///<param name="b"> The second matrix. </param>
            ///<returns> The sum of the two matrices. </returns>
            ///<remarks>This operator is only available for Matrix of the same type. If you want to add Matrix of different types use method Add.</remarks>
            public static Matrix3X3<T> operator +(Matrix3X3<T> a, Matrix3X3<T> b) {
                try {
                    Lock(a, b);

                    return new Matrix3X3<T>(a.m00 + b.m00, a.m01 + b.m01, a.m02 + b.m02, a.m10 + b.m10, a.m11 + b.m11, a.m12 + b.m12, a.m20 + b.m20, a.m21 + b.m21, a.m22 + b.m22);
                }finally {
                    Unlock(a, b);
                }
            }

            ///<summary> Subtracts two matrices. </summary>
            ///<param name="a"> The first matrix. </param>
            ///<param name="b"> The second matrix. </param>
            ///<returns> The difference of the two matrices. </returns>
            ///<remarks>This operator is only available for Matrix of the same type. If you want to subtract Matrix of different types use method Subtract.</remarks>
            public static Matrix3X3<T> operator -(Matrix3X3<T> a, Matrix3X3<T> b) {
                try {
                    Lock(a, b);

                    return new Matrix3X3<T>(a.m00 - b.m00, a.m01 - b.m01, a.m02 - b.m02, a.m10 - b.m10, a.m11 - b.m11, a.m12 - b.m12, a.m20 - b.m20, a.m21 - b.m21, a.m22 - b.m22);
                }
                finally {
                    Unlock(a, b);
                }
            }

            ///<summary> Multiplies two matrices. </summary>
            ///<param name="a"> The first matrix. </param>
            ///<param name="b"> The second matrix. </param>
            ///<returns> The product of the two matrices. </returns>
            ///<remarks>This operator is only available for Matrix of the same type. If you want to multiply Matrix of different types use method Multiply.</remarks>
            public static Matrix3X3<T> operator *(Matrix3X3<T> a, Matrix3X3<T> b) {
                try {
                    Lock(a, b);

                    return new Matrix3X3<T>(
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
                finally {
                    Unlock(a, b);
                }
            }

        #endregion

        #region Public Methods

            public double Determinant() {
                try {
                    Lock(this);

                    return double.CreateChecked(m00 * m11 * m22 + m01 * m12 * m20 + m02 * m10 * m21 - m02 * m11 * m20 - m01 * m10 * m22 - m00 * m12 * m21);
                }finally {
                    Unlock(this);
                }
            }

            public override string ToString() {
                try {
                    Lock(this);

                    return $"[{m00}, {m01}, {m02}]\n[{m10}, {m11}, {m12}]\n[{m20}, {m21}, {m22}]";
                }
                finally {
                    Unlock(this);
                }
            }

        #endregion

        #region Getters

            public T[,] GetData() {
                try {
                    Lock(this);

                    return new[,] { { m00, m01, m02 }, { m10, m11, m12 }, { m20, m21, m22 } };
                }
                finally {
                    Unlock(this);
                }
            }

        #endregion

        #region Setters

            public void SetData(T[,] value) {
                Lock(this);

                    m00 = value[0, 0];
                    m01 = value[0, 1];
                    m02 = value[0, 2];
                    m10 = value[1, 0];
                    m11 = value[1, 1];
                    m12 = value[1, 2];
                    m20 = value[2, 0];
                    m21 = value[2, 1];
                    m22 = value[2, 2];

                Unlock(this);
            }

        #endregion
    }
}
