class_name Employee
extends Node

@export var m_data: EmployeeData
@export var m_timer: Timer

## Anything with Generate(power: IdleNumber, origin = null) -> bool (GeneratorController or ScienceGenerator).
var m_targetGenerator

## The gnome doing this employee's work right now, if one is visible. Returns a GnomeJumper or null.
var GnomeProvider: Callable
var m_employeeStats: EmployeeStats


func _enter_tree() -> void:
	m_timer.autostart = false


func _ready() -> void:
	m_timer.wait_time = m_employeeStats.TickSpeed.Value.ToDouble()
	m_timer.timeout.connect(OnTimerTick)
	m_timer.start()


func Initialize(patent_generator: Node2D, stats: EmployeeStats) -> void:
	m_employeeStats = stats
	m_targetGenerator = patent_generator


func OnTimerTick() -> void:
	# one action = one jump, and the action's symbols start at the gnome
	var gnome: GnomeJumper = GnomeProvider.call() if GnomeProvider.is_valid() else null
	var origin: Variant = gnome.global_position if gnome != null else null
	if m_targetGenerator.Generate(m_employeeStats.WorkForce.Value, origin) and gnome != null:
		gnome.Jump()
	m_timer.wait_time = m_employeeStats.TickSpeed.Value.ToDouble()
