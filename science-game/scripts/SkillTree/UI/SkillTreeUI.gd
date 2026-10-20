class_name SkillTreeUI
extends Control

@export var m_rootNode: Control
@export var m_view: PanZoomView
@export var m_skillTreeController: SkillTreeController

@export_group("Arrows")
@export var m_arrowSheet: Texture2D
@export var m_arrowScale: float = 4.0
@export var m_arrowGap: float = 8.0

# hand-drawn arrows on the sheet: [region, where the arrow head starts (in sheet pixels from the left)]
const ARROWS := [
	[Rect2(118, 27, 26, 9), 17.0],
	[Rect2(118, 39, 26, 8), 19.0],
	[Rect2(117, 49, 28, 9), 20.0],
	[Rect2(117, 62, 30, 8), 21.0],
	[Rect2(118, 75, 29, 8), 20.0],
	[Rect2(118, 87, 29, 9), 19.0],
	[Rect2(118, 99, 27, 9), 16.0],
]

# stands in for float.MaxValue: big, but still finite when multiplied by a zero direction
const FAR_AWAY := 1e30

var m_nodes: Array[SkillTreeNodeUI] = []
var m_uiForData: Dictionary[SkillTreeItemData, SkillTreeNodeUI] = {}


func _ready() -> void:
	m_nodes.clear()
	for child in get_children():
		if child is SkillTreeNodeUI:
			m_nodes.append(child)
	m_uiForData.clear()
	for skill_node in m_nodes:
		m_uiForData[skill_node.Data] = skill_node

	for skill_node in m_nodes:
		skill_node.OnClick.connect(OnSkillNodeClick)
		skill_node.resized.connect(queue_redraw)

	queue_redraw()


func OnSkillNodeClick(obj: SkillTreeItemData) -> void:
	m_skillTreeController.AttemptPurchase(obj)


func Open(bought_upgrades: Dictionary[SkillTreeItemData, SkillTreeNodeViewData]) -> void:
	if not m_rootNode.visible:
		m_view.ResetView()

	SetData(bought_upgrades)
	m_rootNode.visible = true


func SetData(bought_upgrades: Dictionary[SkillTreeItemData, SkillTreeNodeViewData]) -> void:
	for skill_item in bought_upgrades:
		if not m_uiForData.has(skill_item):
			continue

		var ui := m_uiForData[skill_item]
		var view_data := bought_upgrades[skill_item]
		ui.visible = view_data.IsUnlocked
		ui.Display(view_data)
	queue_redraw()


func Close() -> void:
	m_rootNode.visible = false


func _draw() -> void:
	var inverse_transform := get_global_transform().affine_inverse()

	for skill_node in m_nodes:
		if not skill_node.visible:
			continue

		var prerequisites := skill_node.Data.PreRequisites

		if prerequisites.is_empty():
			continue

		var target := GetLocalRect(skill_node, inverse_transform)

		for prerequisite in prerequisites:
			if not m_uiForData.has(prerequisite):
				continue

			var required_node := m_uiForData[prerequisite]
			var source := GetLocalRect(required_node, inverse_transform)
			var arrow_index := (m_nodes.find(required_node) * 31 + m_nodes.find(skill_node)) % ARROWS.size()
			DrawArrow(source, target, ARROWS[arrow_index])

	draw_set_transform(Vector2.ZERO)


func GetLocalRect(node: Control, inverse_transform: Transform2D) -> Rect2:
	var center := inverse_transform * (node.get_global_transform() * (node.size / 2.0))
	return Rect2(center - node.size / 2.0, node.size)


## Arrow from the edge of from to the edge of to: the shaft stretches, the head keeps its shape.
func DrawArrow(from: Rect2, to: Rect2, arrow: Array) -> void:
	if m_arrowSheet == null:
		return

	var region: Rect2 = arrow[0]
	var head_start: float = arrow[1]

	var direction := (to.get_center() - from.get_center()).normalized()
	var start := ExitPoint(from, direction) + direction * m_arrowGap
	var end := ExitPoint(to, -direction) - direction * m_arrowGap
	var length := start.distance_to(end)
	if length <= 0.0 or (end - start).dot(direction) <= 0.0:
		return

	var height := region.size.y * m_arrowScale
	var head_width := minf((region.size.x - head_start) * m_arrowScale, length)
	var shaft_width := length - head_width
	var shaft_region := Rect2(region.position, Vector2(head_start, region.size.y))
	var head_region := Rect2(region.position + Vector2(head_start, 0.0), Vector2(region.size.x - head_start, region.size.y))

	draw_set_transform(start, direction.angle())
	if shaft_width > 0.0:
		draw_texture_rect_region(m_arrowSheet, Rect2(0.0, -height / 2.0, shaft_width, height), shaft_region)
	draw_texture_rect_region(m_arrowSheet, Rect2(shaft_width, -height / 2.0, head_width, height), head_region)


## Where a ray from the rect's center in direction leaves the rect.
static func ExitPoint(rect: Rect2, direction: Vector2) -> Vector2:
	var half := rect.size / 2.0
	var tx := half.x / absf(direction.x) if absf(direction.x) > 0.0001 else FAR_AWAY
	var ty := half.y / absf(direction.y) if absf(direction.y) > 0.0001 else FAR_AWAY
	return rect.get_center() + direction * minf(tx, ty)
