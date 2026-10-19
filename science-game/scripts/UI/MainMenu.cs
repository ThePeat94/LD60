using Godot;
using Nidavellir.GameFeel;

namespace Nidavellir.UI;

public partial class MainMenu : Control
{
    [Export] private MusicPlayer m_musicPlayer;
    
    public void Close()
    {
        this.Visible = false;
    }
}