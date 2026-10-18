using Godot;
using Nidavellir.Generator;

namespace Nidavellir.GDResource;

public partial class ShopUnlocker : Node, IGameFeatureUnlocker
{
    [Export] private Sprite2D m_shopSprite;
    [Export] private Area2D m_shopArea;

    public override void _Ready()
    {
        base._Ready();
        this.m_shopSprite.Visible = false;
        this.m_shopArea.InputPickable = false;
    }

    public void Unlock()
    {
        this.m_shopSprite.Visible = true;
        this.m_shopArea.InputPickable = true;
    }
}