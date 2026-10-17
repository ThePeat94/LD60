namespace Nidavellir.IdleNumbers;

public class ResourceValueChangedEvent(IdleNumber newValue) : System.EventArgs
{
    public IdleNumber NewValue { get; } = newValue;
}