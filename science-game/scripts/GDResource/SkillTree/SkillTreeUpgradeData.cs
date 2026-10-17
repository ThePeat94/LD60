using Godot;

namespace Nidavellir.GDResource.SkillTree;

public abstract partial class SkillTreeUpgradeData : Resource
{
    public abstract void Apply();
}