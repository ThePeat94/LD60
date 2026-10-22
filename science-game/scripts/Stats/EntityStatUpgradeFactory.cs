using System;
using Godot;
using Nidavellir.Automation;
using Nidavellir.GDResource.SkillTree.EntityStatUpgrades;
using Nidavellir.Generator;
using Nidavellir.IdleNumbers;

namespace Nidavellir.Stats;

public partial class EntityStatUpgradeFactory : Node
{
    [Export] private GeneratorController m_patentGenerator;
    [Export] private GeneratorController m_skillPointGenerator;
    [Export] private EmployeeController m_employeeController;
    
    public EntityStatUpgrade Create(EntityStatUpgradeData upgradeData)
    {
        if (upgradeData.Synergy is not null)
        {
            return new DynamicEntityStatUpgrade(
                upgradeData.Amount.ToIdleNumber(), 
                upgradeData.Operation, 
                this.GetDynamicCalcFunc(upgradeData), 
                upgradeData.Synergy.Source, 
                upgradeData.Synergy.ForEach
            );
        }
        return new FlatEntityStatUpgrade(upgradeData.Amount.ToIdleNumber(), upgradeData.Operation);
    }

    private Func<IdleNumber> GetDynamicCalcFunc(EntityStatUpgradeData data)
    {
        return () =>
        {
            var quantity = data.Synergy.Source switch
            {
                SynergySource.PatentLevel => this.m_patentGenerator.CurrentLevel,
                SynergySource.KnowledgeLevel => this.m_skillPointGenerator.CurrentLevel,
                SynergySource.ThinkerQuantity => this.m_employeeController.Thinkers.Count,
                SynergySource.ScribeQuantity => this.m_employeeController.Scribes.Count,
                SynergySource.InventorQuantity => this.m_employeeController.Inventors.Count,
                _ => 0
            };

            quantity /= Math.Max(data.Synergy.ForEach, 1);
            if (quantity == 0)
            {
                return new IdleNumber(data.Operation == StatOperation.Multiply ? 1 : 0, 0);
            }

            return quantity * data.Amount.ToIdleNumber();
        };
    }
}