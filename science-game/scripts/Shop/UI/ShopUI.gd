class_name ShopUI
extends AspectRatioContainer

@export var ItemPrefab: PackedScene

@export var ShopController: ShopController

@export var ItemsParent: Node

var m_items: Dictionary[ShopItemData, ShopItemUI] = {}


func Open(upgrades: Dictionary[ShopItemData, int]) -> void:
	for item_data in upgrades:
		if not m_items.has(item_data):
			var initialized_item: ShopItemUI = ItemPrefab.instantiate()
			ItemsParent.add_child(initialized_item)
			m_items[item_data] = initialized_item
			initialized_item.Show(item_data, upgrades[item_data])
			initialized_item.Clicked.connect(OnItemPurchase)
		else:
			m_items[item_data].Show(item_data, upgrades[item_data])
	visible = true


func OnItemPurchase(obj: ShopItemData) -> void:
	ShopController.AttemptPurchase(obj)


func Close() -> void:
	visible = false


func ClearUI() -> void:
	for item_ui in m_items.values():
		item_ui.queue_free()
	m_items.clear()
