using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Nidavellir.GDResource.Employee;
using Nidavellir.Generator;
using Nidavellir.Stats.Employee;

namespace Nidavellir.Automation;

public partial class EmployeeController : Node
{
    private Action m_employeeBought;
    
    [Export] private PackedScene m_thinkerEmployeeScene;
    [Export] private PackedScene m_inventorEmployeeScene;
    [Export] private PackedScene m_scribeEmployeeScene;
    [Export] private GeneratorController m_patentGenerator;
    [Export] private GeneratorController m_skillpointGenerator;
    [Export] private ScienceGenerator m_scienceGenerator;
    [Export] private EmployeeStatsContainer m_employeeStats;
    
    [Export] private int m_employeesPerGnome = 5;
    [Export] private Array<Sprite2D> m_thinkerGnomes;
    [Export] private Array<Sprite2D> m_inventorGnomes;
    [Export] private Array<Sprite2D> m_scribeGnomes;
    
    private readonly List<Employee> m_thinkers = [];
    private readonly List<Employee> m_inventors = [];
    private readonly List<Employee> m_scribes = [];
    
    public IReadOnlyList<Employee> Thinkers => this.m_thinkers;
    public IReadOnlyList<Employee> Inventors => this.m_inventors;
    public IReadOnlyList<Employee> Scribes => this.m_scribes;
    
    public event Action EmployeeBought
    {
        add => this.m_employeeBought += value;
        remove => this.m_employeeBought -= value;
    }

    public override void _EnterTree()
    {
        base._EnterTree();


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
        var employeeIndex = employees.Count;
        employees.Add(created);
        created.GnomeProvider = () => this.GetGnome(employees, employeeIndex, gnomes);
        
        var max = Math.Min(employees.Count / this.m_employeesPerGnome, gnomes.Count);
        for (var i = 0; i < max; i++)
        {
            gnomes[i].Visible = true;
        }
        
        this.m_employeeBought?.Invoke();
    }

    /// <summary>Employees beyond the visible gnomes share them.</summary>
    private GnomeJumper GetGnome(List<Employee> employees, int employeeIndex, Array<Sprite2D> gnomes)
    {
        var visibleGnomes = Math.Min(employees.Count / this.m_employeesPerGnome, gnomes.Count);
        if (visibleGnomes == 0) return null;

        return gnomes[employeeIndex / this.m_employeesPerGnome % visibleGnomes] as GnomeJumper;
    }

    private void HideGnomes(Array<Sprite2D> gnomes)
    {
        foreach (var gnome in gnomes)
        {
            gnome.Visible = false;
        }
    }
}