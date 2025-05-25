using UnityEngine;

public class BattleBaseManager : Singleton<BattleBaseManager>
{
    [Header("QTE Prefab")]
    public GameObject QTEObject;
    private RectTransform pointerTransform;
    public Transform uiContainer;

    public void ForceQTEPlay() // Fix the scale
    {
        GameObject canvasGO = GameObject.Find("Canvas");

        QTEObject = Resources.Load<GameObject>("prefabs/QTEvent");
        if (QTEObject == null)
        {
            Debug.LogError("QTEObject not found in Resources.");
            return;
        }

        Canvas canvasUI = FindFirstObjectByType<Canvas>();
        if (canvasUI == null)
        {
            Debug.LogError("No Canvas found in scene.");
            return;
        }

        GameObject qteObj = Instantiate(QTEObject, canvasUI.transform);
        RectTransform rect = qteObj.GetComponent<RectTransform>();

        // ChatGPT
        rect.localScale = Vector3.one;
        rect.anchoredPosition = Vector3.zero;
        rect.offsetMin = Vector3.zero;
        rect.offsetMax = Vector3.zero;

        pointerTransform = qteObj.transform.Find("Pointer").GetComponent<RectTransform>();
        if (pointerTransform == null)
        {
            Debug.LogError("Pointer not found in QTE prefab.");
            return;
        }
    }
}