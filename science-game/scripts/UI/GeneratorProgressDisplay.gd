class_name GeneratorProgressDisplay
extends ProgressBar

@export var m_generator: GeneratorController


func _ready() -> void:
	m_generator.Required.ResourceController.ResourceValueChanged.connect(OnGeneratorResourceChanged)


func OnGeneratorResourceChanged(_new_value: IdleNumber) -> void:
	value = m_generator.Required.ResourceController.CurrentValue.GetRelativeAmountTo(m_generator.Requirement)
