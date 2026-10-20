class_name GameHUD
extends Node

@export var ShopUI: ShopUI

@export var SkillTreeUI: SkillTreeUI

@export var m_gameWonUi: GameWonUI


func _ready() -> void:
	ShopUI.Close()
	SkillTreeUI.Close()
	m_gameWonUi.Close()
