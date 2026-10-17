using Godot;
using Nidavellir.IdleNumbers;

namespace Nidavellir.GDResource;

[GlobalClass]
public partial class IdleNumberResource : Resource
{
    [Export] 
    public double Value { get; set; } = 1;
    
    [Export] 
    public int Exponent { get; set; } = 0;

    public IdleNumber ToIdleNumber()
    {
        return new IdleNumber(this.Value, this.Exponent);
    }
}