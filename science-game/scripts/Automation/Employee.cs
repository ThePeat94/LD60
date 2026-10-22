using System;
using Godot;
using Nidavellir.GDResource;
using Nidavellir.Generator;
using Nidavellir.Stats.Employee;
using EmployeeData = Nidavellir.GDResource.Employee.EmployeeData;

namespace Nidavellir.Automation;

public partial class Employee : Node
{
    [Export] private EmployeeData m_data;
    [Export] private Timer m_timer;
    
    private IGenerator m_targetGenerator;

    /// <summary>The gnome doing this employee's work right now, if one is visible.</summary>
    public Func<GnomeJumper> GnomeProvider { get; set; }
    private EmployeeStats m_employeeStats;
    
    public override void _EnterTree()
    {
        base._EnterTree();
        this.m_timer.Autostart = false;
    }

    public override void _Ready()
    {
        base._Ready();
        this.m_timer.WaitTime = this.m_employeeStats.TickSpeed.Value.ToDouble();
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
        // one action = one jump, and the action's symbols start at the gnome
        var gnome = this.GnomeProvider?.Invoke();
        if (this.m_targetGenerator.Generate(this.m_employeeStats.WorkForce.Value, gnome?.GlobalPosition))
            gnome?.Jump();
        this.m_timer.WaitTime = this.m_employeeStats.TickSpeed.Value.ToDouble();
    }
}