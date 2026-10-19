using Godot;

namespace Nidavellir.UI;

/// <summary>
/// A ScrollContainer that can also be panned by holding the left mouse button and dragging,
/// including when the drag starts on top of a child (like a skill node or a buy button).
/// </summary>
[GlobalClass]
public partial class DragScrollContainer : ScrollContainer
{
    [Export] private float m_dragThreshold = 8f;

    private bool m_pressed;
    private bool m_dragged;
    private Vector2 m_pressPosition;
    private Vector2 m_pressScroll;

    /// <summary>
    /// Whether the click that is ending right now was a drag, so children can ignore it.
    /// Only meaningful while the mouse release is being handled.
    /// </summary>
    public bool WasDragged => this.m_dragged;

    /// <summary>True if <paramref name="node"/> sits in a drag container whose current click was a drag.</summary>
    public static bool IsDragClick(Node node)
    {
        for (var parent = node.GetParent(); parent != null; parent = parent.GetParent())
            if (parent is DragScrollContainer container)
                return container.WasDragged;
        return false;
    }

    // _Input sees the mouse before any child control does, so drags work on top of children too
    public override void _Input(InputEvent evt)
    {
        if (!this.IsVisibleInTree())
        {
            this.m_pressed = false;
            return;
        }

        var mouse = this.GetGlobalMousePosition();

        if (evt is InputEventMouseButton { ButtonIndex: MouseButton.Left } button)
        {
            if (button.Pressed)
            {
                this.m_pressed = this.GetGlobalRect().HasPoint(mouse);
                this.m_dragged = false;
                this.m_pressPosition = mouse;
                this.m_pressScroll = new Vector2(this.ScrollHorizontal, this.ScrollVertical);
            }
            else if (this.m_pressed)
            {
                this.m_pressed = false;
                // keep WasDragged true while children handle this release, then reset
                if (this.m_dragged)
                    this.CallDeferred(MethodName.ResetDrag);
            }
            return;
        }

        if (evt is not InputEventMouseMotion || !this.m_pressed) return;

        var offset = mouse - this.m_pressPosition;
        if (!this.m_dragged && offset.Length() < this.m_dragThreshold) return;

        this.m_dragged = true;
        this.ScrollHorizontal = (int)(this.m_pressScroll.X - offset.X);
        this.ScrollVertical = (int)(this.m_pressScroll.Y - offset.Y);
    }

    private void ResetDrag()
    {
        this.m_dragged = false;
    }
}