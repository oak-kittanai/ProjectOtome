using UnityEngine;
using IngameDebugConsole;

public class DevCommand
{
    private InventoryController inventory;
    private ShopControl market;


    public DevCommand(InventoryController inventory, ShopControl shop)
    {
        this.market = shop;
        this.inventory = inventory;
        RegisterCommand();
    }

    private void RegisterCommand()
    {
        DebugLogConsole.AddCommand<string, string>("dialogue", "แสดง dialogue ทันที", (folderid, id) =>
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogWarning("invalid dialogue id");
                return;
            }

            UIManager.Instance.ShowDialogue(new()
            {
                FolderID = folderid,
                DialogueID = id
            });
            Debug.Log("open dialogue " + id);
        });

        DebugLogConsole.AddCommand("QTE", "แสดง QTE ทันที", () =>
        {
            BattleBaseManager.Instance.ForceQTEPlay();
            Debug.Log("open QTE ");
        });

        DebugLogConsole.AddCommand<int>("BattleBase", "แสดง Turnbase ทันที", (Num) =>
        {
            if (Num >= 4)
            {
                Debug.LogWarning("invalid too many enemy to spawn");
            }
            else
            {
                BattleBaseManager.Instance.ForceBattleBasePlay(Num);
                Debug.Log("open Turnbase ");
            }
        });

        // Add to Shop
        DebugLogConsole.AddCommand<int>("add_shopitem", "เพิ่ม item ร้านค้า", (id) =>
        {
            market.AddItem(id);
        });

        DebugLogConsole.AddCommand<int>("remove_showitem", "ลบ item ร้านค้า", (id) =>
        {
            market.RemoveItem(id);
        });

        DebugLogConsole.AddCommand("show_shop", "เปิด shop", () =>
        {
            UIManager.Instance.ShowShop(new()
            {
                Items = market.GetItems()
            });

        });

        // Add to Inventory
        DebugLogConsole.AddCommand<int, int>("add_item", "เพิ่ม item", (id, num) =>
        {
            inventory.AddItem(id);
        });

        DebugLogConsole.AddCommand<int>("remove_item", "ลบ item", (id) =>
        {
            inventory.RemoveItem(id);
        });

        DebugLogConsole.AddCommand("add_random_item", "สุ่ม item", () =>
        {
            for (int i = 0; i < 10; i++)
            {
                int id = Random.Range(0, 15);
                int randomNum = Random.Range(1, 3);
                inventory.AddItem(id);
            }
        });

        DebugLogConsole.AddCommand("show_inventory", "แสดง inventory", () =>
        {
            UIManager.Instance.ShowInventory(new()
            {
                Items = inventory.GetItems()
            });
        });

        DebugLogConsole.AddCommand<string>("scene", "โหลดฉาก", (name) =>
        {
            SceneLoader.Instance.LoadScene(name, () =>
            {
                Debug.Log($"Scene {name} loaded successfully.");
            });
        });
    }
}