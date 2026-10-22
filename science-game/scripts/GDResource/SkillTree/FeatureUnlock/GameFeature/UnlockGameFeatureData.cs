using Godot;
using Nidavellir.SkillTree;

namespace Nidavellir.GDResource.SkillTree.FeatureUnlock;

[GlobalClass]
public partial class UnlockGameFeatureData : SkillTreeUpgradeData
{
    [Export] public GameFeature.GameFeature GameFeature { get; set; }
    public override void Apply(SkillTreeUpgradeController upgradeController)
    {
        upgradeController.UnlockGameFeature(this);
    }
}