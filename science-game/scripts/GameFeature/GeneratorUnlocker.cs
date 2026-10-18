using Godot;

namespace Nidavellir.GDResource;

public partial class GeneratorUnlocker : Node, IGameFeatureUnlocker
{
    [Export] private Sprite2D m_curtains;
    [Export] private Area2D m_clickArea;


    public override void _Ready()
    {
        base._Ready();
        this.m_curtains.Visible = true;
        this.m_clickArea.InputPickable = false;
    }

    public void Unlock()
    {
        this.m_curtains.Visible = false;
        this.m_clickArea.InputPickable = true;
    }
}