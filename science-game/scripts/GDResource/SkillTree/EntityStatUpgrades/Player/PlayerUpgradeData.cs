using Godot;
using Godot.Collections;
using Nidavellir.SkillTree;

namespace Nidavellir.GDResource.SkillTree;

[GlobalClass]
public partial class PlayerUpgradeData : SkillTreeUpgradeData
{
    [Export] public Array<PlayerStatUpgradeData> Upgrades { get; set; }
    public override void Apply(SkillTreeUpgradeController upgradeController)
    {
        upgradeController.ApplyPlayerStatUpgrade(this);
    }
}