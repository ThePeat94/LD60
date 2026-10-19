using System;
using Godot;
using Nidavellir.GDResource.Shop;
using Nidavellir.IdleNumbers;
using Nidavellir.UI;

namespace Nidavellir.Shop.UI;

public partial class ShopItemUI : Node
{
    [Export] private Label m_quantity;
    
    private Action<ShopItemData> m_onClick;

    [Export]
    public Label ItemName { private get; set; }

    [Export]
    public TextureRect Icon { private get; set; }

    [Export]
    public Label Description { private get; set; }

    [Export]
    public TextureButton Purchase { private get; set; }

    [Export] private Label m_cost;

    private const int MinDescriptionFontSize = 16;

    private ShopItemData m_itemData;
    private float m_descriptionBoxHeight;
    private int m_descriptionMaxFontSize;

    public event Action<ShopItemData> Clicked
    {
        add => this.m_onClick += value;
        remove => this.m_onClick -= value;
    }

    public override void _Ready()
    {
        base._Ready();
        this.Purchase.Pressed += this.HandlePurchaseClick;
        this.m_descriptionBoxHeight = this.Description.Size.Y;
        this.m_descriptionMaxFontSize = this.Description.GetThemeFontSize("font_size");
    }

    public void Show(ShopItemData itemData, int upgradeAmount)
    {
        this.ItemName.Text = itemData.UpgradeName;
        this.Description.Text = itemData.Description;
        this.FitDescriptionFont();
        this.Icon.Texture = itemData.Icon;
        this.m_cost.Text = itemData.CostModel.CalculateFor(new IdleNumber(upgradeAmount)).ToString();
        this.m_itemData = itemData;
        this.m_quantity.Text = itemData.MaximumAmount > 0 ? $"{upgradeAmount}/{itemData.MaximumAmount}" : $"{upgradeAmount}";
    }
    
    private void FitDescriptionFont()
    {
        if (this.m_descriptionBoxHeight <= 0) return;

        var fontSize = this.m_descriptionMaxFontSize;
        this.Description.AddThemeFontSizeOverride("font_size", fontSize);
        while (fontSize > MinDescriptionFontSize && this.Description.GetCombinedMinimumSize().Y > this.m_descriptionBoxHeight)
        {
            fontSize -= 2;
            this.Description.AddThemeFontSizeOverride("font_size", fontSize);
        }
    }

    private void HandlePurchaseClick()
    {
        if (DragScrollContainer.IsDragClick(this)) return;

        this.m_onClick?.Invoke(this.m_itemData);
    }
}