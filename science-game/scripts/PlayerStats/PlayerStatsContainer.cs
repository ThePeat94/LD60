using Godot;
using Nidavellir.GDResource;
using Nidavellir.IdleNumbers;
using Nidavellir.Stats;

namespace Nidavellir.PlayerStats;

public partial class PlayerStatsContainer : Node
{
    
    [Export]
    public PlayerStatsData BaseStats { get; set;  }
    
    public EntityStat ClickStrength { get; private set; }
    
    public EntityStat MoneyPerPatent { get; private set; }

    public override void _Ready()
    {
        this.ClickStrength = new (this.BaseStats.InitialClickStrength.ToIdleNumber());
        this.MoneyPerPatent = new(this.BaseStats.InitialMoneyPerPatent.ToIdleNumber());
    }
}