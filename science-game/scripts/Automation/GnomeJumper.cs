using Godot;

namespace Nidavellir.Automation;

/// <summary>A gnome that hops back and forth between its spot and a second point, one hop per action.</summary>
[GlobalClass]
public partial class GnomeJumper : Sprite2D
{
    [Export] private Vector2 m_jumpOffset = new(6f, 0f);
    [Export] private float m_jumpHeight = 4f;
    [Export] private float m_jumpDuration = 0.25f;
    [Export] private AudioStreamPlayer2D m_jumpSound;

    private Vector2 m_home;
    private bool m_atHome = true;
    private Tween m_activeTween;

    public override void _Ready()
    {
        this.m_home = this.Position;
    }

    public void Jump()
    {
        if (!this.IsVisibleInTree()) return;

        // a new action mid-hop: land the current hop right away, then hop on
        this.m_activeTween?.Kill();
        var from = this.m_atHome ? this.m_home : this.m_home + this.m_jumpOffset;
        this.m_atHome = !this.m_atHome;
        var to = this.m_atHome ? this.m_home : this.m_home + this.m_jumpOffset;
        this.Position = from;

        this.m_activeTween = this.CreateTween();
        this.m_activeTween.TweenMethod(Callable.From<float>(t =>
        {
            var arc = 4f * t * (1f - t);
            this.Position = from.Lerp(to, t) + Vector2.Up * arc * this.m_jumpHeight;
        }), 0f, 1f, this.m_jumpDuration);

        this.m_jumpSound?.Play();
    }
}