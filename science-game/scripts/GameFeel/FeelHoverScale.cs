using Godot;

namespace Nidavellir.GameFeel;

/// <summary>Grows a control a little while the mouse is over it, for buttons that have no hover image.</summary>
[GlobalClass]
public partial class FeelHoverScale : Control
{
    [Export] private float m_hoverScale = 1.06f;
    [Export] private float m_duration = 0.1f;

    private Tween m_activeTween;

    public override void _Ready()
    {
        this.PivotOffset = this.Size / 2f;
        this.Resized += () => this.PivotOffset = this.Size / 2f;
        this.MouseEntered += () => this.TweenScale(this.m_hoverScale);
        this.MouseExited += () => this.TweenScale(1f);
        // a hidden window keeps no stale hover size for next time
        this.VisibilityChanged += () => this.Scale = Vector2.One;
    }

    private void TweenScale(float scale)
    {
        this.m_activeTween?.Kill();
        this.m_activeTween = this.CreateTween();
        this.m_activeTween.TweenProperty(this, "scale", Vector2.One * scale, this.m_duration)
            .SetTrans(Tween.TransitionType.Quad)
            .SetEase(Tween.EaseType.Out);
    }
}
