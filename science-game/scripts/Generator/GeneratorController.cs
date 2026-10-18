using Godot;
using Nidavellir.GDResource;
using Nidavellir.IdleNumbers;
using Nidavellir.PlayerStats;

namespace Nidavellir.Generator;

public partial class GeneratorController : Node, IGenerator
{
    [Export] private ResourceControllerNode m_consumed;
    [Export] private ResourceControllerNode m_required;
    [Export] private GeneratorRewardController m_generatorRewardController;

    public ResourceControllerNode Generated => this.m_generatorRewardController.Generated;
    public ResourceControllerNode Required => this.m_required;
    public IdleNumber Requirement => this.GetLevelRequirement();
        
    [Export]
    public PlayerStatsContainer PlayerStatsContainer { get; set; }
    
    [Export]
    public ProgressionLevelData ProgressionLevelData { get; set; }
    

    private int m_currentLevel = 0;
        
    public void OnClick(Node viewport, InputEvent evt, int shapeIdx)
    {
        if (evt is not InputEventMouseButton mouseButton) return;
        if (!mouseButton.Pressed) return;

        this.Generate(this.PlayerStatsContainer.ClickStrength.Value);
        
    }
    
    public void Generate(IdleNumber power)
    {
        var strengthToApply = this.m_consumed.ResourceController.CurrentValue >= power
            ? power
            : this.m_consumed.ResourceController.CurrentValue;
        
        this.m_consumed.ResourceController.UseResource(strengthToApply);
        this.m_required.ResourceController.Add(strengthToApply);
        var valueToBeat = this.GetLevelRequirement();

        if (this.m_required.ResourceController.CurrentValue < valueToBeat) return;
        do
        {
            this.m_required.ResourceController.UseResource(valueToBeat);
            this.m_generatorRewardController.ApplyReward();
            this.m_currentLevel++;
            valueToBeat = this.GetLevelRequirement();
        } while (this.m_required.ResourceController.CurrentValue >= valueToBeat);
    }
    
    private IdleNumber GetLevelRequirement()
    {
        return this.m_currentLevel < this.ProgressionLevelData.FixedLevels.Count ? 
            this.ProgressionLevelData.FixedLevels[this.m_currentLevel].ToIdleNumber() : 
            this.ProgressionLevelData.DynamicLevels.CalculateFor(new IdleNumber(this.m_currentLevel));
    }
}