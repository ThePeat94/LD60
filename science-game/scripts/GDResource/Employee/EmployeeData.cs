using Godot;

namespace Nidavellir.GDResource;

[GlobalClass]
public partial class EmployeeData : Resource
{
    [Export]
    public string EmployeeName { get; set; }
    
    [Export]
    public Texture2D Icon { get; set; }
    
    [Export]
    public IdleNumberResource BaseWorkForce { get; set; }
    
    [Export]
    public IdleNumberResource BaseTickSpeed { get; set; }
    
    [Export]
    public EmployeeType EmployeeType { get; set; }
}