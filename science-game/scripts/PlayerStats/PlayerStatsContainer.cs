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
    
    public EntityStat MoneyConversion { get; private set; }
    
    public EntityStat SkillPointConversion { get; private set; }

    public override void _Ready()
    {
        this.ClickStrength = new (this.BaseStats.InitialClickStrength.ToIdleNumber());
        this.MoneyConversion = new(this.BaseStats.InitialMoneyConversion.ToIdleNumber());
        this.SkillPointConversion = new(this.BaseStats.InitialSkillPointConversion.ToIdleNumber());
    }
}