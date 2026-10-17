using Godot;

namespace Nidavellir.ResourceInfo;

[GlobalClass]
public partial class ResourceDefinition : Resource
{
    [Export]
    public ResourceType Type { get; set; }
    
    [Export]
    public string Name { get; set; }
    
    [Export(PropertyHint.MultilineText)]
    public string Description { get; set; }
    
    [Export]
    public Texture2D  Texture { get; set; }
}