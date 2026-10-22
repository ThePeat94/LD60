using Godot;
using Nidavellir.Stats.Player;

namespace Nidavellir.Generator;

public abstract partial class GeneratorRewardController : Node
{
    [Export] protected PlayerStatsContainer m_playerStats;
    [Export] protected ResourceControllerNode m_generated;
    
    public ResourceControllerNode Generated => this.m_generated;
    
    public abstract void ApplyReward();
}