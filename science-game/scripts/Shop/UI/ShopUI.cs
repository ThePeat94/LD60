using System.Collections.Generic;
using Godot;
using Nidavellir.GDResource.Shop;

namespace Nidavellir.Shop.UI;

public partial class ShopUI : Control
{
    [Export]
    public PackedScene ItemPrefab { private get; set; }
    
    [Export]
    public ShopController ShopController { private get; set; }
    
    [Export]
    public Node ItemsParent { private get; set; }
    
    private readonly Dictionary<ShopItemData, ShopItemUI> m_items = new();
    
    public void Open(Dictionary<ShopItemData, int> upgrades)
    {
        foreach (var keyValuePair in upgrades)
        {
            if (!this.m_items.TryGetValue(keyValuePair.Key, out var value))
            {
                var initializedItem = this.ItemPrefab.Instantiate<ShopItemUI>();
                this.ItemsParent.AddChild(initializedItem);
                this.m_items.Add(keyValuePair.Key, initializedItem);
                initializedItem.Show(keyValuePair.Key, keyValuePair.Value);
                initializedItem.Clicked += this.OnItemPurchase; 
            }
            else
            {
                value.Show(keyValuePair.Key, keyValuePair.Value);
            }
        }
        this.Visible = true;
    }

    private void OnItemPurchase(ShopItemData obj)
    {
        this.ShopController.AttemptPurchase(obj);
    }

    public void Close()
    {
        this.Visible = false;
    }

    private void ClearUI()
    {
        foreach (var keyValuePair in this.m_items)
        {
            keyValuePair.Value.QueueFree();
        }
        this.m_items.Clear();
    }
}