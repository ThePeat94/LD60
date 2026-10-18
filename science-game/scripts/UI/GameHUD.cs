using Godot;
using Nidavellir.Shop.UI;
using Nidavellir.SkillTree.UI;

namespace Nidavellir.UI;

public partial class GameHUD : Node
{
    [Export]
    public ShopUI ShopUI { get; set; }
    
    [Export]
    public SkillTreeUI SkillTreeUI { get; set; }

    public override void _Ready()
    {
        this.ShopUI.Close();
        this.SkillTreeUI.Close();
    }
}