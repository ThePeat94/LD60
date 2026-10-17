using Godot;
using Nidavellir.IdleNumbers;
using Nidavellir.PlayerStats;

namespace Nidavellir.Generator;

[GlobalClass]
public partial class ScienceGenerator : Node2D
{
    [Export]
    public ResourceControllerContainer ResourceControllerContainer { get; set; }
    
    [Export]
    public PlayerStatsContainer PlayerStatsContainer { get; set; }
    
    public void OnClick(Node viewport, InputEvent evt, int shapeIdx)
    {
        if (evt is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed)
            {
                this.ResourceControllerContainer.ScienceResourceController.Add(new IdleNumber(1));
            }
        }
    }
}