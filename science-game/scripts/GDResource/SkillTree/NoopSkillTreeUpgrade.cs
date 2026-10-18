using Godot;

namespace Nidavellir.GDResource.SkillTree;

[GlobalClass]
public partial class NoopSkillTreeUpgrade : SkillTreeUpgradeData
{
    public override void Apply()
    {
        GD.Print("Noop");
    }
}