namespace Nidavellir.Generator;

public partial class SkillPointGeneratorRewardController : GeneratorRewardController
{
    public override void ApplyReward()
    {
        var value = this.m_playerStats.SkillPointConversion.Value;
        this.m_generated.ResourceController.Add(value);
    }
}