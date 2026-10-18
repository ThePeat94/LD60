using Godot;

namespace Nidavellir.GDResource.SkillTree;

public abstract partial class EntityStatUpgradeData : Resource
{
    [Export] public StatOperation Operation { get; set; }
    [Export] public IdleNumberResource Amount { get; set; }
}