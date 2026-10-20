class_name GnomeJumper
extends Sprite2D
## A gnome that hops back and forth between its spot and a second point, one hop per action.

@export var m_jumpOffset: Vector2 = Vector2(6.0, 0.0)
@export var m_jumpHeight: float = 4.0
@export var m_jumpDuration: float = 0.25
@export var m_jumpSound: AudioStreamPlayer2D

var m_home: Vector2
var m_atHome := true
var m_activeTween: Tween


func _ready() -> void:
	m_home = position


func Jump() -> void:
	if not is_visible_in_tree():
		return

	# a new action mid-hop: land the current hop right away, then hop on
	if m_activeTween != null:
		m_activeTween.kill()
	var from := m_home if m_atHome else m_home + m_jumpOffset
	m_atHome = not m_atHome
	var to := m_home if m_atHome else m_home + m_jumpOffset
	position = from

	var hop := func(t: float) -> void:
		var arc := 4.0 * t * (1.0 - t)
		position = from.lerp(to, t) + Vector2.UP * arc * m_jumpHeight
	m_activeTween = create_tween()
	m_activeTween.tween_method(hop, 0.0, 1.0, m_jumpDuration)

	if m_jumpSound != null:
		m_jumpSound.play()
