class_name SkillTreeNodeViewData
extends RefCounted

var Cost: IdleNumber
var IsUnlocked: bool
var IsAvailable: bool
var CanBePurchased: bool
var CurrentAmount: int


func _init(cost: IdleNumber, unlocked: bool, is_available: bool, can_be_purchased: bool, amount: int) -> void:
	Cost = cost
	IsUnlocked = unlocked
	IsAvailable = is_available
	CanBePurchased = can_be_purchased
	CurrentAmount = amount
