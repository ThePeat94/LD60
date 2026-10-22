using System;
using Nidavellir.GameState;
using Nidavellir.GDResource.SkillTree.EntityStatUpgrades;
using Nidavellir.IdleNumbers;

namespace Nidavellir.Stats;

public sealed class EntityStatUpgradeFactory(
    ISynergySourceReader synergySourceReader
)
{
    public EntityStatUpgrade Create(EntityStatUpgradeData upgradeData)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(upgradeData.Amount.Value, 0);
        if (upgradeData.Synergy is not null)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(upgradeData.Synergy.ForEach, 0);
            var upgrade = new DynamicEntityStatUpgrade(
                upgradeData.Amount.ToIdleNumber(), 
                upgradeData.Operation, 
                upgradeData.Synergy.Source, 
                upgradeData.Synergy.ForEach, 
                upgradeData.Synergy.ScalingMode
            );
            
            upgrade.DynamicValueProvider = this.GetDynamicCalcFunc(upgrade);
            return upgrade;
        }
        return new FlatEntityStatUpgrade(upgradeData.Amount.ToIdleNumber(), upgradeData.Operation);
    }

    private Func<IdleNumber> GetDynamicCalcFunc(DynamicEntityStatUpgrade data)
    {
        return () =>
        {
            var quantity = synergySourceReader.GetSynergyAmount(data.SynergySource);

            quantity /= Math.Max(data.ForEach, 1);
            if (quantity == 0)
            {
                return new IdleNumber(data.Operation == StatOperation.Multiply ? 1 : 0, 0);
            }

            return CalculateMagnitude(data.BaseValue, quantity, data.ScalingMode, data.Operation);
        };
    }

    private static IdleNumber CalculateMagnitude(
        IdleNumber amount,
        int quantity,
        SynergyScalingMode scalingMode,
        StatOperation operation)
    {
        var quantityNumber = new IdleNumber(quantity);
        var one = new IdleNumber(1);

        return scalingMode switch
        {
            SynergyScalingMode.Linear when operation == StatOperation.Multiply
                => one + (amount - one) * quantityNumber,
            SynergyScalingMode.Linear => amount * quantityNumber,
            SynergyScalingMode.Compound => amount.Pow(quantityNumber),
            _ => throw new ArgumentOutOfRangeException(nameof(scalingMode), scalingMode, null)
        };
    }
}
