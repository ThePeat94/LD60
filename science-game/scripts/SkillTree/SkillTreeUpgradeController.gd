class_name SkillTreeUpgradeController
extends Node

@export var m_playerStats: PlayerStatsContainer
@export var m_employeeController: EmployeeController
@export var m_shopController: ShopController
@export var m_patentUnlocker: GeneratorUnlocker
@export var m_shopUnlocker: ShopUnlocker
@export var m_gameWonUi: GameWonUI


func ApplyPlayerStatUpgrade(u: PlayerUpgradeData) -> void:
	for upgrade in u.Upgrades:
		match upgrade.Target:
			Enums.PlayerStatTarget.ClickStrength:
				m_playerStats.ClickStrength.AddEntityStatUpgrade(upgrade)
			Enums.PlayerStatTarget.MoneyConversion:
				m_playerStats.MoneyConversion.AddEntityStatUpgrade(upgrade)
			Enums.PlayerStatTarget.SkillPointConversion:
				m_playerStats.SkillPointConversion.AddEntityStatUpgrade(upgrade)


func ApplyEmployeeStatUpgrade(u: EmployeeUpgradeData) -> void:
	for upgrade in u.Upgrades:
		m_employeeController.ApplyEmployeeUpgrades(upgrade)


func UnlockShopFeature(unlock_shop_feature_data: UnlockShopFeatureData) -> void:
	m_shopController.UnlockFeature(unlock_shop_feature_data)


func UnlockGameFeature(unlock_game_feature_data: UnlockGameFeatureData) -> void:
	print("Unlocking Game Feature %s" % Enums.GameFeature.keys()[unlock_game_feature_data.GameFeature])
	match unlock_game_feature_data.GameFeature:
		Enums.GameFeature.Money:
			m_patentUnlocker.Unlock()
		Enums.GameFeature.Shop:
			m_shopUnlocker.Unlock()
		Enums.GameFeature.Skillpoints:
			pass
		_:
			push_error("Unknown GameFeature to unlock")


func UnlockGameWon() -> void:
	m_gameWonUi.Open()
