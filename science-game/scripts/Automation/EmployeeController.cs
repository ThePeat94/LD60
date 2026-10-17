using System.Collections.Generic;
using Godot;
using Nidavellir.Generator;

namespace Nidavellir.Automation;

public partial class EmployeeController : Node
{
    [Export] private PackedScene m_thinkerEmployeeScene;
    [Export] private PackedScene m_inventorEmployeeScene;
    [Export] private PatentGenerator m_patentGenerator;
    [Export] private ScienceGenerator m_scienceGenerator;
    
    private readonly List<Employee> m_thinkers = [];
    private readonly List<Employee> m_inventors = [];
    
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
}