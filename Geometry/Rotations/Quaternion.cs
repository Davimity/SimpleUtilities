using SimpleUtilities.Geometry.Vectors;
using SimpleUtilities.Threading;

namespace SimpleUtilities.Geometry.Rotations
{
    ///<summary> A Quaternion class </summary>
    ///<remarks> THREAD SAFE </remarks>
    public class Quaternion
    {

        #region Variables

        public static Quaternion Identity => new Quaternion(0, 0, 0, 1);

        private float x { get; set; }
        private float y { get; set; }
        private float z { get; set; }
        private float w { get; set; }

        private object lockObject;

        #endregion

        #region Properties

        public float X
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
        public float Y
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
        public float Z
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return z;
                }
            }
            set
            {
                using (new SimpleLock(lockObject))
                {
                    z = value;
                }
            }
        }
        public float W
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return w;
                }
            }
            set
            {
                using (new SimpleLock(lockObject))
                {
                    w = value;
                }
            }
        }

        public float[] XYZW
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return [x, y, z, w];
                }
            }
        }
        public float[] XYZ
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return [x, y, z];
                }
            }
        }
        public float[] YZW
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return [y, z, w];
                }
            }
        }
        public float[] XY
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return [x, y];
                }
            }
        }
        public float[] YZ
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return [y, z];
                }
            }
        }
        public float[] ZW
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return [z, w];
                }
            }
        }
        public float[] XW
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return [x, w];
                }
            }
        }
        public float[] XZ
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return [x, z];
                }
            }
        }
        public float[] YW
        {
            get
            {
                using (new SimpleLock(lockObject))
                {
                    return [y, w];
                }
            }
        }

        #endregion

        #region Constructors

        public Quaternion()
        {
            x = 0;
            y = 0;
            z = 0;
            w = 1;

            lockObject = new object();
        }

        public Quaternion(Quaternion q)
        {
            using (new SimpleLock(q.lockObject))
            {
                x = q.x;
                y = q.y;
                z = q.z;
                w = q.w;
            }

            lockObject = new object();
        }

        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;

            lockObject = new object();
        }

        public Quaternion(Vector3<float> vector, float w)
        {

            float[] c = vector.XYZ;

            x = c[0];
            y = c[1];
            z = c[2];
            this.w = this.w;

            lockObject = new object();
        }

        #endregion

        #region Operators

        ///<summary> Multiplies two quaternions (Combines two rotations) </summary>
        ///<param name="a"> The first quaternion </param>
        ///<param name="b"> The second quaternion </param>
        ///<returns> The result of the addition </returns>
        public static Quaternion operator *(Quaternion a, Quaternion b)
        {
            using (new SimpleLock(a.lockObject, b.lockObject))
            {
                return new Quaternion(
                    a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y,
                    a.w * b.y - a.x * b.z + a.y * b.w + a.z * b.x,
                    a.w * b.z + a.x * b.y - a.y * b.x + a.z * b.w,
                    a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z
                );
            }
        }

        ///<summary>Obtain a System.Numerics.Quaternion from a Quaternion</summary>
        ///<param name="a">The Quaternion to convert</param>
        ///<returns>The System.Numerics.Quaternion</returns>
        public static implicit operator System.Numerics.Quaternion(Quaternion a)
        {
            using (new SimpleLock(a.lockObject))
            {
                return new System.Numerics.Quaternion(a.X, a.Y, a.Z, a.W);
            }
        }

        ///<summary>Obtain a Quaternion from a System.Numerics.Quaternion</summary>
        ///<param name="a">The System.Numerics.Quaternion to convert</param>
        ///<returns>The Quaternion</returns>
        public static explicit operator Quaternion(System.Numerics.Quaternion a)
        {
            return new Quaternion(a.X, a.Y, a.Z, a.W);
        }

        #endregion

        #region Methods

        ///<summary> Rotates the quaternion by an angle around an axis </summary>
        ///<param name="axis"> The axis to rotate around </param>
        ///<param name="angle"> The angle to rotate </param>
        ///<remarks> The angle is in radians </remarks>
        public void Rotate(Vector3<float> axis, float angle)
        {
            using (new SimpleLock(lockObject))
            {
                var rotation = CreateFromAxisAngle(axis, angle);
                Quaternion q = rotation * this;

                x = q.x;
                y = q.y;
                z = q.z;
                w = q.w;
            }
        }

        ///<summary> Rotates a vector by the quaternion </summary>
        ///<param name="vector"> The vector to rotate </param>
        ///<returns> The rotated vector </returns>
        public Vector3<float> Rotate(Vector3<float> vector)
        {
            using (new SimpleLock(lockObject))
            {
                var qVector = new Quaternion(vector, 0);
                var qConjugate = new Quaternion(-x, -y, -z, w);

                var result = this * qVector * qConjugate;

                return new Vector3<float>(result.x, result.y, result.z);
            }
        }

        ///<summary> Normalizes the quaternion </summary>
        public void Normalize()
        {
            using (new SimpleLock(lockObject))
            {
                float magnitude = (float)Math.Sqrt(x * x + y * y + z * z + w * w);
                if (magnitude > float.Epsilon)
                {
                    x /= magnitude;
                    y /= magnitude;
                    z /= magnitude;
                    w /= magnitude;
                }
            }
        }

        public override string ToString()
        {
            using (new SimpleLock(lockObject))
            {
                return $"({x}, {y}, {z}, {w})";
            }
        }

        #endregion

        #region Static Methods

        ///<summary> Creates a quaternion from an axis and an angle </summary>
        ///<param name="axis"> The axis to rotate around </param>
        ///<param name="angle"> The angle to rotate </param>
        ///<remarks> The angle is in radians </remarks>
        public static Quaternion CreateFromAxisAngle(Vector3<float> axis, float angle)
        {
            var halfAngle = angle / 2f;
            var sin = (float)Math.Sin(halfAngle);

            float[] q = axis.XYZ;

            return new Quaternion(q[0] * sin, q[1] * sin, q[2] * sin, (float)Math.Cos(halfAngle));
        }

        ///<summary> Creates a quaternion from Euler angles </summary>
        ///<param name="pitch"> The pitch angle </param>
        ///<param name="roll"> The roll angle </param>
        ///<param name="yaw"> The yaw angle </param>
        ///<remarks> The angles are in radians </remarks>
        public static Quaternion CreateFromEulerAngles(float pitch, float yaw, float roll)
        {
            float cy = (float)Math.Cos(yaw * 0.5f);
            float sy = (float)Math.Sin(yaw * 0.5f);
            float cr = (float)Math.Cos(roll * 0.5f);
            float sr = (float)Math.Sin(roll * 0.5f);
            float cp = (float)Math.Cos(pitch * 0.5f);
            float sp = (float)Math.Sin(pitch * 0.5f);

            return new Quaternion(
                sr * cp * cy - cr * sp * sy,
                cr * sp * cy + sr * cp * sy,
                cr * cp * sy - sr * sp * cy,
                cr * cp * cy + sr * sp * sy
            );
        }

        ///<summary> Converts a quaternion to Euler angles </summary>
        ///<returns> The Euler angles </returns>
        public Vector3<float> ToEulerAngles()
        {
            Vector3<float> angles = new Vector3<float>();

            // roll (x rotation)
            double sinr_cosp = 2 * (w * x + y * z);
            double cosr_cosp = 1 - 2 * (x * x + y * y);
            angles.X = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch (y rotation)
            double sinp = 2 * (w * y - z * x);
            if (Math.Abs(sinp) >= 1)
                angles.Y = (float)Math.CopySign(Math.PI / 2, sinp); // use 90 degrees if out of range
            else
                angles.Y = (float)Math.Asin(sinp);

            // yaw (z rotation)
            double siny_cosp = 2 * (w * z + x * y);
            double cosy_cosp = 1 - 2 * (y * y + z * z);
            angles.Z = (float)Math.Atan2(siny_cosp, cosy_cosp);

            return angles;
        }

        #endregion
    }
}
