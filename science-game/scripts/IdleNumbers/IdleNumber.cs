using System;

namespace Nidavellir.IdleNumbers
{
    /// <summary>
    ///     This is an abstract representation of "big idle numbers" as in the scientific notification.
    ///     The value is calculated by Value * 10^Exponent
    /// </summary>
    public struct IdleNumber 
    {
        // Max exponent diff between idle numbers
        private const int MAX_MAGNITUDE = 12;

        // 10^3
        private const int TEN_CUBED = 1000;
        private double m_value;
        private int m_exponent;

        public IdleNumber(double value, int exponent)
        {
            this.m_value = value;
            this.m_exponent = exponent;
        }

        public IdleNumber(double value)
        {
            this.m_value = value;
            this.m_exponent = 0;
        }

        public double Value => this.m_value;
        public int Exponent => this.m_exponent;

        public IdleNumber Align(int targetExponent)
        {
            var delta = targetExponent - this.Exponent;
            if (delta > 0)
            {
                var value = delta <= MAX_MAGNITUDE ? this.Value / Math.Pow(10, delta) : 0;
                var exponent = targetExponent;
                return new IdleNumber(value, exponent);
            }

            return this;
        }

        public IdleNumber Normalize()
        {
            if (this.Value < 1 && this.Exponent != 0)
            {
                var calculatedNumber = this;
                while(calculatedNumber.Value < 1 && calculatedNumber.Exponent != 0)
                    calculatedNumber = new IdleNumber(calculatedNumber.Value * TEN_CUBED, calculatedNumber.Exponent - 3);

                return calculatedNumber;
            }

            if (this.Value >= TEN_CUBED)
            {
                var value = this.Value;
                var exponent = this.Exponent;

                while (Math.Abs(value) >= TEN_CUBED)
                {
                    value /= TEN_CUBED;
                    exponent += 3;
                }

                return new IdleNumber(value, exponent);
            }

            return this;
        }

        public IdleNumber Pow(IdleNumber exponent)
        {
            if (this.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(this.Value),
                    "Only positive bases are supported.");

            if (exponent.Value == 0)
                return new IdleNumber(1);

            // log10(base)
            var logBase = Math.Log10(this.Value) + Exponent;

            if (logBase == 0)
                return new IdleNumber(1);

            // Convert exponent to double
            var power = exponent.Value * Math.Pow(10, exponent.Exponent);

            // log10(base^power)
            var logResult = power * logBase;

            if (!double.IsFinite(logResult) ||
                logResult <= int.MinValue + 1 ||
                logResult >= int.MaxValue - 1)
                throw new OverflowException("Exponent exceeds IdleNumber range.");

            // Split into exponent and mantissa
            var resultExponent = (int)Math.Floor(logResult);
            var mantissa = Math.Pow(10, logResult - resultExponent);

            return new IdleNumber(mantissa, resultExponent).Normalize();
        }

        public static IdleNumber operator +(IdleNumber left, IdleNumber right)
        {
            var bigger = left.Exponent >= right.Exponent ? left : right;
            var smaller = left.Exponent >= right.Exponent ? right : left;

            var value = smaller.Align(bigger.Exponent).Value + bigger.Value;
            return new IdleNumber(value, bigger.Exponent).Normalize();
        }

        public static IdleNumber operator /(IdleNumber number, double divisor)
        {
            if (divisor == 0)
                throw new DivideByZeroException();

            if (divisor < 0)
                throw new ArithmeticException();

            var value = number.Value / divisor;
            return new IdleNumber(value, number.Exponent).Normalize();
        }

        public static bool operator >(IdleNumber left, IdleNumber right)
        {
            var alignedLeft = left.Exponent > right.Exponent ? left : left.Align(right.Exponent);
            var alignedRight = left.Exponent > right.Exponent ? right.Align(left.Exponent) : right;

            return alignedLeft.Value > alignedRight.Value;
        }

        public static bool operator >=(IdleNumber left, IdleNumber right)
        {
            var alignedLeft = left.Exponent > right.Exponent ? left : left.Align(right.Exponent);
            var alignedRight = left.Exponent > right.Exponent ? right.Align(left.Exponent) : right;

            return alignedLeft.Value >= alignedRight.Value;
        }

        public static bool operator <(IdleNumber left, IdleNumber right)
        {
            var alignedLeft = left.Exponent > right.Exponent ? left : left.Align(right.Exponent);
            var alignedRight = left.Exponent > right.Exponent ? right.Align(left.Exponent) : right;

            return alignedLeft.Value < alignedRight.Value;
        }

        public static bool operator <=(IdleNumber left, IdleNumber right)
        {
            var alignedLeft = left.Exponent > right.Exponent ? left : left.Align(right.Exponent);
            var alignedRight = left.Exponent > right.Exponent ? right.Align(left.Exponent) : right;

            return alignedLeft.Value <= alignedRight.Value;
        }

        public static IdleNumber operator *(IdleNumber number, double factor)
        {
            if (factor <= 0)
                throw new ArithmeticException();

            var value = number.Value * factor;
            return new IdleNumber(value, number.Exponent).Normalize();
        }

        public static IdleNumber operator *(double factor, IdleNumber number) => number * factor;

        public static IdleNumber operator *(IdleNumber left, IdleNumber right)
        {
            return new IdleNumber(left.Value * right.Value, left.Exponent + right.Exponent).Normalize();
        }
        
        public static IdleNumber operator -(IdleNumber left, IdleNumber right)
        {
            var alignedLeft = left.Exponent > right.Exponent ? left : left.Align(right.Exponent);
            var alignedRight = left.Exponent > right.Exponent ? right.Align(left.Exponent) : right;

            var value = alignedLeft.Value - alignedRight.Value;
            return new IdleNumber(value, alignedLeft.Exponent).Normalize();
        }

        public override string ToString()
        {
            return IdleNumberFormatter.FormatIdleNumber(this);
        }
    }
}