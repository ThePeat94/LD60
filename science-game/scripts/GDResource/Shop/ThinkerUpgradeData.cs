using Godot;
using Nidavellir.Upgrades;

namespace Nidavellir.GDResource.Shop;

[GlobalClass]
public partial class ThinkerUpgradeData : ShopUpgradeData
{
    public override void Apply(UpgradeController upgradeController)
    {
        upgradeController.ApplyThinkerUpgrade(this);
    }
}