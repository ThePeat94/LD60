using Nidavellir.GDResource.SkillTree.EntityStatUpgrades;
using Nidavellir.IdleNumbers;

namespace Nidavellir.Stats;

public class FlatEntityStatUpgrade(IdleNumber value, StatOperation operation) : EntityStatUpgrade(value, operation)
{
    public override IdleNumber Value { get; } = value;
}