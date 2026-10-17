using Godot;
using Nidavellir.Automation;
using Nidavellir.GDResource.Shop;

namespace Nidavellir.Upgrades;

public partial class UpgradeController : Node
{
    [Export] private EmployeeController m_employeeController;
    
    public void ApplyThinkerUpgrade(ThinkerUpgradeData thinkerUpgradeData)
    {
        this.m_employeeController.AddThinker();
    }

    public void ApplyInventorUpgrade(InventorUpgradeData upgradeData)
    {
        this.m_employeeController.AddInventor();
    }
}