using Godot;

namespace Nidavellir.GDResource.SkillTree;

[GlobalClass]
public partial class EmployeeStatUpgradeData : EntityStatUpgradeData
{
    [Export] public EmployeeStatTarget TargetStat { get; set; }
    [Export] public EmployeeType TargetType { get; set; }
}