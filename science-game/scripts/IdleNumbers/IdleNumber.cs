
using System;

namespace Nidavellir.IdleNumbers
{
    /// <summary>
    /// Represents big idle numbers in engineering notation.
    /// The value is calculated by Value * 10^Exponent.
    /// </summary>
    public readonly struct IdleNumber(double value, int exponent = 0)
    {
        private const int MAX_MAGNITUDE = 12;
        private const int TEN_CUBED = 1000;

        private readonly (double Mantissa, int Exponent) m_normalized =
            Normalize(value, exponent);

        public double Value => this.m_normalized.Mantissa;
        public int Exponent => this.m_normalized.Exponent;

        private static (double Mantissa, int Exponent) Normalize(double value, int exponent)
        {
            if (!double.IsFinite(value))
                throw new ArgumentOutOfRangeException(
                    nameof(value), "Value must be finite.");

            if (value == 0)
                return (0, 0);

            long exp = exponent;
            var val = value;

            while (Math.Abs(val) >= TEN_CUBED)
            {
                val /= TEN_CUBED;
                exp += 3;
            }

            while (Math.Abs(val) < 1)
            {
                val *= TEN_CUBED;
                exp -= 3;
            }

            // Ensure exponent is a multiple of 3.
            var remainder = (exp % 3 + 3) % 3;

            if (remainder != 0)
            {
                val *= Math.Pow(10, remainder);
                exp -= remainder;

                if (Math.Abs(val) >= TEN_CUBED)
                {
                    val /= TEN_CUBED;
                    exp += 3;
                }
            }

            if (exp is < int.MinValue or > int.MaxValue)
                throw new OverflowException(
                    "Exponent exceeds IdleNumber range.");

            return (val, (int)exp);
        }

        // Returns the mantissa aligned to the target exponent.
        // Does not construct a new IdleNumber, as that would
        // immediately normalize the result again.
        private double AlignValue(int targetExponent)
        {
            var delta = (long)targetExponent - this.Exponent;

            return delta switch
            {
                < 0 => throw new ArgumentOutOfRangeException(nameof(targetExponent)),
                > MAX_MAGNITUDE => 0,
                _ => this.Value / Math.Pow(10, delta)
            };
        }

        private int CompareTo(IdleNumber other)
        {
            var leftSign = Math.Sign(this.Value);
            var rightSign = Math.Sign(other.Value);

            if (leftSign != rightSign)
                return leftSign.CompareTo(rightSign);

            if (leftSign == 0)
                return 0;

            var exponentComparison = this.Exponent.CompareTo(other.Exponent);

            if (exponentComparison != 0)
                return exponentComparison * leftSign;

            return this.Value.CompareTo(other.Value);
        }

        public IdleNumber Pow(IdleNumber exponent)
        {
            if (this.Value <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(this.Value),
                    "Only positive bases are supported.");

            if (exponent.Value == 0)
                return new IdleNumber(1);

            var logBase = Math.Log10(this.Value) + this.Exponent;

            if (logBase == 0)
                return new IdleNumber(1);

            var power = exponent.Value *
                        Math.Pow(10, exponent.Exponent);

            var logResult = power * logBase;

            if (!double.IsFinite(logResult) ||
                logResult <= int.MinValue + 1 ||
                logResult >= int.MaxValue - 1)
                throw new OverflowException(
                    "Exponent exceeds IdleNumber range.");

            var resultExponent = (int)Math.Floor(logResult);
            var mantissa = Math.Pow(
                10, logResult - resultExponent);

            return new IdleNumber(mantissa, resultExponent);
        }

        public double GetRelativeAmountTo(IdleNumber other)
        {
            if (other.Value == 0)
                throw new DivideByZeroException();

            var exponentDifference =
                (long)this.Exponent - other.Exponent;

            return (this.Value / other.Value) *
                   Math.Pow(10, exponentDifference);
        }

        public double ToDouble()
        {
            return this.Value * Math.Pow(10, this.Exponent);
        }

        public static IdleNumber operator +(
            IdleNumber left, IdleNumber right)
        {
            if (left.Value == 0) return right;
            if (right.Value == 0) return left;

            var exp = Math.Max(left.Exponent, right.Exponent);
            var val = left.AlignValue(exp) +
                      right.AlignValue(exp);

            return new IdleNumber(val, exp);
        }

        public static IdleNumber operator -(
            IdleNumber left, IdleNumber right)
        {
            if (right.Value == 0) return left;

            if (left.Value == 0)
                return new IdleNumber(
                    -right.Value, right.Exponent);

            var exp = Math.Max(left.Exponent, right.Exponent);
            var val = left.AlignValue(exp) -
                      right.AlignValue(exp);

            return new IdleNumber(val, exp);
        }

        public static IdleNumber operator /(
            IdleNumber number, double divisor)
        {
            return divisor switch
            {
                0 => throw new DivideByZeroException(),
                < 0 => throw new ArithmeticException(),
                _ => new IdleNumber(number.Value / divisor, number.Exponent)
            };
        }

        public static bool operator >(
            IdleNumber left, IdleNumber right)
            => left.CompareTo(right) > 0;

        public static bool operator >=(
            IdleNumber left, IdleNumber right)
            => left.CompareTo(right) >= 0;

        public static bool operator <(
            IdleNumber left, IdleNumber right)
            => left.CompareTo(right) < 0;

        public static bool operator <=(
            IdleNumber left, IdleNumber right)
            => left.CompareTo(right) <= 0;

        public static IdleNumber operator *(
            IdleNumber number, double factor)
        {
            if (factor <= 0)
                throw new ArithmeticException();

            return new IdleNumber(
                number.Value * factor,
                number.Exponent);
        }

        public static IdleNumber operator *(
            double factor, IdleNumber number)
            => number * factor;

        public static IdleNumber operator *(
            IdleNumber left, IdleNumber right)
        {
            return new IdleNumber(
                left.Value * right.Value,
                left.Exponent + right.Exponent);
        }

        public override string ToString()
        {
            return IdleNumberFormatter.FormatIdleNumber(this);
        }
    }
}
