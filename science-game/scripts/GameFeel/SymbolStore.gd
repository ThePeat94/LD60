class_name SymbolStore
extends Node2D
## Symbols left behind on a generator that represent what is stored in it.
## Symbols can be sent to another store (they land there) or flown to a point where they vanish.

@export var m_area: CollisionShape2D
@export var m_texture: Texture2D
@export var m_hFrames: int = 5
@export var m_vFrames: int = 4
@export var m_maxSymbols: int = 100
@export var m_symbolScale: float = 1.8
@export var m_flightDuration: float = 0.45
@export var m_arcHeight: float = 80.0
@export var m_landingSpread: float = 220.0
@export var m_vanishStagger: float = 0.03

@export_group("Floating")
@export var m_floatAmplitude: Vector2 = Vector2(6.0, 8.0)
@export var m_floatSpeed: float = 1.2

const c_flyingZIndex := 50

var m_resting: Array[Sprite2D] = []
var m_incoming: Array[Sprite2D] = []
var m_tweens: Dictionary[Sprite2D, Tween] = {}

var m_floating: Dictionary[Sprite2D, FloatState] = {}
var m_time: float


class FloatState:
	var Home: Vector2
	var Phase: Vector2
	var Speed: float

	func _init(home: Vector2, phase: Vector2, speed: float) -> void:
		Home = home
		Phase = phase
		Speed = speed


var Count: int:
	get:
		return m_resting.size() + m_incoming.size()


func _ready() -> void:
	# symbols live in world space, unaffected by the scaled generator they belong to
	top_level = true
	global_transform = Transform2D.IDENTITY


func _process(delta: float) -> void:
	m_time += delta
	for symbol in m_floating:
		symbol.global_position = m_floating[symbol].Home + GetFloatOffset(m_floating[symbol])


func AddAt(at: Vector2) -> void:
	if Count >= m_maxSymbols:
		return

	var symbol := CreateSymbol(at)
	m_resting.append(symbol)

	# pop in at the click, then scatter away from it so repeated clicks on one spot don't pile up
	var tween := StartTween(symbol)
	tween.tween_property(symbol, "scale", Vector2.ONE * m_symbolScale, 0.2) \
		.set_trans(Tween.TRANS_BACK) \
		.set_ease(Tween.EASE_OUT)
	tween.parallel().tween_property(symbol, "global_position", PointNear(at), 0.35) \
		.set_trans(Tween.TRANS_CUBIC) \
		.set_ease(Tween.EASE_OUT)
	tween.tween_callback(func() -> void: StartFloating(symbol))


func AddRandom() -> void:
	AddAt(RandomPoint())


func RandomPoint() -> Vector2:
	var half := GetAreaHalfSize()
	var local_point := Vector2(randf_range(-half.x, half.x), randf_range(-half.y, half.y))
	return m_area.global_transform * local_point


## A random point around at, kept inside the area.
func PointNear(at: Vector2) -> Vector2:
	var inverse := m_area.global_transform.affine_inverse()
	var half := GetAreaHalfSize()
	var local_point := inverse * at

	# re-roll points outside the area instead of clamping them, which would line symbols up on the edges
	for attempt in 8:
		var distance := randf_range(0.0, m_landingSpread)
		var offset := Vector2.from_angle(randf_range(0.0, TAU)) * distance
		local_point = inverse * (at + offset)
		if absf(local_point.x) <= half.x and absf(local_point.y) <= half.y:
			return m_area.global_transform * local_point

	return m_area.global_transform * local_point.clamp(-half, half)


## Sends one resting symbol to to. It lands in land_in, or vanishes there when land_in is null.
## Without a symbol to send, land_in gets a fresh one at the target instead.
func SendOne(to: Vector2, land_in: SymbolStore) -> void:
	if m_resting.is_empty():
		if land_in != null:
			land_in.AddAt(to)
		return

	var symbol := TakeNewest()
	if land_in != null:
		land_in.Receive(symbol, to)
	else:
		FlyAndVanish(symbol, to, 0.0)


## Sends every resting symbol into land_in, landing around around.
func SendAll(around: Vector2, land_in: SymbolStore) -> void:
	while not m_resting.is_empty():
		land_in.Receive(TakeNewest(), around)


