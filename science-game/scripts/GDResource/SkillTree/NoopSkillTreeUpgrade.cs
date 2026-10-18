using Godot;
using Nidavellir.SkillTree;

namespace Nidavellir.GDResource.SkillTree;

[GlobalClass]
public partial class NoopSkillTreeUpgrade : SkillTreeUpgradeData
{
    public override void Apply(SkillTreeUpgradeController upgradeController)
    {
        GD.Print("Noop");
    }
}