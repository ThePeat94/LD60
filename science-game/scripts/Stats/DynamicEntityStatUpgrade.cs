using System;
using Nidavellir.GDResource.SkillTree.EntityStatUpgrades;
using Nidavellir.IdleNumbers;

namespace Nidavellir.Stats;

public class DynamicEntityStatUpgrade(
    IdleNumber value,
    StatOperation operation,
    SynergySource synergySource,
    int forEach,
    SynergyScalingMode scalingMode
) : EntityStatUpgrade(value, operation)
{
    public override IdleNumber Value => this.DynamicValueProvider();
    public SynergySource SynergySource => synergySource;
    public int ForEach => forEach;
    public SynergyScalingMode ScalingMode => scalingMode;
    public IdleNumber BaseValue { get; } = value;
    public Func<IdleNumber> DynamicValueProvider { get; set; }
}