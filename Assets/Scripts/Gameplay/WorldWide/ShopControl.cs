using System.Collections.Generic;
using UnityEngine;

public class ShopControl : MonoBehaviour
{
    private readonly List<Item> items = new List<Item>();
    private Database database;

    public const string MessageOnUpdateItem = "OnUpdateItem";

    public ShopControl(Database database)
    {
        this.database = database;
    }

    public List<Item> GetItems()
    {
        return items;
    }

    public void SetUp(int[] items)
    {
        foreach (var item in items)
        {
            AddItem(item);
        }
    }

    public void AddItem(int id)
    {
        var item = GetItemById(id);
        if (item == null) return;

        // Add Item to the Shop
    }

    public void BuyItem(int id)
    {
        var item = GetItemById(id);
        if (item == null) return;


    }

    public void RemoveItem(int id)
    {
        var item = GetItemById(id);
        if (item == null) return;

        // Add Remove Item
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
