using Godot;
using Nidavellir.Generator;
using Nidavellir.IdleNumbers;

namespace Nidavellir.UI;

public partial class GeneratorProgressDisplay : ProgressBar
{
    [Export]
    public PatentGenerator PatentGenerator { get; set; }

    public override void _Ready()
    {
        this.PatentGenerator.ResourceControllerNode.ResourceController.ResourceValueChanged += this.OnGeneratorResourceChanged;
    }

    private void OnGeneratorResourceChanged(object sender, ResourceValueChangedEvent e)
    {
        this.Value = this.PatentGenerator.ResourceControllerNode.ResourceController.CurrentValue.GetRelativeAmountTo(this.PatentGenerator.Requirement);
    }
}