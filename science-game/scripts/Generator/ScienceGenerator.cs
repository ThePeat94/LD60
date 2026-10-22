using Godot;
using Nidavellir.GameFeel;
using Nidavellir.IdleNumbers;
using Nidavellir.Stats.Player;

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

    /// <summary>A click produced <paramref name="amount"/> science.</summary>
    [Signal]
    public delegate void ProducedEventHandler(double amount);

    public void OnClick(Node viewport, InputEvent evt, int shapeIdx)
    {
        if (evt is not InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left }) return;

        var produced = this.PlayerStatsContainer.ClickStrength.Value;
        this.Generated.ResourceController.Add(produced);
        this.Symbols?.AddAt(this.GetGlobalMousePosition());
        this.EmitSignalProduced(produced.ToDouble());
    }

    public bool Generate(IdleNumber value, Vector2? origin = null)
    {
        this.Generated.ResourceController.Add(value);
        this.Symbols?.AddAt(origin ?? this.Symbols.RandomPoint());
        return true;
    }
}