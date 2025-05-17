using System;
using UnityEngine;

public class Game : Singleton<Game>
{
    [Header("Console")]
    [SerializeField]
    private GameObject devConsole;
    [SerializeField]
    private bool enableConsole;

    private Database database;
    private InventoryController inventory;
    private DevCommand devCommand;

    protected override void Awake()
    {
        base.Awake();
        Utility.Setup(this);
        Init();
    }

    void Init()
    {
        database = new();
        inventory = new(database);
        inventory.Setup(Array.Empty<int>());

        devCommand = new(inventory);
        if (enableConsole) Instantiate(devConsole);
    }
}
