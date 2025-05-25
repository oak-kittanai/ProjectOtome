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

    public void ShowInventory(UIInventory.Param param)
    {
        Utility.LoadUI("Inventory", obj =>
        {
            if (obj == null) return;
            var inventory = Instantiate(obj, canvas.transform).GetComponent<UIInventory>();
            if (inventory != null) inventory.Setup(param);
        });
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
