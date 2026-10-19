using System.Collections.Generic;
using System.Linq;
using Godot;
using Nidavellir.GDResource.SkillTree;
using Nidavellir.IdleNumbers;
using Nidavellir.UI;

namespace Nidavellir.SkillTree.UI;

public partial class SkillTreeUI : Control
{
    [Export] private Control m_rootNode;
    [Export] private PanZoomView m_view;
    [Export] private SkillTreeController  m_skillTreeController;

    [ExportGroup("Arrows")]
    [Export] private Texture2D m_arrowSheet;
    [Export] private float m_arrowScale = 4f;
    [Export] private float m_arrowGap = 8f;

    // hand-drawn arrows on the sheet: region, and where the arrow head starts (in sheet pixels from the left)
    private static readonly (Rect2 Region, float HeadStart)[] s_arrows =
    [
        (new Rect2(118, 27, 26, 9), 17f),
        (new Rect2(118, 39, 26, 8), 19f),
        (new Rect2(117, 49, 28, 9), 20f),
        (new Rect2(117, 62, 30, 8), 21f),
        (new Rect2(118, 75, 29, 8), 20f),
        (new Rect2(118, 87, 29, 9), 19f),
        (new Rect2(118, 99, 27, 9), 16f),
    ];
    
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
            skillNode.Resized += this.QueueRedraw;
        }

        this.QueueRedraw();
    }

    private void OnSkillNodeClick(SkillTreeItemData obj)
    {
        this.m_skillTreeController.AttemptPurchase(obj);
    }

    public void Open(Dictionary<SkillTreeItemData, SkillTreeNodeViewData> boughtUpgrades)
    {
        if (!this.m_rootNode.Visible)
        {
            this.m_view.ResetView();
        }

        this.SetData(boughtUpgrades);
        this.m_rootNode.Visible = true;
    }

    public void SetData(Dictionary<SkillTreeItemData, SkillTreeNodeViewData> boughtUpgrades)
    {
        foreach (var skillItem in boughtUpgrades)
        {
            if (!this.m_uiForData.TryGetValue(skillItem.Key, out var ui))
                continue;

            ui.Visible = skillItem.Value.IsUnlocked;
            var viewData = boughtUpgrades[skillItem.Key];
            ui.Display(viewData);
        }
        this.QueueRedraw();
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
            if (!skillNode.Visible)
                continue;
            
            var prerequisites = skillNode.Data.PreRequisites;

            if (prerequisites is null || prerequisites.Count == 0)
                continue;

            var target = this.GetLocalRect(skillNode, inverseTransform);

            foreach (var prerequisite in prerequisites)
            {
                if (!this.m_uiForData.TryGetValue(prerequisite, out var requiredNode))
                    continue;

                var source = this.GetLocalRect(requiredNode, inverseTransform);
                var variant = (this.m_nodes.IndexOf(requiredNode) * 31 + this.m_nodes.IndexOf(skillNode)) % s_arrows.Length;
                this.DrawArrow(source, target, s_arrows[variant]);
            }
        }

        this.DrawSetTransform(Vector2.Zero);
    }

    private Rect2 GetLocalRect(Control node, Transform2D inverseTransform)
    {
        var center = inverseTransform * (node.GetGlobalTransform() * (node.Size / 2f));
        return new Rect2(center - node.Size / 2f, node.Size);
    }

    /// <summary>Arrow from the edge of <paramref name="from"/> to the edge of <paramref name="to"/>: the shaft stretches, the head keeps its shape.</summary>
    private void DrawArrow(Rect2 from, Rect2 to, (Rect2 Region, float HeadStart) arrow)
    {
        if (this.m_arrowSheet == null) return;

        var direction = (to.GetCenter() - from.GetCenter()).Normalized();
        var start = ExitPoint(from, direction) + direction * this.m_arrowGap;
        var end = ExitPoint(to, -direction) - direction * this.m_arrowGap;
        var length = start.DistanceTo(end);
        if (length <= 0f || (end - start).Dot(direction) <= 0f) return;

        var height = arrow.Region.Size.Y * this.m_arrowScale;
        var headWidth = Mathf.Min((arrow.Region.Size.X - arrow.HeadStart) * this.m_arrowScale, length);
        var shaftWidth = length - headWidth;
        var shaftRegion = new Rect2(arrow.Region.Position, new Vector2(arrow.HeadStart, arrow.Region.Size.Y));
        var headRegion = new Rect2(arrow.Region.Position + new Vector2(arrow.HeadStart, 0f), new Vector2(arrow.Region.Size.X - arrow.HeadStart, arrow.Region.Size.Y));

        this.DrawSetTransform(start, direction.Angle());
        if (shaftWidth > 0f)
            this.DrawTextureRectRegion(this.m_arrowSheet, new Rect2(0f, -height / 2f, shaftWidth, height), shaftRegion);
        this.DrawTextureRectRegion(this.m_arrowSheet, new Rect2(shaftWidth, -height / 2f, headWidth, height), headRegion);
    }

    /// <summary>Where a ray from the rect's center in <paramref name="direction"/> leaves the rect.</summary>
    private static Vector2 ExitPoint(Rect2 rect, Vector2 direction)
    {
        var half = rect.Size / 2f;
        var tx = Mathf.Abs(direction.X) > 0.0001f ? half.X / Mathf.Abs(direction.X) : float.MaxValue;
        var ty = Mathf.Abs(direction.Y) > 0.0001f ? half.Y / Mathf.Abs(direction.Y) : float.MaxValue;
        return rect.GetCenter() + direction * Mathf.Min(tx, ty);
    }
}