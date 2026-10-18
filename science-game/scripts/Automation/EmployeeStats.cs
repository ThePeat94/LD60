using System.Collections.Generic;
using Nidavellir.GDResource;
using Nidavellir.GDResource.SkillTree;
using Nidavellir.IdleNumbers;
using Nidavellir.Stats;

namespace Nidavellir.Automation;

public class EmployeeStats(EmployeeData data)
{
    private readonly Dictionary<EmployeeStatTarget, EntityStat> m_stats = new()
    {
        [EmployeeStatTarget.BaseForce] = new(data.BaseWorkForce.ToIdleNumber()),
        [EmployeeStatTarget.TickSpeed] = new(data.BaseTickSpeed.ToIdleNumber()),
    };
    
    public EntityStat WorkForce => this.m_stats[EmployeeStatTarget.BaseForce];
    public EntityStat TickSpeed => this.m_stats[EmployeeStatTarget.TickSpeed];
    public EntityStat this[EmployeeStatTarget target] => this.m_stats[target];
}
