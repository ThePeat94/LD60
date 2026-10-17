using System.Collections.Generic;
using Godot;
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
    
    private readonly List<Employee> m_thinkers = [];
    private readonly List<Employee> m_inventors = [];
    private readonly List<Employee> m_scribes = [];
    
    public void AddThinker()
    {
        var created = this.m_thinkerEmployeeScene.Instantiate<Employee>();
        this.AddChild(created);
        created.Initialize(this.m_scienceGenerator);
        this.m_thinkers.Add(created);
    }

    public void AddInventor()
    {
        var created = this.m_inventorEmployeeScene.Instantiate<Employee>();
        this.AddChild(created);
        created.Initialize(this.m_patentGenerator);
        this.m_inventors.Add(created);
    }

    public void AddScribe()
    {
        var created = this.m_scribeEmployeeScene.Instantiate<Employee>();
        this.AddChild(created);
        created.Initialize(this.m_skillpointGenerator);
        this.m_scribes.Add(created);
    }
}