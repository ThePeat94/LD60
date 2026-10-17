using Godot;
using Nidavellir.GDResource;
using Nidavellir.IdleNumbers;
using Nidavellir.PlayerStats;

namespace Nidavellir.Generator;

public partial class PatentGenerator : Node
{
    [Export]
    public ResourceControllerContainer ResourceControllerContainer { get; set; }
    
    [Export]
    public PlayerStatsContainer PlayerStatsContainer { get; set; }
    
    [Export]
    public ProgressionLevelData ProgressionLevelData { get; set; }
    
    private readonly IdleNumberResourceController m_resourceController = new();
    private int m_currentLevel = 0;
    
    public void OnClick(Node viewport, InputEvent evt, int shapeIdx)
    {
        if (evt is not InputEventMouseButton mouseButton) return;
        if (!mouseButton.Pressed) return;

        if (this.ResourceControllerContainer.ScienceResourceController.CurrentValue >=
            this.PlayerStatsContainer.ClickStrength)
        {
            var valueToUse = this.PlayerStatsContainer.ClickStrength;
            this.ResourceControllerContainer.ScienceResourceController.UseResource(valueToUse);
            this.m_resourceController.Add(valueToUse);
            
            var valueToBeat = this.m_currentLevel < this.ProgressionLevelData.FixedLevels.Count ? 
                this.ProgressionLevelData.FixedLevels[this.m_currentLevel].ToIdleNumber() : 
                this.ProgressionLevelData.DynamicLevels.CalculateFor(new IdleNumber(this.m_currentLevel));

            if (this.m_resourceController.CurrentValue >= valueToBeat)
            {
                var moneyToAdd = this.PlayerStatsContainer.MoneyPerPatent;
                this.m_currentLevel++;
                this.ResourceControllerContainer.MoneyResourceController.Add(moneyToAdd);
                this.m_resourceController.ResetValues();
                GD.Print("Reached Level. Going for next.");
            }
        }
    }
}