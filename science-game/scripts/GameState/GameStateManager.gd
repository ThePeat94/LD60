class_name GameStateManager
extends Node

@export var m_musicPlayer: MusicPlayer
@export var m_employeeController: EmployeeController

var m_currentProgressionState: Enums.GameProgressionState = Enums.GameProgressionState.Beginning


func _ready() -> void:
	m_employeeController.EmployeeBought.connect(OnEmployeeBought)


func OnEmployeeBought() -> void:
	# advance only once: the music has no clip after the midgame one
	if m_currentProgressionState != Enums.GameProgressionState.Beginning:
		return
	if m_employeeController.Inventors.size() > 0 and m_employeeController.Scribes.size() > 0 and m_employeeController.Thinkers.size() > 0:
		m_musicPlayer.Advance()
		m_currentProgressionState = Enums.GameProgressionState.Midgame
