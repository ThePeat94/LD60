using System;
using System.Collections.Generic;
using Nidavellir.GDResource.SkillTree;
using Nidavellir.IdleNumbers;

namespace Nidavellir.Stats;

public class EntityStat(IdleNumber baseValue)
{
    public IdleNumber Value { get; private set; } = baseValue;
    private readonly IdleNumber m_baseValue = baseValue;
    private readonly List<EntityStatUpgrade> m_additivePlayerStatUpgrades = [];
    private readonly List<EntityStatUpgrade> m_multiplicativeEntityStatUpgrades = [];
    private readonly List<EntityStatUpgrade> m_subtractiveEntityStatUpgrades = [];

    public void AddEntityStatUpgrade(EntityStatUpgradeData upgrade)
    {
        var increase = upgrade.Amount.ToIdleNumber();
        switch (upgrade.Operation)
        {
            case StatOperation.Add:
                this.m_additivePlayerStatUpgrades.Add(new EntityStatUpgrade(increase));
                break;
            case StatOperation.Multiply:
                this.m_multiplicativeEntityStatUpgrades.Add(new EntityStatUpgrade(increase));
                break;
            case StatOperation.Subtract:
                this.m_subtractiveEntityStatUpgrades.Add(new EntityStatUpgrade(increase));
                break;
        }

        this.Recalculate();
    }

    private void Recalculate()
    {
        this.Value = this.m_baseValue;
        foreach (var additive in this.m_additivePlayerStatUpgrades)
        {
            this.Value += additive.Value;
        }

        foreach (var subtraction in this.m_subtractiveEntityStatUpgrades)
        {
            this.Value -= subtraction.Value;
        }

        foreach (var multiplicative in this.m_multiplicativeEntityStatUpgrades)
        {
            this.Value *= multiplicative.Value;
        }
    }
}