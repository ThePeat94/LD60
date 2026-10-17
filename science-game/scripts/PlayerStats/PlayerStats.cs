using Nidavellir.IdleNumbers;

namespace Nidavellir.PlayerStats;

public class PlayerStats(IdleNumber clickStrength)
{
    public IdleNumber ClickStrength { get; set; } = clickStrength;
}