using System.Collections.Generic;
using Godot;
using Nidavellir.GDResource.Employee;

namespace Nidavellir.Stats.Employee;

[GlobalClass]
public partial class EmployeeStatsContainer : Node
{
    [Export] private EmployeeDataContainer m_employeeDataContainer;
    
    private Dictionary<EmployeeType, EmployeeStats> m_employeeStats = new();
    
    public IReadOnlyDictionary<EmployeeType, EmployeeStats> EmployeeStats => this.m_employeeStats;

    public EmployeeStats this[EmployeeType index] => this.m_employeeStats[index];

    public override void _EnterTree()
    {
        base._EnterTree();
        this.m_employeeStats = new()
        {
            [EmployeeType.Scribe] = new(this.m_employeeDataContainer.ScribeData),
            [EmployeeType.Thinker] = new(this.m_employeeDataContainer.ThinkerData),
            [EmployeeType.Inventor] = new(this.m_employeeDataContainer.InventorData),
        };
    }
}