using Godot;

namespace Nidavellir.GDResource.Employee;

[GlobalClass]
public partial class EmployeeDataContainer : Resource
{
    [Export] public EmployeeData ThinkerData { get; set; }
    [Export] public EmployeeData InventorData { get; set; }
    [Export] public EmployeeData ScribeData { get; set; }
    
}