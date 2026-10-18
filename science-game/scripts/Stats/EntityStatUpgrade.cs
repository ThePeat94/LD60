using Nidavellir.GDResource.SkillTree;
using Nidavellir.IdleNumbers;

namespace Nidavellir.Stats;

public class EntityStatUpgrade(IdleNumber value)
{
    public IdleNumber Value { get; private set; } = value;
}