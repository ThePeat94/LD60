class_name FeelGlow
extends Sprite2D
## A glow that fades in when active and gently pulses while it is shown.

@export var m_fadeDuration: float = 0.3
@export var m_pulseSpeed: float = 3.0
@export_range(0, 1) var m_pulseMinAlpha: float = 0.55

var m_active: bool
var m_strength: float
var m_time: float


func _ready() -> void:
	visible = false


func SetActive(active: bool) -> void:
	m_active = active


func _process(delta: float) -> void:
	m_time += delta
	m_strength = move_toward(m_strength, 1.0 if m_active else 0.0, delta / m_fadeDuration)

	visible = m_strength > 0.0
	if not visible:
		return

	var pulse := 0.5 + 0.5 * sin(m_time * m_pulseSpeed)
	modulate.a = m_strength * lerpf(m_pulseMinAlpha, 1.0, pulse)