## All symbols (resting and still arriving) fly to to and vanish there.
func FlyAllTo(to: Vector2) -> void:
	var all: Array[Sprite2D] = m_resting.duplicate()
	all.append_array(m_incoming)
	m_resting.clear()
	m_floating.clear()
	m_incoming.clear()

	for i in all.size():
		FlyAndVanish(all[i], to, i * m_vanishStagger)


func Receive(symbol: Sprite2D, to: Vector2) -> void:
	Adopt(symbol)

	if Count >= m_maxSymbols:
		FlyAndVanish(symbol, to, 0.0)
		return

	m_incoming.append(symbol)
	var tween := StartFlight(symbol, PointNear(to), 0.0)
	var land := func() -> void:
		m_incoming.erase(symbol)
		m_resting.append(symbol)
		StartFloating(symbol)
		symbol.z_index = 0
		m_tweens.erase(symbol)
	tween.tween_callback(land)


func FlyAndVanish(symbol: Sprite2D, to: Vector2, delay: float) -> void:
	Adopt(symbol)

	var tween := StartFlight(symbol, to, delay)
	tween.parallel() \
		.tween_property(symbol, "scale", Vector2.ZERO, m_flightDuration * 0.35) \
		.set_delay(m_flightDuration * 0.65)
	var vanish := func() -> void:
		m_tweens.erase(symbol)
		symbol.queue_free()
	tween.tween_callback(vanish)


func StartFlight(symbol: Sprite2D, to: Vector2, delay: float) -> Tween:
	var from := symbol.global_position
	var control := (from + to) / 2.0 + Vector2.UP * m_arcHeight
	symbol.z_index = c_flyingZIndex

	var tween := StartTween(symbol)
	if delay > 0.0:
		tween.tween_interval(delay)
	var fly := func(t: float) -> void:
		var u := 1.0 - t
		symbol.global_position = u * u * from + 2.0 * u * t * control + t * t * to
	tween.tween_method(fly, 0.0, 1.0, m_flightDuration) \
		.set_trans(Tween.TRANS_SINE) \
		.set_ease(Tween.EASE_IN_OUT)
	# finish a pop-in that may have been cut short
	tween.parallel().tween_property(symbol, "scale", Vector2.ONE * m_symbolScale, 0.1)
	return tween


func StartTween(symbol: Sprite2D) -> Tween:
	if m_tweens.has(symbol):
		m_tweens[symbol].kill()

	var tween := create_tween()
	m_tweens[symbol] = tween
	return tween


func Adopt(symbol: Sprite2D) -> void:
	if symbol.get_parent() == self:
		return

	symbol.reparent(self)


func TakeNewest() -> Sprite2D:
	var symbol: Sprite2D = m_resting.pop_back()
	m_floating.erase(symbol)
	# stop a running pop-in, otherwise it would keep driving the symbol after it moved stores
	if m_tweens.has(symbol):
		m_tweens[symbol].kill()
		m_tweens.erase(symbol)
	return symbol


func StartFloating(symbol: Sprite2D) -> void:
	var state := FloatState.new(
		Vector2.ZERO,
		Vector2(randf_range(0.0, TAU), randf_range(0.0, TAU)),
		m_floatSpeed * randf_range(0.7, 1.3))
	# pick the home so the symbol starts floating exactly where it is, without a jump
	state.Home = symbol.global_position - GetFloatOffset(state)
	m_floating[symbol] = state


func GetFloatOffset(state: FloatState) -> Vector2:
	var t := m_time * state.Speed
	# different frequencies on x and y give a loose figure-eight drift instead of a straight bob
	return Vector2(
		sin(t * 0.7 + state.Phase.x) * m_floatAmplitude.x,
		sin(t + state.Phase.y) * m_floatAmplitude.y)


func CreateSymbol(at: Vector2) -> Sprite2D:
	var symbol := Sprite2D.new()
	symbol.texture = m_texture
	symbol.hframes = m_hFrames
	symbol.vframes = m_vFrames
	symbol.frame = randi_range(0, m_hFrames * m_vFrames - 1)
	symbol.scale = Vector2.ZERO
	add_child(symbol)
	symbol.global_position = at
	return symbol


func GetAreaHalfSize() -> Vector2:
	# keep symbols a bit away from the edges of the click area
	var rect := m_area.shape as RectangleShape2D
	return rect.size / 2.0 * 0.8 if rect != null else Vector2.ZERO
