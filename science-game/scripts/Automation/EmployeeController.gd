class_name EmployeeController
extends Node

signal EmployeeBought

@export var m_thinkerEmployeeScene: PackedScene
@export var m_inventorEmployeeScene: PackedScene
@export var m_scribeEmployeeScene: PackedScene
@export var m_patentGenerator: GeneratorController
@export var m_skillpointGenerator: GeneratorController
@export var m_scienceGenerator: ScienceGenerator
@export var m_employeeDataContainer: EmployeeDataContainer

@export var m_employeesPerGnome: int = 5
@export var m_thinkerGnomes: Array[Sprite2D]
@export var m_inventorGnomes: Array[Sprite2D]
@export var m_scribeGnomes: Array[Sprite2D]

var m_thinkers: Array[Employee] = []
var m_inventors: Array[Employee] = []
var m_scribes: Array[Employee] = []
var m_employeeStats: Dictionary[int, EmployeeStats] = {}

var Thinkers: Array[Employee]:
	get:
		return m_thinkers

var Inventors: Array[Employee]:
	get:
		return m_inventors

var Scribes: Array[Employee]:
	get:
		return m_scribes


func _enter_tree() -> void:
	m_employeeStats = {
		Enums.EmployeeType.Scribe: EmployeeStats.new(m_employeeDataContainer.ScribeData),
		Enums.EmployeeType.Thinker: EmployeeStats.new(m_employeeDataContainer.ThinkerData),
		Enums.EmployeeType.Inventor: EmployeeStats.new(m_employeeDataContainer.InventorData),
	}

	HideGnomes(m_thinkerGnomes)
	HideGnomes(m_inventorGnomes)
	HideGnomes(m_scribeGnomes)


func AddThinker() -> void:
	AddEmployee(
		m_thinkers,
		m_scienceGenerator,
		m_thinkerEmployeeScene,
		m_employeeStats[Enums.EmployeeType.Thinker],
		m_thinkerGnomes
	)


func AddInventor() -> void:
	AddEmployee(
		m_inventors,
		m_patentGenerator,
		m_inventorEmployeeScene,
		m_employeeStats[Enums.EmployeeType.Inventor],
		m_inventorGnomes
	)


func AddScribe() -> void:
	AddEmployee(
		m_scribes,
		m_skillpointGenerator,
		m_scribeEmployeeScene,
		m_employeeStats[Enums.EmployeeType.Scribe],
		m_scribeGnomes
	)


@warning_ignore("integer_division")
func AddEmployee(employees: Array[Employee], affected: Node2D, employee_scene: PackedScene, stats: EmployeeStats, gnomes: Array[Sprite2D]) -> void:
	var created: Employee = employee_scene.instantiate()
	created.Initialize(affected, stats)
	add_child(created)
	var employee_index := employees.size()
	employees.append(created)
	created.GnomeProvider = func() -> GnomeJumper: return GetGnome(employees, employee_index, gnomes)

	var max_visible := mini(employees.size() / m_employeesPerGnome, gnomes.size())
	for i in max_visible:
		gnomes[i].visible = true

	EmployeeBought.emit()


## Employees beyond the visible gnomes share them.
@warning_ignore("integer_division")
func GetGnome(employees: Array[Employee], employee_index: int, gnomes: Array[Sprite2D]) -> GnomeJumper:
	var visible_gnomes := mini(employees.size() / m_employeesPerGnome, gnomes.size())
	if visible_gnomes == 0:
		return null

	return gnomes[employee_index / m_employeesPerGnome % visible_gnomes] as GnomeJumper


func HideGnomes(gnomes: Array[Sprite2D]) -> void:
	for gnome in gnomes:
		gnome.visible = false


func ApplyEmployeeUpgrades(upgrade_data: EmployeeStatUpgradeData) -> void:
	var affected_employee_stats := m_employeeStats[upgrade_data.TargetType]
	affected_employee_stats.GetStat(upgrade_data.TargetStat).AddEntityStatUpgrade(upgrade_data)
