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

            DialogueSystem.Instance.ForcePlayDialogue(id);
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
    }
}