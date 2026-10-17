using Godot;

namespace Nidavellir.GDResource.Shop;

[GlobalClass]
public abstract partial class ShopUpgradeData : Resource
{
    public abstract void Apply();
}