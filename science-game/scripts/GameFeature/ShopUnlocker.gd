class_name ShopUnlocker
extends Node

@export var m_shopSprite: Sprite2D
@export var m_shopArea: Area2D


func _ready() -> void:
	m_shopSprite.visible = false
	m_shopArea.input_pickable = false


func Unlock() -> void:
	m_shopSprite.visible = true
	m_shopArea.input_pickable = true
