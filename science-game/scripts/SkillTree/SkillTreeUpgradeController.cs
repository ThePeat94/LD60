using System;
using Godot;
using Nidavellir.Automation;
using Nidavellir.GDResource;
using Nidavellir.GDResource.SkillTree.EntityStatUpgrades;
using Nidavellir.GDResource.SkillTree.EntityStatUpgrades.Employee;
using Nidavellir.GDResource.SkillTree.EntityStatUpgrades.Player;
using Nidavellir.GDResource.SkillTree.FeatureUnlock;
using Nidavellir.PlayerStats;
using Nidavellir.Shop;
using Nidavellir.Stats;
using Nidavellir.UI;

namespace Nidavellir.SkillTree;

public partial class SkillTreeUpgradeController : Node
{
    [Export] private PlayerStatsContainer m_playerStats;
    [Export] private EmployeeController m_employeeController;
    [Export] private ShopController m_shopController;
    [Export] private GeneratorUnlocker m_patentUnlocker;
    [Export] private ShopUnlocker m_shopUnlocker;
    [Export] private GameWonUI m_gameWonUi;
    [Export] private EntityStatUpgradeFactory m_entityStatUpgradeFactory;
    
    public void ApplyPlayerStatUpgrade(PlayerUpgradeData u)
    {
        foreach (var upgrade in u.Upgrades)
        {
            switch (upgrade.Target)
            {
                case PlayerStatTarget.ClickStrength:
                    this.m_playerStats.ClickStrength.AddEntityStatUpgrade(this.m_entityStatUpgradeFactory.Create(upgrade));
                    break;
                case PlayerStatTarget.MoneyConversion:
                    this.m_playerStats.MoneyConversion.AddEntityStatUpgrade(this.m_entityStatUpgradeFactory.Create(upgrade));
                    break;
                case PlayerStatTarget.SkillPointConversion:
                    this.m_playerStats.SkillPointConversion.AddEntityStatUpgrade(this.m_entityStatUpgradeFactory.Create(upgrade));
                    break;
                default:
                    GD.PrintErr($"Unknown PlayerStatTarget to upgrade: {upgrade.Target}");
                    throw new ArgumentOutOfRangeException();
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

    public void UnlockGameFeature(UnlockGameFeatureData unlockGameFeatureData)
    {
        GD.Print($"Unlocking Game Feature {unlockGameFeatureData.GameFeature}");
        switch (unlockGameFeatureData.GameFeature)
        {
            case GameFeature.GameFeature.Money:
                this.m_patentUnlocker.Unlock();
                break;
            case GameFeature.GameFeature.Shop:
                this.m_shopUnlocker.Unlock();
                break;
            case GameFeature.GameFeature.Skillpoints:
                break;
            default:
                GD.PrintErr("Unknown GameFeature to unlock");
                throw new ArgumentOutOfRangeException();
        }
    }

    public void UnlockGameWon()
    {
        this.m_gameWonUi.Open();
    }
}