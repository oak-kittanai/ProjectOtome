using UnityEngine;

public class Database
{
    private readonly Item[] items = 
    {
        new(0, "Axe", ItemType.Material),
        new(1, "Elixir", ItemType.Consumable),
        new(2, "Hammer", ItemType.Material),
        new(3, "Antidote", ItemType.Consumable),
        new(4, "Bow", ItemType.Material),
        new(5, "Energy Drink", ItemType.Consumable),
        new(6, "Shield", ItemType.Material),
        new(7, "Herb", ItemType.Consumable),
        new(8, "Dagger", ItemType.Material),
        new(9, "Magic Water", ItemType.Consumable),
        new(10, "Staff", ItemType.Material),
        new(11, "Healing Salve", ItemType.Consumable),
        new(12, "Helmet", ItemType.Material),
        new(13, "Mana Potion", ItemType.Consumable),
        new(14, "Armor", ItemType.Material),
        new(15, "Stamina Pill", ItemType.Consumable),
        new(16, "Spear", ItemType.Material),
        new(17, "Revive Potion", ItemType.Consumable),
        new(18, "Crossbow", ItemType.Material),
        new(19, "Vitality Potion", ItemType.Consumable)
    };

    public Item[] GetItemData()
    {
        return items;
    }
}
