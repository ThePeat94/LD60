class_name IdleNumber
extends RefCounted
## Represents big idle numbers in engineering notation.
## The value is calculated by Value * 10^Exponent.
## Immutable: every operation returns a new IdleNumber (GDScript has no operator overloading,
## so +, -, *, / and the comparisons are methods).

const MAX_MAGNITUDE := 12
const TEN_CUBED := 1000.0
const INT_MIN := -2147483648.0
const INT_MAX := 2147483647.0

## The mantissa, normalized to [1, 1000) (or 0).
var Value: float = 0.0
## Always a multiple of 3.
var Exponent: int = 0


func _init(value: float = 0.0, exponent: int = 0) -> void:
	if is_nan(value) or is_inf(value):
		push_error("IdleNumber: value must be finite, got %s." % value)
		return

	if value == 0.0:
		return

	var e := exponent
	var val := value

	while absf(val) >= TEN_CUBED:
		val /= TEN_CUBED
		e += 3

	while absf(val) < 1.0:
		val *= TEN_CUBED
		e -= 3

	# Ensure exponent is a multiple of 3.
	var remainder := posmod(e, 3)

	if remainder != 0:
		val *= pow(10.0, remainder)
		e -= remainder

		if absf(val) >= TEN_CUBED:
			val /= TEN_CUBED
			e += 3

	if e < INT_MIN or e > INT_MAX:
		push_error("IdleNumber: exponent exceeds IdleNumber range.")
		return

	Value = val
	Exponent = e


# Returns the mantissa aligned to the target exponent.
# Does not construct a new IdleNumber, as that would
# immediately normalize the result again.
func _AlignValue(target_exponent: int) -> float:
	var delta := target_exponent - Exponent

	if delta < 0:
		push_error("IdleNumber: can only align to a bigger exponent.")
		return Value
	if delta > MAX_MAGNITUDE:
		return 0.0
	return Value / pow(10.0, delta)


func CompareTo(other: IdleNumber) -> int:
	var left_sign := int(signf(Value))
	var right_sign := int(signf(other.Value))

	if left_sign != right_sign:
		return 1 if left_sign > right_sign else -1

	if left_sign == 0:
		return 0

	if Exponent != other.Exponent:
		return (1 if Exponent > other.Exponent else -1) * left_sign

	if Value == other.Value:
		return 0
	return 1 if Value > other.Value else -1


func Pow(exponent: IdleNumber) -> IdleNumber:
	if Value <= 0.0:
		push_error("IdleNumber.Pow: only positive bases are supported.")
		return IdleNumber.new()

	if exponent.Value == 0.0:
		return IdleNumber.new(1.0)

	var log_base := _Log10(Value) + Exponent

	if log_base == 0.0:
		return IdleNumber.new(1.0)

	var power := exponent.Value * pow(10.0, exponent.Exponent)

	var log_result := power * log_base

	if is_nan(log_result) or is_inf(log_result) or log_result <= INT_MIN + 1 or log_result >= INT_MAX - 1:
		push_error("IdleNumber.Pow: exponent exceeds IdleNumber range.")
		return IdleNumber.new()

	var result_exponent := int(floor(log_result))
	var mantissa := pow(10.0, log_result - result_exponent)

	return IdleNumber.new(mantissa, result_exponent)


func GetRelativeAmountTo(other: IdleNumber) -> float:
	if other.Value == 0.0:
		push_error("IdleNumber.GetRelativeAmountTo: division by zero.")
		return 0.0

	var exponent_difference := Exponent - other.Exponent

	return (Value / other.Value) * pow(10.0, exponent_difference)


func ToDouble() -> float:
	return Value * pow(10.0, Exponent)


## this + other
func Add(other: IdleNumber) -> IdleNumber:
	if Value == 0.0:
		return other
	if other.Value == 0.0:
		return self

	var e := maxi(Exponent, other.Exponent)
	var val := _AlignValue(e) + other._AlignValue(e)

	return IdleNumber.new(val, e)


## this - other
func Sub(other: IdleNumber) -> IdleNumber:
	if other.Value == 0.0:
		return self

	if Value == 0.0:
		return IdleNumber.new(-other.Value, other.Exponent)

	var e := maxi(Exponent, other.Exponent)
	var val := _AlignValue(e) - other._AlignValue(e)

	return IdleNumber.new(val, e)


## this / divisor
func DivFloat(divisor: float) -> IdleNumber:
	if divisor <= 0.0:
		push_error("IdleNumber.DivFloat: divisor must be positive.")
		return self
	return IdleNumber.new(Value / divisor, Exponent)


## this * factor
func MulFloat(factor: float) -> IdleNumber:
	if factor <= 0.0:
		push_error("IdleNumber.MulFloat: factor must be positive.")
		return self
	return IdleNumber.new(Value * factor, Exponent)


## this * other
func Mul(other: IdleNumber) -> IdleNumber:
	return IdleNumber.new(Value * other.Value, Exponent + other.Exponent)


## this > other
func Gt(other: IdleNumber) -> bool:
	return CompareTo(other) > 0


## this >= other
func Ge(other: IdleNumber) -> bool:
	return CompareTo(other) >= 0


## this < other
func Lt(other: IdleNumber) -> bool:
	return CompareTo(other) < 0


## this <= other
func Le(other: IdleNumber) -> bool:
	return CompareTo(other) <= 0


func _to_string() -> String:
	return IdleNumberFormatter.FormatIdleNumber(self)


# log(x) / log(10) is off by a rounding error for exact powers of ten (log10(1000) = 2.9999...),
# which would turn 1K into 999.99. Math.Log10 in C# is exact there, so snap those cases.
static func _Log10(x: float) -> float:
	var result := log(x) / log(10.0)
	var rounded := roundf(result)
	if absf(result - rounded) < 1e-9 and pow(10.0, rounded) == x:
		return rounded
	return result
