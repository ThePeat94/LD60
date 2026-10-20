class_name MusicPlayer
extends AudioStreamPlayer2D

var m_currentTrack: int = 0


func Advance() -> void:
	var playback := get_stream_playback() as AudioStreamPlaybackInteractive
	if playback == null:
		push_warning("MusicPlayer: no interactive playback to advance.")
		return

	m_currentTrack += 1
	playback.switch_to_clip(m_currentTrack)
