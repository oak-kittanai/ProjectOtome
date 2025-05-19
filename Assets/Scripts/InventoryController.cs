using System.Collections.Generic;
using UnityEngine;

public class InventoryController
{
    private readonly List<Item> items = new List<Item>();
    private Database database;

    public const string MessageOnUpdateItem = "OnUpdateItem";

    public InventoryController(Database database)
    {
        this.database = database;
    }

    public void Setup(int[] items)
    {
        foreach (var item in items)
        {
            AddItem(item);
        }
    }

    public List<Item> GetItems()
    {
        return items;
    }

    public void AddItem(int id)
    {
        var item = GetItemById(id);
        if (item == null) return;
        if (item.Type != ItemType.Consumable && items.Contains(item)) return; // can stack only consumable item

        items.Add(item);
        MessagingCenter.Send(this, MessageOnUpdateItem, items);
    }

    public void RemoveItem(int id)
    {
        var item = GetItemById(id);
        if (item == null || !items.Contains(item)) return;

        items.Remove(item);
        MessagingCenter.Send(this, MessageOnUpdateItem, items);
    }

    private Item GetItemById(int id)
    {
        var itemData = database.GetItemData();
        for (int i = 0; i < itemData.Length; i++)
        {
            if (itemData[i].Id == id)
            {
                return itemData[i];
            }
        }

        Debug.LogWarning("Item id " + id + " not found");
        return null;
    }
}

public enum ItemType
{
    Material,
    Consumable,
    Moment
}

public class Item
{
    public int Id;
    public string Name;
    public string Description;
    public ItemType Type;

    public Item(int id, string name, string description, ItemType type)
    {
        Id = id;
        Name = name;
        Description = description;
        Type = type;
    }

    public Item(int id, string name, string description, string type)
    {
        Id = id;
        Name = name;
        Description = description;
        Type = (ItemType)System.Enum.Parse(typeof(ItemType), type);
    }

    public Item(int id, string name, string description, int type)
    {
        Id = id;
        Name = name;
        Description = description;
        Type = (ItemType)type;
    }
}
