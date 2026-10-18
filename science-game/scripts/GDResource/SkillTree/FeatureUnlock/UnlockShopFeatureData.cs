using Godot;
using Nidavellir.GDResource.Shop;
using Nidavellir.SkillTree;

namespace Nidavellir.GDResource.SkillTree.FeatureUnlock;

[GlobalClass]
public partial class UnlockShopFeatureData : SkillTreeUpgradeData
{
    [Export] public ShopItemData UnlockedShopItem { get; set; }
    
    public override void Apply(SkillTreeUpgradeController upgradeController)
    {
        upgradeController.UnlockShopFeature(this);
    }
}