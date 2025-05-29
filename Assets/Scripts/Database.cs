using UnityEngine;

public class Database
{
    private readonly Item[] items = 
    {
        new(0, "Axe", "Basic axe for chopping wood", ItemType.Material),
        new(1, "Elixir", "Restores HP and MP", ItemType.Consumable),
        new(2, "Hammer", "Basic hammer for crafting", ItemType.Material),
        new(3, "Antidote", "Removes poison effect", ItemType.Consumable),
        new(4, "Bow", "Basic bow for ranged attack", ItemType.Material),
        new(5, "Energy Drink", "Restores HP and MP", ItemType.Consumable),
        new(6, "Shield", "Basic shield for defense", ItemType.Material),
        new(7, "Herb", "Restores HP", ItemType.Consumable),
        new(8, "Dagger", "Basic dagger for close combat", ItemType.Material),
        new(9, "Magic Water", "Restores HP and MP", ItemType.Consumable),
        new(10, "Staff", "Basic staff for magic attacks", ItemType.Material),
        new(11, "Healing Salve", "Restores HP", ItemType.Consumable),
        new(12, "Helmet", "Basic helmet for defense", ItemType.Material),
        new(13, "Mana Potion", "Restores MP", ItemType.Consumable),
        new(14, "Armor", "Basic armor for defense", ItemType.Material),
        new(15, "Stamina Pill", "Restores HP", ItemType.Consumable),
        new(16, "Spear", "Basic spear for close combat", ItemType.Material),
        new(17, "Revive Potion", "Revives player if they died", ItemType.Consumable),
        new(18, "Crossbow", "Basic crossbow for ranged attack", ItemType.Material),
        new(19, "Vitality Potion", "Restores HP and MP", ItemType.Consumable)
    };

    public Item[] GetItemData()
    {
        return items;
    }
}
