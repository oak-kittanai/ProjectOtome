using UnityEngine;

public class Database
{
    private readonly Item[] items = 
    {
        new(0, "Axe", "Basic axe for chopping wood", ItemType.Gift),
        new(1, "Antidote", "Removes poison effect", ItemType.Consumable , 10f , 1),
        new(2, "Antidote", "Removes poison effect", ItemType.Consumable , BuffType.Strength, 1)
    };

    public Item[] GetItemData()
    {
        return items;
    }
}
