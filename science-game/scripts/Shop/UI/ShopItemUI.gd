class_name ShopItemUI
extends Node

signal Clicked(item_data: ShopItemData)

@export var m_quantity: Label

@export var ItemName: Label

@export var Icon: TextureRect

@export var Description: Label

@export var Purchase: TextureButton

@export var m_cost: Label

const MinDescriptionFontSize := 16

var m_itemData: ShopItemData
var m_descriptionBoxHeight: float
var m_descriptionMaxFontSize: int


func _ready() -> void:
	Purchase.pressed.connect(HandlePurchaseClick)
	m_descriptionBoxHeight = Description.size.y
	m_descriptionMaxFontSize = Description.get_theme_font_size("font_size")


func Show(item_data: ShopItemData, upgrade_amount: int) -> void:
	ItemName.text = item_data.UpgradeName
	Description.text = item_data.Description
	FitDescriptionFont()
	Icon.texture = item_data.Icon
	m_cost.text = str(item_data.CostModel.CalculateFor(IdleNumber.new(upgrade_amount)))
	m_itemData = item_data
	m_quantity.text = "%d/%d" % [upgrade_amount, item_data.MaximumAmount] if item_data.MaximumAmount > 0 else str(upgrade_amount)


func FitDescriptionFont() -> void:
	if m_descriptionBoxHeight <= 0.0:
		return

	var font_size := m_descriptionMaxFontSize
	Description.add_theme_font_size_override("font_size", font_size)
	while font_size > MinDescriptionFontSize and Description.get_combined_minimum_size().y > m_descriptionBoxHeight:
		font_size -= 2
		Description.add_theme_font_size_override("font_size", font_size)


func HandlePurchaseClick() -> void:
	if DragScrollContainer.IsDragClick(self):
		return

	Clicked.emit(m_itemData)
