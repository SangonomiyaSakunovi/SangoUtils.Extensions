using System;

namespace SangoUtils_FixedNum
{
    public class FixedNumArgs
    {
        public int Value { get; set; }
        public uint Multipler { get; set; }

        public FixedNumArgs(int value, uint multipler)
        {
            Value = value;
            Multipler = multipler;
        }

        public static FixedNumArgs Zero = new FixedNumArgs(0, 10000);
        public static FixedNumArgs HALFPI = new FixedNumArgs(15708, 10000);
        public static FixedNumArgs PI = new FixedNumArgs(31416, 10000);
        public static FixedNumArgs TWOPI = new FixedNumArgs(62832, 10000);

        #region Operator
        public static bool operator >(FixedNumArgs a, FixedNumArgs b)
        {
            if (a.Multipler == b.Multipler)
            {
                return a.Value > b.Value;
            }
            else
            {
                throw new System.Exception("multipler is unequal.");
            }
        }
        public static bool operator <(FixedNumArgs a, FixedNumArgs b)
        {
            if (a.Multipler == b.Multipler)
            {
                return a.Value < b.Value;
            }
            else
            {
                throw new System.Exception("multipler is unequal.");
            }
        }
        public static bool operator >=(FixedNumArgs a, FixedNumArgs b)
        {
            if (a.Multipler == b.Multipler)
            {
                return a.Value >= b.Value;
            }
            else
            {
                throw new System.Exception("multipler is unequal.");
            }
        }
        public static bool operator <=(FixedNumArgs a, FixedNumArgs b)
        {
            if (a.Multipler == b.Multipler)
            {
                return a.Value <= b.Value;
            }
            else
            {
                throw new System.Exception("multipler is unequal.");
            }
        }
        public static bool operator ==(FixedNumArgs a, FixedNumArgs b)
        {
            if (a.Multipler == b.Multipler)
            {
                return a.Value == b.Value;
            }
            else
            {
                throw new System.Exception("multipler is unequal.");
            }
        }
        public static bool operator !=(FixedNumArgs a, FixedNumArgs b)
        {
            if (a.Multipler == b.Multipler)
            {
                return a.Value != b.Value;
            }
            else
            {
                throw new System.Exception("multipler is unequal.");
            }
        }
        #endregion

        public int ConvertViewAngle()
        {
            float radians = ConvertToRadian();
            return (int)Math.Round(radians / Math.PI * 180);
        }

        public float ConvertToRadian()
        {
            return Value * 1.0f / Multipler;
        }

        public override bool Equals(object obj)
        {
            return obj is FixedNumArgs args &&
                Value == args.Value &&
                Multipler == args.Multipler;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"value:{Value} multipler:{Multipler}";
        }
    }
}
