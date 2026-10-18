using Godot;
using Nidavellir.GDResource;
using Nidavellir.IdleNumbers;
using Nidavellir.PlayerStats;

namespace Nidavellir.Generator;

public partial class GeneratorController : Node2D, IGenerator
{
    [Export] private ResourceControllerNode m_consumed;
    [Export] private ResourceControllerNode m_required;
    [Export] private GeneratorRewardController m_generatorRewardController;

    public ResourceControllerNode Generated => this.m_generatorRewardController.Generated;
    public ResourceControllerNode Required => this.m_required;
    public IdleNumber Requirement => this.GetLevelRequirement(this.m_currentLevel);
        
    [Export]
    public PlayerStatsContainer PlayerStatsContainer { get; set; }
    
    [Export]
    public ProgressionLevelData ProgressionLevelData { get; set; }
    
    [Signal]
    public delegate void GeneratedRewardEventHandler();
    

    private int m_currentLevel = 0;
        
    public void OnClick(Node viewport, InputEvent evt, int shapeIdx)
    {
        if (evt is not InputEventMouseButton mouseButton) return;
        if (!mouseButton.Pressed) return;

        this.Generate(this.PlayerStatsContainer.ClickStrength.Value);
        
    }
    
     public void Generate(IdleNumber power)                                                                                                                               
  {                                                                                                                                                                    
      var available = this.m_consumed.ResourceController.CurrentValue;                                                                                                 
      var strengthToApply = available >= power ? power : available;                                                                                                    
      if (strengthToApply.Value <= 0) return;                                                                                                                          
                                                                                                                                                                       
      var pool = this.m_required.ResourceController.CurrentValue + strengthToApply;                                                                                    
      var levelsGained = 0;                                                                                                                                            
      var requirement = this.GetLevelRequirement(this.m_currentLevel);                                                                                                 
                                                                                                                                                                       
      while (requirement.Value > 0 && pool >= requirement)                                                                                                             
      {                                                                                                                                                                
          pool -= requirement;                                                                                                                                         
          levelsGained++;                                                                                                                                              
          requirement = this.GetLevelRequirement(this.m_currentLevel + levelsGained);                                                                                  
      }                                                                                                                                                                
                                                                                                                                                                       
      this.m_currentLevel += levelsGained;                                                                                                                             
      this.m_consumed.ResourceController.UseResource(strengthToApply);                                                                                                 
      this.m_required.ResourceController.SetValue(pool);                                                                                                               
                                                                                                                                                                       
      for (var i = 0; i < levelsGained; i++)                                                                                                                           
          this.m_generatorRewardController.ApplyReward();                                                                                                              
                                                                                                                                                                       
      if (levelsGained > 0)                                                                                                                                            
          this.EmitSignalGeneratedReward();                                                                                                                            
  }                                                                                                                                                                    
                                                                                                                                                                       
  private IdleNumber GetLevelRequirement(int level)                                                                                                                    
  {                                                                                                                                                                    
      return level < this.ProgressionLevelData.FixedLevels.Count                                                                                                       
          ? this.ProgressionLevelData.FixedLevels[level].ToIdleNumber()                                                                                                
          : this.ProgressionLevelData.DynamicLevels.CalculateFor(new IdleNumber(level));                                                                               
  }  
}