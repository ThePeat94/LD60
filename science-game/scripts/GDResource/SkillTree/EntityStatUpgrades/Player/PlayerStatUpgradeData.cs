using Godot;

namespace Nidavellir.GDResource.SkillTree;

[GlobalClass]
public partial class PlayerStatUpgradeData : EntityStatUpgradeData
{
    [Export] public PlayerStatTarget Target { get; set; }
}