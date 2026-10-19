using System.Linq;
using Godot;
using Godot.Collections;
using Nidavellir.GDResource.Shop;
using Nidavellir.IdleNumbers;
using Nidavellir.Upgrades;
using ShopUI = Nidavellir.Shop.UI.ShopUI;
using UnlockShopFeatureData = Nidavellir.GDResource.SkillTree.FeatureUnlock.UnlockShopFeatureData;

namespace Nidavellir.Shop;

public partial class ShopController : Node2D
{
    [Export] private UpgradeController m_upgradeController;
    
    private System.Collections.Generic.Dictionary<ShopItemData, int> m_boughtUpgrades = new();
    
    [Export]
    public Array<ShopItemData> ShopUpgrades { get; set; }
    
    [Export]
    public ResourceControllerNode Money { get; set; }
    
    [Export]
    public ShopUI ShopUI { get; set; }

    public override void _Ready()
    {
        this.m_boughtUpgrades = this.ShopUpgrades.ToDictionary(k => k, _ => 0);
    }

    public void OnClick(Node viewport, InputEvent evt, int shapeIdx)
    {
        if (evt is not InputEventMouseButton { Pressed: true })
            return;
        
        this.ShopUI.Open(this.m_boughtUpgrades);
    }

    public void AttemptPurchase(ShopItemData itemData)
    {
        GD.Print("AttemptPurchase");
        var currentAmount = this.m_boughtUpgrades[itemData];

        if (currentAmount >= itemData.MaximumAmount)
            return;
        
        
        var cost = itemData.CostModel.CalculateFor(new IdleNumber(currentAmount));
        GD.Print($"Cost to buy {itemData.UpgradeName}: {cost}");
        if (!this.Money.ResourceController.CanAfford(cost))
        {
            return;
        }

        this.Money.ResourceController.UseResource(cost);
        this.m_boughtUpgrades[itemData]++;
        itemData.UpgradeData.Apply(this.m_upgradeController);
        this.ShopUI.Open(this.m_boughtUpgrades);
    }

    public void UnlockFeature(UnlockShopFeatureData unlockShopFeatureData)
    {
        this.m_boughtUpgrades.Add(unlockShopFeatureData.UnlockedShopItem, 0);
    }
}