using System;
using Nidavellir.GDResource.SkillTree.EntityStatUpgrades;
using Nidavellir.IdleNumbers;

namespace Nidavellir.Stats;

public abstract class EntityStatUpgrade(IdleNumber value, StatOperation operation)
{
    public abstract IdleNumber Value { get; }
    public StatOperation Operation { get; } = operation;
}