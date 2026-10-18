using Godot;
using Godot.Collections;

namespace Nidavellir.GameFeel;

public partial class FeelVfxIconSpawner : Node
{
    [Export] private Node2D m_origin;
    [Export] private PackedScene m_particles;

    public void OnClick(Node viewport, InputEvent evt, int shapeIdx)
    {
        if (evt is not InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
            return;

        var mousePos = this.m_origin.GetGlobalMousePosition();
        var instantiated = this.m_particles.Instantiate<GpuParticles2D>();
        this.GetTree().CurrentScene.AddChild(instantiated);
        instantiated.GlobalPosition = mousePos;
        instantiated.Restart();
        instantiated.Finished += instantiated.QueueFree;
    }
}