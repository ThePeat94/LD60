using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Nidavellir.GDResource;
using Nidavellir.GDResource.Employee;
using Nidavellir.GDResource.SkillTree;
using Nidavellir.Generator;
using EmployeeDataContainer = Nidavellir.GDResource.Employee.EmployeeDataContainer;
using EmployeeStatUpgradeData = Nidavellir.GDResource.SkillTree.EntityStatUpgrades.Employee.EmployeeStatUpgradeData;

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
    
    [Export] private int m_employeesPerGnome = 5;
    [Export] private Array<Sprite2D> m_thinkerGnomes;
    [Export] private Array<Sprite2D> m_inventorGnomes;
    [Export] private Array<Sprite2D> m_scribeGnomes;
    
    private readonly List<Employee> m_thinkers = [];
    private readonly List<Employee> m_inventors = [];
    private readonly List<Employee> m_scribes = [];
    private System.Collections.Generic.Dictionary<EmployeeType, EmployeeStats> m_employeeStats = new();

    public override void _EnterTree()
    {
        base._EnterTree();
        this.m_employeeStats = new()
        {
            [EmployeeType.Scribe] = new(this.m_employeeDataContainer.ScribeData),
            [EmployeeType.Thinker] = new(this.m_employeeDataContainer.ThinkerData),
            [EmployeeType.Inventor] = new(this.m_employeeDataContainer.InventorData),
        };

        this.HideGnomes(this.m_thinkerGnomes);
        this.HideGnomes(this.m_inventorGnomes);
        this.HideGnomes(this.m_scribeGnomes);
    }

    public void AddThinker()
    {
        this.AddEmployee(
            this.m_thinkers,
            this.m_scienceGenerator,
            this.m_thinkerEmployeeScene,
            this.m_employeeStats[EmployeeType.Thinker],
            this.m_thinkerGnomes
        );
    }

    public void AddInventor()
    {
        this.AddEmployee(
            this.m_inventors,
            this.m_patentGenerator,
            this.m_inventorEmployeeScene,
            this.m_employeeStats[EmployeeType.Inventor],
            this.m_inventorGnomes
        );
    }

    public void AddScribe()
    {
        this.AddEmployee(
            this.m_scribes, 
            this.m_skillpointGenerator, 
            this.m_scribeEmployeeScene, 
            this.m_employeeStats[EmployeeType.Scribe],
            this.m_scribeGnomes
        );
    }

    private void AddEmployee(List<Employee> employees, IGenerator affected, PackedScene employeeScene, EmployeeStats stats, Array<Sprite2D> gnomes)
    {
        var created = employeeScene.Instantiate<Employee>();
        created.Initialize(affected, stats);
        this.AddChild(created);
        employees.Add(created);
        
        var max = Math.Min(employees.Count / this.m_employeesPerGnome, gnomes.Count);
        for (var i = 0; i < max; i++)
        {
            gnomes[i].Visible = true;
        }
    }

    private void HideGnomes(Array<Sprite2D> gnomes)
    {
        foreach (var gnome in gnomes)
        {
            gnome.Visible = false;
        }
    }

    public void ApplyEmployeeUpgrades(EmployeeStatUpgradeData upgradeData)
    {
        var affectedEmployeeStats = this.m_employeeStats[upgradeData.TargetType];
        affectedEmployeeStats[upgradeData.TargetStat].AddEntityStatUpgrade(upgradeData);
    }
}