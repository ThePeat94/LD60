using System;
using Nidavellir.GDResource.SkillTree.EntityStatUpgrades;
using Nidavellir.IdleNumbers;

namespace Nidavellir.Stats;

public class DynamicEntityStatUpgrade(IdleNumber value, StatOperation operation, Func<IdleNumber> dynamicValueProvider, SynergySource synergySource, int forEach) : EntityStatUpgrade(value, operation)
{
    private readonly IdleNumber m_baseValue = value;
    public override IdleNumber Value => dynamicValueProvider();
    public SynergySource SynergySource => synergySource;
    public int ForEach => forEach;
}