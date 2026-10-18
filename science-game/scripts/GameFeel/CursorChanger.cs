using Godot;

namespace Nidavellir.GameFeel;

[GlobalClass]
public partial class CursorChanger : Node2D
{
    [Export] private Area2D m_clickArea;
    [Export] private CursorManager m_cursorManager;

    public override void _Ready()
    {
        base._Ready();
        this.m_cursorManager = GetNode<CursorManager>("/root/CursorManager");
        this.m_clickArea.MouseEntered += () => this.m_cursorManager.Enter(this.m_clickArea);
        this.m_clickArea.MouseExited += () => this.m_cursorManager.Exit(this.m_clickArea);
    }
}