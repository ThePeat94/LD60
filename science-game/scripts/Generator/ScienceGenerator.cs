using Godot;
using Nidavellir.GameFeel;
using Nidavellir.IdleNumbers;
using Nidavellir.PlayerStats;

namespace Nidavellir.Generator;

[GlobalClass]
public partial class ScienceGenerator : Node2D, IGenerator
{
    [Export]
    public ResourceControllerNode Generated { get; set; }

    [Export]
    public PlayerStatsContainer PlayerStatsContainer { get; set; }

    [Export]
    public SymbolStore Symbols { get; set; }

    public void OnClick(Node viewport, InputEvent evt, int shapeIdx)
    {
        if (evt is not InputEventMouseButton { Pressed: true }) return;

        this.Generated.ResourceController.Add(this.PlayerStatsContainer.ClickStrength.Value);
        this.Symbols?.AddAt(this.GetGlobalMousePosition());
    }

    public void Generate(IdleNumber value)
    {
        this.Generated.ResourceController.Add(value);
        this.Symbols?.AddRandom();
    }
}