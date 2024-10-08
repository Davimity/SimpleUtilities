using SimpleUtilities.Geometry.Vectors;

using static SimpleUtilities.Threading.SimpleLock;

namespace SimpleUtilities.Utilities.Colors{
    /// <summary>Class that represents a color with RGBA values between 0 and 1.</summary>
    ///<remarks>THREAD SAFE</remarks>
    public class Color{

        #region Static Colors

            public static readonly Color White = new(1, 1, 1);
            public static readonly Color Black = new(0, 0, 0);
            public static readonly Color Red = new(1, 0, 0);
            public static readonly Color Green = new(0, 1, 0);
            public static readonly Color Blue = new(0, 0, 1);
            public static readonly Color Yellow = new(1, 1, 0);
            public static readonly Color Cyan = new(0, 1, 1);
            public static readonly Color Magenta = new(1, 0, 1);
            public static readonly Color Gray = new(0.5f, 0.5f, 0.5f);

        #endregion

        #region Variables

            private float r, g, b, a;

        #endregion

        #region Constructors

            public Color(float r, float g, float b){
                this.r = r;
                this.g = g;
                this.b = b;
                a = 1;
            }

            public Color(float r, float g, float b, float a){
                this.r = r;
                this.g = g;
                this.b = b;
                this.a = a;
            }

        #endregion

        #region Operators

            public static implicit operator Vector4<float>(Color color) {
                return color.GetRgba();
            }

        #endregion

        #region Public methods

            public string ToHex(){
                try {
                    Lock(this);
                    return $"#{(int)(r * 255):X2}{(int)(g * 255):X2}{(int)(b * 255):X2}";
                }
                finally {
                    Unlock(this);
                }
            }

            public Color Blend(params Color[] color){

                var locks = new object[color.Length + 1];

                for(var i = 0; i < color.Length; i++) locks[i] = color[i];
                locks[^1] = this;

                try {
                    Lock(locks);

                    var ar = r;
                    var ag = g;
                    var ab = b;
                    var aa = a;

                    foreach (var c in color) {
                        ar += c.r;
                        ag += c.g;
                        ab += c.b;
                        aa += c.a;
                    }

                    ar /= color.Length;
                    ag /= color.Length;
                    ab /= color.Length;
                    aa /= color.Length;

                    return new Color(ar, ag, ab, aa);
                }
                finally {
                    Unlock(locks);
                }
            }

            public override string ToString(){
                try {
                    Lock(this);
                    return $"R: {r}, G: {g}, B: {b}, A: {a}";
                }
                finally {
                    Unlock(this);
                }
            }

            public override int GetHashCode(){
                Lock(this);

                    var hash = 17;
                    hash = hash * 23 + r.GetHashCode() * 255;
                    hash = hash * 23 + g.GetHashCode() * 255;
                    hash = hash * 23 + b.GetHashCode() * 255;
                    hash = hash * 23 + a.GetHashCode() * 255;
                    
                Unlock(this);

                return hash;
            }

            public override bool Equals(object? obj){
                try {
                    Lock(this);
                    if (obj == null || GetType() != obj.GetType()) return false;

                    var color = (Color)obj;
                    return r - color.r < float.Epsilon && g - color.g < float.Epsilon && b - color.b < float.Epsilon && a - color.a < float.Epsilon;
                }
                finally {
                    Unlock(this);
                }
            }

        #endregion

        #region Static Methods

            public Color From255(byte r255, byte b255, byte g255, byte a255) {
                return new Color(r255 / 255f, g255 / 255f, b255 / 255f, a255 / 255f);
            }

        #endregion

        #region Getters
            
            public float GetR() {
                try {
                    Lock(this);
                    return r;
                }
                finally {
                    Unlock(this);
                }
            }

            public float GetG() {
                try {
                    Lock(this);
                    return g;
                }
                finally {
                    Unlock(this);
                }
            }

            public float GetB() {
                try {
                    Lock(this);
                    return b;
                }
                finally {
                    Unlock(this);
                }
            }

            public float GetA() {
                try {
                    Lock(this);
                    return a;
                }
                finally {
                    Unlock(this);
                }
            }

            public Vector3<float> GetRgb() {
                try {
                    Lock(this);
                    return new Vector3<float>(r, g, b);
                }
                finally {
                    Unlock(this);
                }
            }

            public Vector4<float> GetRgba() {
                try {
                    Lock(this);
                    return new Vector4<float>(r, g, b, a);
                }
                finally {
                    Unlock(this);
                }
            }

        #endregion

        #region Setters

            public void SetR(float rval) {
                Lock(this);

                r = rval switch
                {
                    < 0 => 0,
                    > 1 => 1,
                    _ => rval,
                };

                Unlock(this);
            }

            public void SetG(float gval) {
                Lock(this);

                g = gval switch
                {
                    < 0 => 0,
                    > 1 => 1,
                    _ => gval,
                };

                Unlock(this);
            }

            public void SetB(float bval) {
                Lock(this);

                b = bval switch
                {
                    < 0 => 0,
                    > 1 => 1,
                    _ => bval,
                };

                Unlock(this);
            }

            public void SetA(float aval) {
                Lock(this);

                a = aval switch
                {
                    < 0 => 0,
                    > 1 => 1,
                    _ => aval,
                };

                Unlock(this);
            }

            public void SetRgb(Vector3<float> rgb) {
                Lock(this);

                r = rgb.GetX();
                g = rgb.GetY();
                b = rgb.GetZ();

                Unlock(this);
            }

            public void SetRgba(Vector4<float> rgba) {
                Lock(this);

                r = rgba.GetX();
                g = rgba.GetY();
                b = rgba.GetZ();
                a = rgba.GetW();

                Unlock(this);
            }

        #endregion
    }
}
