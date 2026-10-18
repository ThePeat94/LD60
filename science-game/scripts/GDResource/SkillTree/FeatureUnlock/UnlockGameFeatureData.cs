using Godot;
using Nidavellir.SkillTree;

namespace Nidavellir.GDResource.SkillTree.FeatureUnlock;

public partial class UnlockGameFeatureData : SkillTreeUpgradeData
{
    public override void Apply(SkillTreeUpgradeController upgradeController)
    {
        GD.Print("Apply UnlockGameFeatureData");
    }
}