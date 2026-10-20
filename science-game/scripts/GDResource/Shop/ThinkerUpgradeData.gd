class_name ThinkerUpgradeData
extends ShopUpgradeData


func Apply(upgrade_controller: UpgradeController) -> void:
	upgrade_controller.ApplyThinkerUpgrade(self)
