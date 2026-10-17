using Godot;
using Godot.Collections;
using Nidavellir.GDResource.MathModels;

namespace Nidavellir.GDResource.SkillTree;

[GlobalClass]
public partial class SkillTreeItemData : Resource
{
    [Export] public string SkillName { get; set; }
    [Export] public string Description { get; set; }
    [Export] public Texture2D Icon { get; set; }
    [Export] public MathModelResource CostModel { get; set; }
    [Export] public SkillTreeUpgradeData Upgrade { get; set; }
    [Export] public Array<SkillTreeItemData> Unlocks { get; set; }
    [Export] public int MaximumAmount { get; set; }
}