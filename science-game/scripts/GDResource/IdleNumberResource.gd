class_name IdleNumberResource
extends Resource

@export var Value: float = 1.0

@export var Exponent: int = 0


func ToIdleNumber() -> IdleNumber:
	return IdleNumber.new(Value, Exponent)
