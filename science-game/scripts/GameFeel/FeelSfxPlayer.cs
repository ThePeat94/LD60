using Godot;
using Godot.Collections;

namespace Nidavellir.GameFeel;

public partial class FeelSfxPlayer : AudioStreamPlayer2D
{
    public void Trigger(Node viewport, InputEvent evt, int shapeIdx)
    {
        if (evt is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
        {
            this.Play();
        }
    }

    public void OnClick()
    {
        this.Play();
    }
}