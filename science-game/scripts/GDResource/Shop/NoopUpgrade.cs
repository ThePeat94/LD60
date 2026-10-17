using Godot;
using Nidavellir.Upgrades;

namespace Nidavellir.GDResource.Shop;

[GlobalClass]
public partial class NoopUpgrade : ShopUpgradeData
{
    public override void Apply(UpgradeController upgradeController)
    {
        GD.Print("Noop upgrade");
    }
}