using Godot;

namespace Nidavellir.GDResource.SkillTree.EntityStatUpgrades;

[GlobalClass]
public partial class SynergyData : Resource
{
    [Export] public SynergySource Source { get; set; }
    [Export] public int ForEach { get; set; }
}