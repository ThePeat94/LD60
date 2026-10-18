using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Nidavellir.Automation;
using Nidavellir.GDResource.SkillTree;
using Nidavellir.GDResource.SkillTree.FeatureUnlockUpgrades;
using Nidavellir.PlayerStats;
using Nidavellir.Shop;

namespace Nidavellir.SkillTree;

public partial class SkillTreeUpgradeController : Node
{
    [Export] private PlayerStatsContainer m_playerStats;
    [Export] private EmployeeController m_employeeController;
    [Export] private ShopController m_shopController;

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

    public void UnlockShopFeature(UnlockShopFeatureData unlockShopFeatureData)
    {
        this.m_shopController.UnlockFeature(unlockShopFeatureData);
    }
}