class_name ShopController
extends Node2D

@export var m_upgradeController: UpgradeController
@export var m_shopSprite: CanvasItem
@export var m_glow: FeelGlow
@export var m_cannotBuySound: AudioStreamPlayer
@export var m_boughtSound: AudioStreamPlayer
@export var m_notEnoughPopup: FeelPopupLabel

var m_boughtUpgrades: Dictionary[ShopItemData, int] = {}

@export var ShopUpgrades: Array[ShopItemData]

@export var Money: ResourceControllerNode

@export var ShopUI: ShopUI


func _ready() -> void:
	m_boughtUpgrades = {}
	for item in ShopUpgrades:
		m_boughtUpgrades[item] = 0
	Money.ResourceController.ResourceValueChanged.connect(func(_new_value: IdleNumber) -> void: UpdateGlow())
	# the shop gets unlocked by showing its sprite
	if m_shopSprite != null:
		m_shopSprite.visibility_changed.connect(UpdateGlow)
	UpdateGlow()


func OnClick(_viewport: Node, evt: InputEvent, _shape_idx: int) -> void:
	var button := evt as InputEventMouseButton
	if button == null or not button.pressed:
		return

	ShopUI.Open(m_boughtUpgrades)


func AttemptPurchase(item_data: ShopItemData) -> void:
	print("AttemptPurchase")
	var current_amount := m_boughtUpgrades[item_data]

	if current_amount >= item_data.MaximumAmount:
		if m_cannotBuySound != null:
			m_cannotBuySound.play()
		return

	var cost := item_data.CostModel.CalculateFor(IdleNumber.new(current_amount))
	print("Cost to buy %s: %s" % [item_data.UpgradeName, cost])
	if not Money.ResourceController.CanAfford(cost):
		if m_cannotBuySound != null:
			m_cannotBuySound.play()
		if m_notEnoughPopup != null:
			m_notEnoughPopup.Trigger()
		return

	Money.ResourceController.UseResource(cost)
	if m_boughtSound != null:
		m_boughtSound.play()
	m_boughtUpgrades[item_data] += 1
	item_data.UpgradeData.Apply(m_upgradeController)
	ShopUI.Open(m_boughtUpgrades)
	UpdateGlow()


func UnlockFeature(unlock_shop_feature_data: UnlockShopFeatureData) -> void:
	if not m_boughtUpgrades.has(unlock_shop_feature_data.UnlockedShopItem):
		m_boughtUpgrades[unlock_shop_feature_data.UnlockedShopItem] = 0
	UpdateGlow()


func UpdateGlow() -> void:
	if m_glow == null:
		return

	var shop_unlocked := m_shopSprite.is_visible_in_tree() if m_shopSprite != null else true
	m_glow.SetActive(shop_unlocked and m_boughtUpgrades.keys().any(CanBuy))


func CanBuy(item: ShopItemData) -> bool:
	var current_amount := m_boughtUpgrades[item]
	if current_amount >= item.MaximumAmount:
		return false

	return Money.ResourceController.CanAfford(item.CostModel.CalculateFor(IdleNumber.new(current_amount)))
