using UnityEngine;

public class Game : Singleton<Game>
{
    [Header("Console")]
    [SerializeField]
    private GameObject devConsole;
    [SerializeField]
    private bool enableConsole;

    private DevCommand devCommand;

    protected override void Awake()
    {
        base.Awake();
        Utility.Setup(this);
        Init();
    }

    void Init()
    {
        devCommand = new();
        if (enableConsole) Instantiate(devConsole);
    }
}
