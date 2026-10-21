using System;
using Nidavellir.GDResource.SkillTree.EntityStatUpgrades;
using Nidavellir.IdleNumbers;

namespace Nidavellir.Stats;

public class DynamicEntityStatUpgrade(IdleNumber value, StatOperation operation, Func<IdleNumber> dynamicValueProvider) : EntityStatUpgrade(value, operation)
{
    private readonly IdleNumber m_baseValue = value;
    public override IdleNumber Value => dynamicValueProvider();
}