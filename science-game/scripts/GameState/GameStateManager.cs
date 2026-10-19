using Godot;
using Nidavellir.Automation;
using Nidavellir.GameFeel;

namespace Nidavellir.GameState;

public partial class GameStateManager : Node
{
    [Export] private MusicPlayer m_musicPlayer;
    [Export] private EmployeeController m_employeeController;
    
    private GameProgressionState m_currentProgressionState = GameProgressionState.Beginning;

    public override void _Ready()
    {
        base._Ready();
        this.m_employeeController.EmployeeBought += this.OnEmployeeBought;
    }

    private void OnEmployeeBought()
    {
        if (this.m_employeeController.Inventors.Count > 0 && this.m_employeeController.Scribes.Count > 0 && this.m_employeeController.Thinkers.Count > 0)
        {
            this.m_musicPlayer.Advance();
            this.m_currentProgressionState = GameProgressionState.Midgame;
        }
    }
}