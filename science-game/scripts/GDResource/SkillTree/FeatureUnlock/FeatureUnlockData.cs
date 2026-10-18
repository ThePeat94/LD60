using Nidavellir.SkillTree;

namespace Nidavellir.GDResource.SkillTree.FeatureUnlockUpgrades;

public partial class FeatureUnlockData : SkillTreeUpgradeData
{
    public override void Apply(SkillTreeUpgradeController upgradeController)
    {
        upgradeController.UnlockFeature(this);
    }
}