class_name FeelSfxPlayer
extends AudioStreamPlayer2D


func _ready() -> void:
	volume_db = -10.0


## Works both as an input_event handler (plays on left click) and without arguments (always plays).
func Trigger(_viewport: Node = null, evt: InputEvent = null, _shape_idx: int = 0) -> void:
	if evt != null:
		var button := evt as InputEventMouseButton
		if button == null or not button.pressed or button.button_index != MOUSE_BUTTON_LEFT:
			return
	play()


func OnClick() -> void:
	Trigger()
