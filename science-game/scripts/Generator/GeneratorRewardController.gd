@abstract
class_name GeneratorRewardController
extends Node

@export var m_playerStats: PlayerStatsContainer
@export var m_generated: ResourceControllerNode

var Generated: ResourceControllerNode:
	get:
		return m_generated


@abstract func ApplyReward() -> void
