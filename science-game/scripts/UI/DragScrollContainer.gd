class_name DragScrollContainer
extends ScrollContainer
## A ScrollContainer that can also be panned by holding the left mouse button and dragging,
## including when the drag starts on top of a child (like a skill node or a buy button).

@export var m_dragThreshold: float = 8.0

var m_pressed: bool
var m_dragged: bool
var m_pressPosition: Vector2
var m_pressScroll: Vector2

## Whether the click that is ending right now was a drag, so children can ignore it.
## Only meaningful while the mouse release is being handled.
var WasDragged: bool:
	get:
		return m_dragged


## True if node sits in a drag surface (DragScrollContainer or PanZoomView) whose current click was a drag.
static func IsDragClick(node: Node) -> bool:
	var parent := node.get_parent()
	while parent != null:
		if parent is DragScrollContainer:
			return (parent as DragScrollContainer).WasDragged
		if parent is PanZoomView:
			return (parent as PanZoomView).WasDragged
		parent = parent.get_parent()
	return false


# _input sees the mouse before any child control does, so drags work on top of children too
func _input(evt: InputEvent) -> void:
	if not is_visible_in_tree():
		m_pressed = false
		return

	var mouse := get_global_mouse_position()

	var button := evt as InputEventMouseButton
	if button != null and button.button_index == MOUSE_BUTTON_LEFT:
		if button.pressed:
			m_pressed = get_global_rect().has_point(mouse)
			m_dragged = false
			m_pressPosition = mouse
			m_pressScroll = Vector2(scroll_horizontal, scroll_vertical)
		elif m_pressed:
			m_pressed = false
			# keep WasDragged true while children handle this release, then reset
			if m_dragged:
				ResetDrag.call_deferred()
		return

	if not (evt is InputEventMouseMotion) or not m_pressed:
		return

	var offset := mouse - m_pressPosition
	if not m_dragged and offset.length() < m_dragThreshold:
		return

	m_dragged = true
	scroll_horizontal = int(m_pressScroll.x - offset.x)
	scroll_vertical = int(m_pressScroll.y - offset.y)


func ResetDrag() -> void:
	m_dragged = false
