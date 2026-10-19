using System;
using Godot;
using Nidavellir.GDResource.SkillTree;
using Nidavellir.IdleNumbers;
using Nidavellir.UI;

namespace Nidavellir.SkillTree.UI;

public partial class SkillTreeNodeUI : PanelContainer
{
    [Export] private SkillTreeItemData m_data;
    [Export] private TextureRect m_icon;
    [Export] private Label m_name;
    [Export] private Label m_amount;
    [Export] private Label m_cost;
    [Export] private Label m_description;
    [Export] private Color m_hoverColor = Colors.LawnGreen;
    [Export] private Color m_lockedColor = Colors.DimGray;
    [Export] private PackedScene m_infoDialog;
    
    private bool m_canBePurchased;
    private bool m_isUnlocked;
    private Color m_originalColor;
 
    private Action<SkillTreeItemData> m_onClick;
    
    public event Action<SkillTreeItemData> OnClick
    {
        add => this.m_onClick += value;
        remove => this.m_onClick -= value;
    }
    
    public SkillTreeItemData Data => this.m_data;

    public override void _EnterTree()
    {
        this.m_originalColor = this.SelfModulate;
        base._EnterTree();
        this.MouseEntered += this.OnMouseEntered;
        this.MouseExited += this.OnMouseExited;
    }

    private void OnMouseEntered()
    {
        this.SelfModulate = this.m_hoverColor;
    }

    private void OnMouseExited()
    {
        this.SelfModulate = this.m_canBePurchased ? this.m_originalColor : this.m_lockedColor;
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
        this.m_icon.Texture = this.m_data.Icon;
        this.m_name.Text = this.m_data.SkillName;
        this.m_cost.Text = viewData.Cost.ToString();
        this.m_description.Text = this.m_data.Description;
        this.m_description.Visible = !string.IsNullOrEmpty(this.m_data.Description);
        this.m_amount.Text = $"{viewData.CurrentAmount}/{this.m_data.MaximumAmount}";
        this.m_canBePurchased = viewData.CanBePurchased;
        this.m_isUnlocked = viewData.IsUnlocked;
        this.SelfModulate = this.m_canBePurchased && this.m_isUnlocked ? this.m_originalColor : this.m_lockedColor;
    }
}