using Nidavellir.GDResource;
using Nidavellir.IdleNumbers;

namespace Nidavellir.Automation;

public class EmployeeStats(EmployeeData data)
{
    public IdleNumber WorkForce { get; set; } =  data.BaseWorkForce.ToIdleNumber();
    public double TickSpeed { get; set; } = data.BaseTickSpeed;
}