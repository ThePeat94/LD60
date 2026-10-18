using System;


namespace Nidavellir.IdleNumbers;

public class IdleNumberResourceController
{
    private EventHandler<ResourceValueChangedEvent> m_resourceValueChanged;

    public IdleNumber CurrentValue { get; private set; }
    public IdleNumber SessionEarnings { get; private set; }

    public event EventHandler<ResourceValueChangedEvent> ResourceValueChanged
    {
        add => this.m_resourceValueChanged += value;
        remove => this.m_resourceValueChanged -= value;
    }

    public void Add(IdleNumber value)
    {
        if (value.Value < 0)
            throw new ArgumentException($"{value} is less than 0");

        this.CurrentValue = this.CurrentValue + value;
        this.SessionEarnings += value;
        this.m_resourceValueChanged?.Invoke(this, new ResourceValueChangedEvent(this.CurrentValue));
    }

    public bool CanAfford(IdleNumber amount)
    {
        if (amount.Value < 0)
            throw new ArgumentException($"{amount} is less than 0");

        return amount <= this.CurrentValue;
    }

    public void ResetValues()
    {
        this.CurrentValue = new IdleNumber();
        this.SessionEarnings = new IdleNumber();
        this.m_resourceValueChanged?.Invoke(this, new ResourceValueChangedEvent(this.CurrentValue));
    }


    public void UseResource(IdleNumber amount)
    {
        if (amount.Value < 0)
            throw new ArgumentException($"{amount} is less than 0");

        if (amount > this.CurrentValue)
            throw new InvalidOperationException($"{amount} is greater than {this.CurrentValue}");

        this.CurrentValue -= amount;
        this.m_resourceValueChanged?.Invoke(this, new ResourceValueChangedEvent(this.CurrentValue));
    }

    public void SetValue(IdleNumber value)
    {
        if (value.Value < 0)
            throw new ArgumentException($"{value} is less than 0");
        this.CurrentValue = value;
        this.m_resourceValueChanged?.Invoke(this, new ResourceValueChangedEvent(this.CurrentValue));
    }
}