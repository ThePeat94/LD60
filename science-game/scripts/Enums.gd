class_name Enums
extends RefCounted
## All game enums in one place: GDScript has no top-level enums, and several of them
## are used as export types by resources whose properties carry the same name.

enum EmployeeType { Thinker, Inventor, Scribe }

enum EmployeeStatTarget { BaseForce, TickSpeed }

enum PlayerStatTarget { ClickStrength, MoneyConversion, SkillPointConversion }

enum StatOperation { Add, Multiply, Subtract }

enum GameFeature { Shop, Money, Skillpoints }

enum ResourceType { Science, Money, Skillpoint }

enum GameProgressionState { Beginning, Midgame, Endgame }
