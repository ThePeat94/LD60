class_name GameWonUpgradeData
extends SkillTreeUpgradeData


func Apply(upgrade_controller: SkillTreeUpgradeController) -> void:
	upgrade_controller.UnlockGameWon()
