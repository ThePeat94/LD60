using System.Collections.Generic;
using Godot;
using Nidavellir.GDResource.SkillTree;
using Nidavellir.GDResource.SkillTree.FeatureUnlockUpgrades;
using Nidavellir.PlayerStats;

namespace Nidavellir.SkillTree;

public partial class SkillTreeUpgradeController : Node
{
    [Export] private PlayerStatsContainer m_playerStats;

    public void ApplyPlayerStatUpgrade(EntityStatUpgradeData u)
    {
        if (u.ClickStrengthUpgrade is not null)
        {
            this.m_playerStats.ClickStrength.AddEntityStatUpgrade(u, u.ClickStrengthUpgrade.ToIdleNumber());
        }

        if (u.MoneyConversionIncrease is not null)
        {
            this.m_playerStats.MoneyConversion.AddEntityStatUpgrade(u, u.MoneyConversionIncrease.ToIdleNumber());
        }

        if (u.SkillPointConversionIncrease is not null)
        {
            this.m_playerStats.SkillPointConversion.AddEntityStatUpgrade(u, u.SkillPointConversionIncrease.ToIdleNumber());
        }
    }

    public void UnlockFeature(FeatureUnlockData featureUnlockData)
    {
    }
}