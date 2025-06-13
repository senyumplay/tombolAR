using System;
using TMPro;
using UnityEngine;

public class ContentUIManager : MonoBehaviour
{
    [SerializeField] private ImageTargetContentHandler imageTargetContentHandler;
    [SerializeField] private GameEventSO onRecentButtonPressed;
    [SerializeField] private GameEventSO onCloseContentButtonPressed;
    [SerializeField] private GameEventSO onCloseSettingButtonPressed;
    [SerializeField] private GameEventSO onImageTargetDetected;
    [SerializeField] private int idContent;
    [SerializeField] private ToggleTranslate toggleTranslate;

    [Header("UI Component")]
    [SerializeField] private GameObject contentPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text arabicText;
    [SerializeField] private TMP_Text translateText;

    private string cachedIndo;
    private string cachedEnglish;

    private const string TEXT_SIZE_KEY = "TextSize";

    private void OnEnable()
    {
        onRecentButtonPressed?.Register(HandleOnRecentButtonPressed);
        onCloseContentButtonPressed?.Register(HandleOnCloseRecentButtonPressed);
        onCloseSettingButtonPressed?.Register(ApplyTextSizeSetting);
        onImageTargetDetected?.Register(HandleOnImageTargetDetected);

        SettingUIManager.OnTextSizeChanged += HandleTextSizeChanged;

        ContentManager.onLoadContentCompleted += UpdateContentUI;
        ToggleTranslate.OnModeChanged += UpdateTranslationLanguage;
    }

    private void OnDisable()
    {
        onRecentButtonPressed?.Unregister(HandleOnRecentButtonPressed);
        onCloseContentButtonPressed?.Unregister(HandleOnCloseRecentButtonPressed);
        onCloseSettingButtonPressed?.Unregister(ApplyTextSizeSetting);
        onImageTargetDetected?.Unregister(HandleOnImageTargetDetected);

        SettingUIManager.OnTextSizeChanged -= HandleTextSizeChanged;

        ContentManager.onLoadContentCompleted -= UpdateContentUI;
        ToggleTranslate.OnModeChanged -= UpdateTranslationLanguage;
    }

    private void Start()
    {
        ApplyTextSizeSetting();
    }

    private void UpdateContentUI(int id, string title, string arabic, string indo, string english)
    {
        idContent = id;
        titleText.text = title;
        arabicText.text = arabic;

        cachedIndo = indo;
        cachedEnglish = english;

        UpdateTranslationLanguage(toggleTranslate.CurrentMode);
    }
    private void HandleOnImageTargetDetected() {
        contentPanel.SetActive(true);
    }
    private void UpdateTranslationLanguage(ToggleTranslate.Mode mode)
    {
        translateText.text = (mode == ToggleTranslate.Mode.ID) ? cachedIndo : cachedEnglish;
    }
    private void HandleTextSizeChanged(float newSize)
    {
        arabicText.fontSize = newSize;
        translateText.fontSize = newSize;
    }

    private void HandleOnCloseRecentButtonPressed()
    {
        contentPanel.SetActive(false);
        imageTargetContentHandler.EnableARCamera();
    }

    private void HandleOnRecentButtonPressed()
    {
        contentPanel.SetActive(true);
    }

    private void ApplyTextSizeSetting()
    {
        float textSize = PlayerPrefs.GetFloat(TEXT_SIZE_KEY, 36f); // default size
        arabicText.fontSize = textSize;
        translateText.fontSize = textSize;
    }
}
