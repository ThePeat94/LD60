using Godot;
using Nidavellir.GDResource.MathModels;

namespace Nidavellir.GDResource.Shop;

[GlobalClass]
public partial class ShopItemData : Resource
{
    [Export]
    public MathModelResource CostModel { get; set; }
    
    [Export]
    public string UpgradeName { get; set; }
    
    [Export]
    public string Description { get; set; }
    
    [Export]
    public Texture2D Icon { get; set; }
    
    [Export]
    public ShopUpgradeData UpgradeData { get; set; }
}