using Godot;
using Nidavellir.GameFeel;
using Nidavellir.Generator;
using Nidavellir.IdleNumbers;

namespace Nidavellir.UI;

[GlobalClass]
public partial class GeneratorProgressBar : Node2D
{
    [Export] private GeneratorController m_generator;
    [Export] private TextureProgressBar m_bar;
    [Export] private FeelPopupLabel m_filledPopup;
    [Export] private float m_fillDuration = 0.15f;

    private Tween m_activeTween;

    public override void _Ready()
    {
        this.m_generator.Required.ResourceController.ResourceValueChanged += this.OnGeneratorResourceChanged;
        this.m_generator.GeneratedReward += this.OnGeneratorFilled;
        this.m_bar.Value = this.GetProgress();
    }

    private void OnGeneratorResourceChanged(object sender, ResourceValueChangedEvent e)
    {
        var target = this.GetProgress();

        this.m_activeTween?.Kill();
        // a batch was paid out: restart from empty instead of draining backwards
        if (target < this.m_bar.Value)
            this.m_bar.Value = 0;

        this.m_activeTween = this.CreateTween();
        this.m_activeTween.TweenProperty(this.m_bar, "value", target, this.m_fillDuration);
    }

    private void OnGeneratorFilled()
    {
        this.m_filledPopup?.Trigger();
    }

    private double GetProgress()
    {
        var requirement = this.m_generator.Requirement;
        if (requirement.Value <= 0) return 0;

        return this.m_generator.Required.ResourceController.CurrentValue.GetRelativeAmountTo(requirement);
    }
}