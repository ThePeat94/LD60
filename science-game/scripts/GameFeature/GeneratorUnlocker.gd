class_name GeneratorUnlocker
extends Node

@export var m_curtains: Sprite2D
@export var m_clickArea: Area2D
@export var m_otherUnlocks: Array[Node2D]


func _ready() -> void:
	m_curtains.visible = true
	m_clickArea.input_pickable = false

	for unlock in m_otherUnlocks:
		unlock.visible = false


func Unlock() -> void:
	m_curtains.visible = false
	m_clickArea.input_pickable = true
	for unlock in m_otherUnlocks:
		unlock.visible = true
