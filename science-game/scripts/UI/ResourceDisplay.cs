using Godot;
using Nidavellir.IdleNumbers;
using Nidavellir.ResourceInfo;

namespace Nidavellir.UI;

public partial class ResourceDisplay : Label
{
    [Export]
    public ResourceType TypeToDisplay { get; set; }
    
    [Export]
    public ResourceControllerContainer ControllerContainer { get; set; }

    public override void _Ready()
    {
        switch (this.TypeToDisplay)
        {
            case ResourceType.Science:
                this.ControllerContainer.ScienceResourceController.ResourceValueChanged += OnScienceValueChanged;
                break;
        }
    }

    private void OnScienceValueChanged(object sender, ResourceValueChangedEvent e)
    {
        this.Text = $"Science: {e.NewValue}";
    }
}