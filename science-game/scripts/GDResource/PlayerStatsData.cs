using Godot;
using Nidavellir.IdleNumbers;

namespace Nidavellir.GDResource;

[GlobalClass]
public partial class PlayerStatsData : Resource
{
    [Export]
    public IdleNumberResource InitialClickStrength { get; set; }
    
    [Export]
    public IdleNumberResource InitialMoneyConversion { get; set; }

    [Export]
    public IdleNumberResource InitialSkillPointConversion { get; set; }
}