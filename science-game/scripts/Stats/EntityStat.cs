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

    public void AddEntityStatUpgrade(EntityStatUpgradeData entityStatUpgrade, IdleNumber increase)
    {
        switch (entityStatUpgrade)
        {
            case AdditiveEntityStatUpgradeData u:
                this.m_additivePlayerStatUpgrades.Add(new EntityStatUpgrade(increase));
                break;
            case MultiplicativeEntityStatUpgradeData m:
                this.m_multiplicativeEntityStatUpgrades.Add(new EntityStatUpgrade(increase));
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

        foreach (var multiplicative in this.m_multiplicativeEntityStatUpgrades)
        {
            this.Value *= multiplicative.Value;
        }
    }
}