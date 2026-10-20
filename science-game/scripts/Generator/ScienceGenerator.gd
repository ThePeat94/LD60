class_name ScienceGenerator
extends Node2D

## A click produced amount science.
signal Produced(amount: float)

@export var Generated: ResourceControllerNode

@export var PlayerStatsContainer: PlayerStatsContainer

@export var Symbols: SymbolStore


func OnClick(_viewport: Node, evt: InputEvent, _shape_idx: int) -> void:
	var button := evt as InputEventMouseButton
	if button == null or not button.pressed or button.button_index != MOUSE_BUTTON_LEFT:
		return

	var produced := PlayerStatsContainer.ClickStrength.Value
	Generated.ResourceController.Add(produced)
	if Symbols != null:
		Symbols.AddAt(get_global_mouse_position())
	Produced.emit(produced.ToDouble())


## origin: where the work happened (e.g. a gnome, a Vector2 or null), so its symbols can start there.
func Generate(value: IdleNumber, origin: Variant = null) -> bool:
	Generated.ResourceController.Add(value)
	if Symbols != null:
		Symbols.AddAt(origin if origin != null else Symbols.RandomPoint())
	return true
