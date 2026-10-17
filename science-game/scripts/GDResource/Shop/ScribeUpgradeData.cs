using Godot;
using Nidavellir.Upgrades;

namespace Nidavellir.GDResource.Shop;

[GlobalClass]
public partial class ScribeUpgradeData : ShopUpgradeData
{
    public override void Apply(UpgradeController upgradeController)
    {
        upgradeController.ApplyScribeUpgrade(this);
    }
}