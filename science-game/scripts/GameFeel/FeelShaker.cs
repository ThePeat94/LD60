using Godot;

namespace Nidavellir.GameFeel;

public partial class FeelShaker : Node
{
    [Export] private Node2D m_toShake;
    
    private Vector2 m_originalPosition;
    
    private Tween m_activeTween;

    public override void _EnterTree()
    {
        base._EnterTree();
        this.m_originalPosition = this.m_toShake.Position;
    }

    public void OnClick(Node viewport, InputEvent evt, int shapeIdx)
    {
        if (evt is InputEventMouseButton {Pressed: true, ButtonIndex: MouseButton.Left})
            this.Shake();
    }

    public void Shake()
    {
        this.m_activeTween?.Kill();
        this.m_toShake.Position = this.m_originalPosition;
        this.m_activeTween = this.CreateTween();
        this.m_activeTween.TweenProperty(this.m_toShake, "position:x", 4f, 0.04f).AsRelative();
        this.m_activeTween.TweenProperty(this.m_toShake, "position:x", -8f, 0.08f).AsRelative();
        this.m_activeTween.TweenProperty(this.m_toShake, "position:x", 4f, 0.04f).AsRelative();
    }
}