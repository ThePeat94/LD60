class_name PanZoomView
extends Control
## A clipped view on one big child: drag with the left mouse button to pan, mouse wheel to zoom towards the mouse.
## Unlike a ScrollContainer it never scrolls on the wheel.

@export var m_content: Control
@export var m_dragThreshold: float = 8.0
@export var m_maxZoom: float = 1.5
@export var m_zoomStep: float = 1.1
@export var m_zoomDuration: float = 0.08

var m_pressed: bool
var m_dragged: bool
var m_pressMouse: Vector2
var m_pressContentPosition: Vector2
var m_zoom: float = 1.0
var m_zoomTween: Tween

var WasDragged: bool:
	get:
		return m_dragged

# fully zoomed out shows the whole content
var MinZoom: float:
	get:
		return minf(1.0, minf(size.x / ContentSize.x, size.y / ContentSize.y))

var ContentSize: Vector2:
	get:
		return m_content.custom_minimum_size.max(m_content.size)


func _ready() -> void:
	clip_contents = true
	m_content.size = ContentSize
	resized.connect(func() -> void: ApplyView(m_content.position))
	ResetView()


## Back to 100% zoom, looking at the top-left corner.
func ResetView() -> void:
	if m_zoomTween != null:
		m_zoomTween.kill()
	m_zoom = 1.0
	ApplyView(Vector2.ZERO)


# _input sees the mouse before the content does, so drags and zooms also work on top of skill nodes
func _input(evt: InputEvent) -> void:
	if not is_visible_in_tree():
		m_pressed = false
		return

	var mouse := get_local_mouse_position()
	var mouse_inside := Rect2(Vector2.ZERO, size).has_point(mouse)

	var button := evt as InputEventMouseButton
	if button != null and button.pressed and (button.button_index == MOUSE_BUTTON_WHEEL_UP or button.button_index == MOUSE_BUTTON_WHEEL_DOWN):
		if not mouse_inside:
			return

		var factor := m_zoomStep if button.button_index == MOUSE_BUTTON_WHEEL_UP else 1.0 / m_zoomStep
		ZoomTowards(mouse, m_zoom * factor)
		get_viewport().set_input_as_handled()
		return

	if button != null and button.button_index == MOUSE_BUTTON_LEFT:
		if button.pressed:
			m_pressed = mouse_inside
			m_dragged = false
			m_pressMouse = mouse
			m_pressContentPosition = m_content.position
		elif m_pressed:
			m_pressed = false
			# keep WasDragged true while the content handles this release, then reset
			if m_dragged:
				ResetDrag.call_deferred()
		return

	if not (evt is InputEventMouseMotion) or not m_pressed:
		return

	var offset := mouse - m_pressMouse
	if not m_dragged and offset.length() < m_dragThreshold:
		return

	m_dragged = true
	ApplyView(m_pressContentPosition + offset)


func ZoomTowards(focus: Vector2, target_zoom: float) -> void:
	target_zoom = clampf(target_zoom, MinZoom, m_maxZoom)
	# the content point under the mouse stays under the mouse
	var content_point := (focus - m_content.position) / m_zoom

	if m_zoomTween != null:
		m_zoomTween.kill()
	var apply_zoom := func(zoom: float) -> void:
		m_zoom = zoom
		ApplyView(focus - content_point * zoom)
	m_zoomTween = create_tween()
	m_zoomTween.tween_method(apply_zoom, m_zoom, target_zoom, m_zoomDuration)


func ApplyView(content_position: Vector2) -> void:
	var scaled := ContentSize * m_zoom
	m_content.scale = Vector2.ONE * m_zoom
	m_content.position = Vector2(ClampAxis(content_position.x, scaled.x, size.x), ClampAxis(content_position.y, scaled.y, size.y))


# smaller than the view: centered; bigger: no empty space at the edges
static func ClampAxis(axis_position: float, content: float, view: float) -> float:
	return (view - content) / 2.0 if content <= view else clampf(axis_position, view - content, 0.0)


func ResetDrag() -> void:
	m_dragged = false
