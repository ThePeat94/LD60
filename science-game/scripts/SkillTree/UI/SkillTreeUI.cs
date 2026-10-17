using System.Collections.Generic;
using System.Linq;
using Godot;
using Nidavellir.GDResource.SkillTree;

namespace Nidavellir.SkillTree.UI;

public partial class SkillTreeUI : Control
{
    private List<SkillTreeNodeUI> m_nodes = [];
    private Dictionary<SkillTreeItemData, SkillTreeNodeUI> m_uiForData = new();

    public override void _Ready()
    {
        base._Ready();
        this.m_nodes = [.. this.GetChildren().OfType<SkillTreeNodeUI>()];
        this.m_uiForData = this.m_nodes.ToDictionary(x => x.Data);
        this.QueueRedraw();
    }

    public override void _Draw()
    {
        base._Draw();

        var inverseTransform = this.GetGlobalTransform().AffineInverse();

        foreach (var skillNode in this.m_nodes)
        {
            var prerequisites = skillNode.Data.PreRequisites;

            if (prerequisites is null || prerequisites.Count == 0)
                continue;

            var target = inverseTransform * (
                skillNode.GetGlobalTransform() * (skillNode.Size / 2f)
            );

            foreach (var prerequisite in prerequisites)
            {
                if (!this.m_uiForData.TryGetValue(prerequisite, out var requiredNode))
                    continue;

                var source = inverseTransform * (
                    requiredNode.GetGlobalTransform() * (requiredNode.Size / 2f)
                );

                this.DrawLine(
                    source,
                    target,
                    Colors.White,
                    3f,
                    true
                );
            }
        }
    }
}