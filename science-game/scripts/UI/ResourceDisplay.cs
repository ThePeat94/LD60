using System;
using Godot;
using Nidavellir.IdleNumbers;
using Nidavellir.ResourceInfo;

namespace Nidavellir.UI;

public partial class ResourceDisplay : Label
{
    [Export]
    public ResourceControllerNode ResourceControllerNode { get; set; }

    public override void _Ready()
    {
        this.ResourceControllerNode.ResourceController.ResourceValueChanged += this.OnValueChanged;
    }
    
    private void OnValueChanged(object sender, ResourceValueChangedEvent e)
    {
        this.Text = $"{e.NewValue}";
    }
}