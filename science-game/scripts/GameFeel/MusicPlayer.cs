using Godot;
using Godot.Collections;

namespace Nidavellir.GameFeel;

public partial class MusicPlayer : AudioStreamPlayer2D
{
    private int m_currentTrack = 0;
    
    public void Advance()
    {
        var playback = (AudioStreamPlaybackInteractive)this.GetStreamPlayback();
        this.m_currentTrack++;
        playback.SwitchToClip(this.m_currentTrack);
    }
}