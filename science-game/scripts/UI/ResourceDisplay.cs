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
        switch (this.TypeToDisplay)
        {
            case ResourceType.Science:
                this.ResourceControllerNode.ResourceController.ResourceValueChanged += this.OnScienceValueChanged;
                break;
            case ResourceType.Money:
                this.ResourceControllerNode.ResourceController.ResourceValueChanged += this.OnMoneyValueChanged;
                break;
        }
    }

    private void OnScienceValueChanged(object sender, ResourceValueChangedEvent e)
    {
        this.Text = $"Science: {e.NewValue}";
    }
    
    private void OnMoneyValueChanged(object sender, ResourceValueChangedEvent e)
    {
        this.Text = $"Money: {e.NewValue}";
    }
}