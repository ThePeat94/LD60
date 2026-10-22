using Nidavellir.GDResource.SkillTree.EntityStatUpgrades;

namespace Nidavellir.GameState;

public interface ISynergySourceReader
{
    public int GetSynergyAmount(SynergySource source);
}