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
    public double BaseTickSpeed { get; set; }
}