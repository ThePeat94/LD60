using Godot;

namespace Nidavellir.GameFeel;

[GlobalClass]
public partial class FeelPopupLabel : Node
{
    [Export] private Node2D m_origin;
    [Export] private string m_text = "Not enough Thinking done yet!";
    [Export] private int m_fontSize = 32;
    [Export] private float m_riseDistance = 40f;
    [Export] private float m_duration = 1.2f;
    [Export] private bool m_followMouse = true;
    // e.g. a HUD CanvasLayer, so the label shows above UI; defaults to the current scene
    [Export] private Node m_labelParent;

    private Label m_label;
    private Tween m_activeTween;

    public void Trigger()
    {
        var label = this.GetOrCreateLabel();

        this.m_activeTween?.Kill();
        label.Size = label.GetCombinedMinimumSize();
        // asking the label itself gives the mouse in the label's own canvas (world or HUD layer)
        var center = this.m_followMouse ? label.GetGlobalMousePosition() : this.m_origin.GlobalPosition;
        label.GlobalPosition = center - label.Size / 2;
        label.Modulate = Colors.White;
        label.Visible = true;

        this.m_activeTween = this.CreateTween();
        this.m_activeTween.TweenProperty(label, "position:y", -this.m_riseDistance, this.m_duration).AsRelative();
        this.m_activeTween.Parallel().TweenProperty(label, "modulate:a", 0f, this.m_duration * 0.5f).SetDelay(this.m_duration * 0.5f);
        this.m_activeTween.TweenCallback(Callable.From(() => label.Visible = false));
    }

    private Label GetOrCreateLabel()
    {
        if (this.m_label != null) return this.m_label;

        this.m_label = new Label
        {
            Text = this.m_text,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            MouseFilter = Control.MouseFilterEnum.Ignore,
            ZIndex = 20,
            Visible = false,
        };
        this.m_label.AddThemeFontSizeOverride("font_size", this.m_fontSize);
        this.m_label.AddThemeColorOverride("font_color", Colors.White);
        this.m_label.AddThemeColorOverride("font_outline_color", Colors.Black);
        this.m_label.AddThemeConstantOverride("outline_size", 7);
        (this.m_labelParent ?? this.GetTree().CurrentScene).AddChild(this.m_label);
        return this.m_label;
    }
}