class_name SkillTreeNodeUI
extends PanelContainer

signal OnClick(data: SkillTreeItemData)

@export var m_data: SkillTreeItemData
@export var m_name: Label
@export var m_amount: Label
@export var m_cost: Label
@export var m_description: Label
@export var m_infoDialog: PackedScene

@export_group("Look")
# hand-drawn frames: scrolls for unlocking new features, boxes for everything else
@export var m_boxFrames: Array[StyleBox]
@export var m_scrollFrames: Array[StyleBox]
@export_range(0, 1) var m_unaffordableAlpha: float = 0.45
@export var m_hoverScale: float = 1.05

var m_canBePurchased: bool
var m_isUnlocked: bool
var m_hoverTween: Tween

var Data: SkillTreeItemData:
	get:
		return m_data


func _enter_tree() -> void:
	mouse_entered.connect(OnMouseEntered)
	mouse_exited.connect(OnMouseExited)
	resized.connect(func() -> void: pivot_offset = size / 2.0)


func _ready() -> void:
	ApplyFrame()


func ApplyFrame() -> void:
	var upgrade: SkillTreeUpgradeData = m_data.Upgrade if m_data != null else null
	var unlocks_feature := upgrade is UnlockGameFeatureData or upgrade is UnlockShopFeatureData or upgrade is GameWonUpgradeData
	var frames := m_scrollFrames if unlocks_feature else m_boxFrames
	if frames.is_empty():
		return

	# alternate between the drawings so neighbours don't look copy-pasted
	add_theme_stylebox_override("panel", frames[get_index() % frames.size()])


func OnMouseEntered() -> void:
	TweenScale(m_hoverScale)


func OnMouseExited() -> void:
	TweenScale(1.0)


func TweenScale(target_scale: float) -> void:
	if m_hoverTween != null:
		m_hoverTween.kill()
	m_hoverTween = create_tween()
	m_hoverTween.tween_property(self, "scale", Vector2.ONE * target_scale, 0.1) \
		.set_trans(Tween.TRANS_QUAD) \
		.set_ease(Tween.EASE_OUT)


func _gui_input(evt: InputEvent) -> void:
	# act on release, so pressing a node to drag the tree around doesn't buy it
	var button := evt as InputEventMouseButton
	if button == null or button.pressed or button.button_index != MOUSE_BUTTON_LEFT:
		return

	# released outside the node, or the press turned into dragging the tree
	if not Rect2(Vector2.ZERO, size).has_point(button.position) or DragScrollContainer.IsDragClick(self):
		return

	# unaffordable clicks still go through, the controller refuses them with feedback
	if not m_isUnlocked:
		return

	accept_event()
	print("CLICK SKILL TREE NODE")
	OnClick.emit(Data)


func Display(view_data: SkillTreeNodeViewData) -> void:
	m_name.text = m_data.SkillName
	m_cost.text = str(view_data.Cost)
	m_description.text = m_data.Description
	m_description.visible = not m_data.Description.is_empty()
	m_amount.text = "%d/%d" % [view_data.CurrentAmount, m_data.MaximumAmount]
	m_canBePurchased = view_data.CanBePurchased
	m_isUnlocked = view_data.IsUnlocked
	# faded ink for what can't be bought right now
	modulate.a = 1.0 if m_canBePurchased and m_isUnlocked else m_unaffordableAlpha
