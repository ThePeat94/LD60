class_name EmployeeUpgradeData
extends SkillTreeUpgradeData

@export var Upgrades: Array[EmployeeStatUpgradeData]


func Apply(upgrade_controller: SkillTreeUpgradeController) -> void:
	print("Apply EmployeeStatUpgrades")
	upgrade_controller.ApplyEmployeeStatUpgrade(self)
