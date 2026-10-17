using Godot;
using Nidavellir.GDResource;
using Nidavellir.IdleNumbers;

namespace Nidavellir.PlayerStats;

public partial class PlayerStatsContainer : Node
{
    [Export]
    public PlayerStatsData PlayerStatsData { get; set;  }
    
    public IdleNumber ClickStrength { get; private set; }
    
    public IdleNumber MoneyPerPatent { get; private set; }

    public override void _Ready()
    {
        this.ClickStrength = this.PlayerStatsData.InitialClickStrength.ToIdleNumber();
        this.MoneyPerPatent = this.PlayerStatsData.InitialMoneyPerPatent.ToIdleNumber();
    }
}