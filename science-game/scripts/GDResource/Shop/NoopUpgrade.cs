using Godot;

namespace Nidavellir.GDResource.Shop;

[GlobalClass]
public partial class NoopUpgrade : ShopUpgradeData
{
    public override void Apply()
    {
        GD.Print("Noop upgrade");
    }
}