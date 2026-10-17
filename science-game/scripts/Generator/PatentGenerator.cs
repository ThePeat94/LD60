using Godot;
using Nidavellir.GDResource;
using Nidavellir.IdleNumbers;
using Nidavellir.PlayerStats;

namespace Nidavellir.Generator;

public partial class PatentGenerator : Node, IGenerator
{
    [Export]
    public ResourceControllerNode ScienceResourceControllerNode { get; set; }
    
    [Export]
    public ResourceControllerNode MoneyResourceControllerNode { get; set; }
    
    [Export]
    public PlayerStatsContainer PlayerStatsContainer { get; set; }
    
    [Export]
    public ProgressionLevelData ProgressionLevelData { get; set; }
    
    [Export]
    public ResourceControllerNode ResourceControllerNode { get; set; }

    public IdleNumber Requirement => this.GetLevelRequirement();
    
    private int m_currentLevel = 0;
    
    public void OnClick(Node viewport, InputEvent evt, int shapeIdx)
    {
        if (evt is not InputEventMouseButton mouseButton) return;
        if (!mouseButton.Pressed) return;

        this.Generate(this.PlayerStatsContainer.ClickStrength);
        
    }

    public void Generate(IdleNumber strength)
    {
        var strengthToApply = this.ScienceResourceControllerNode.ResourceController.CurrentValue >= strength
            ? strength
            : this.ScienceResourceControllerNode.ResourceController.CurrentValue;
        
        this.ScienceResourceControllerNode.ResourceController.UseResource(strengthToApply);
        this.ResourceControllerNode.ResourceController.Add(strengthToApply);
        var valueToBeat = this.GetLevelRequirement();

        if (this.ResourceControllerNode.ResourceController.CurrentValue >= valueToBeat)
        {
            var moneyToAdd = this.PlayerStatsContainer.MoneyPerPatent;
            this.m_currentLevel++;
            this.MoneyResourceControllerNode.ResourceController.Add(moneyToAdd);
            this.ResourceControllerNode.ResourceController.ResetValues();
            GD.Print("Reached Level. Going for next.");
        }
    }

    private IdleNumber GetLevelRequirement()
    {
        return this.m_currentLevel < this.ProgressionLevelData.FixedLevels.Count ? 
            this.ProgressionLevelData.FixedLevels[this.m_currentLevel].ToIdleNumber() : 
            this.ProgressionLevelData.DynamicLevels.CalculateFor(new IdleNumber(this.m_currentLevel));
    }
}