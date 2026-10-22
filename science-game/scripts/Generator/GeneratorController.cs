using Godot;
using Nidavellir.GameFeel;
using Nidavellir.GDResource;
using Nidavellir.IdleNumbers;
using Nidavellir.Stats.Player;

namespace Nidavellir.Generator;

[GlobalClass]
public partial class GeneratorController : Node2D, IGenerator
{
    [Export] private ResourceControllerNode m_consumed;
    [Export] private ResourceControllerNode m_required;
    [Export] private GeneratorRewardController m_generatorRewardController;
    [Export] private SymbolStore m_symbols;
    [Export] private SymbolStore m_sourceSymbols;
    [Export] private CanvasItem m_payoutTarget;

    public ResourceControllerNode Generated => this.m_generatorRewardController.Generated;
    public ResourceControllerNode Required => this.m_required;
    public IdleNumber Requirement => this.GetLevelRequirement(this.m_currentLevel);
    public int CurrentLevel => this.m_currentLevel;
        
    [Export]
    public PlayerStatsContainer PlayerStatsContainer { get; set; }
    
    [Export]
    public ProgressionLevelData ProgressionLevelData { get; set; }
    
    [Signal]
    public delegate void GeneratedRewardEventHandler(int levels);

    [Signal]
    public delegate void ClickedEventHandler();

    [Signal]
    public delegate void ClickRejectedEventHandler();

    /// <summary>A click added <paramref name="amount"/> to the current batch.</summary>
    [Signal]
    public delegate void ProducedEventHandler(double amount);


    private int m_currentLevel = 0;

    public void OnClick(Node viewport, InputEvent evt, int shapeIdx)
    {
        if (evt is not InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left }) return;

        if (this.m_consumed.ResourceController.CurrentValue.Value <= 0)
        {
            this.EmitSignalClickRejected();
            return;
        }

        this.EmitSignalClicked();
        this.Generate(this.PlayerStatsContainer.ClickStrength.Value, this.GetGlobalMousePosition(), true);
    }
    
    public bool Generate(IdleNumber power, Vector2? origin = null)
    {
        return this.Generate(power, origin, false);
    }

    private bool Generate(IdleNumber power, Vector2? origin, bool isClick)
    {
        var available = this.m_consumed.ResourceController.CurrentValue;
        var strengthToApply = available >= power ? power : available;
        if (strengthToApply.Value <= 0) return false;

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

        this.MoveSymbols(origin, levelsGained > 0, pool.Value > 0);

        if (isClick)
            this.EmitSignalProduced(strengthToApply.ToDouble());

        if (levelsGained > 0)
            this.EmitSignalGeneratedReward(levelsGained);

        return true;
    }

    private void MoveSymbols(Vector2? origin, bool batchFilled, bool hasRemainder)
    {
        if (this.m_symbols == null) return;

        var landing = origin ?? this.m_symbols.RandomPoint();
        var payoutPosition = GetCenter(this.m_payoutTarget);

        if (batchFilled && payoutPosition.HasValue)
            this.m_symbols.FlyAllTo(payoutPosition.Value);

        // the converted symbol stays only if something is left for the next batch
        var landIn = !batchFilled || hasRemainder || !payoutPosition.HasValue ? this.m_symbols : null;
        var target = landIn != null ? landing : payoutPosition.Value;

        if (this.m_sourceSymbols == null)
        {
            landIn?.AddAt(target);
            return;
        }

        this.m_sourceSymbols.SendOne(target, landIn);

        // nothing left to consume: the source must not keep showing symbols
        if (this.m_consumed.ResourceController.CurrentValue.Value > 0) return;
        if (landIn != null)
            this.m_sourceSymbols.SendAll(landing, landIn);
        else
            this.m_sourceSymbols.FlyAllTo(target);
    }

    private static Vector2? GetCenter(CanvasItem target)
    {
        return target switch
        {
            Control control => control.GetGlobalRect().GetCenter(),
            Node2D node => node.GlobalPosition,
            _ => null,
        };
    }

    private IdleNumber GetLevelRequirement(int level)
    {
        return level < this.ProgressionLevelData.FixedLevels.Count
            ? this.ProgressionLevelData.FixedLevels[level].ToIdleNumber()
            : this.ProgressionLevelData.DynamicLevels.CalculateFor(new IdleNumber(level));
    }
}
