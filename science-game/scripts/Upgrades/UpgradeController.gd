class_name UpgradeController
extends Node

@export var m_employeeController: EmployeeController


func ApplyThinkerUpgrade(_thinker_upgrade_data: ThinkerUpgradeData) -> void:
	m_employeeController.AddThinker()


func ApplyInventorUpgrade(_upgrade_data: InventorUpgradeData) -> void:
	m_employeeController.AddInventor()


func ApplyScribeUpgrade(_upgrade_data: ScribeUpgradeData) -> void:
	m_employeeController.AddScribe()
