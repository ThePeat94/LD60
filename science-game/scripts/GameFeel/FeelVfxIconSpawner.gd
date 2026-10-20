class_name FeelVfxIconSpawner
extends Node

@export var m_origin: Node2D
@export var m_particles: PackedScene
@export var m_maxParticles: int = 50


func OnClick(_viewport: Node, evt: InputEvent, _shape_idx: int) -> void:
	var button := evt as InputEventMouseButton
	if button == null or not button.pressed or button.button_index != MOUSE_BUTTON_LEFT:
		return

	Spawn()


## One particle per produced unit, so stronger clicks burst harder.
func SpawnAmount(amount: float) -> void:
	Spawn(int(clampf(roundf(amount), 1, m_maxParticles)))


## amount 0 keeps the particle count of the scene.
func Spawn(amount: int = 0) -> void:
	var mouse_pos := m_origin.get_global_mouse_position()
	var instantiated: GPUParticles2D = m_particles.instantiate()
	if amount > 0:
		instantiated.amount = amount
	get_tree().current_scene.add_child(instantiated)
	instantiated.global_position = mouse_pos
	instantiated.restart()
	instantiated.finished.connect(instantiated.queue_free)
