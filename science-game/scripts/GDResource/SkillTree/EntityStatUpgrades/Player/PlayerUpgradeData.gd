class_name PlayerUpgradeData
extends SkillTreeUpgradeData

@export var Upgrades: Array[PlayerStatUpgradeData]


func Apply(upgrade_controller: SkillTreeUpgradeController) -> void:
	upgrade_controller.ApplyPlayerStatUpgrade(self)
