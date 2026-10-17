using Godot;
using Nidavellir.IdleNumbers;
using Nidavellir.PlayerStats;

namespace Nidavellir.Generator;

[GlobalClass]
public partial class ScienceGenerator : Node2D
{
    [Export]
    public ResourceControllerNode ScienceResourceControllerNode { get; set; }
    
    [Export]
    public PlayerStatsContainer PlayerStatsContainer { get; set; }
    
    public void OnClick(Node viewport, InputEvent evt, int shapeIdx)
    {
        if (evt is not InputEventMouseButton { Pressed: true }) return;
        
        this.GenerateScience(new IdleNumber(1));
    }

    public void GenerateScience(IdleNumber value)
    {
        this.ScienceResourceControllerNode.ResourceController.Add(value);
    }
}