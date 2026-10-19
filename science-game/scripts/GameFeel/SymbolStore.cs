using System.Collections.Generic;
using Godot;

namespace Nidavellir.GameFeel;

/// <summary>
/// Symbols left behind on a generator that represent what is stored in it.
/// Symbols can be sent to another store (they land there) or flown to a point where they vanish.
/// </summary>
[GlobalClass]
public partial class SymbolStore : Node2D
{
    [Export] private CollisionShape2D m_area;
    [Export] private Texture2D m_texture;
    [Export] private int m_hFrames = 5;
    [Export] private int m_vFrames = 4;
    [Export] private int m_maxSymbols = 100;
    [Export] private float m_symbolScale = 2.4f;
    [Export] private float m_flightDuration = 0.45f;
    [Export] private float m_arcHeight = 80f;
    [Export] private float m_landingSpread = 220f;
    [Export] private float m_vanishStagger = 0.03f;

    [ExportGroup("Floating")]
    [Export] private Vector2 m_floatAmplitude = new(6f, 8f);
    [Export] private float m_floatSpeed = 1.2f;

    private const int c_flyingZIndex = 50;

    private readonly List<Sprite2D> m_resting = [];
    private readonly List<Sprite2D> m_incoming = [];
    private readonly Dictionary<Sprite2D, Tween> m_tweens = [];

    private readonly Dictionary<Sprite2D, FloatState> m_floating = [];
    private float m_time;

    private record struct FloatState(Vector2 Home, Vector2 Phase, float Speed);

    private int Count => this.m_resting.Count + this.m_incoming.Count;

    public override void _Ready()
    {
        // symbols live in world space, unaffected by the scaled generator they belong to
        this.TopLevel = true;
        this.GlobalTransform = Transform2D.Identity;
    }

    public override void _Process(double delta)
    {
        this.m_time += (float)delta;
        foreach (var (symbol, state) in this.m_floating)
            symbol.GlobalPosition = state.Home + this.GetFloatOffset(state);
    }

