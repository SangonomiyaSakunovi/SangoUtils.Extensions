using System;

namespace SangoUtils.FixedNum
{
    public struct FixedInt
    {
        public long ScaledValue { get; set; }

        private const int BIT_MOVE_COUNT = 10;
        private const long MULTIPLIER_FACTOR = 1 << BIT_MOVE_COUNT;

        public static readonly FixedInt ZERO = new FixedInt(0);
        public static readonly FixedInt ONE = new FixedInt(1);

        private FixedInt(long scaledValue)
        {
            ScaledValue = scaledValue;
        }
        public FixedInt(int val)
        {
            ScaledValue = val * MULTIPLIER_FACTOR;
        }
        public FixedInt(float val)
        {
            ScaledValue = (long)Math.Round(val * MULTIPLIER_FACTOR);
        }
        public static explicit operator FixedInt(float f)
        {
            return new FixedInt((long)Math.Round(f * MULTIPLIER_FACTOR));
        }
        public static implicit operator FixedInt(int i)
        {
            return new FixedInt(i);
        }

        #region Operator
        public static FixedInt operator +(FixedInt a, FixedInt b)
        {
            return new FixedInt(a.ScaledValue + b.ScaledValue);
        }
        public static FixedInt operator -(FixedInt a, FixedInt b)
        {
            return new FixedInt(a.ScaledValue - b.ScaledValue);
        }
        public static FixedInt operator *(FixedInt a, FixedInt b)
        {
            long value = a.ScaledValue * b.ScaledValue;
            if (value >= 0)
            {
                value >>= BIT_MOVE_COUNT;
            }
            else
            {
                value = -(-value >> BIT_MOVE_COUNT);
            }
            return new FixedInt(value);
        }
        public static FixedInt operator /(FixedInt a, FixedInt b)
        {
            if (b.ScaledValue == 0)
            {
                throw new Exception();
            }
            return new FixedInt((a.ScaledValue << BIT_MOVE_COUNT) / b.ScaledValue);
        }
        public static FixedInt operator -(FixedInt value)
        {
            return new FixedInt(-value.ScaledValue);
        }
        public static bool operator ==(FixedInt a, FixedInt b)
        {
            return a.ScaledValue == b.ScaledValue;
        }
        public static bool operator !=(FixedInt a, FixedInt b)
        {
            return a.ScaledValue != b.ScaledValue;
        }
        public static bool operator >(FixedInt a, FixedInt b)
        {
            return a.ScaledValue > b.ScaledValue;
        }
        public static bool operator <(FixedInt a, FixedInt b)
        {
            return a.ScaledValue < b.ScaledValue;
        }
        public static bool operator >=(FixedInt a, FixedInt b)
        {
            return a.ScaledValue >= b.ScaledValue;
        }
        public static bool operator <=(FixedInt a, FixedInt b)
        {
            return a.ScaledValue <= b.ScaledValue;
        }

        public static FixedInt operator >>(FixedInt value, int moveCount)
        {
            if (value.ScaledValue >= 0)
            {
                return new FixedInt(value.ScaledValue >> moveCount);
            }
            else
            {
                return new FixedInt(-(-value.ScaledValue >> moveCount));
            }
        }
        public static FixedInt operator <<(FixedInt value, int moveCount)
        {
            return new FixedInt(value.ScaledValue << moveCount);
        }
        #endregion

        public readonly float RawFloat
        {
            get
            {
                return ScaledValue * 1.0f / MULTIPLIER_FACTOR;
            }
        }

        public readonly int RawInt
        {
            get
            {
                if (ScaledValue >= 0)
                {
                    return (int)(ScaledValue >> BIT_MOVE_COUNT);
                }
                else
                {
                    return -(int)(-ScaledValue >> BIT_MOVE_COUNT);
                }
            }
        }

        public override readonly bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            FixedInt vInt = (FixedInt)obj;
            return ScaledValue == vInt.ScaledValue;
        }

        public override readonly int GetHashCode()
        {
            return ScaledValue.GetHashCode();
        }

        public override readonly string ToString()
        {
            return RawFloat.ToString();
        }
    }
}
