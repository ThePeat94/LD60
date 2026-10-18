using Godot;
using Nidavellir.SkillTree;

namespace Nidavellir.GDResource.SkillTree;

public abstract partial class SkillTreeUpgradeData : Resource
{
    public abstract void Apply(SkillTreeUpgradeController upgradeController);
}