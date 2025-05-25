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
    private Button? exitButton;
    [SerializeField]
    private UISetting? uiSetting;
    [SerializeField]
    private UISave? uiSave;

    public void Setup(Param param)
    {
        playButton?.onClick.AddListener(() =>
        {
            param.OnPlay?.Invoke();
        });
        exitButton?.onClick.AddListener(() =>
        {
            param.OnExit?.Invoke();
        });
    }
}
