class_name EmployeeStats
extends RefCounted

var m_stats: Dictionary[int, EntityStat] = {}

var WorkForce: EntityStat:
	get:
		return m_stats[Enums.EmployeeStatTarget.BaseForce]

var TickSpeed: EntityStat:
	get:
		return m_stats[Enums.EmployeeStatTarget.TickSpeed]


func _init(data: EmployeeData) -> void:
	m_stats = {
		Enums.EmployeeStatTarget.BaseForce: EntityStat.new(data.BaseWorkForce.ToIdleNumber()),
		Enums.EmployeeStatTarget.TickSpeed: EntityStat.new(data.BaseTickSpeed.ToIdleNumber()),
	}


## The C# indexer this[target].
func GetStat(target: Enums.EmployeeStatTarget) -> EntityStat:
	return m_stats[target]
