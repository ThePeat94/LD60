using Godot;
using Nidavellir.SkillTree;

namespace Nidavellir.GDResource.SkillTree;

[GlobalClass]
public partial class MultiplicativeEntityStatUpgradeData : EntityStatUpgradeData
{
    public override void Apply(SkillTreeUpgradeController upgradeController)
    {
        GD.Print("Apply MultiplicativeEntityStatUpgrade");
        upgradeController.ApplyPlayerStatUpgrade(this);
    }
}