using Godot;
using Nidavellir.Generator;
using Nidavellir.IdleNumbers;

namespace Nidavellir.UI;

public partial class GeneratorProgressDisplay : ProgressBar
{
    [Export] private GeneratorController m_generator;

    public override void _Ready()
    {
        this.m_generator.Required.ResourceController.ResourceValueChanged += this.OnGeneratorResourceChanged;
    }

    private void OnGeneratorResourceChanged(object sender, ResourceValueChangedEvent e)
    {
        this.Value = this.m_generator.Required.ResourceController.CurrentValue.GetRelativeAmountTo(this.m_generator.Requirement);
    }
}