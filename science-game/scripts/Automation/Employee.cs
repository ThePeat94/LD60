using Godot;
using Nidavellir.GDResource;
using Nidavellir.Generator;

namespace Nidavellir.Automation;

public partial class Employee : Node
{
    [Export] private EmployeeData m_data;
    [Export] private Timer m_timer;
    
    private IGenerator m_targetGenerator;
    private EmployeeStats m_employeeStats;
    
    public override void _EnterTree()
    {
        base._EnterTree();
        this.m_timer.Autostart = false;
    }

    public override void _Ready()
    {
        base._Ready();
        this.m_timer.WaitTime = this.m_employeeStats.TickSpeed.Value.Value;
        this.m_timer.Timeout += this.OnTimerTick;
        this.m_timer.Start();
    }

    public void Initialize(IGenerator patentGenerator, EmployeeStats stats)
    {
        this.m_employeeStats = stats;
        this.m_targetGenerator = patentGenerator;
    }

    private void OnTimerTick()
    {
        this.m_targetGenerator.Generate(this.m_employeeStats.WorkForce.Value);
        this.m_timer.WaitTime = this.m_employeeStats.TickSpeed.Value.Value;
    }
}