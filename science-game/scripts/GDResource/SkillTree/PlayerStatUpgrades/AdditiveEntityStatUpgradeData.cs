using Godot;
using Nidavellir.SkillTree;

namespace Nidavellir.GDResource.SkillTree;

[GlobalClass]
public partial class AdditiveEntityStatUpgradeData : EntityStatUpgradeData
{
    public override void Apply(SkillTreeUpgradeController upgradeController)
    {
        GD.Print("Apply AdditiveEntityStatUpgrade");
        upgradeController.ApplyPlayerStatUpgrade(this);
    }
}