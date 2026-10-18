using Godot;
using Godot.Collections;
using Nidavellir.SkillTree;

namespace Nidavellir.GDResource.SkillTree;

[GlobalClass]
public partial class EmployeeUpgradeData : SkillTreeUpgradeData
{
    [Export] public Array<EmployeeStatUpgradeData> Upgrades { get; set; }
    public override void Apply(SkillTreeUpgradeController upgradeController)
    {
        upgradeController.ApplyEmployeeStatUpgrade(this);
    }
}