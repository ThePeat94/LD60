class_name PatentGeneratorRewardController
extends GeneratorRewardController


func ApplyReward() -> void:
	var value := m_playerStats.MoneyConversion.Value
	m_generated.ResourceController.Add(value)
