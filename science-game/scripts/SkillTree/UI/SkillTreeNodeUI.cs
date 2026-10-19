using System;
using Godot;
using Godot.Collections;
using Nidavellir.GDResource.SkillTree;
using Nidavellir.GDResource.SkillTree.FeatureUnlock;
using Nidavellir.IdleNumbers;
using Nidavellir.UI;

namespace Nidavellir.SkillTree.UI;

public partial class SkillTreeNodeUI : PanelContainer
{
    [Export] private SkillTreeItemData m_data;
    [Export] private Label m_name;
    [Export] private Label m_amount;
    [Export] private Label m_cost;
    [Export] private Label m_description;
    [Export] private PackedScene m_infoDialog;

    [ExportGroup("Look")]
    // hand-drawn frames: scrolls for unlocking new features, boxes for everything else
    [Export] private Array<StyleBox> m_boxFrames;
    [Export] private Array<StyleBox> m_scrollFrames;
    [Export(PropertyHint.Range, "0,1")] private float m_unaffordableAlpha = 0.45f;
    [Export] private float m_hoverScale = 1.05f;

    private bool m_canBePurchased;
    private bool m_isUnlocked;
    private Tween m_hoverTween;
 
    private Action<SkillTreeItemData> m_onClick;
    
    public event Action<SkillTreeItemData> OnClick
    {
        add => this.m_onClick += value;
        remove => this.m_onClick -= value;
    }
    
    public SkillTreeItemData Data => this.m_data;

    public override void _EnterTree()
    {
        base._EnterTree();
        this.MouseEntered += this.OnMouseEntered;
        this.MouseExited += this.OnMouseExited;
        this.Resized += () => this.PivotOffset = this.Size / 2f;
    }

    public override void _Ready()
    {
        base._Ready();
        this.ApplyFrame();
    }

    private void ApplyFrame()
    {
        var unlocksFeature = this.m_data?.Upgrade is UnlockGameFeatureData or UnlockShopFeatureData or GameWonUpgradeData;
        var frames = unlocksFeature ? this.m_scrollFrames : this.m_boxFrames;
        if (frames == null || frames.Count == 0) return;

        // alternate between the drawings so neighbours don't look copy-pasted
        this.AddThemeStyleboxOverride("panel", frames[this.GetIndex() % frames.Count]);
    }

    private void OnMouseEntered()
    {
        this.TweenScale(this.m_hoverScale);
    }

    private void OnMouseExited()
    {
        this.TweenScale(1f);
    }

    private void TweenScale(float scale)
    {
        this.m_hoverTween?.Kill();
        this.m_hoverTween = this.CreateTween();
        this.m_hoverTween.TweenProperty(this, "scale", Vector2.One * scale, 0.1f)
            .SetTrans(Tween.TransitionType.Quad)
            .SetEase(Tween.EaseType.Out);
    }
    

    public override void _GuiInput(InputEvent evt)
    {
        base._GuiInput(evt);
        // act on release, so pressing a node to drag the tree around doesn't buy it
        if (evt is not InputEventMouseButton { Pressed: false, ButtonIndex: MouseButton.Left } button)
        {
            return;
        }

        // released outside the node, or the press turned into dragging the tree
        if (!new Rect2(Vector2.Zero, this.Size).HasPoint(button.Position) || DragScrollContainer.IsDragClick(this)) return;

        // unaffordable clicks still go through, the controller refuses them with feedback
        if (!this.m_isUnlocked) return;
        
        this.AcceptEvent();
        GD.Print("CLICK SKILL TREE NODE");
        this.m_onClick?.Invoke(this.Data);
    }

    public void Display(SkillTreeNodeViewData viewData)
    {
        this.m_name.Text = this.m_data.SkillName;
        this.m_cost.Text = viewData.Cost.ToString();
        this.m_description.Text = this.m_data.Description;
        this.m_description.Visible = !string.IsNullOrEmpty(this.m_data.Description);
        this.m_amount.Text = $"{viewData.CurrentAmount}/{this.m_data.MaximumAmount}";
        this.m_canBePurchased = viewData.CanBePurchased;
        this.m_isUnlocked = viewData.IsUnlocked;
        // faded ink for what can't be bought right now
        this.Modulate = this.Modulate with { A = this.m_canBePurchased && this.m_isUnlocked ? 1f : this.m_unaffordableAlpha };
    }
}