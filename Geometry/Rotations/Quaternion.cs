using SimpleUtilities.Geometry.Vectors;
using static SimpleUtilities.Threading.SimpleLock;

namespace SimpleUtilities.Geometry.Rotations {
    ///<summary> Represents a quaternion. </summary>
    ///<remarks> THREAD SAFE </remarks>
    public class Quaternion {

        #region Static Variables

            public static Quaternion Identity => new(0, 0, 0, 1);

        #endregion

        #region Variables

            private float x;
            private float y;
            private float z;
            private float w;

        #endregion

        #region Constructors

            public Quaternion() {
                x = 0;
                y = 0;
                z = 0;
                w = 1;
            }

            public Quaternion(Quaternion q) {
                try {
                    Lock(q);

                    x = q.x;
                    y = q.y;
                    z = q.z;
                    w = q.w;
                }finally {
                    Unlock(q);
                }
            }

            public Quaternion(float x, float y, float z, float w) {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

            public Quaternion(Vector3<float> vector, float w) {

                var c = vector.GetXyz();

                x = c[0];
                y = c[1];
                z = c[2];
                this.w = w;
            }

            public Quaternion(Vector4<float> vector) {

                var c = vector.GetXyzw();

                x = c[0];
                y = c[1];
                z = c[2];
                w = c[3];
            }

        #endregion

        #region Operators

            ///<summary> Multiplies two quaternions (Combines two rotations) </summary>
            ///<param name="a"> The first quaternion </param>
            ///<param name="b"> The second quaternion </param>
            ///<returns> The result of the addition </returns>
            public static Quaternion operator *(Quaternion a, Quaternion b) {
                try {
                    Lock(a, b);

                    return new Quaternion(
                        a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y,
                        a.w * b.y - a.x * b.z + a.y * b.w + a.z * b.x,
                        a.w * b.z + a.x * b.y - a.y * b.x + a.z * b.w,
                        a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z
                    );
                }
                finally {
                    Unlock(a, b);
                }
            }

            ///<summary>Obtain a System.Numerics.Quaternion from a Quaternion</summary>
            ///<param name="a">The Quaternion to convert</param>
            ///<returns>The System.Numerics.Quaternion</returns>
            public static implicit operator System.Numerics.Quaternion(Quaternion a) {
                try {
                    Lock(a);

                    var xyzw = a.GetXyzw();
                    return new System.Numerics.Quaternion(xyzw[0], xyzw[1], xyzw[2], xyzw[3]);
                }
                finally {
                    Unlock(a);
                }
            }

            ///<summary>Obtain a Quaternion from a System.Numerics.Quaternion</summary>
            ///<param name="a">The System.Numerics.Quaternion to convert</param>
            ///<returns>The Quaternion</returns>
            public static explicit operator Quaternion(System.Numerics.Quaternion a) {
                return new Quaternion(a.X, a.Y, a.Z, a.W);
            }

        #endregion

        #region Public Methods

            ///<summary> Rotates the quaternion by an angle around an axis </summary>
            ///<param name="axis"> The axis to rotate around </param>
            ///<param name="angle"> The angle to rotate </param>
            ///<remarks> The angle is in radians </remarks>
            public void Rotate(Vector3<float> axis, float angle) {
                Lock(this);

                    var rotation = CreateFromAxisAngle(axis, angle);
                    var q = rotation * this;

                    x = q.x;
                    y = q.y;
                    z = q.z;
                    w = q.w;
                
                Unlock(this);
            }

            ///<summary> Rotates a vector by the quaternion </summary>
            ///<param name="vector"> The vector to rotate </param>
            ///<returns> The rotated vector </returns>
            public Vector3<float> Rotate(Vector3<float> vector) {
                Lock(this);

                    var qVector = new Quaternion(vector, 0);
                    var qConjugate = new Quaternion(-x, -y, -z, w);

                Unlock(this);

                var result = this * qVector * qConjugate;

                return new Vector3<float>(result.x, result.y, result.z);
            }

            ///<summary> Converts a quaternion to Euler angles </summary>
            ///<returns> The Euler angles </returns>
            public Vector3<float> ToEulerAngles() {
                var angles = new Vector3<float>();

                Lock(this);

                    double sinrCosp = 2 * (w * x + y * z);
                    double cosrCosp = 1 - 2 * (x * x + y * y);
                    angles.SetX((float)Math.Atan2(sinrCosp, cosrCosp));

                    double sinp = 2 * (w * y - z * x);
                    if (Math.Abs(sinp) >= 1)
                        angles.SetY((float)Math.CopySign(Math.PI / 2, sinp));
                    else
                        angles.SetY((float)Math.Asin(sinp));

                    double sinyCosp = 2 * (w * z + x * y);
                    double cosyCosp = 1 - 2 * (y * y + z * z);
                    angles.SetZ((float)Math.Atan2(sinyCosp, cosyCosp));

                Unlock(this);

                return angles;
            }

            ///<summary> Normalizes the quaternion </summary>
            public void Normalize() {
                Lock(this);

                    var magnitude = (float)Math.Sqrt(x * x + y * y + z * z + w * w);
                    if(magnitude <= float.Epsilon) return;

                    x /= magnitude;
                    y /= magnitude;
                    z /= magnitude;
                    w /= magnitude;

                Unlock(this);
            }

            public override string ToString() {
                try {
                    Lock(this);

                    return $"({x}, {y}, {z}, {w})";
                }finally {
                    Unlock(this);
                }
            }

        #endregion

        #region Static Methods

            ///<summary> Creates a quaternion from an axis and an angle </summary>
            ///<param name="axis"> The axis to rotate around </param>
            ///<param name="angle"> The angle to rotate </param>
            ///<remarks> The angle is in radians </remarks>
            public static Quaternion CreateFromAxisAngle(Vector3<float> axis, float angle) {
                var halfAngle = angle / 2f;
                var sin = (float)Math.Sin(halfAngle);

                var q = axis.GetXyz();

                return new Quaternion(q[0] * sin, q[1] * sin, q[2] * sin, (float)Math.Cos(halfAngle));
            }

            ///<summary> Creates a quaternion from Euler angles </summary>
            ///<param name="pitch"> The pitch angle </param>
            ///<param name="roll"> The roll angle </param>
            ///<param name="yaw"> The yaw angle </param>
            ///<remarks> The angles are in radians </remarks>
            public static Quaternion CreateFromEulerAngles(float pitch, float yaw, float roll) {
                var cy = (float)Math.Cos(yaw * 0.5f);
                var sy = (float)Math.Sin(yaw * 0.5f);
                var cr = (float)Math.Cos(roll * 0.5f);
                var sr = (float)Math.Sin(roll * 0.5f);
                var cp = (float)Math.Cos(pitch * 0.5f);
                var sp = (float)Math.Sin(pitch * 0.5f);

                return new Quaternion(
                    sr * cp * cy - cr * sp * sy,
                    cr * sp * cy + sr * cp * sy,
                    cr * cp * sy - sr * sp * cy,
                    cr * cp * cy + sr * sp * sy
                );
            }

            ///<summary> Parses a quaternion from a string </summary>
            ///<param name="str"> The string to parse </param>
            ///<returns> The parsed quaternion </returns>
            public static Quaternion Parse(string str) {
                str = str.Replace("(", "").Replace(")", "").Replace("\n", "").Replace(" ", "");
                var values = str.Split(',');
                return new Quaternion(
                    float.Parse(values[0]),
                    float.Parse(values[1]),
                    float.Parse(values[2]),
                    float.Parse(values[3])
                );
            }

        #endregion

        #region Getters

            public float GetX() {
                try {
                    Lock(this);

                    return x;
                }
                finally {
                    Unlock(this);
                }
            }

            public float GetY() {
                try {
                    Lock(this);

                    return y;
                }
                finally {
                    Unlock(this);
                }
            }

            public float GetZ() {
                try {
                    Lock(this);

                    return z;
                }
                finally {
                    Unlock(this);
                }
            }

            public float GetW() {
                try {
                    Lock(this);

                    return w;
                }
                finally {
                    Unlock(this);
                }
            }

            public float[] GetXyz() {
                try {
                    Lock(this);

                    return [x, y, z];
                }
                finally {
                    Unlock(this);
                }
            }

            public float[] GetXyzw() {
                try {
                    Lock(this);

                    return [x, y, z, w];
                }
                finally {
                    Unlock(this);
                }
            }

        #endregion

        #region Setters

            public void SetX(float value) {
                Lock(this);

                    x = value;

                Unlock(this);
            }
            
            public void SetY(float value) {
                Lock(this);

                    y = value;

                Unlock(this);
            }

            public void SetZ(float value) {
                Lock(this);

                    z = value;

                Unlock(this);
            }
            
            public void SetW(float value) {
                Lock(this);

                    w = value;

                Unlock(this);
            }

        #endregion
    }
}
