using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LoginUIManager : MonoBehaviour
{
    [Header("Event Button")]
    [SerializeField] private GameEventSO onBackButtonPressed;
    [SerializeField] private GameEventSO onSelectCodeCountryButtonPressed;

    [SerializeField] private BoolGameEventSO onSignUpSuccess;
    [SerializeField] private IntGameEventSO onSelectedCodeCountry;

    [Header("Code Country Data")]
    [SerializeField] private SoutheastAsiaCountryCodes seaData;
    [Header("Code Country Prefab")]
    [SerializeField] private GameObject codeCountryPrefab;
    [Header("New UI Sign in")]
    [SerializeField] private Transform codeCountryContainer;
    [SerializeField] private GameObject codeCountryPanel;
    [SerializeField] private Button selectCodeCountryButton;
    [SerializeField] private TMP_Text codeCountryText;
    [SerializeField] private TMP_Text nameCountryText;
    [SerializeField] private TMP_InputField phoneNumberInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Toggle isAgree;

    [Header("New UI Sign in Button")]
    [SerializeField] private Button signInButton;
    [SerializeField] private TMP_Text signInButtonText;

    [Header("UI Components")]
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField nicknameInputField;
    
    [SerializeField] private RectTransform formLoginParent;
    

    [Header("Reference")]
    [SerializeField] private PhoneNumberManager logicManager;
    [SerializeField] private BoolGameEventSO eligibleToRegister;

    private void OnEnable()
    {
        onBackButtonPressed.Register(HandleBackButtonPressed);
        onSelectCodeCountryButtonPressed.Register(HandleCodeCountryPressed);
        onSelectedCodeCountry.Register(HandleSelectedCodeCountry);

        onSignUpSuccess.Register(HandleOnSignUpSuccess);
        eligibleToRegister.Register(HandleOnEligibleToRegister);
    }
    private void OnDisable()
    {
        onBackButtonPressed.Unregister(HandleBackButtonPressed);
        onSelectCodeCountryButtonPressed.Unregister(HandleCodeCountryPressed);
        onSelectedCodeCountry.Unregister(HandleSelectedCodeCountry);

        onSignUpSuccess.Unregister(HandleOnSignUpSuccess);
        eligibleToRegister.Unregister(HandleOnEligibleToRegister);
    }
    private void Start()
    {

        phoneNumberInputField.onValueChanged.AddListener(OnPhoneChanged);

        GenerateCountryCode();
        
    }
    private void HandleOnSignUpSuccess(bool value) { 
        
    }
    private void GenerateCountryCode()
    {
        for (int i = 0; i < seaData.countryCodes.Count; i++)
        {

            GameObject item = Instantiate(codeCountryPrefab, codeCountryContainer);
            TMP_Text itemText = item.GetComponentInChildren<TMP_Text>();
            EventButtonSO buttonSO = item.GetComponent<EventButtonSO>();

            itemText.text = $"{seaData.countryCodes[i].countryName} - ({seaData.countryCodes[i].countryCode})";
            buttonSO.intValue = i;
        }
    }
    
    private void HandleSelectedCodeCountry(int value) {

        codeCountryText.text = seaData.countryCodes[value].countryCode;
        nameCountryText.text = seaData.countryCodes[value].countryName;

        logicManager.SetCountryCode(seaData.countryCodes[value].countryCode);

        codeCountryPanel.SetActive(false);
    }
    private void HandleCodeCountryPressed() {
        codeCountryPanel.SetActive(true);
    }
    private void HandleBackButtonPressed() {
        emailInputField.text = "";
        passwordInputField.text = "";
        nicknameInputField.text = "";
        phoneNumberInputField.text = "";
    }
    private void HandleOnEligibleToRegister(bool isEligible) {
        if(isEligible && isAgree)
            SetSignInInteractable(isEligible);
    }

    private void OnPhoneChanged(string input)
    {
        logicManager.UpdatePhoneNumber(input);
    }

    private void SetSignInInteractable(bool state)
    {
        signInButton.interactable = state;

        Color targetColor = state
            ? signInButton.colors.normalColor
            : signInButton.colors.disabledColor;

        signInButtonText.color = targetColor;

    }

    
    public string GetEmail()=> emailInputField.text;
    public string GetPassword()=> passwordInputField.text;
    public string GetNickname()=> nicknameInputField.text;
}
