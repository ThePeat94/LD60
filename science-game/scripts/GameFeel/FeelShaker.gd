class_name FeelShaker
extends Node

@export var m_toShake: Node2D

var m_originalPosition: Vector2

var m_activeTween: Tween


func _enter_tree() -> void:
	m_originalPosition = m_toShake.position


func OnClick(_viewport: Node, evt: InputEvent, _shape_idx: int) -> void:
	var button := evt as InputEventMouseButton
	if button != null and button.pressed and button.button_index == MOUSE_BUTTON_LEFT:
		Shake()


func Shake() -> void:
	if m_activeTween != null:
		m_activeTween.kill()
	m_toShake.position = m_originalPosition
	m_activeTween = create_tween()
	m_activeTween.tween_property(m_toShake, "position:x", 4.0, 0.04).as_relative()
	m_activeTween.tween_property(m_toShake, "position:x", -8.0, 0.08).as_relative()
	m_activeTween.tween_property(m_toShake, "position:x", 4.0, 0.04).as_relative()
