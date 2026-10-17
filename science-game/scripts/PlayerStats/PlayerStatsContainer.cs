using Godot;
using Nidavellir.GDResource;
using Nidavellir.IdleNumbers;

namespace Nidavellir.PlayerStats;

public partial class PlayerStatsContainer : Node
{
    [Export]
    public PlayerStatsData PlayerStatsData { get; set;  }
    
    public PlayerStats PlayerStats { get; private set; }

    public override void _Ready()
    {
        var initialStrength = new IdleNumber(this.PlayerStatsData.InitialClickStrength);
        this.PlayerStats = new PlayerStats(initialStrength);
    }
}