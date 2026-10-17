using Godot;
using System.Collections.Generic;
using Godot.Collections;

namespace Nidavellir.GDResource;

[GlobalClass]
public partial class ProgressionLevelData : Resource
{
    [Export]
    public Array<IdleNumberResource> FixedLevels { get; set; }
}