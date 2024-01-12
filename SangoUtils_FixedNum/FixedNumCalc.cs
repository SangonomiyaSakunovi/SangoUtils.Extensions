using System;

namespace SangoUtils_FixedNum
{
    public class FixedNumCalc
    {
        public static FixedInt Sqrt(FixedInt value, int interatorCount = 8)
        {
            if (value == FixedInt.ZERO)
            {
                return 0;
            }
            if (value < FixedInt.ZERO)
            {
                throw new Exception();
            }

            FixedInt result = value;
            FixedInt history;
            int count = 0;
            do
            {
                history = result;
                result = (result + value / result) >> 1;
                ++count;
            } while (result != history && count < interatorCount);
            return result;
        }

        public static FixedNumArgs Acos(FixedInt value)
        {
            FixedInt rate = (value * ArcCosTable.HalfIndexCount) + ArcCosTable.HalfIndexCount;
            rate = Clamp(rate, FixedInt.ZERO, ArcCosTable.IndexCount);
            return new FixedNumArgs(ArcCosTable.table[rate.RawInt], ArcCosTable.Multipler);
        }


        public static FixedInt Clamp(FixedInt input, FixedInt min, FixedInt max)
        {
            if (input < min)
            {
                return min;
            }
            if (input > max)
            {
                return max;
            }
            return input;
        }
    }
}
