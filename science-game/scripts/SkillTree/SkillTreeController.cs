using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using Nidavellir.GameFeel;
using Nidavellir.GDResource.SkillTree;
using Nidavellir.IdleNumbers;
using Nidavellir.SkillTree.UI;

namespace Nidavellir.SkillTree;

public partial class SkillTreeController : Node
{
    [Export] private SkillTreeUI m_skillTreeUi;
    [Export] private SkillTreeData m_skillTreeData;
    [Export] private ResourceControllerNode m_skillPoints;
    [Export] private SkillTreeUpgradeController m_upgradeController;
    [Export] private FeelGlow m_glow;
    [Export] private AudioStreamPlayer m_cannotBuySound;
    [Export] private AudioStreamPlayer m_boughtSound;
    [Export] private FeelPopupLabel m_notEnoughPopup;
    
    private System.Collections.Generic.Dictionary<SkillTreeItemData, int> m_boughtUpgrades = new();

    public override void _EnterTree()
    {
        base._EnterTree();
        this.m_boughtUpgrades = this.m_skillTreeData.Upgrades.ToDictionary(x => x, x => 0);
        this.m_skillPoints.ResourceController.ResourceValueChanged += this.HandleSkillPointsChange;
    }

    public override void _Ready()
    {
        this.UpdateGlow();
    }

    private void HandleSkillPointsChange(object sender, ResourceValueChangedEvent e)
    {
        var viewData = this.m_boughtUpgrades.ToDictionary(
            x => x.Key,
            x => this.GetViewDataForItem(x.Key)
        );
        this.m_skillTreeUi.SetData(viewData);
        this.UpdateGlow();
    }

    public void OnClick(Viewport viewport, InputEvent evt, int shapeIdx)
    {
        if (evt is not InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
            return;
        GD.Print("OPEN SKILLTREEE");
        this.OpenSkillTree();
    }

    public void AttemptPurchase(SkillTreeItemData upgradeData)
    {
        var bought = this.m_boughtUpgrades[upgradeData];
        if (bought >= upgradeData.MaximumAmount)
        {
            this.m_cannotBuySound?.Play();
            return;
        }

        if (!this.HasUnlocked(upgradeData))
        {
            this.m_cannotBuySound?.Play();
            return;
        }
        
        var costToBuy = upgradeData.CostModel.CalculateFor(new IdleNumber(bought + 1));

        if (!this.m_skillPoints.ResourceController.CanAfford(costToBuy))
        {
            this.m_cannotBuySound?.Play();
            this.m_notEnoughPopup?.Trigger();
            return;
        }
        
        this.m_skillPoints.ResourceController.UseResource(costToBuy);
        this.m_boughtSound?.Play();
        upgradeData.Upgrade.Apply(this.m_upgradeController);
        this.m_boughtUpgrades[upgradeData]++;
        this.OpenSkillTree();
        this.UpdateGlow();
    }

    private void UpdateGlow()
    {
        this.m_glow?.SetActive(this.m_boughtUpgrades.Keys.Any(this.CanBuy));
    }

    private bool CanBuy(SkillTreeItemData item)
    {
        var bought = this.m_boughtUpgrades[item];
        if (bought >= item.MaximumAmount || !this.HasUnlocked(item)) return false;

        var costToBuy = item.CostModel.CalculateFor(new IdleNumber(bought + 1));
        return this.m_skillPoints.ResourceController.CanAfford(costToBuy);
    }

    private void OpenSkillTree()
    {
        var viewData = this.m_boughtUpgrades.ToDictionary(
            x => x.Key,
            x => this.GetViewDataForItem(x.Key)
        );
        this.m_skillTreeUi.Open(viewData);
    }

    private bool HasUnlocked(SkillTreeItemData item)
    {
        if (item.PreRequisites is null) return true;
        return item.PreRequisites.Count <= 0 || item.PreRequisites.Any(this.HasBought);
    }
    
    private bool HasBought(SkillTreeItemData item) => this.m_boughtUpgrades.GetValueOrDefault(item) > 0;
    
    private SkillTreeNodeViewData GetViewDataForItem(SkillTreeItemData item)
    {
        var costToBuy = item.CostModel.CalculateFor(new IdleNumber(this.m_boughtUpgrades[item] + 1));
        var amount = this.m_boughtUpgrades[item];
        return new SkillTreeNodeViewData(
            costToBuy,
            this.HasUnlocked(item),
            true,
            this.m_skillPoints.ResourceController.CanAfford(costToBuy) && amount < item.MaximumAmount,
            amount
        );
    }
}