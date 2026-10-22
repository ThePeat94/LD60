using System.Collections.Generic;
using Nidavellir.GDResource.SkillTree.EntityStatUpgrades.Employee;
using EmployeeData = Nidavellir.GDResource.Employee.EmployeeData;

namespace Nidavellir.Stats.Employee;

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
