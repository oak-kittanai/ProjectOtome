using System;
using UnityEngine;

public class Game : Singleton<Game>
{
    [Header("Singleton")]
    [SerializeField]
    private UIManager uiManager;

    [Header("Console")]
    [SerializeField]
    private GameObject devConsole;
    [SerializeField]
    private bool enableConsole;

    private Database database;
    public Database GetDatabase() => database;
    
    private InventoryController inventory;
    public InventoryController GetInventory() => inventory;

    private DevCommand devCommand;

    protected override void Awake()
    {
        base.Awake();
        Utility.Setup(this);
        Init();
    }

    void Init()
    {
        Instantiate(uiManager);

        database = new();
        inventory = new(database);
        inventory.Setup(Array.Empty<int>());

        devCommand = new(inventory);
        if (enableConsole) Instantiate(devConsole);
    }
}
