using Godot;
using Nidavellir.Upgrades;

namespace Nidavellir.GDResource.Shop;

[GlobalClass]
public partial class InventorUpgradeData : ShopUpgradeData
{
    public override void Apply(UpgradeController upgradeController)
    {
        upgradeController.ApplyInventorUpgrade(this);
    }
}