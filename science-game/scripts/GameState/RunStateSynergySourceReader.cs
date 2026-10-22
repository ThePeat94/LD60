using System;
using Nidavellir.Automation;
using Nidavellir.GDResource.SkillTree.EntityStatUpgrades;
using Nidavellir.Generator;

namespace Nidavellir.GameState;

public class RunStateSynergySourceReader(
    EmployeeController employeeController, 
    GeneratorController knowledgeGeneratorController, 
    GeneratorController patentGeneratorController
)
    : ISynergySourceReader
{
    public int GetSynergyAmount(SynergySource source)
    {
        return source switch
        {
            SynergySource.PatentLevel => patentGeneratorController.CurrentLevel,
            SynergySource.KnowledgeLevel => knowledgeGeneratorController.CurrentLevel,
            SynergySource.ScribeQuantity => employeeController.Scribes.Count,
            SynergySource.InventorQuantity => employeeController.Inventors.Count,
            SynergySource.ThinkerQuantity => employeeController.Thinkers.Count,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
    }
}