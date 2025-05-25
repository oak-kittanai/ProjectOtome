# nullable enable

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISaveItem : MonoBehaviour
{
    [SerializeField]
    private Button? button;
    [SerializeField]
    private TMP_Text? dateText;

    public void Setup(SaveData data, Action? onClick = null)
    {
        button?.onClick.AddListener(() => onClick?.Invoke());
        if (dateText != null) dateText.text = data.SaveTime.ToString("g");
    }
}
