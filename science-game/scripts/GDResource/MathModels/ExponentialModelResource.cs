using Godot;
using Nidavellir.IdleNumbers;

namespace Nidavellir.GDResource.MathModels;

[GlobalClass]
public partial class ExponentialModelResource : MathModelResource
{
    // b
    [Export]
    public IdleNumberResource Base { get; set; }
    
    // a
    [Export]
    public IdleNumberResource Slope { get; set; }
    
    // f(x) = a * b ^ x 
    public override IdleNumber CalculateFor(IdleNumber value)
    {
        return this.Base.ToIdleNumber() * this.Slope.ToIdleNumber().Pow(value);
    }
}