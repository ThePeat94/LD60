using Godot;
using System.Collections.Generic;
using Godot.Collections;
using Nidavellir.GDResource.MathModels;

namespace Nidavellir.GDResource;

[GlobalClass]
public partial class ProgressionLevelData : Resource
{
    [Export]
    public Array<IdleNumberResource> FixedLevels { get; set; }
    
    [Export]
    public MathModelResource DynamicLevels { get; set; }
}