using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingUIManager : MonoBehaviour
{
    [SerializeField] private GameEventSO onSettingButtonPressed;
    [SerializeField] private GameEventSO onCloseSettingButtonPressed;

    [Header("UI Component")]
    [SerializeField] private GameObject settingPanel;

    [Header("Text Size")]
    [SerializeField] private Slider textSize;
    [SerializeField] private float minTextSize = 24f;
    [SerializeField] private float maxTextSize = 48f;

    [Header("Video Volume")]
    [SerializeField] private Slider videoVolume;
    [SerializeField] private float minVideoVolume = 0f;
    [SerializeField] private float maxVideoVolume = 1f;

    [Header("Reciter")]
    [SerializeField] private TMP_Dropdown selectReciter;
    [SerializeField] private Slider reciterVolume;
    [SerializeField] private float minReciterVolume = 0f;
    [SerializeField] private float maxReciterVolume = 1f;

    private const string TEXT_SIZE_KEY = "TextSize";
    private const string VIDEO_VOLUME_KEY = "VideoVolume";
    private const string RECITER_SELECTED_KEY = "ReciterSelected";
    private const string RECITER_VOLUME_KEY = "ReciterVolume";

    public static Action<float> OnTextSizeChanged;       // Called when text size updated
    public static Action<float> OnVideoVolumeChanged;    // Called when video volume updated
    public static Action<float> OnReciterVolumeChanged;  // Called when reciter volume updated

    private void OnEnable()
    {
        onSettingButtonPressed.Register(HandleSettingButtonPressed);
        onCloseSettingButtonPressed.Register(HandleCloseSettingButtonPressed);

        LoadSettings();
        AddListeners();
    }

    private void OnDisable()
    {
        onSettingButtonPressed.Unregister(HandleSettingButtonPressed);
        onCloseSettingButtonPressed.Unregister(HandleCloseSettingButtonPressed);

        RemoveListeners();
    }

    private void HandleSettingButtonPressed()
    {
        settingPanel.SetActive(true);
    }

    private void HandleCloseSettingButtonPressed()
    {
        SaveAllSettings();
        settingPanel.SetActive(false);
    }

    private void LoadSettings()
    {
        float savedTextSize = PlayerPrefs.GetFloat(TEXT_SIZE_KEY, minTextSize);
        float normalizedTextSize = Mathf.InverseLerp(minTextSize, maxTextSize, savedTextSize);
        textSize.value = normalizedTextSize;

        float savedVideoVolume = PlayerPrefs.GetFloat(VIDEO_VOLUME_KEY, minVideoVolume);
        float normalizedVideoVolume = Mathf.InverseLerp(minVideoVolume, maxVideoVolume, savedVideoVolume);
        videoVolume.value = normalizedVideoVolume;

        selectReciter.value = PlayerPrefs.GetInt(RECITER_SELECTED_KEY, 0);

        float savedReciterVolume = PlayerPrefs.GetFloat(RECITER_VOLUME_KEY, minReciterVolume);
        float normalizedReciterVolume = Mathf.InverseLerp(minReciterVolume, maxReciterVolume, savedReciterVolume);
        reciterVolume.value = normalizedReciterVolume;

        // Apply immediately
        ApplyTextSize(textSize.value);
        ApplyVideoVolume(videoVolume.value);
        ApplyReciterVolume(reciterVolume.value);
    }

    private void SaveAllSettings()
    {
        float actualTextSize = Mathf.Lerp(minTextSize, maxTextSize, textSize.value);
        float actualVideoVolume = Mathf.Lerp(minVideoVolume, maxVideoVolume, videoVolume.value);
        float actualReciterVolume = Mathf.Lerp(minReciterVolume, maxReciterVolume, reciterVolume.value);

        PlayerPrefs.SetFloat(TEXT_SIZE_KEY, actualTextSize);
        PlayerPrefs.SetFloat(VIDEO_VOLUME_KEY, actualVideoVolume);
        PlayerPrefs.SetInt(RECITER_SELECTED_KEY, selectReciter.value);
        PlayerPrefs.SetFloat(RECITER_VOLUME_KEY, actualReciterVolume);

        PlayerPrefs.Save();
    }

    private void AddListeners()
    {
        textSize.onValueChanged.AddListener(ApplyTextSize);
        videoVolume.onValueChanged.AddListener(ApplyVideoVolume);
        reciterVolume.onValueChanged.AddListener(ApplyReciterVolume);
    }

    private void RemoveListeners()
    {
        textSize.onValueChanged.RemoveListener(ApplyTextSize);
        videoVolume.onValueChanged.RemoveListener(ApplyVideoVolume);
        reciterVolume.onValueChanged.RemoveListener(ApplyReciterVolume);
    }

    private void ApplyTextSize(float sliderValue)
    {
        float actualSize = Mathf.Lerp(minTextSize, maxTextSize, sliderValue);
        OnTextSizeChanged?.Invoke(actualSize);
    }

    private void ApplyVideoVolume(float sliderValue)
    {
        float volume = Mathf.Lerp(minVideoVolume, maxVideoVolume, sliderValue);
        OnVideoVolumeChanged?.Invoke(volume);
    }

    private void ApplyReciterVolume(float sliderValue)
    {
        float volume = Mathf.Lerp(minReciterVolume, maxReciterVolume, sliderValue);
        OnReciterVolumeChanged?.Invoke(volume);
    }
}
