using Godot;

namespace Nidavellir.GameFeel;

[GlobalClass]
public partial class CursorInstaller : Node
{
    public override void _Ready()
    {
        var normal = GD.Load<Texture2D>("res://assets/art/cursor/normal.png");
        var hover = GD.Load<Texture2D>("res://assets/art/cursor/interactable.png");

        Input.SetCustomMouseCursor(normal, Input.CursorShape.Arrow);
        Input.SetCustomMouseCursor(hover, Input.CursorShape.PointingHand);
    }
}