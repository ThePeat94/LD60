class_name SkillTreeController
extends Node

@export var m_skillTreeUi: SkillTreeUI
@export var m_skillTreeData: SkillTreeData
@export var m_skillPoints: ResourceControllerNode
@export var m_upgradeController: SkillTreeUpgradeController
@export var m_glow: FeelGlow
@export var m_cannotBuySound: AudioStreamPlayer
@export var m_boughtSound: AudioStreamPlayer
@export var m_notEnoughPopup: FeelPopupLabel

var m_boughtUpgrades: Dictionary[SkillTreeItemData, int] = {}


func _enter_tree() -> void:
	m_boughtUpgrades = {}
	for item in m_skillTreeData.Upgrades:
		m_boughtUpgrades[item] = 0
	m_skillPoints.ResourceController.ResourceValueChanged.connect(HandleSkillPointsChange)


func _ready() -> void:
	UpdateGlow()


func HandleSkillPointsChange(_new_value: IdleNumber) -> void:
	m_skillTreeUi.SetData(GetViewData())
	UpdateGlow()


func OnClick(_viewport: Node, evt: InputEvent, _shape_idx: int) -> void:
	var button := evt as InputEventMouseButton
	if button == null or not button.pressed or button.button_index != MOUSE_BUTTON_LEFT:
		return
	print("OPEN SKILLTREEE")
	OpenSkillTree()


func AttemptPurchase(upgrade_data: SkillTreeItemData) -> void:
	var bought := m_boughtUpgrades[upgrade_data]
	if bought >= upgrade_data.MaximumAmount:
		if m_cannotBuySound != null:
			m_cannotBuySound.play()
		return

	if not HasUnlocked(upgrade_data):
		if m_cannotBuySound != null:
			m_cannotBuySound.play()
		return

	var cost_to_buy := upgrade_data.CostModel.CalculateFor(IdleNumber.new(bought + 1))

	if not m_skillPoints.ResourceController.CanAfford(cost_to_buy):
		if m_cannotBuySound != null:
			m_cannotBuySound.play()
		if m_notEnoughPopup != null:
			m_notEnoughPopup.Trigger()
		return

	m_skillPoints.ResourceController.UseResource(cost_to_buy)
	if m_boughtSound != null:
		m_boughtSound.play()
	upgrade_data.Upgrade.Apply(m_upgradeController)
	m_boughtUpgrades[upgrade_data] += 1
	OpenSkillTree()
	UpdateGlow()


func UpdateGlow() -> void:
	if m_glow != null:
		m_glow.SetActive(m_boughtUpgrades.keys().any(CanBuy))


func CanBuy(item: SkillTreeItemData) -> bool:
	var bought := m_boughtUpgrades[item]
	if bought >= item.MaximumAmount or not HasUnlocked(item):
		return false

	var cost_to_buy := item.CostModel.CalculateFor(IdleNumber.new(bought + 1))
	return m_skillPoints.ResourceController.CanAfford(cost_to_buy)


func OpenSkillTree() -> void:
	m_skillTreeUi.Open(GetViewData())


func GetViewData() -> Dictionary[SkillTreeItemData, SkillTreeNodeViewData]:
	var view_data: Dictionary[SkillTreeItemData, SkillTreeNodeViewData] = {}
	for item in m_boughtUpgrades:
		view_data[item] = GetViewDataForItem(item)
	return view_data


func HasUnlocked(item: SkillTreeItemData) -> bool:
	return item.PreRequisites.size() <= 0 or item.PreRequisites.any(HasBought)


func HasBought(item: SkillTreeItemData) -> bool:
	return m_boughtUpgrades.get(item, 0) > 0


func GetViewDataForItem(item: SkillTreeItemData) -> SkillTreeNodeViewData:
	var cost_to_buy := item.CostModel.CalculateFor(IdleNumber.new(m_boughtUpgrades[item] + 1))
	var amount := m_boughtUpgrades[item]
	return SkillTreeNodeViewData.new(
		cost_to_buy,
		HasUnlocked(item),
		true,
		m_skillPoints.ResourceController.CanAfford(cost_to_buy) and amount < item.MaximumAmount,
		amount
	)
