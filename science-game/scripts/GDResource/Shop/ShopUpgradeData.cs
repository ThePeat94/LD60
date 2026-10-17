using Godot;
using Nidavellir.Upgrades;

namespace Nidavellir.GDResource.Shop;

public abstract partial class ShopUpgradeData : Resource
{
    public abstract void Apply(UpgradeController upgradeController);
}