using System.Collections.Generic;
using System.Linq;
using Godot;
using Nidavellir.GDResource.SkillTree;
using Nidavellir.IdleNumbers;

namespace Nidavellir.SkillTree.UI;

public partial class SkillTreeUI : Control
{
    [Export] private Control m_rootNode;
    [Export] private SkillTreeController  m_skillTreeController;
    
    private List<SkillTreeNodeUI> m_nodes = [];
    private Dictionary<SkillTreeItemData, SkillTreeNodeUI> m_uiForData = new();

    public override void _Ready()
    {
        base._Ready();
        this.m_nodes = [.. this.GetChildren().OfType<SkillTreeNodeUI>()];
        this.m_uiForData = this.m_nodes.ToDictionary(x => x.Data);

        foreach (var skillNode in this.m_nodes)
        {
            skillNode.OnClick += this.OnSkillNodeClick;
        }
        
        this.QueueRedraw();
    }

    private void OnSkillNodeClick(SkillTreeItemData obj)
    {
        this.m_skillTreeController.AttemptPurchase(obj);
    }

    public void Open(Dictionary<SkillTreeItemData, int> boughtUpgrades)
    {
        foreach (var skillItem in boughtUpgrades)
        {
            if (!this.m_uiForData.TryGetValue(skillItem.Key, out var ui))
                continue;
            
            var boughtAmount = boughtUpgrades[skillItem.Key];
            ui.Display(boughtAmount,skillItem.Key.MaximumAmount, skillItem.Key.CostModel.CalculateFor(new IdleNumber(boughtAmount)));
        }

        this.m_rootNode.Visible = true;
    }

    public void Close()
    {
        this.m_rootNode.Visible = false;
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