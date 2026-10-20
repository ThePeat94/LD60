class_name LinearModelResource
extends MathModelResource

## b
@export var Base: IdleNumberResource

## a
@export var Slope: IdleNumberResource


## f(x) = a * x + b
func CalculateFor(value: IdleNumber) -> IdleNumber:
	return Slope.ToIdleNumber().Mul(value).Add(Base.ToIdleNumber())
