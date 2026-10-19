using Godot;
using Godot.Collections;

namespace Nidavellir.GameFeel;

public partial class FeelVfxIconSpawner : Node
{
    [Export] private Node2D m_origin;
    [Export] private PackedScene m_particles;
    [Export] private int m_maxParticles = 50;

    public void OnClick(Node viewport, InputEvent evt, int shapeIdx)
    {
        if (evt is not InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
            return;

        this.Spawn();
    }

    /// <summary>One particle per produced unit, so stronger clicks burst harder.</summary>
    public void SpawnAmount(double amount)
    {
        this.Spawn((int)Mathf.Clamp(Mathf.Round(amount), 1, this.m_maxParticles));
    }

    public void Spawn()
    {
        this.Spawn(0);
    }

    private void Spawn(int amount)
    {
        var mousePos = this.m_origin.GetGlobalMousePosition();
        var instantiated = this.m_particles.Instantiate<GpuParticles2D>();
        if (amount > 0)
            instantiated.Amount = amount;
        this.GetTree().CurrentScene.AddChild(instantiated);
        instantiated.GlobalPosition = mousePos;
        instantiated.Restart();
        instantiated.Finished += instantiated.QueueFree;
    }
}