using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopControl : MonoBehaviour
{
    [Header("Price Setting")]
    private int itemPrice;

    private readonly List<Item> items = new List<Item>();
    private Database database;

    public const string MessageOnUpdateItem = "OnUpdateItem";

    public ShopControl(Database database)
    {
        this.database = database;
    }

    public void SetUp(int[] items)
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

    public void BuyItem(int id, int num)
    {
        var item = GetItemById(id);
        if (item == null) return;

        Game.Instance.GetInventory().AddItem(id); // need to add num
    }

    public void RemoveItem(int id)
    {
        var item = GetItemById(id);
        if (item == null) return;

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
