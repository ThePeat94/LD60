using Godot;
using Nidavellir.GDResource.Employee;

namespace Nidavellir.GDResource.SkillTree.EntityStatUpgrades.Employee;

[GlobalClass]
public partial class EmployeeStatUpgradeData : EntityStatUpgradeData
{
    [Export] public EmployeeStatTarget TargetStat { get; set; }
    [Export] public EmployeeType TargetType { get; set; }
}