using System;
using UnityEngine;

public class Game : Singleton<Game>
{
    [Header("Singleton")]
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private SceneLoader sceneLoader;

    [Header("Console")]
    [SerializeField]
    private GameObject devConsole;
    [SerializeField]
    private bool enableConsole;

    private SaveSystem saveSystem;
    public SaveSystem GetSaveSystem() => saveSystem;

    private Database database;
    public Database GetDatabase() => database;
    
    private InventoryController inventory;
    public InventoryController GetInventory() => inventory;

    private ShopControl shop;
    public ShopControl GetShop() => shop;

    private DevCommand devCommand;

    protected override void Awake()
    {
        base.Awake();
        Utility.Setup(this);
        Init();
    }

    void Init()
    {
        if (uiManager != null) Instantiate(uiManager);
        if (sceneLoader != null) Instantiate(sceneLoader);

        saveSystem = new();
        database = new();
        inventory = new(database);
        inventory.Setup(Array.Empty<int>());

        shop = new(database);
        inventory.Setup(Array.Empty<int>());

        devCommand = new(inventory, shop);
        if (enableConsole && devConsole != null) Instantiate(devConsole);
    }
}
