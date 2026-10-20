class_name IdleNumberResourceController
extends RefCounted

signal ResourceValueChanged(new_value: IdleNumber)

var CurrentValue: IdleNumber = IdleNumber.new()
var SessionEarnings: IdleNumber = IdleNumber.new()


func Add(value: IdleNumber) -> void:
	if value.Value < 0.0:
		push_error("%s is less than 0" % value)
		return

	CurrentValue = CurrentValue.Add(value)
	SessionEarnings = SessionEarnings.Add(value)
	ResourceValueChanged.emit(CurrentValue)


func CanAfford(amount: IdleNumber) -> bool:
	if amount.Value < 0.0:
		push_error("%s is less than 0" % amount)
		return false

	return amount.Le(CurrentValue)


func ResetValues() -> void:
	CurrentValue = IdleNumber.new()
	SessionEarnings = IdleNumber.new()
	ResourceValueChanged.emit(CurrentValue)


func UseResource(amount: IdleNumber) -> void:
	if amount.Value < 0.0:
		push_error("%s is less than 0" % amount)
		return

	if amount.Gt(CurrentValue):
		push_error("%s is greater than %s" % [amount, CurrentValue])
		return

	CurrentValue = CurrentValue.Sub(amount)
	ResourceValueChanged.emit(CurrentValue)


func SetValue(value: IdleNumber) -> void:
	if value.Value < 0.0:
		push_error("%s is less than 0" % value)
		return
	CurrentValue = value
	ResourceValueChanged.emit(CurrentValue)
