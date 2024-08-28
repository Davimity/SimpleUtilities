using SimpleUtilities.Threading;

namespace SimpleUtilities.Utilities.Colors{
    /// <summary>Class that represents a color with RGBA values between 0 and 1.</summary>
    ///<remarks>THREAD SAFE</remarks>
    public class Color{

        #region Static Colors

            public static Color White = new Color(1, 1, 1);
            public static Color Black = new Color(0, 0, 0);
            public static Color Red = new Color(1, 0, 0);
            public static Color Green = new Color(0, 1, 0);
            public static Color Blue = new Color(0, 0, 1);
            public static Color Yellow = new Color(1, 1, 0);
            public static Color Cyan = new Color(0, 1, 1);
            public static Color Magenta = new Color(1, 0, 1);
            public static Color Gray = new Color(0.5f, 0.5f, 0.5f);

        #endregion

        #region Variables

            private object lockObject;

            private float r, g, b, a;

        #endregion

        #region Properties

            public float R{
                get{
                    using (new SimpleLock(lockObject)){
                        return r;
                    }
                }
                set{
                    using (new SimpleLock(lockObject)){
                        if (value < 0) r = 0;
                        else if(value > 1) r = 1;
                        else r = value;
                    }
                }
            }
            public float G{
                get{
                    using (new SimpleLock(lockObject)){
                        return g;
                    }
                }
                set{
                    using (new SimpleLock(lockObject)){
                        if (value < 0) g = 0;
                        else if(value > 1) g = 1;
                        else g = value;
                    }
                }
            }
            public float B{
                get{
                    using (new SimpleLock(lockObject)){
                        return b;
                    }
                }
                set{
                    using (new SimpleLock(lockObject)){
                        if (value < 0) b = 0;
                        else if(value > 1) b = 1;
                        else b = value;
                    }
                }
            }
            public float A{
                get{
                    using (new SimpleLock(lockObject)){
                        return a;
                    }
                }
                set{
                    using (new SimpleLock(lockObject)){
                        if (value < 0) a = 0;
                        else if(value > 1) a = 1;
                        else a = value;
                    }
                }
            }

            public Geometry.Vectors.Vector3<float> RGB{
                get{
                    using (new SimpleLock(lockObject)){
                        return new Geometry.Vectors.Vector3<float>(r, g, b);
                    }
                }
                set{
                    using (new SimpleLock(lockObject)){
                        r = value.X;
                        g = value.Y;
                        b = value.Z;
                    }
                }
            }
            public Geometry.Vectors.Vector4<float> RGBA{
                get{
                    using (new SimpleLock(lockObject)){
                        return new Geometry.Vectors.Vector4<float>(r, g, b, a);
                    }
                }
                set{
                    using (new SimpleLock(lockObject)){
                        r = value.X;
                        g = value.Y;
                        b = value.Z;
                        a = value.W;
                    }
                }
            }

        #endregion

        #region Constructors

            public Color(float r, float g, float b){
                this.r = r;
                this.g = g;
                this.b = b;
                a = 1;

                lockObject = new object();
            }

            public Color(float r, float g, float b, float a){
                this.r = r;
                this.g = g;
                this.b = b;
                this.a = a;

                lockObject = new object();
            }

        #endregion

        #region Operators

            public static implicit operator Geometry.Vectors.Vector4<float>(Color color) {
                return color.RGBA;
            }

        #endregion

        #region Methods

        public string ToHex(){
                using (new SimpleLock(lockObject)){
                    return $"#{(int)(r * 255):X2}{(int)(g * 255):X2}{(int)(b * 255):X2}";
                }
            }

            public Color Blend(params Color[] color){

                object[] locks = new object[color.Length];

                for(int i = 0; i < color.Length; i++) locks[i] = color[i].lockObject;

                using (new SimpleLock(lockObject, locks)){

                    float r = this.r;
                    float g = this.g;
                    float b = this.b;
                    float a = this.a;

                    foreach (Color c in color){
                        r += c.r;
                        g += c.g;
                        b += c.b;
                        a += c.a;
                    }

                    r /= color.Length;
                    g /= color.Length;
                    b /= color.Length;
                    a /= color.Length;

                    return new Color(r, g, b, a);
                }
            }

            public override string ToString(){
                using (new SimpleLock(lockObject)){
                    return $"R: {r}, G: {g}, B: {b}, A: {a}";
                }
            }

            public override int GetHashCode(){
                using (new SimpleLock(lockObject)){
                    int hash = 17;
                    hash = hash * 23 + r.GetHashCode() * 255;
                    hash = hash * 23 + g.GetHashCode() * 255;
                    hash = hash * 23 + b.GetHashCode() * 255;
                    hash = hash * 23 + a.GetHashCode() * 255;
                    return hash;
                }
            }

            public override bool Equals(object? obj){
                using (new SimpleLock(lockObject)){
                    if (obj == null || GetType() != obj.GetType()) return false;
                    Color color = (Color)obj;
                    return r == color.r && g == color.g && b == color.b && a == color.a;
                }
            }

        #endregion
    }
}
