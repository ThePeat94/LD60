using Godot;

namespace Nidavellir.UI;

/// <summary>
/// A clipped view on one big child: drag with the left mouse button to pan, mouse wheel to zoom towards the mouse.
/// Unlike a ScrollContainer it never scrolls on the wheel.
/// </summary>
[GlobalClass]
public partial class PanZoomView : Control, IDragSurface
{
    [Export] private Control m_content;
    [Export] private float m_dragThreshold = 8f;
    [Export] private float m_maxZoom = 1.5f;
    [Export] private float m_zoomStep = 1.1f;
    [Export] private float m_zoomDuration = 0.08f;

    private bool m_pressed;
    private bool m_dragged;
    private Vector2 m_pressMouse;
    private Vector2 m_pressContentPosition;
    private float m_zoom = 1f;
    private Tween m_zoomTween;

    public bool WasDragged => this.m_dragged;

    // fully zoomed out shows the whole content
    private float MinZoom => Mathf.Min(1f, Mathf.Min(this.Size.X / this.ContentSize.X, this.Size.Y / this.ContentSize.Y));

    private Vector2 ContentSize => this.m_content.CustomMinimumSize.Max(this.m_content.Size);

    public override void _Ready()
    {
        this.ClipContents = true;
        this.m_content.Size = this.ContentSize;
        this.Resized += () => this.ApplyView(this.m_content.Position);
        this.ResetView();
    }

    /// <summary>Back to 100% zoom, looking at the top-left corner.</summary>
    public void ResetView()
    {
        this.m_zoomTween?.Kill();
        this.m_zoom = 1f;
        this.ApplyView(Vector2.Zero);
    }

    // _Input sees the mouse before the content does, so drags and zooms also work on top of skill nodes
    public override void _Input(InputEvent evt)
    {
        if (!this.IsVisibleInTree())
        {
            this.m_pressed = false;
            return;
        }

        var mouse = this.GetLocalMousePosition();
        var mouseInside = new Rect2(Vector2.Zero, this.Size).HasPoint(mouse);

        if (evt is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.WheelUp or MouseButton.WheelDown } wheel)
        {
            if (!mouseInside) return;

            var factor = wheel.ButtonIndex == MouseButton.WheelUp ? this.m_zoomStep : 1f / this.m_zoomStep;
            this.ZoomTowards(mouse, this.m_zoom * factor);
            this.GetViewport().SetInputAsHandled();
            return;
        }

        if (evt is InputEventMouseButton { ButtonIndex: MouseButton.Left } button)
        {
            if (button.Pressed)
            {
                this.m_pressed = mouseInside;
                this.m_dragged = false;
                this.m_pressMouse = mouse;
                this.m_pressContentPosition = this.m_content.Position;
            }
            else if (this.m_pressed)
            {
                this.m_pressed = false;
                // keep WasDragged true while the content handles this release, then reset
                if (this.m_dragged)
                    this.CallDeferred(MethodName.ResetDrag);
            }
            return;
        }

        if (evt is not InputEventMouseMotion || !this.m_pressed) return;

        var offset = mouse - this.m_pressMouse;
        if (!this.m_dragged && offset.Length() < this.m_dragThreshold) return;

        this.m_dragged = true;
        this.ApplyView(this.m_pressContentPosition + offset);
    }

    private void ZoomTowards(Vector2 focus, float targetZoom)
    {
        targetZoom = Mathf.Clamp(targetZoom, this.MinZoom, this.m_maxZoom);
        // the content point under the mouse stays under the mouse
        var contentPoint = (focus - this.m_content.Position) / this.m_zoom;

        this.m_zoomTween?.Kill();
        this.m_zoomTween = this.CreateTween();
        this.m_zoomTween.TweenMethod(Callable.From<float>(zoom =>
        {
            this.m_zoom = zoom;
            this.ApplyView(focus - contentPoint * zoom);
        }), this.m_zoom, targetZoom, this.m_zoomDuration);
    }

    private void ApplyView(Vector2 position)
    {
        var scaled = this.ContentSize * this.m_zoom;
        this.m_content.Scale = Vector2.One * this.m_zoom;
        this.m_content.Position = new Vector2(ClampAxis(position.X, scaled.X, this.Size.X), ClampAxis(position.Y, scaled.Y, this.Size.Y));
    }

    // smaller than the view: centered; bigger: no empty space at the edges
    private static float ClampAxis(float position, float content, float view)
    {
        return content <= view ? (view - content) / 2f : Mathf.Clamp(position, view - content, 0f);
    }

    private void ResetDrag()
    {
        this.m_dragged = false;
    }
}