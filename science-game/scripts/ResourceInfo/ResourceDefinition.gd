class_name ResourceDefinition
extends Resource

@export var Type: Enums.ResourceType

@export var Name: String

@export_multiline var Description: String

# Called "Texture" in the C# version, but GDScript forbids members named like a native class.
@export var ResourceTexture: Texture2D
