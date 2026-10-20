class_name UnlockShopFeatureData
extends SkillTreeUpgradeData

@export var UnlockedShopItem: ShopItemData


func Apply(upgrade_controller: SkillTreeUpgradeController) -> void:
	upgrade_controller.UnlockShopFeature(self)
