using FreeWorld;
using UnityEngine;

public class Database
{
    private readonly Item[] items =
    {
        new(0, "Axe", "Basic axe for chopping wood", ItemType.Gift),
        new(1, "Antidote", "Removes poison effect", ItemType.Consumable , 10f , 1),
        new(2, "Antidote", "Removes poison effect", ItemType.Consumable , BuffType.Strength, 1)
    };

    private readonly CharacterProfileStats[] characterStats =
    {
        new("phomor", false, true, CharacterType.phomor),
        new("payanak", false, true,CharacterType.payanak),
        new("karut", false, true,CharacterType.karut),
        new("yak", false, false,CharacterType.yak),
        new("ginnorn", false, false, CharacterType.ginnorn)
    };


    public Item[] GetItemData()
    {
        return items;
    }

    public CharacterProfileStats[] GetCharacterStats()
    {
        return characterStats;
    }
}
