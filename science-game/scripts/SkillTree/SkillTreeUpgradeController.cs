using System.Collections.Generic;
using Godot;
using Nidavellir.GDResource.SkillTree;
using Nidavellir.PlayerStats;

namespace Nidavellir.SkillTree;

public partial class SkillTreeUpgradeController : Node
{
    [Export] private PlayerStatsContainer m_playerStats;

    public void ApplyPlayerStatUpgrade(EntityStatUpgradeData u)
    {
        if (u.ClickStrengthUpgrade is not null)
        {
            this.m_playerStats.ClickStrength.AddEntityStatUpgrade(u);
        }
    }
}