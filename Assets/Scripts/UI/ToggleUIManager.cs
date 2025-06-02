using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUIManager<T> : MonoBehaviour where T : System.Enum
{
    [Header("Reference")]
    [SerializeField] private ToggleButton<T> targetToggle;

    [Header("UI")]
    [SerializeField] private Button toggleButton;
    [SerializeField] private RectTransform activeImage;
    [SerializeField] private Image value1IconImage;
    [SerializeField] private Image value2IconImage;

    [Header("Setting Values")]
    [SerializeField] private float xOffset = 80;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float targetAlpha = 0.5f;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(OnToggleButtonClicked);
        }
    }

    private void OnDestroy()
    {
        if (toggleButton != null)
        {
            toggleButton.onClick.RemoveListener(OnToggleButtonClicked);
        }
    }

    private void OnEnable()
    {
        if (targetToggle != null)
        {
            ToggleButton<T>.OnSetModeActive += UpdateUIToggle;
            UpdateUIToggle(targetToggle.CurrentMode); // Force refresh UI on start
        }
    }

    private void OnDisable()
    {
        if (targetToggle != null)
        {
            ToggleButton<T>.OnSetModeActive -= UpdateUIToggle;
        }
    }

    private void OnToggleButtonClicked()
    {
        if (targetToggle != null)
        {
            targetToggle.ToggleState();
        }
    }

    private void UpdateUIToggle(T mode)
    {
        if (targetToggle == null)
            return;

        // Perlu mapping: Mode apa artinya untuk Icon
        if (mode.Equals(System.Enum.GetValues(typeof(T)).GetValue(0)))
        {
            // Mode pertama (value 0)
            value1IconImage.DOFade(1.0f, fadeDuration);
            value2IconImage.DOFade(targetAlpha, fadeDuration);
            activeImage.DOAnchorPos(new Vector2(-xOffset, 0f), duration).SetEase(Ease.OutQuad);
        }
        else
        {
            // Mode kedua (value 1 atau lainnya)
            value2IconImage.DOFade(1.0f, fadeDuration);
            value1IconImage.DOFade(targetAlpha, fadeDuration);
            activeImage.DOAnchorPos(new Vector2(xOffset, 0f), duration).SetEase(Ease.OutQuad);
        }
    }
}
