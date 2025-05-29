using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    private Canvas canvas;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        var obj = GameObject.Find("Canvas");
        if (obj != null)
        {
            canvas = obj.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Single)
        {
            Setup();
        }
    }

    public void ShowMainMenu(UIMainMenu.Param param)
    {
        Utility.LoadUI("MainMenu", obj =>
        {
            if (obj == null) return;
            var mainMenu = Instantiate(obj, canvas.transform).GetComponent<UIMainMenu>();
            if (mainMenu != null) mainMenu.Setup(param);
        });
    }

    public void ShowInventory(UIInventory.Param param)
    {
        Utility.LoadUI("Inventory", obj =>
        {
            if (obj == null) return;
            var inventory = Instantiate(obj, canvas.transform).GetComponent<UIInventory>();
            if (inventory != null) inventory.Setup(param);
        });
    }

    public void ShowDialogue(DialogueSystem.Param param)
    {
        Utility.LoadUI("Dialogue", obj =>
        {
            if (obj == null) return;
            var dialogue = Instantiate(obj, canvas.transform).GetComponent<DialogueSystem>();
            if (dialogue != null) dialogue.Setup(param);
        });
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
