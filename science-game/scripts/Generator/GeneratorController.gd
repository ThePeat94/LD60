class_name GeneratorController
extends Node2D

signal GeneratedReward

signal Clicked

signal ClickRejected

## A click added amount to the current batch.
signal Produced(amount: float)

@export var m_consumed: ResourceControllerNode
@export var m_required: ResourceControllerNode
@export var m_generatorRewardController: GeneratorRewardController
@export var m_symbols: SymbolStore
@export var m_sourceSymbols: SymbolStore
@export var m_payoutTarget: CanvasItem

@export var PlayerStatsContainer: PlayerStatsContainer

@export var ProgressionLevelData: ProgressionLevelData

var Generated: ResourceControllerNode:
	get:
		return m_generatorRewardController.Generated

var Required: ResourceControllerNode:
	get:
		return m_required

var Requirement: IdleNumber:
	get:
		return GetLevelRequirement(m_currentLevel)

var CurrentLevel: int:
	get:
		return m_currentLevel

var m_currentLevel: int = 0


func OnClick(_viewport: Node, evt: InputEvent, _shape_idx: int) -> void:
	var button := evt as InputEventMouseButton
	if button == null or not button.pressed or button.button_index != MOUSE_BUTTON_LEFT:
		return

	if m_consumed.ResourceController.CurrentValue.Value <= 0.0:
		ClickRejected.emit()
		return

	Clicked.emit()
	Generate(PlayerStatsContainer.ClickStrength.Value, get_global_mouse_position(), true)


## origin: where the work happened (e.g. a gnome, a Vector2 or null), so its symbols can start there.
## Returns whether anything was generated.
func Generate(power: IdleNumber, origin: Variant = null, is_click: bool = false) -> bool:
	var available := m_consumed.ResourceController.CurrentValue
	var strength_to_apply := power if available.Ge(power) else available
	if strength_to_apply.Value <= 0.0:
		return false

	var pool := m_required.ResourceController.CurrentValue.Add(strength_to_apply)
	var levels_gained := 0
	var requirement := GetLevelRequirement(m_currentLevel)

	while requirement.Value > 0.0 and pool.Ge(requirement):
		pool = pool.Sub(requirement)
		levels_gained += 1
		requirement = GetLevelRequirement(m_currentLevel + levels_gained)

	m_currentLevel += levels_gained
	m_consumed.ResourceController.UseResource(strength_to_apply)
	m_required.ResourceController.SetValue(pool)

	for i in levels_gained:
		m_generatorRewardController.ApplyReward()

	MoveSymbols(origin, levels_gained > 0, pool.Value > 0.0)

	if is_click:
		Produced.emit(strength_to_apply.ToDouble())

	if levels_gained > 0:
		GeneratedReward.emit()

	return true


func MoveSymbols(origin: Variant, batch_filled: bool, has_remainder: bool) -> void:
	if m_symbols == null:
		return

	var landing: Vector2 = origin if origin != null else m_symbols.RandomPoint()
	var payout_position: Variant = GetCenter(m_payoutTarget)

	if batch_filled and payout_position != null:
		m_symbols.FlyAllTo(payout_position)

	# the converted symbol stays only if something is left for the next batch
	var land_in: SymbolStore = m_symbols if (not batch_filled or has_remainder or payout_position == null) else null
	var target: Vector2 = landing if land_in != null else payout_position

	if m_sourceSymbols == null:
		if land_in != null:
			land_in.AddAt(target)
		return

	m_sourceSymbols.SendOne(target, land_in)

	# nothing left to consume: the source must not keep showing symbols
	if m_consumed.ResourceController.CurrentValue.Value > 0.0:
		return
	if land_in != null:
		m_sourceSymbols.SendAll(landing, land_in)
	else:
		m_sourceSymbols.FlyAllTo(target)


## The center of a Control or the position of a Node2D, null for anything else.
static func GetCenter(target: CanvasItem) -> Variant:
	if target is Control:
		return (target as Control).get_global_rect().get_center()
	if target is Node2D:
		return (target as Node2D).global_position
	return null


func GetLevelRequirement(level: int) -> IdleNumber:
	if level < ProgressionLevelData.FixedLevels.size():
		return ProgressionLevelData.FixedLevels[level].ToIdleNumber()
	return ProgressionLevelData.DynamicLevels.CalculateFor(IdleNumber.new(level))
