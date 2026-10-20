class_name SkillPointGeneratorRewardController
extends GeneratorRewardController


func ApplyReward() -> void:
	var value := m_playerStats.SkillPointConversion.Value
	m_generated.ResourceController.Add(value)
