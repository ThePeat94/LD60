class_name EntityStat
extends RefCounted
## A stat with upgrades on top of its base value: additions and subtractions first, then multiplications.

var Value: IdleNumber
var m_baseValue: IdleNumber
var m_additivePlayerStatUpgrades: Array[IdleNumber] = []
var m_multiplicativeEntityStatUpgrades: Array[IdleNumber] = []
var m_subtractiveEntityStatUpgrades: Array[IdleNumber] = []


func _init(base_value: IdleNumber) -> void:
	m_baseValue = base_value
	Value = base_value


func AddEntityStatUpgrade(upgrade: EntityStatUpgradeData) -> void:
	var increase := upgrade.Amount.ToIdleNumber()
	match upgrade.Operation:
		Enums.StatOperation.Add:
			m_additivePlayerStatUpgrades.append(increase)
		Enums.StatOperation.Multiply:
			m_multiplicativeEntityStatUpgrades.append(increase)
		Enums.StatOperation.Subtract:
			m_subtractiveEntityStatUpgrades.append(increase)

	Recalculate()


func Recalculate() -> void:
	Value = m_baseValue
	for additive in m_additivePlayerStatUpgrades:
		Value = Value.Add(additive)

	for subtraction in m_subtractiveEntityStatUpgrades:
		Value = Value.Sub(subtraction)

	for multiplicative in m_multiplicativeEntityStatUpgrades:
		Value = Value.Mul(multiplicative)
