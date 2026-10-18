using Nidavellir.GDResource.SkillTree;
using Nidavellir.IdleNumbers;

namespace Nidavellir.Stats;

public class EntityStatUpgrade(EntityStatUpgradeData upgradeData)
{
    public IdleNumber Value { get; private set; } = upgradeData.ClickStrengthUpgrade.ToIdleNumber();
}