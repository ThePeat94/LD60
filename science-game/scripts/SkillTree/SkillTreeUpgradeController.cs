using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Nidavellir.Automation;
using Nidavellir.GDResource.SkillTree;
using Nidavellir.GDResource.SkillTree.FeatureUnlockUpgrades;
using Nidavellir.PlayerStats;

namespace Nidavellir.SkillTree;

public partial class SkillTreeUpgradeController : Node
{
    [Export] private PlayerStatsContainer m_playerStats;
    [Export] private EmployeeController m_employeeController;

    public void ApplyPlayerStatUpgrade(PlayerUpgradeData u)
    {
        foreach (var upgrade in u.Upgrades)
        {
            switch (upgrade.Target)
            {
                case PlayerStatTarget.ClickStrength:
                    this.m_playerStats.ClickStrength.AddEntityStatUpgrade(upgrade);
                    break;
                case PlayerStatTarget.MoneyConversion:
                    this.m_playerStats.MoneyConversion.AddEntityStatUpgrade(upgrade);
                    break;
                case PlayerStatTarget.SkillPointConversion:
                    this.m_playerStats.SkillPointConversion.AddEntityStatUpgrade(upgrade);
                    break;
            }
        }
    }

    public void ApplyEmployeeStatUpgrade(EmployeeUpgradeData u)
    {
        foreach (var upgrade in u.Upgrades)
        {
            this.m_employeeController.ApplyEmployeeUpgrades(upgrade);
        }
    }

    public void UnlockFeature(FeatureUnlockData featureUnlockData)
    {
    }
}