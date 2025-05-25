# nullable enable

using System;
using UnityEngine;
using UnityEngine.UI;

public class UISave : MonoBehaviour
{
    public enum Mode { Save, Load }

    public struct Param
    {
        public SaveData[] SaveDatas;
        public Action OnNewGame;
        public Action<int> OnLoadGame;
    }

    [SerializeField]
    private Button? newButton;
    [SerializeField]
    private UISaveItem? saveItemPrefab;
    [SerializeField]
    private Transform? contentTransform;
    [SerializeField]
    private GameObject? noData;

    private Action<int>? onLoadGame;
    private SaveData[] saveDatas = Array.Empty<SaveData>();

    public void Setup(Param param, Mode mode)
    {
        if (newButton != null)
        {
            newButton.gameObject.SetActive(mode == Mode.Save);
            newButton.onClick.AddListener(() =>
            {
                newButton.interactable = false;
                Utility.WaitForSeconds(0.5f, () => newButton.interactable = true);
                if (mode == Mode.Save)
                {
                    param.OnNewGame?.Invoke();
                }
                else
                {
                    onLoadGame?.Invoke(-1); // -1 indicates new game
                }
            });
        }

        onLoadGame = mode == Mode.Load ? param.OnLoadGame : null;
        saveDatas = param.SaveDatas;       
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (contentTransform == null || saveItemPrefab == null)
        {
            Debug.LogError("Content Transform or Save Item Prefab is not set.");
            return;
        }
        
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < saveDatas.Length; i++)
        {
            int index = i;
            var saveData = saveDatas[index];
            var saveItem = Instantiate(saveItemPrefab, contentTransform);
            saveItem?.Setup(saveData, () =>
            {
                onLoadGame?.Invoke(index);
            });
        }

        noData?.SetActive(saveDatas.Length == 0);
    }
}
