using System;
using System.Collections.Generic;
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
        var baseVal = baseValue;
        foreach (var additive in this.m_additivePlayerStatUpgrades)
        {
            baseVal += additive.Value;
        }

        foreach (var subtraction in this.m_subtractiveEntityStatUpgrades)
        {
            baseVal -= subtraction.Value;
        }

        foreach (var multiplicative in this.m_multiplicativeEntityStatUpgrades)
        {
            baseVal *= multiplicative.Value;
        }

        return baseVal;
    }
}