using Godot;
using Nidavellir.IdleNumbers;

namespace Nidavellir;

public partial class ResourceControllerContainer : Node
{
    public IdleNumberResourceController ScienceResourceController { get; set; } = new();
    public IdleNumberResourceController MoneyResourceController { get; set; } = new();
}