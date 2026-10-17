using System;
using Godot;
using Nidavellir.IdleNumbers;
using Nidavellir.ResourceInfo;

namespace Nidavellir.UI;

public partial class ResourceDisplay : Label
{
    [Export]
    public ResourceType TypeToDisplay { get; set; }
    
    [Export]
    public ResourceControllerNode ResourceControllerNode { get; set; }

    public override void _Ready()
    {
        this.ResourceControllerNode.ResourceController.ResourceValueChanged += this.TypeToDisplay switch
        {
            ResourceType.Science => this.OnScienceValueChanged,
            ResourceType.Money => this.OnMoneyValueChanged,
            ResourceType.Skillpoint => this.OnSkillPointValueChanged,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void OnScienceValueChanged(object sender, ResourceValueChangedEvent e)
    {
        this.Text = $"Science: {e.NewValue}";
    }
    
    private void OnMoneyValueChanged(object sender, ResourceValueChangedEvent e)
    {
        this.Text = $"Money: {e.NewValue}";
    }

    private void OnSkillPointValueChanged(object sender, ResourceValueChangedEvent e)
    {
        this.Text = $"Skill Points: {e.NewValue}";
    }
}