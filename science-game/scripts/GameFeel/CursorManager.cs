using System.Collections.Generic;
using Godot;

namespace Nidavellir.GameFeel;

public partial class CursorManager : Node
{
    private readonly HashSet<Area2D> m_hoveredAreas = new();

    public void Enter(Area2D area)
    {
        this.m_hoveredAreas.Add(area);
        this.UpdateCursor();
    }

    public void Exit(Area2D area)
    {
        this.m_hoveredAreas.Remove(area);
        this.UpdateCursor();
    }

    private void UpdateCursor()
    {
        Input.SetDefaultCursorShape(this.m_hoveredAreas.Count > 0
                ? Input.CursorShape.PointingHand
                : Input.CursorShape.Arrow
        );
    }
}