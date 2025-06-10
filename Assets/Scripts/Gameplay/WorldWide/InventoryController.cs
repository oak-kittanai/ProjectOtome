using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController
{
    public int currentCurrency;
    public int maxCurrency = 99999;

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

        if (item.Type != ItemType.Consumable && items.Contains(item)) return;

        if (item.Type == ItemType.Consumable)
        {
            var existingItem = items.FirstOrDefault(i => i.Id == item.Id);
            if (existingItem != null)
            {
                int totalQuantity = existingItem.Quantity + item.Quantity;
                existingItem.Quantity = Mathf.Min(totalQuantity, existingItem.MaxStack);

                MessagingCenter.Send(this, MessageOnUpdateItem, items);
                return;
            }
        }

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

    public void DecreaseItemQuantity(int id)
    {
        var item = GetItemById(id);
        if (item == null) return;

        item.Quantity -= 1;
        if (item.Quantity <= 0)
        {
            RemoveItem(item.Id);
        }
    }

    private Item GetItemById(int id)
    {
        var itemData = database.GetItemData();
        for (int i = 0; i < itemData.Length; i++)
        {
            if (itemData[i].Id == id)
            {
                var template = itemData[i];

                return new Item(template.Id, template.Name, template.Description, template.Type, template.Calorie, 1);
            }
        }

        Debug.LogWarning("Item id " + id + " not found");
        return null;
    }
}

public enum ItemType
{
    Consumable,
    Gift
}

public enum BuffType
{
    Strength,
    Hpregen,
    Focus
}

public class Item
{
    public int Id;
    public string Name;
    public string Description;
    public ItemType Type;
    public float Calorie;
    public BuffType BuffType;

    public int MaxStack = 10;
    public int Quantity;

    public Item(int id, string name, string description, ItemType type) // Gift
    {
        Id = id;
        Name = name;
        Description = description;
        Type = type;
    }

    public Item(int id, string name, string description, ItemType type, BuffType buff, int quantity)
    {
        Id = id;
        Name = name;
        Description = description;
        Type = type;
        BuffType = buff;

        Quantity = Mathf.Min(quantity, MaxStack);
    }

    public Item(int id, string name, string description, ItemType type, float calorie, int quantity) // Food
    {
        Id = id;
        Name = name;
        Description = description;
        Type = type;
        Calorie = calorie;

        Quantity = Mathf.Min(quantity, MaxStack);
    }

    /*public Item(int id, string name, string description, int type, float calorie)
    {
        Id = id;
        Name = name;
        Description = description;
        Type = (ItemType)type;
        Calorie = calorie;
    }
    
    public Item(int id, string name, string description, string type)
    {
        Id = id;
        Name = name;
        Description = description;
        Type = (ItemType)System.Enum.Parse(typeof(ItemType), type);
    }*/
}
