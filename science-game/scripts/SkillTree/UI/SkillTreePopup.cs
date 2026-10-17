using Godot;

namespace Nidavellir.SkillTree.UI;

public partial class SkillTreePopup : Control
{
    [Export] private Label m_name;
    [Export] private Label m_description;
    [Export] private TextureRect m_icon;
    [Export] private Label m_cost;
    [Export] private Label m_amountDisplay;
}