class_name GeneratorProgressBar
extends Node2D

@export var m_generator: GeneratorController
@export var m_bar: TextureProgressBar
@export var m_filledPopup: FeelPopupLabel
@export var m_countLabel: Label
@export var m_fillDuration: float = 0.15

var m_activeTween: Tween


func _ready() -> void:
	m_generator.Required.ResourceController.ResourceValueChanged.connect(OnGeneratorResourceChanged)
	m_generator.GeneratedReward.connect(OnGeneratorFilled)
	m_bar.value = GetProgress()
	UpdateCountLabel()


func OnGeneratorResourceChanged(_new_value: IdleNumber) -> void:
	var target := GetProgress()
	UpdateCountLabel()

	if m_activeTween != null:
		m_activeTween.kill()
	# a batch was paid out: restart from empty instead of draining backwards
	if target < m_bar.value:
		m_bar.value = 0

	m_activeTween = create_tween()
	m_activeTween.tween_property(m_bar, "value", target, m_fillDuration)


func OnGeneratorFilled() -> void:
	if m_filledPopup != null:
		m_filledPopup.Trigger()


func UpdateCountLabel() -> void:
	if m_countLabel == null:
		return

	m_countLabel.text = "%s/%s" % [m_generator.Required.ResourceController.CurrentValue, m_generator.Requirement]


func GetProgress() -> float:
	var requirement := m_generator.Requirement
	if requirement.Value <= 0.0:
		return 0.0

	return m_generator.Required.ResourceController.CurrentValue.GetRelativeAmountTo(requirement)
