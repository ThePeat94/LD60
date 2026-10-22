using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Nidavellir.GDResource.SkillTree.EntityStatUpgrades;
using Nidavellir.IdleNumbers;

namespace Nidavellir.Stats;

public class EntityStat(IdleNumber baseValue)
{
    public IdleNumber Value => this.Recalculate();
    private readonly List<EntityStatUpgrade> m_additivePlayerStatUpgrades = [];
    private readonly List<EntityStatUpgrade> m_multiplicativeEntityStatUpgrades = [];
    private readonly List<EntityStatUpgrade> m_subtractiveEntityStatUpgrades = [];

    public void AddEntityStatUpgrade(EntityStatUpgrade upgrade)
    {
        switch (upgrade.Operation)
        {
            case StatOperation.Add:
                this.m_additivePlayerStatUpgrades.Add(upgrade);
                break;
            case StatOperation.Multiply:
                this.m_multiplicativeEntityStatUpgrades.Add(upgrade);
                break;
            case StatOperation.Subtract:
                this.m_subtractiveEntityStatUpgrades.Add(upgrade);
                break;
            default:
                GD.PrintErr($"Unknown StatOperation: {upgrade.Operation}");
                throw new ArgumentOutOfRangeException();
        }
    }

    private IdleNumber Recalculate()
    {
        var baseVal = this.m_additivePlayerStatUpgrades.Aggregate(baseValue, (current, additive) => current + additive.Value);
        baseVal = this.m_subtractiveEntityStatUpgrades.Aggregate(baseVal, (current, subtraction) => current - subtraction.Value);
        return this.m_multiplicativeEntityStatUpgrades.Aggregate(baseVal, (current, multiplicative) => current * multiplicative.Value);
    }
}