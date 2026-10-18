using Godot;

namespace Nidavellir.GDResource.SkillTree.EntityStatUpgrades.Player;

[GlobalClass]
public partial class PlayerStatUpgradeData : EntityStatUpgradeData
{
    [Export] public PlayerStatTarget Target { get; set; }
}