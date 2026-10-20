class_name PlayerStatsContainer
extends Node

@export var BaseStats: PlayerStatsData

var ClickStrength: EntityStat

var MoneyConversion: EntityStat

var SkillPointConversion: EntityStat


func _ready() -> void:
	ClickStrength = EntityStat.new(BaseStats.InitialClickStrength.ToIdleNumber())
	MoneyConversion = EntityStat.new(BaseStats.InitialMoneyConversion.ToIdleNumber())
	SkillPointConversion = EntityStat.new(BaseStats.InitialSkillPointConversion.ToIdleNumber())
