using Godot;
using Nidavellir.IdleNumbers;

namespace Nidavellir;

public partial class ResourceControllerNode : Node
{
    public IdleNumberResourceController ResourceController { get; set; } = new();
}