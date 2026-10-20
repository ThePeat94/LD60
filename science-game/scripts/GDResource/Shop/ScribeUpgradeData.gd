class_name ScribeUpgradeData
extends ShopUpgradeData


func Apply(upgrade_controller: UpgradeController) -> void:
	upgrade_controller.ApplyScribeUpgrade(self)
