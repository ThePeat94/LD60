extends Node
## Autoload "CursorManager": shows the pointing hand while the mouse is over any interactable area.
## No class_name, GDScript does not allow a class named like its autoload.

var m_hoveredAreas: Dictionary[Area2D, bool] = {}


func Enter(area: Area2D) -> void:
	m_hoveredAreas[area] = true
	UpdateCursor()


func Exit(area: Area2D) -> void:
	m_hoveredAreas.erase(area)
	UpdateCursor()


func UpdateCursor() -> void:
	Input.set_default_cursor_shape(Input.CURSOR_POINTING_HAND if m_hoveredAreas.size() > 0 else Input.CURSOR_ARROW)
