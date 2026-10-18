using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using Nidavellir.GDResource.SkillTree;
using Nidavellir.IdleNumbers;
using Nidavellir.SkillTree.UI;

namespace Nidavellir.SkillTree;

public partial class SkillTreeController : Node
{
    [Export] private SkillTreeUI m_skillTreeUi;
    [Export] private Array<SkillTreeItemData> m_shopUpgrades;
    [Export] private ResourceControllerNode m_skillPoints;
    [Export] private SkillTreeUpgradeController m_upgradeController;
    
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

    public void AttemptPurchase(SkillTreeItemData upgradeData)
    {
        var bought = this.m_boughtUpgrades[upgradeData];
        if (bought >= upgradeData.MaximumAmount)
            return;

        if (upgradeData.PreRequisites is not null)
        {
            if (upgradeData.PreRequisites.Count > 0 && !upgradeData.PreRequisites.Any(this.HasBought))
                return;
        }
        
        var costToBuy = upgradeData.CostModel.CalculateFor(new IdleNumber(bought));

        if (!this.m_skillPoints.ResourceController.CanAfford(costToBuy))
            return;
        
        this.m_skillPoints.ResourceController.UseResource(costToBuy);
        upgradeData.Upgrade.Apply(this.m_upgradeController);
        this.m_boughtUpgrades[upgradeData]++;
        this.m_skillTreeUi.Open(this.m_boughtUpgrades);
    }
    
    private bool HasBought(SkillTreeItemData item) => this.m_boughtUpgrades.GetValueOrDefault(item) > 0;                                                                  
}