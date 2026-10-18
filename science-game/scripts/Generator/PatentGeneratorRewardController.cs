namespace Nidavellir.Generator;

public partial class PatentGeneratorRewardController : GeneratorRewardController
{
    public override void ApplyReward()
    {
        var value = this.m_playerStats.MoneyConversion.Value;
        this.m_generated.ResourceController.Add(value);
    }
}