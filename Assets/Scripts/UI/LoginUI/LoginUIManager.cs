using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CountryCodeData
{
    public string label;
    public string code;
}

public class LoginUIManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TMP_InputField phoneNumberInput;
    [SerializeField] private TMP_Dropdown codeDropdown;
    [SerializeField] private TMP_Text codeText;
    [SerializeField] private Button signInButton;
    [SerializeField] private RectTransform phoneParent;

    [Header("Data")]
    [SerializeField] private List<CountryCodeData> countryCodes;

    [Header("Reference")]
    [SerializeField] private PhoneNumberManager logicManager;
    [SerializeField] private BoolGameEventSO eligibleToRegister;

    [Header("Animation")]
    [SerializeField] private float moveYAmount = 300f;
    [SerializeField] private float moveDuration = 0.3f;

    private Vector2 originalPos;
    private void OnEnable()
    {
        eligibleToRegister.Register(HandleOnEligibleToRegister);
    }
    private void OnDisable()
    {
        eligibleToRegister.Unregister(HandleOnEligibleToRegister);
    }
    private void Start()
    {
        if (phoneParent != null)
            originalPos = phoneParent.anchoredPosition;

        codeDropdown.onValueChanged.AddListener(OnCountryCodeChanged);
        phoneNumberInput.onValueChanged.AddListener(OnPhoneChanged);
        phoneNumberInput.onSelect.AddListener(_ => MovePanel(true));
        phoneNumberInput.onDeselect.AddListener(_ => MovePanel(false));
        phoneNumberInput.onEndEdit.AddListener(_ => MovePanel(false));

        SetInitialCountryCode();
    }
    private void HandleOnEligibleToRegister(bool isEligible) {
        SetSignInInteractable(isEligible);
    }
    private void SetInitialCountryCode()
    {
        OnCountryCodeChanged(0);
    }

    private void OnCountryCodeChanged(int index)
    {
        if (index >= 0 && index < countryCodes.Count)
        {
            string code = countryCodes[index].code;
            codeText.text = code;
            logicManager.SetCountryCode(code);
        }
    }

    private void OnPhoneChanged(string input)
    {
        logicManager.UpdatePhoneNumber(input);
    }

    private void SetSignInInteractable(bool state)
    {
        signInButton.interactable = state;
    }

    private void MovePanel(bool up)
    {
        if (phoneParent == null) return;

        Vector2 target = up ? originalPos + new Vector2(0, moveYAmount) : originalPos;
        phoneParent.DOAnchorPos(target, moveDuration).SetEase(Ease.OutQuad);
    }
}
