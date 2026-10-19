using Godot;
using Nidavellir.SkillTree;

namespace Nidavellir.GDResource.SkillTree;

[GlobalClass]
public partial class GameWonUpgradeData : SkillTreeUpgradeData
{
    public override void Apply(SkillTreeUpgradeController upgradeController)
    {
        upgradeController.UnlockGameWon();
    }
}