using Godot;
using Godot.Collections;

namespace Nidavellir.GDResource.SkillTree;

[GlobalClass]
public partial class SkillTreeData : Resource
{
    [Export]
    public Array<SkillTreeItemData> Upgrades { get; set; }
}