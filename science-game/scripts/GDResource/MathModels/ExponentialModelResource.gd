class_name ExponentialModelResource
extends MathModelResource

## b
@export var Base: IdleNumberResource

## a
@export var Slope: IdleNumberResource


## f(x) = b * a ^ x
func CalculateFor(value: IdleNumber) -> IdleNumber:
	return Base.ToIdleNumber().Mul(Slope.ToIdleNumber().Pow(value))
