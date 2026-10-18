using Godot;
using Godot.Collections;

namespace Nidavellir.GDResource;

public partial class GeneratorUnlocker : Node, IGameFeatureUnlocker
{
    [Export] private Sprite2D m_curtains;
    [Export] private Area2D m_clickArea;
    [Export] private Array<Node2D> m_otherUnlocks;


    public override void _Ready()
    {
        base._Ready();
        this.m_curtains.Visible = true;
        this.m_clickArea.InputPickable = false;

        foreach (var unlock in m_otherUnlocks)
        {
            unlock.Visible = false;
        }
    }

    public void Unlock()
    {
        this.m_curtains.Visible = false;
        this.m_clickArea.InputPickable = true;
        foreach (var unlock in m_otherUnlocks)
        {
            unlock.Visible = true;
        }
    }
}