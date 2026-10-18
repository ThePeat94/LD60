using System;
using System.Collections.Generic;
using Godot;
using Nidavellir.GDResource;
using Nidavellir.GDResource.SkillTree;
using Nidavellir.Generator;

namespace Nidavellir.Automation;

public partial class EmployeeController : Node
{
    [Export] private PackedScene m_thinkerEmployeeScene;
    [Export] private PackedScene m_inventorEmployeeScene;
    [Export] private PackedScene m_scribeEmployeeScene;
    [Export] private GeneratorController m_patentGenerator;
    [Export] private GeneratorController m_skillpointGenerator;
    [Export] private ScienceGenerator m_scienceGenerator;
    [Export] private EmployeeDataContainer m_employeeDataContainer;
    
    private readonly List<Employee> m_thinkers = [];
    private readonly List<Employee> m_inventors = [];
    private readonly List<Employee> m_scribes = [];
    private Dictionary<EmployeeType, EmployeeStats> m_employeeStats = new();

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

    public void AddThinker()
    {
        this.AddEmployee(
            this.m_thinkers,
            this.m_scienceGenerator,
            this.m_thinkerEmployeeScene,
            this.m_employeeStats[EmployeeType.Thinker]
        );
    }

    public void AddInventor()
    {
        this.AddEmployee(
            this.m_inventors,
            this.m_patentGenerator,
            this.m_inventorEmployeeScene,
            this.m_employeeStats[EmployeeType.Inventor]
        );
    }

    public void AddScribe()
    {
        this.AddEmployee(
            this.m_scribes, 
            this.m_skillpointGenerator, 
            this.m_scribeEmployeeScene, 
            this.m_employeeStats[EmployeeType.Scribe]
        );
    }

    private void AddEmployee(List<Employee> employees, IGenerator affected, PackedScene employeeScene, EmployeeStats stats)
    {
        var created = employeeScene.Instantiate<Employee>();
        created.Initialize(affected, stats);
        this.AddChild(created);
        employees.Add(created);
    }

    public void ApplyEmployeeUpgrades(EmployeeStatUpgradeData upgradeData)
    {
        var affectedEmployeeStats = this.m_employeeStats[upgradeData.TargetType];
        affectedEmployeeStats[upgradeData.TargetStat].AddEntityStatUpgrade(upgradeData);
    }
}