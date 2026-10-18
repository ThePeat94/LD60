using Godot;
using Nidavellir.Generator;
using Nidavellir.IdleNumbers;

namespace Nidavellir.UI;

[GlobalClass]
public partial class GeneratorRemainingDisplay : Label
{
    [Export] private GeneratorController m_generator;

    public override void _Ready()
    {
        this.m_generator.Required.ResourceController.ResourceValueChanged += this.OnGeneratorResourceChanged;
        this.UpdateText();
    }

    private void OnGeneratorResourceChanged(object sender, ResourceValueChangedEvent e)
    {
        this.UpdateText();
    }

    private void UpdateText()
    {
        this.Text = $"{this.m_generator.Requirement - this.m_generator.Required.ResourceController.CurrentValue}";
    }
}