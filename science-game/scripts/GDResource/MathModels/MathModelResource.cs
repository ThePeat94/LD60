using Godot;
using Nidavellir.IdleNumbers;

namespace Nidavellir.GDResource.MathModels;

public abstract partial class MathModelResource : Resource
{
    public abstract IdleNumber CalculateFor(IdleNumber value);
}