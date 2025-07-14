using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginForm : MonoBehaviour
{
    [SerializeField] private SoutheastAsiaCountryCodes codeCountryData;

    [Header("Form 1 UI")]
    [SerializeField] private GameObject form1Panel;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField nicknameInput;
    [SerializeField] private Button selectCodeCountryButton;
    [SerializeField] private TMP_Text selectedCodeCountryText;
    [SerializeField] private TMP_InputField phoneNumberInput;
    [SerializeField] private TMP_Text codeCountryText;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_Text statusMessageText;
    [SerializeField] private Toggle isAgree;
    [SerializeField] private Button continueButton;
    [SerializeField] private TMP_Text continueText;
    [SerializeField] private Button backButton;
    [Space(5)]
    [Header("Select Code Country UI")]
    [SerializeField] private GameObject codeCountryButtonPrefab;
    [SerializeField] private GameObject codeCountryPanel;
    [SerializeField] private Transform contentPanel;
    [SerializeField] private Button closeButton;
    [Space(5)]
    [Header("Loading UI")]
    [SerializeField] private GameObject loadingPanel;

    [Space(10)]
    [Header("Event Form 1")]
    [SerializeField] private GameEventSO OnSelectCodeCountryButtonPressed;
    [SerializeField] private IntGameEventSO OnSelectedCodeCountry;
    [SerializeField] private GameEventSO OnContinueButtonPressed;
    [SerializeField] private GameEventSO OnBackButtonPressed;
    [SerializeField] private StringGameEventSO OnStatusMessageReceived;
    [SerializeField] private GameEventSO OnCloseCodeCountryButtonPressed;
    [SerializeField] private BoolGameEventSO OnSignUpSuccess;
    [SerializeField] private BoolGameEventSO OnSignInSuccess;
    [SerializeField] private BoolGameEventSO IsSigningSuccess;

    [SerializeField] private string fullPhoneNumber = "";
    private string currentCountryCode = "+62"; // default
    private bool isPhoneNumberEligible = false;

    public string GetPhoneNumber() => fullPhoneNumber;
    public string GetPassword() => passwordInput.text;
    public string GetEmail() => emailInput.text;
    public string GetNickname() => nicknameInput.text;


    [ContextMenu("Log out")]
    public void DeletePlayerprefSessionLogin() {
        SaveManager.ClearLoginSession();
    }
    private void OnEnable()
    {
        OnSelectCodeCountryButtonPressed.Register(HandleOnSelectCodeCountryButtonPressed);
        OnSelectedCodeCountry.Register(HandleOnSelectedCodeCountry);
        OnBackButtonPressed.Register(HandleOnBackButtonPressed);
        OnStatusMessageReceived.Register(HandleOnStatusMessageReceived);
        OnCloseCodeCountryButtonPressed.Register(HandleOnCloseCodeCreatorPressed);
        OnContinueButtonPressed.Register(HandleOnContinueButtonPressed);
        OnSignUpSuccess.Register(HandleCloseLoadingPanel);
        OnSignInSuccess.Register(HandleCloseLoadingPanel);
        IsSigningSuccess.Register(HandleOnSigningSuccess);

        phoneNumberInput.onValueChanged.AddListener(HandleOnPhoneChanged);
        isAgree.onValueChanged.AddListener(HandleOnAgreeChanged);
    }
    private void OnDisable()
    {
        OnSelectCodeCountryButtonPressed.Unregister(HandleOnSelectCodeCountryButtonPressed);
        OnSelectedCodeCountry.Unregister(HandleOnSelectedCodeCountry);
        OnBackButtonPressed.Unregister(HandleOnBackButtonPressed);
        OnStatusMessageReceived.Unregister(HandleOnStatusMessageReceived);
        OnCloseCodeCountryButtonPressed.Unregister(HandleOnCloseCodeCreatorPressed);
        OnContinueButtonPressed.Unregister(HandleOnContinueButtonPressed);
        OnSignUpSuccess.Unregister(HandleCloseLoadingPanel);
        OnSignInSuccess.Unregister(HandleCloseLoadingPanel);
        IsSigningSuccess.Unregister(HandleOnSigningSuccess);

        phoneNumberInput.onValueChanged.RemoveListener(HandleOnPhoneChanged);
        isAgree.onValueChanged.RemoveListener(HandleOnAgreeChanged);
    }
    private void Start()
    {
        GenerateCodeCountry();
    }
    private void HandleOnSigningSuccess(bool value) {
        loadingPanel.SetActive(false);
    }
    private void HandleCloseLoadingPanel(bool value) {
        loadingPanel.SetActive(false);
    }
    private void HandleOnContinueButtonPressed() {
        loadingPanel.SetActive(true);
    }
    private void HandleOnCloseCodeCreatorPressed() {
        codeCountryPanel.SetActive(false);
    }
    private void HandleOnAgreeChanged(bool value)
    {
        EvaluateSignInEligibility();
    }
    private void HandleOnStatusMessageReceived(string message) {
        statusMessageText.text = message;
        loadingPanel.SetActive(false);
    }
    
    private void HandleOnPhoneChanged(string input)
    {
        UpdatePhoneNumber(input);
        EvaluateSignInEligibility(); 
    }
    private void HandleOnSelectedCodeCountry(int index) {
        selectedCodeCountryText.text = codeCountryData.countryCodes[index].countryName;
        codeCountryText.text = codeCountryData.countryCodes[index].countryCode;

        SetCountryCode(codeCountryData.countryCodes[index].countryCode);

        UpdatePhoneNumber(phoneNumberInput.text);

        codeCountryPanel.SetActive(false);
    }
    private void HandleOnSelectCodeCountryButtonPressed() { 
        //show code country panel
        codeCountryPanel.SetActive(true);
    }
    private void HandleOnBackButtonPressed()
    {
        //clear form text
        ResetForm();
    }


    /// <summary>
    /// Logic's Here
    /// </summary>
    private void ResetForm()
    {
        selectedCodeCountryText.text = "Country/Region";
        phoneNumberInput.text = "";
        codeCountryText.text = "";
        passwordInput.text = "";
        isAgree.isOn = false;
    }
    private void EvaluateSignInEligibility()
    {
        bool isEligible = isPhoneNumberEligible && isAgree.isOn;
        SetSignInInteractable(isEligible);
    }
    private void GenerateCodeCountry()
    {
        for (int i = 0; i < codeCountryData.countryCodes.Count; i++)
        {

            GameObject item = Instantiate(codeCountryButtonPrefab, contentPanel);
            TMP_Text itemText = item.GetComponentInChildren<TMP_Text>();
            EventButtonSO buttonSO = item.GetComponent<EventButtonSO>();

            itemText.text = $"{codeCountryData.countryCodes[i].countryName} - ({codeCountryData.countryCodes[i].countryCode})";
            buttonSO.intValue = i;
        }
    }
    private void SetCountryCode(string code)
    {
        currentCountryCode = code;
        ValidatePhoneNumber(fullPhoneNumber);
    }
    private void ValidatePhoneNumber(string number)
    {
        isPhoneNumberEligible = number.Length > 7;
        //OnEligibleToRegister.Raise(isPhoneNumberEligible);
    }
    private void UpdatePhoneNumber(string input)
    {
        fullPhoneNumber = ConvertToInternationalFormat(input);
        ValidatePhoneNumber(fullPhoneNumber);
    }
    private string ConvertToInternationalFormat(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";

        if (input.StartsWith("0"))
            return currentCountryCode + input.Substring(1);
        if (input.StartsWith("+"))
            return input;

        return currentCountryCode + input;
    }
    private void SetSignInInteractable(bool state)
    {
        continueButton.interactable = state;

        Color targetColor = state
            ? continueButton.colors.normalColor
            : continueButton.colors.disabledColor;

        continueText.color = targetColor;

    }
}
