class_name FeelHoverScale
extends Control
## Grows a control a little while the mouse is over it, for buttons that have no hover image.

@export var m_hoverScale: float = 1.06
@export var m_duration: float = 0.1

var m_activeTween: Tween


func _ready() -> void:
	pivot_offset = size / 2.0
	resized.connect(func() -> void: pivot_offset = size / 2.0)
	mouse_entered.connect(func() -> void: TweenScale(m_hoverScale))
	mouse_exited.connect(func() -> void: TweenScale(1.0))
	# a hidden window keeps no stale hover size for next time
	visibility_changed.connect(func() -> void: scale = Vector2.ONE)


func TweenScale(target_scale: float) -> void:
	if m_activeTween != null:
		m_activeTween.kill()
	m_activeTween = create_tween()
	m_activeTween.tween_property(self, "scale", Vector2.ONE * target_scale, m_duration) \
		.set_trans(Tween.TRANS_QUAD) \
		.set_ease(Tween.EASE_OUT)
