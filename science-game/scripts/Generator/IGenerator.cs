using Godot;
using Nidavellir.IdleNumbers;

namespace Nidavellir.Generator;

public interface IGenerator
{
    /// <param name="origin">where the work happened (e.g. a gnome), so its symbols can start there</param>
    /// <returns>whether anything was generated</returns>
    public bool Generate(IdleNumber power, Vector2? origin = null);
}