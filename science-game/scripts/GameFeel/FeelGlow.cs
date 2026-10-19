using Godot;

namespace Nidavellir.GameFeel;

/// <summary>A glow that fades in when active and gently pulses while it is shown.</summary>
[GlobalClass]
public partial class FeelGlow : Sprite2D
{
    [Export] private float m_fadeDuration = 0.3f;
    [Export] private float m_pulseSpeed = 3f;
    [Export(PropertyHint.Range, "0,1")] private float m_pulseMinAlpha = 0.55f;

    private bool m_active;
    private float m_strength;
    private float m_time;

    public override void _Ready()
    {
        this.Visible = false;
    }

    public void SetActive(bool active)
    {
        this.m_active = active;
    }

    public override void _Process(double delta)
    {
        this.m_time += (float)delta;
        this.m_strength = Mathf.MoveToward(this.m_strength, this.m_active ? 1f : 0f, (float)delta / this.m_fadeDuration);

        this.Visible = this.m_strength > 0f;
        if (!this.Visible) return;

        var pulse = 0.5f + 0.5f * Mathf.Sin(this.m_time * this.m_pulseSpeed);
        this.Modulate = this.Modulate with { A = this.m_strength * Mathf.Lerp(this.m_pulseMinAlpha, 1f, pulse) };
    }
}