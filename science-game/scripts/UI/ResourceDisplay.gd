class_name ResourceDisplay
extends Label

@export var ResourceControllerNode: ResourceControllerNode


func _ready() -> void:
	ResourceControllerNode.ResourceController.ResourceValueChanged.connect(OnValueChanged)
	UpdateText(ResourceControllerNode.ResourceController.CurrentValue)


func OnValueChanged(new_value: IdleNumber) -> void:
	UpdateText(new_value)


func UpdateText(value: IdleNumber) -> void:
	text = str(value)
