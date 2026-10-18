using System.Linq;
using Godot;
using Godot.Collections;
using Nidavellir.GDResource.SkillTree;
using Nidavellir.SkillTree.UI;

namespace Nidavellir.SkillTree;

public partial class SkillTreeController : Node
{
    [Export] private SkillTreeUI m_skillTreeUi;
    [Export] private Array<SkillTreeItemData> m_shopUpgrades;
    
    private System.Collections.Generic.Dictionary<SkillTreeItemData, int> m_boughtUpgrades = new();

    public override void _EnterTree()
    {
        base._EnterTree();
        this.m_boughtUpgrades = this.m_shopUpgrades.ToDictionary(x => x, x => 0);
    }

    public void OnClick(Viewport viewport, InputEvent evt, int shapeIdx)
    {
        if (evt is not InputEventMouseButton { Pressed: true })
            return;
        
        this.m_skillTreeUi.Open(this.m_boughtUpgrades);
    }
}