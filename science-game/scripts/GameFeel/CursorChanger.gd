class_name CursorChanger
extends Node2D

@export var m_clickArea: Area2D
# filled from the CursorManager autoload in _ready
@export var m_cursorManager: Node


func _ready() -> void:
	m_cursorManager = get_node("/root/CursorManager")
	m_clickArea.mouse_entered.connect(func() -> void: m_cursorManager.Enter(m_clickArea))
	m_clickArea.mouse_exited.connect(func() -> void: m_cursorManager.Exit(m_clickArea))
