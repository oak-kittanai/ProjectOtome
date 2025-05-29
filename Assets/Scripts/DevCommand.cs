using UnityEngine;
using IngameDebugConsole;
using UnityEditor.SearchService;

public class DevCommand
{
    private InventoryController inventory;

    public DevCommand(InventoryController inventory)
    {
        this.inventory = inventory;
        RegisterCommand();
    }

    private void RegisterCommand()
    {
        DebugLogConsole.AddCommand<string>("dialogue", "แสดง dialogue ทันที", (id) =>
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogWarning("invalid dialogue id");
                return;
            }

            UIManager.Instance.ShowDialogue(new()
            {
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

        DebugLogConsole.AddCommand<int>("add_item", "เพิ่ม item", (id) =>
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