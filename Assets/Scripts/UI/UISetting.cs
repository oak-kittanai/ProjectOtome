using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    public struct Param
    {
        public float MasterVolume;
        public float BgmVolume;
        public float SfxVolume;
        public Language Language;
        public Action<float> OnMasterChanged;
        public Action<float> OnBgmChanged;
        public Action<float> OnSfxChanged;
        public Action<Language> OnLanguageChanged;
        public Action OnDefault;
        public Action OnSave;
    }

    [Serializable]
    public class SliderSetting
    {
        public Slider slider;
        public TMP_Text percentText;
    }

    [SerializeField]
    private SliderSetting masterSlider;
    [SerializeField]
    private SliderSetting bgmSlider;
    [SerializeField]
    private SliderSetting sfxSlider;
    [SerializeField]
    private Toggle[] languageToggles;
    [SerializeField]
    private Button defaultButton;
    [SerializeField]
    private Button saveButton;

    public void Setup(Param param)
    {
        if (masterSlider != null)
        {
            masterSlider.slider.value = param.MasterVolume;
            masterSlider.slider.onValueChanged.AddListener(value =>
            {
                param.OnMasterChanged?.Invoke(value);
                masterSlider.percentText.text = $"{value * 100:F0}%";
            });
        }

        if (bgmSlider != null)
        {
            bgmSlider.slider.value = param.BgmVolume;
            bgmSlider.slider.onValueChanged.AddListener(value =>
            {
                param.OnBgmChanged?.Invoke(value);
                bgmSlider.percentText.text = $"{value * 100:F0}%";
            });
        }

        if (sfxSlider != null)
        {
            sfxSlider.slider.value = param.SfxVolume;
            sfxSlider.slider.onValueChanged.AddListener(value =>
            {
                param.OnSfxChanged?.Invoke(value);
                sfxSlider.percentText.text = $"{value * 100:F0}%";
            });
        }

        for (int i = 0; i < languageToggles.Length; i++)
        {
            int index = i;
            languageToggles[i].isOn = (int)param.Language == index;
            languageToggles[i].onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    param.OnLanguageChanged?.Invoke((Language)index);
                }
            });
        }

        defaultButton.onClick.AddListener(() => param.OnDefault?.Invoke());
        saveButton.onClick.AddListener(() => param.OnSave?.Invoke());
    }
}
