using Nidavellir.IdleNumbers;

namespace Nidavellir.SkillTree.UI;

public class SkillTreeNodeViewData(IdleNumber cost, bool unlocked, bool isAvailable, bool canBePurchased, int amount)
{
    public IdleNumber Cost { get; } = cost;
    public bool IsUnlocked { get; } = unlocked;
    public bool IsAvailable { get; } = isAvailable;
    public bool CanBePurchased { get; } = canBePurchased;
    public int CurrentAmount { get; } = amount;
}