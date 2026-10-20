class_name InventorUpgradeData
extends ShopUpgradeData


func Apply(upgrade_controller: UpgradeController) -> void:
	upgrade_controller.ApplyInventorUpgrade(self)
