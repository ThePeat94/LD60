using Godot;
using Nidavellir.Shop.UI;

namespace Nidavellir.UI;

public partial class GameHUD : Node
{
    [Export]
    public ShopUI ShopUI { get; set; }

    public override void _Ready()
    {
        this.ShopUI.Close();
    }
}