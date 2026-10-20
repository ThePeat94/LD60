class_name IdleNumberFormatter
extends RefCounted

const UNIT_MAP := ["", "K", "M", "B", "T"]
const CHAR_A := 97 # 'a'


static func FormatIdleNumber(idle_number: IdleNumber) -> String:
	if idle_number.Exponent >= 0:
		return _FormatTwoDecimals(floorf(idle_number.Value * 100.0) / 100.0) + GetUnitForIdleNumber(idle_number)

	var plain := idle_number.Value * pow(10.0, idle_number.Exponent)
	# C# rounds 0.995 half away from zero to "1", while "%.2f" sees the binary 0.99499... and gives "0.99"
	return "0" if plain < 0.01 else _FormatTwoDecimals(roundf(plain * 100.0) / 100.0)


@warning_ignore("integer_division")
static func GetUnitForIdleNumber(idle_number: IdleNumber) -> String:
	var magnitude := idle_number.Exponent / 3
	if magnitude < 0:
		return ""
	if magnitude < UNIT_MAP.size():
		return UNIT_MAP[magnitude]

	var unit_int := magnitude - UNIT_MAP.size()
	var second_unit := unit_int % 26
	var first_unit := unit_int / 26
	return char(first_unit + CHAR_A) + char(second_unit + CHAR_A)


# Same as C#'s ToString("0.##"): at most two decimals, no trailing zeros.
static func _FormatTwoDecimals(value: float) -> String:
	var text := "%.2f" % value
	if text.contains("."):
		text = text.rstrip("0").rstrip(".")
	if text == "-0":
		text = "0"
	return text
