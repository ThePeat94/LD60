namespace Nidavellir.UI;

/// <summary>Something that can be dragged around with the mouse, so clicks on its children can tell a drag from a click.</summary>
public interface IDragSurface
{
    /// <summary>Whether the click that is ending right now was a drag. Only meaningful while the mouse release is handled.</summary>
    bool WasDragged { get; }
}
