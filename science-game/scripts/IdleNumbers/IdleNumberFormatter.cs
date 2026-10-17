using System;
using System.Collections.Generic;

namespace Nidavellir.IdleNumbers;

public static class IdleNumberFormatter
{
    private static readonly Dictionary<int, string> s_unitMap = new Dictionary<int, string>
    {
        { 0, "" },
        { 1, "K" },
        { 2, "M" },
        { 3, "B" },
        { 4, "T" }
    };

    private static readonly int s_charA = Convert.ToInt32('a');

    public static string FormatIdleNumber(IdleNumber idleNumber)
    {
        return (Math.Floor(idleNumber.Value * 100) / 100).ToString("0.##") + GetUnitForIdleNumber(idleNumber);
    }

    public static string GetUnitForIdleNumber(IdleNumber idleNumber)
    {
        string unit;
        var magnitude = idleNumber.Exponent / 3;
        if (magnitude < s_unitMap.Count)
        {
            unit = s_unitMap[magnitude];
        }
        else
        {
            var unitInt = magnitude - s_unitMap.Count;
            var secondUnit = unitInt % 26;
            var firstUnit = unitInt / 26;
            unit = Convert.ToChar(firstUnit + s_charA) + Convert.ToChar(secondUnit + s_charA).ToString();
        }

        return unit;
    }
}