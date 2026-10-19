using System.Linq;
using Godot;
using Godot.Collections;
using Nidavellir.GameFeel;
using Nidavellir.GDResource.Shop;
using Nidavellir.IdleNumbers;
using Nidavellir.Upgrades;
using ShopUI = Nidavellir.Shop.UI.ShopUI;
using UnlockShopFeatureData = Nidavellir.GDResource.SkillTree.FeatureUnlock.UnlockShopFeatureData;

namespace Nidavellir.Shop;

public partial class ShopController : Node2D
{
    [Export] private UpgradeController m_upgradeController;
    [Export] private CanvasItem m_shopSprite;
    [Export] private FeelGlow m_glow;
    
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
        this.Money.ResourceController.ResourceValueChanged += (_, _) => this.UpdateGlow();
        // the shop gets unlocked by showing its sprite
        if (this.m_shopSprite != null)
            this.m_shopSprite.VisibilityChanged += this.UpdateGlow;
        this.UpdateGlow();
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
        this.UpdateGlow();
    }

    public void UnlockFeature(UnlockShopFeatureData unlockShopFeatureData)
    {
        this.m_boughtUpgrades.Add(unlockShopFeatureData.UnlockedShopItem, 0);
        this.UpdateGlow();
    }

    private void UpdateGlow()
    {
        if (this.m_glow == null) return;

        var shopUnlocked = this.m_shopSprite?.IsVisibleInTree() ?? true;
        this.m_glow.SetActive(shopUnlocked && this.m_boughtUpgrades.Keys.Any(this.CanBuy));
    }

    private bool CanBuy(ShopItemData item)
    {
        var currentAmount = this.m_boughtUpgrades[item];
        if (currentAmount >= item.MaximumAmount) return false;

        return this.Money.ResourceController.CanAfford(item.CostModel.CalculateFor(new IdleNumber(currentAmount)));
    }
}