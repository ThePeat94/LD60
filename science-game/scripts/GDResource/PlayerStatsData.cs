using Godot;
using Nidavellir.IdleNumbers;

namespace Nidavellir.GDResource;

[GlobalClass]
public partial class PlayerStatsData : Resource
{
    [Export]
    public double InitialClickStrength { get; set; }
}