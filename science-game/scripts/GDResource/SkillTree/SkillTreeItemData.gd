class_name SkillTreeItemData
extends Resource

@export var SkillName: String
@export var Description: String
@export var Icon: Texture2D
@export var CostModel: MathModelResource
@export var Upgrade: SkillTreeUpgradeData
@export var PreRequisites: Array[SkillTreeItemData]
@export var MaximumAmount: int
