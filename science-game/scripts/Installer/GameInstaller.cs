using Godot;
using Nidavellir.Automation;
using Nidavellir.GameState;
using Nidavellir.Generator;
using Nidavellir.SkillTree;
using Nidavellir.Stats;

namespace Nidavellir.Installer;

[GlobalClass]
public partial class GameInstaller : Node
{
    [Export] private SkillTreeUpgradeController m_skillTreeUpgradeController;
    [Export] private EmployeeController m_employeeController;
    [Export] private GeneratorController m_skillpointGenerator;
    [Export] private GeneratorController m_patentGenerator;

    public override void _Ready()
    {
        base._Ready();

        var runStateSynergySourceReader = new RunStateSynergySourceReader(
            this.m_employeeController, 
            knowledgeGeneratorController: this.m_skillpointGenerator,
            patentGeneratorController: this.m_patentGenerator
        );
        
        var entityUpgradeFactory = new EntityStatUpgradeFactory(runStateSynergySourceReader);

        this.m_skillTreeUpgradeController.Install(entityUpgradeFactory);
    }
}
