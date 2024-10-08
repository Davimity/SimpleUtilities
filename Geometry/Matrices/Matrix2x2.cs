using System.Numerics;

using static SimpleUtilities.Threading.SimpleLock;

namespace SimpleUtilities.Geometry.Matrices {
    ///<summary>Represents a 2x2 matrix.</summary>
    ///<remarks>THREAD SAFE</remarks>
    public class Matrix2X2<T> where T : INumber<T> {

        #region Static Variables

            public static readonly Matrix2X2<T> Identity = new(T.One, T.Zero, T.Zero, T.One);
            public static readonly Matrix2X2<T> Zero = new();
            public static readonly Matrix2X2<T> One = new(T.One);
            
        #endregion

        #region Variables

            private T m00;
            private T m01;
            private T m10;
            private T m11;

        #endregion

        #region Constructors

            public Matrix2X2() {

                m00 = T.Zero;
                m01 = T.Zero;
                m10 = T.Zero;
                m11 = T.Zero;
            }

            public Matrix2X2(T all) {
                m00 = all;
                m01 = all;
                m10 = all;
                m11 = all;
            }

            public Matrix2X2(T m00, T m01, T m10, T m11) {

                this.m00 = m00;
                this.m01 = m01;
                this.m10 = m10;
                this.m11 = m11;
            }

            public Matrix2X2(Matrix2X2<T> matrix) {

                Lock(matrix);

                    m00 = matrix.m00;
                    m01 = matrix.m01;
                    m10 = matrix.m10;
                    m11 = matrix.m11;

                Unlock(matrix);
            }

        #endregion

        #region Operators

            ///<summary> Adds two matrices. </summary>
            ///<param name="a"> The first matrix. </param>
            ///<param name="b"> The second matrix. </param>
            ///<returns> The sum of the two matrices. </returns>
            ///<remarks>This operator is only available for Matrix of the same type. If you want to add Matrix of different types use method Add.</remarks>
            public static Matrix2X2<T> operator +(Matrix2X2<T> a, Matrix2X2<T> b) {
                try {
                    Lock(a, b);

                    return new Matrix2X2<T>(a.m00 + b.m00, a.m01 + b.m01, a.m10 + b.m10, a.m11 + b.m11);
                }
                finally {
                    Unlock(a, b);
                }
            }

            ///<summary> Subtracts two matrices. </summary>
            ///<param name="a"> The first matrix. </param>
            ///<param name="b"> The second matrix. </param>
            ///<returns> The difference of the two matrices. </returns>
            ///<remarks>This operator is only available for Matrix of the same type. If you want to subtract Matrix of different types use method Subtract.</remarks>
            public static Matrix2X2<T> operator -(Matrix2X2<T> a, Matrix2X2<T> b) {
                try {
                    Lock(a, b);

                    return new Matrix2X2<T>(a.m00 - b.m00, a.m01 - b.m01, a.m10 - b.m10, a.m11 - b.m11);
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
            public static Matrix2X2<T> operator *(Matrix2X2<T> a, Matrix2X2<T> b) {
                try {
                    Lock(a, b);

                    return new Matrix2X2<T>(
                        a.m00 * b.m00 + a.m01 * b.m10,
                        a.m00 * b.m01 + a.m01 * b.m11,
                        a.m10 * b.m00 + a.m11 * b.m10,
                        a.m10 * b.m01 + a.m11 * b.m11
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

                    return double.CreateChecked(m00 * m11 - m01 * m10);
                }
                finally {
                    Unlock(this);
                }
            }

            public override string ToString() {
                return $"[{m00}, {m01}]\n[{m10}, {m11}]";
            }

        #endregion

        #region Getters

            public T[,] GetData() {
                try {
                    Lock(this);

                    return new[,] { { m00, m01 }, { m10, m11 } };
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
                m10 = value[1, 0];
                m11 = value[1, 1];

                Unlock(this);
            }

        #endregion
    }
}
