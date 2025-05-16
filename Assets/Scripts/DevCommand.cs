using UnityEngine;
using IngameDebugConsole;

public class DevCommand
{
    public DevCommand()
    {
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
    }
}
