#nullable enable

using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    public struct Param
    {
        public Action OnPlay;
        public Action OnExit;
    }

    [SerializeField]
    private Button? playButton;
    [SerializeField]
    private Button? settingButton;
    [SerializeField]
    private Button? exitButton;
    [SerializeField]
    private Button? backButton;
    [SerializeField]
    private GameObject? uiTitle;
    [SerializeField]
    private UISetting? uiSetting;
    [SerializeField]
    private UISave? uiSave;

    void Start()
    {
        uiTitle?.SetActive(true);
        uiSetting?.gameObject.SetActive(false);
        uiSave?.gameObject.SetActive(false);
        backButton?.gameObject.SetActive(false);
    }

    public void Setup(Param param)
    {
        uiSave?.Setup(new UISave.Param
        {
            SaveDatas = Game.Instance.GetSaveSystem().GetSaveDatas(),
            OnNewGame = param.OnPlay
        }, UISave.Mode.Save);

        playButton?.onClick.AddListener(() =>
        {
            uiSave?.gameObject.SetActive(true);
            uiTitle?.SetActive(false);
            SetBackButton(() =>
            {
                uiSave?.gameObject.SetActive(false);
                uiTitle?.SetActive(true);
            });
        });
        settingButton?.onClick.AddListener(() =>
        {
            uiSetting?.gameObject.SetActive(true);
            uiTitle?.SetActive(false);
            SetBackButton(() =>
            {
                uiSetting?.gameObject.SetActive(false);
                uiTitle?.SetActive(true);
            });
        });
        exitButton?.onClick.AddListener(() =>
        {
            param.OnExit?.Invoke();
        });
    }

    private void SetBackButton(Action onBack)
    {
        if (backButton != null)
        {
            backButton.gameObject.SetActive(true);
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() =>
            {
                onBack?.Invoke();
                uiSetting?.gameObject.SetActive(false);
                uiSave?.gameObject.SetActive(false);
                backButton.gameObject.SetActive(false);
            });
        }
    }
}
