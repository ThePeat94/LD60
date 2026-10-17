using Godot;
using Nidavellir.IdleNumbers;

namespace Nidavellir.GDResource.MathModels;

[GlobalClass]
public partial class LinearModelResource : MathModelResource
{
    // b
    [Export]
    public IdleNumberResource Base { get; set; }
    
    // a
    [Export]
    public IdleNumberResource Slope { get; set; }
    
    // f(x) = a * x + b
    public override IdleNumber CalculateFor(IdleNumber value)
    {
        return this.Slope.ToIdleNumber() * value + this.Base.ToIdleNumber();
    }
}