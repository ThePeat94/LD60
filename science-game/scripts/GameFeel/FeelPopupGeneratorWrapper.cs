using Godot;
using Nidavellir.Generator;

namespace Nidavellir.GameFeel;

[GlobalClass]
public partial class FeelPopupGeneratorWrapper : Node
{
    [Export] private GeneratorController m_generator;
    [Export] private FeelPopupLabel m_popup;
    [Export] private string m_textFormat = "{0}";

    public override void _Ready()
    {
        this.m_generator.GeneratedReward += this.OnGeneratedReward;
    }

    public override void _ExitTree()
    {
        this.m_generator.GeneratedReward -= this.OnGeneratedReward;
    }

    private void OnGeneratedReward(int levelsGained)
    {
        this.m_popup.TriggerWithText(string.Format(this.m_textFormat, levelsGained));
    }
}
