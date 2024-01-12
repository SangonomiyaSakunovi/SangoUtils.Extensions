#if UNITY_ENV
using UnityEngine;
#endif

namespace SangoUtils_FixedNum
{
    public class FixedVector3
    {
        public FixedInt X { get; set; }
        public FixedInt Y { get; set; }
        public FixedInt Z { get; set; }
        public FixedVector3(FixedInt x, FixedInt y, FixedInt z)
        {
            X = x;
            Y = y;
            Z = z;
        }


#if UNITY_ENV
        public FixedVector3(Vector3 v)
        {
            X = (FixedInt)v.x;
            Y = (FixedInt)v.y;
            Z = (FixedInt)v.z;
        }
#endif

        public FixedInt this[int index]
        {
            get => index switch
            {
                0 => X,
                1 => Y,
                2 => Z,
                _ => (FixedInt)0,
            };
            set
            {
                switch (index)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    case 2:
                        Z = value;
                        break;
                    default:
                        break;
                }
            }
        }
        #region Commons
        public static FixedVector3 Zero
        {
            get
            {
                return new FixedVector3(0, 0, 0);
            }
        }
        public static FixedVector3 One
        {
            get
            {
                return new FixedVector3(1, 1, 1);
            }
        }
        public static FixedVector3 Forward
        {
            get
            {
                return new FixedVector3(0, 0, 1);
            }
        }
        public static FixedVector3 Back
        {
            get
            {
                return new FixedVector3(0, 0, -1);
            }
        }
        public static FixedVector3 Left
        {
            get
            {
                return new FixedVector3(-1, 0, 0);
            }
        }
        public static FixedVector3 Right
        {
            get
            {
                return new FixedVector3(1, 0, 0);
            }
        }
        public static FixedVector3 Up
        {
            get
            {
                return new FixedVector3(0, 1, 0);
            }
        }
        public static FixedVector3 Down
        {
            get
            {
                return new FixedVector3(0, -1, 0);
            }
        }
        #endregion

        #region Operator
        public static FixedVector3 operator +(FixedVector3 v1, FixedVector3 v2)
        {
            FixedInt x = v1.X + v2.X;
            FixedInt y = v1.Y + v2.Y;
            FixedInt z = v1.Z + v2.Z;
            return new FixedVector3(x, y, z);
        }
        public static FixedVector3 operator -(FixedVector3 v1, FixedVector3 v2)
        {
            FixedInt x = v1.X - v2.X;
            FixedInt y = v1.Y - v2.Y;
            FixedInt z = v1.Z - v2.Z;
            return new FixedVector3(x, y, z);
        }
        public static FixedVector3 operator *(FixedVector3 v, FixedInt value)
        {
            FixedInt x = v.X * value;
            FixedInt y = v.Y * value;
            FixedInt z = v.Z * value;
            return new FixedVector3(x, y, z);
        }
        public static FixedVector3 operator *(FixedInt value, FixedVector3 v)
        {
            FixedInt x = v.X * value;
            FixedInt y = v.Y * value;
            FixedInt z = v.Z * value;
            return new FixedVector3(x, y, z);
        }
        public static FixedVector3 operator /(FixedVector3 v, FixedInt value)
        {
            FixedInt x = v.X / value;
            FixedInt y = v.Y / value;
            FixedInt z = v.Z / value;
            return new FixedVector3(x, y, z);
        }
        public static FixedVector3 operator -(FixedVector3 v)
        {
            FixedInt x = -v.X;
            FixedInt y = -v.Y;
            FixedInt z = -v.Z;
            return new FixedVector3(x, y, z);
        }

        public static bool operator ==(FixedVector3 v1, FixedVector3 v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }
        public static bool operator !=(FixedVector3 v1, FixedVector3 v2)
        {
            return v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z;
        }
        #endregion

        public FixedInt PowMagnitude
        {
            get
            {
                return X * X + Y * Y + Z * Z;
            }
        }

        public static FixedInt Pow(FixedVector3 v)
        {
            return v.X * v.X + v.Y * v.Y + v.Z * v.Z;
        }

        public FixedInt Magnitude
        {
            get
            {
                return FixedNumCalc.Sqrt(this.PowMagnitude);
            }
        }

        public FixedVector3 Normalized
        {
            get
            {
                if (Magnitude > 0)
                {
                    FixedInt rate = FixedInt.ONE / Magnitude;
                    return new FixedVector3(X * rate, Y * rate, Z * rate);
                }
                else
                {
                    return Zero;
                }
            }
        }

        public static FixedVector3 Normalize(FixedVector3 v)
        {
            if (v.Magnitude > 0)
            {
                FixedInt rate = FixedInt.ONE / v.Magnitude;
                return new FixedVector3(v.X * rate, v.Y * rate, v.Z * rate);
            }
            else
            {
                return Zero;
            }
        }

        public void Normalize()
        {
            FixedInt rate = FixedInt.ONE / Magnitude;
            X *= rate;
            Y *= rate;
            Z *= rate;
        }

        public static FixedInt Dot(FixedVector3 a, FixedVector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public static FixedVector3 Cross(FixedVector3 a, FixedVector3 b)
        {
            return new FixedVector3(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }

        public static FixedNumArgs Angle(FixedVector3 from, FixedVector3 to)
        {
            FixedInt dot = Dot(from, to);
            FixedInt mod = from.Magnitude * to.Magnitude;
            if (mod == 0)
            {
                return FixedNumArgs.Zero;
            }
            FixedInt value = dot / mod;
            //反余弦函数计算
            return FixedNumCalc.Acos(value);
        }

#if UNITY_ENV
        public Vector3 ConvertToVector3()
        {
            return new Vector3(X.RawFloat, Y.RawFloat, Z.RawFloat);
        }
#endif

        public long[] CovertToLongArray()
        {
            return new long[] { X.ScaledValue, Y.ScaledValue, Z.ScaledValue };
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            FixedVector3 v = (FixedVector3)obj;
            return v.X == X && v.Y == Y && v.Z == Z;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("x:{0} y:{1} z:{2}", X, Y, Z);
        }
    }
}
