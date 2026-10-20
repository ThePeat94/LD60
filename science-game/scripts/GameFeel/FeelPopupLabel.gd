class_name FeelPopupLabel
extends Node

@export var m_origin: Node2D
@export var m_text: String = "Not enough Thinking done yet!"
@export var m_fontSize: int = 32
@export var m_riseDistance: float = 40.0
@export var m_duration: float = 1.2
@export var m_followMouse: bool = true
# e.g. a HUD CanvasLayer, so the label shows above UI; defaults to the current scene
@export var m_labelParent: Node

var m_label: Label
var m_activeTween: Tween


func Trigger() -> void:
	var label := GetOrCreateLabel()

	if m_activeTween != null:
		m_activeTween.kill()
	label.size = label.get_combined_minimum_size()
	# asking the label itself gives the mouse in the label's own canvas (world or HUD layer)
	var center := label.get_global_mouse_position() if m_followMouse else m_origin.global_position
	label.global_position = center - label.size / 2.0
	label.modulate = Color.WHITE
	label.visible = true

	m_activeTween = create_tween()
	m_activeTween.tween_property(label, "position:y", -m_riseDistance, m_duration).as_relative()
	m_activeTween.parallel().tween_property(label, "modulate:a", 0.0, m_duration * 0.5).set_delay(m_duration * 0.5)
	m_activeTween.tween_callback(func() -> void: label.visible = false)


func GetOrCreateLabel() -> Label:
	if m_label != null:
		return m_label

	m_label = Label.new()
	m_label.text = m_text
	m_label.horizontal_alignment = HORIZONTAL_ALIGNMENT_CENTER
	m_label.vertical_alignment = VERTICAL_ALIGNMENT_CENTER
	m_label.mouse_filter = Control.MOUSE_FILTER_IGNORE
	m_label.z_index = 20
	m_label.visible = false
	m_label.add_theme_font_size_override("font_size", m_fontSize)
	m_label.add_theme_color_override("font_color", Color.WHITE)
	m_label.add_theme_color_override("font_outline_color", Color.BLACK)
	m_label.add_theme_constant_override("outline_size", 7)
	(m_labelParent if m_labelParent != null else get_tree().current_scene).add_child(m_label)
	return m_label