    public void AddAt(Vector2 globalPosition)
    {
        if (this.Count >= this.m_maxSymbols) return;

        var symbol = this.CreateSymbol(globalPosition);
        this.m_resting.Add(symbol);

        // pop in at the click, then scatter away from it so repeated clicks on one spot don't pile up
        var tween = this.StartTween(symbol);
        tween.TweenProperty(symbol, "scale", Vector2.One * this.m_symbolScale, 0.2f)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.Out);
        tween.Parallel().TweenProperty(symbol, "global_position", this.PointNear(globalPosition), 0.35f)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.Out);
        tween.TweenCallback(Callable.From(() => this.StartFloating(symbol)));
    }

    public void AddRandom()
    {
        this.AddAt(this.RandomPoint());
    }

    public Vector2 RandomPoint()
    {
        var half = this.GetAreaHalfSize();
        var local = new Vector2((float)GD.RandRange(-half.X, half.X), (float)GD.RandRange(-half.Y, half.Y));
        return this.m_area.GlobalTransform * local;
    }

    /// <summary>A random point around <paramref name="globalPosition"/>, kept inside the area.</summary>
    public Vector2 PointNear(Vector2 globalPosition)
    {
        var toLocal = this.m_area.GlobalTransform.AffineInverse();
        var half = this.GetAreaHalfSize();
        var local = toLocal * globalPosition;

        // re-roll points outside the area instead of clamping them, which would line symbols up on the edges
        for (var attempt = 0; attempt < 8; attempt++)
        {
            var distance = (float)GD.RandRange(0, this.m_landingSpread);
            var offset = Vector2.FromAngle((float)GD.RandRange(0, Mathf.Tau)) * distance;
            local = toLocal * (globalPosition + offset);
            if (Mathf.Abs(local.X) <= half.X && Mathf.Abs(local.Y) <= half.Y)
                return this.m_area.GlobalTransform * local;
        }

        return this.m_area.GlobalTransform * local.Clamp(-half, half);
    }

    /// <summary>
    /// Sends one resting symbol to <paramref name="to"/>. It lands in <paramref name="landIn"/>,
    /// or vanishes there when <paramref name="landIn"/> is null.
    /// Without a symbol to send, <paramref name="landIn"/> gets a fresh one at the target instead.
    /// </summary>
    public void SendOne(Vector2 to, SymbolStore landIn)
    {
        if (this.m_resting.Count == 0)
        {
            landIn?.AddAt(to);
            return;
        }

        var symbol = this.TakeNewest();
        if (landIn != null)
            landIn.Receive(symbol, to);
        else
            this.FlyAndVanish(symbol, to, 0f);
    }

    /// <summary>Sends every resting symbol into <paramref name="landIn"/>, landing around <paramref name="around"/>.</summary>
    public void SendAll(Vector2 around, SymbolStore landIn)
    {
        while (this.m_resting.Count > 0)
            landIn.Receive(this.TakeNewest(), around);
    }

    /// <summary>All symbols (resting and still arriving) fly to <paramref name="to"/> and vanish there.</summary>
    public void FlyAllTo(Vector2 to)
    {
        var all = new List<Sprite2D>(this.m_resting);
        all.AddRange(this.m_incoming);
        this.m_resting.Clear();
        this.m_floating.Clear();
        this.m_incoming.Clear();

        for (var i = 0; i < all.Count; i++)
            this.FlyAndVanish(all[i], to, i * this.m_vanishStagger);
    }

    private void Receive(Sprite2D symbol, Vector2 to)
    {
        this.Adopt(symbol);

        if (this.Count >= this.m_maxSymbols)
        {
            this.FlyAndVanish(symbol, to, 0f);
            return;
        }

        this.m_incoming.Add(symbol);
        var tween = this.StartFlight(symbol, this.PointNear(to), 0f);
        tween.TweenCallback(Callable.From(() =>
        {
            this.m_incoming.Remove(symbol);
            this.m_resting.Add(symbol);
            this.StartFloating(symbol);
            symbol.ZIndex = 0;
            this.m_tweens.Remove(symbol);
        }));
    }

    private void FlyAndVanish(Sprite2D symbol, Vector2 to, float delay)
    {
        this.Adopt(symbol);

        var tween = this.StartFlight(symbol, to, delay);
        tween.Parallel()
            .TweenProperty(symbol, "scale", Vector2.Zero, this.m_flightDuration * 0.35f)
            .SetDelay(this.m_flightDuration * 0.65f);
        tween.TweenCallback(Callable.From(() =>
        {
            this.m_tweens.Remove(symbol);
            symbol.QueueFree();
        }));
    }

    private Tween StartFlight(Sprite2D symbol, Vector2 to, float delay)
    {
        var from = symbol.GlobalPosition;
        var control = (from + to) / 2f + Vector2.Up * this.m_arcHeight;
        symbol.ZIndex = c_flyingZIndex;

        var tween = this.StartTween(symbol);
        if (delay > 0f)
            tween.TweenInterval(delay);
        tween.TweenMethod(Callable.From<float>(t =>
            {
                var u = 1f - t;
                symbol.GlobalPosition = u * u * from + 2f * u * t * control + t * t * to;
            }), 0f, 1f, this.m_flightDuration)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.InOut);
        // finish a pop-in that may have been cut short
        tween.Parallel().TweenProperty(symbol, "scale", Vector2.One * this.m_symbolScale, 0.1f);
        return tween;
    }

    private Tween StartTween(Sprite2D symbol)
    {
        if (this.m_tweens.TryGetValue(symbol, out var running))
            running.Kill();

        var tween = this.CreateTween();
        this.m_tweens[symbol] = tween;
        return tween;
    }

    private void Adopt(Sprite2D symbol)
    {
        if (symbol.GetParent() == this) return;

        symbol.Reparent(this);
    }

    private Sprite2D TakeNewest()
    {
        var symbol = this.m_resting[^1];
        this.m_resting.RemoveAt(this.m_resting.Count - 1);
        this.m_floating.Remove(symbol);
        // stop a running pop-in, otherwise it would keep driving the symbol after it moved stores
        if (this.m_tweens.Remove(symbol, out var tween))
            tween.Kill();
        return symbol;
    }

    private void StartFloating(Sprite2D symbol)
    {
        var state = new FloatState(
            Vector2.Zero,
            new Vector2((float)GD.RandRange(0, Mathf.Tau), (float)GD.RandRange(0, Mathf.Tau)),
            this.m_floatSpeed * (float)GD.RandRange(0.7, 1.3));
        // pick the home so the symbol starts floating exactly where it is, without a jump
        this.m_floating[symbol] = state with { Home = symbol.GlobalPosition - this.GetFloatOffset(state) };
    }

    private Vector2 GetFloatOffset(FloatState state)
    {
        var t = this.m_time * state.Speed;
        // different frequencies on x and y give a loose figure-eight drift instead of a straight bob
        return new Vector2(
            Mathf.Sin(t * 0.7f + state.Phase.X) * this.m_floatAmplitude.X,
            Mathf.Sin(t + state.Phase.Y) * this.m_floatAmplitude.Y);
    }

    private Sprite2D CreateSymbol(Vector2 globalPosition)
    {
        var symbol = new Sprite2D
        {
            Texture = this.m_texture,
            Hframes = this.m_hFrames,
            Vframes = this.m_vFrames,
            Frame = GD.RandRange(0, this.m_hFrames * this.m_vFrames - 1),
            Scale = Vector2.Zero,
        };
        this.AddChild(symbol);
        symbol.GlobalPosition = globalPosition;
        return symbol;
    }

    private Vector2 GetAreaHalfSize()
    {
        // keep symbols a bit away from the edges of the click area
        return this.m_area.Shape is RectangleShape2D rect ? rect.Size / 2f * 0.8f : Vector2.Zero;
    }
}