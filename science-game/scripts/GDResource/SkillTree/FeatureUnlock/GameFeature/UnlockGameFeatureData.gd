class_name UnlockGameFeatureData
extends SkillTreeUpgradeData

@export var GameFeature: Enums.GameFeature


func Apply(upgrade_controller: SkillTreeUpgradeController) -> void:
	upgrade_controller.UnlockGameFeature(self)
