using Godot;

namespace Nidavellir.UI;

public partial class GameWonUI : Control
{
    public void Open()
    {
        this.Visible = true;
    }

    public void Close()
    {
        this.Visible = false;
    }
}