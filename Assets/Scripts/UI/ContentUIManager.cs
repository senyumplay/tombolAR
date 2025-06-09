using System;
using TMPro;
using UnityEngine;

public class ContentUIManager : MonoBehaviour
{
    [SerializeField] private GameEventSO onRecentButtonPressed;
    [SerializeField] private GameEventSO onCloseContentButtonPressed;
    [SerializeField] private int idContent;
    [SerializeField] private ToggleTranslate toggleTranslate;

    [Header("UI Component")]
    [SerializeField] private GameObject contentPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text arabicText;
    [SerializeField] private TMP_Text translateText;

    private string cachedIndo;
    private string cachedEnglish;

    private void OnEnable()
    {
        onRecentButtonPressed?.Register(HandleOnRecentButtonPressed);
        onCloseContentButtonPressed?.Register(HandleOnCloseRecentButtonPressed);

        ContentManager.onLoadContentCompleted += UpdateContentUI;
        ToggleTranslate.OnModeChanged += UpdateTranslationLanguage;
    }

    private void OnDisable()
    {
        onRecentButtonPressed?.Unregister(HandleOnRecentButtonPressed);
        onCloseContentButtonPressed?.Unregister(HandleOnCloseRecentButtonPressed);

        ContentManager.onLoadContentCompleted -= UpdateContentUI;
        ToggleTranslate.OnModeChanged -= UpdateTranslationLanguage;
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

    private void UpdateTranslationLanguage(ToggleTranslate.Mode mode)
    {
        translateText.text = (mode == ToggleTranslate.Mode.ID) ? cachedIndo : cachedEnglish;
    }

    private void HandleOnCloseRecentButtonPressed()
    {
        contentPanel.SetActive(false);
    }

    private void HandleOnRecentButtonPressed()
    {
        contentPanel.SetActive(true);
    }
}
