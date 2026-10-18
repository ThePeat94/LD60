using Godot;

namespace Nidavellir.GDResource.SkillTree;

public abstract partial class EntityStatUpgradeData : SkillTreeUpgradeData
{
    [Export]
    public IdleNumberResource ClickStrengthUpgrade { get; set; }
    
    [Export]
    public IdleNumberResource MoneyConversionIncrease { get; set; }
    
    [Export]
    public IdleNumberResource SkillPointConversionIncrease { get; set; }
}