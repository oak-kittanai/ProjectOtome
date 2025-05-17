using UnityEngine;
using IngameDebugConsole;

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
            // OpenDialogue(id);
            Debug.Log("open dialogue " + id);
        });

        DebugLogConsole.AddCommand<int>("add_item", "เพิ่ม item", (id) =>
        {
            inventory.AddItem(id);
        });

        DebugLogConsole.AddCommand<int>("remove_item", "ลบ item", (id) =>
        {
            inventory.RemoveItem(id);
        });
    }
}
