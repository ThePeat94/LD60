using Godot;
using Nidavellir.GDResource.SkillTree;
using Nidavellir.IdleNumbers;

namespace Nidavellir.SkillTree.UI;

public partial class SkillTreeNodeUI : Control
{
    [Export] private SkillTreeItemData m_data;
    [Export] private TextureRect m_icon;
    [Export] private Label m_name;
    [Export] private Label m_amount;
    [Export] private Label m_cost;
    
    public SkillTreeItemData Data => this.m_data;

    public void Display(int amount, int maxAmount, IdleNumber cost)
    {
        this.m_icon.Texture = this.m_data.Icon;
        this.m_name.Text = this.m_data.SkillName;
        this.m_cost.Text = cost.ToString();
        this.m_amount.Text =  amount.ToString();
    }
}