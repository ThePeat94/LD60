using System;
using Godot;
using Nidavellir.GDResource.SkillTree;
using Nidavellir.IdleNumbers;

namespace Nidavellir.SkillTree.UI;

public partial class SkillTreeNodeUI : PanelContainer
{
    [Export] private SkillTreeItemData m_data;
    [Export] private TextureRect m_icon;
    [Export] private Label m_name;
    [Export] private Label m_amount;
    [Export] private Label m_cost;
 
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
    }

    private void OnMouseEntered()
    {
        
    }

    public override void _GuiInput(InputEvent evt)
    {
        base._GuiInput(evt);
        if (evt is not InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
        {
            return;
        }
        this.AcceptEvent();
        GD.Print("CLICK SKILL TREE NODE");
        this.m_onClick?.Invoke(this.Data);
    }

    public void Display(int amount, int maxAmount, IdleNumber cost)
    {
        this.m_icon.Texture = this.m_data.Icon;
        this.m_name.Text = this.m_data.SkillName;
        this.m_cost.Text = cost.ToString();
        this.m_amount.Text = $"{amount}/{maxAmount}";
    }
}