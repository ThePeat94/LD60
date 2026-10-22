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
        if (upgradeData.Synergy is not null)
        {
            return new DynamicEntityStatUpgrade(
                upgradeData.Amount.ToIdleNumber(), 
                upgradeData.Operation, 
                this.GetDynamicCalcFunc(upgradeData), 
                upgradeData.Synergy.Source, 
                upgradeData.Synergy.ForEach, 
                upgradeData.Synergy.ScalingMode
            );
        }
        return new FlatEntityStatUpgrade(upgradeData.Amount.ToIdleNumber(), upgradeData.Operation);
    }

    private Func<IdleNumber> GetDynamicCalcFunc(EntityStatUpgradeData data)
    {
        return () =>
        {
            var quantity = synergySourceReader.GetSynergyAmount(data.Synergy.Source);

            quantity /= Math.Max(data.Synergy.ForEach, 1);
            if (quantity == 0)
            {
                return new IdleNumber(data.Operation == StatOperation.Multiply ? 1 : 0, 0);
            }

            return CalculateMagnitude(data.Amount.ToIdleNumber(), quantity, data.Synergy.ScalingMode, data.Operation);
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
