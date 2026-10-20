class_name CursorInstaller
extends Node


func _ready() -> void:
	var normal: Texture2D = load("res://assets/art/cursor/normal.png")
	var hover: Texture2D = load("res://assets/art/cursor/interactable.png")

	Input.set_custom_mouse_cursor(normal, Input.CURSOR_ARROW)
	Input.set_custom_mouse_cursor(hover, Input.CURSOR_POINTING_HAND)
