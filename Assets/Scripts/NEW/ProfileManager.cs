using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    [Header("Cognito")]
    [SerializeField] private CognitoSDKController cognitoSDKController;
    [Space(5)]
    [Header("UI Component")]
    [SerializeField] private Image profilePictureImage;
    [SerializeField] private TMP_Text nickNameText;

    [SerializeField] private TMP_Text nickNameProfileText;
    [SerializeField] private TMP_InputField nickNameProfileInput;
    [SerializeField] private TMP_Text emailProfileText;
    [SerializeField] private TMP_InputField emailProfileInput;
    [SerializeField] private TMP_Text phoneProfileText;
    [SerializeField] private TMP_InputField phoneProfileInput;

    [SerializeField] private Button submitButton;
    [SerializeField] private Button logOutButton;
    [SerializeField] private Button deleteAccountButton;

    [Space(5)]
    [Header("Events")]
    [SerializeField] private GameEventSO onLogoutButtonPressed;
    [SerializeField] private GameEventSO onDeleteButtonPressed;
    [SerializeField] private GameEventSO onProfileButtonPressed;
    [SerializeField] private GameEventSO onUpdateProfile;
    [SerializeField] private BoolGameEventSO OnSignUpSuccess;
    [SerializeField] private BoolGameEventSO OnSignInSuccess;

    public string GetNickname() => nickNameProfileInput.text;
    public string GetEmail() => emailProfileInput.text;
    private void OnEnable()
    {
        onLogoutButtonPressed.Register(HandleLogoutButtonPressed);
        onDeleteButtonPressed.Register(HandleDeleteButtonPressed);
        OnSignUpSuccess.Register(HandleOnSignUpOnSignInSuccess);
        OnSignInSuccess.Register(HandleOnSignUpOnSignInSuccess);

        onProfileButtonPressed.Register(HandleOnProfileButtonPressed);
        onUpdateProfile.Register(HandleOnUpdateInfo);

    }
    private void OnDisable()
    {
        onLogoutButtonPressed.Unregister(HandleLogoutButtonPressed);
        onDeleteButtonPressed.Unregister(HandleDeleteButtonPressed);
        OnSignUpSuccess.Unregister(HandleOnSignUpOnSignInSuccess);
        OnSignInSuccess.Unregister(HandleOnSignUpOnSignInSuccess);

        onProfileButtonPressed.Unregister(HandleOnProfileButtonPressed);
        onUpdateProfile.Unregister(HandleOnUpdateInfo);
    }
    private void HandleOnProfileButtonPressed() {
        UpdateProfile();
    }
    private void HandleOnUpdateInfo() {
        UpdateProfile();
    }
    private void HandleLogoutButtonPressed()
    {
        Debug.Log("Log out");
    }

    private void HandleDeleteButtonPressed()
    {
        Debug.Log("Delete");
    }

    private void HandleOnSignUpOnSignInSuccess(bool obj)
    {
        UpdateProfile();
    }

    private void UpdateProfile()
    {
        string nickname = cognitoSDKController.GetUserNickname();
        string email = cognitoSDKController.GetUserEmail();
        string phone = cognitoSDKController.GetUserPhone();

        bool isNicknameEmpty = string.IsNullOrEmpty(nickname) || nickname == "-";
        bool isEmailEmpty = string.IsNullOrEmpty(email) || email == "-";

        // Handle Nickname UI
        nickNameText.text = isNicknameEmpty ? "Guest" : nickname;
        nickNameProfileInput.gameObject.SetActive(isNicknameEmpty);
        nickNameProfileText.gameObject.SetActive(!isNicknameEmpty);
        if (!isNicknameEmpty) nickNameProfileText.text = nickname;

        // Handle Email UI
        emailProfileInput.gameObject.SetActive(isEmailEmpty);
        emailProfileText.gameObject.SetActive(!isEmailEmpty);
        if (!isEmailEmpty) emailProfileText.text = email;

        // Submit Button active jika ada data yang kosong
        submitButton.gameObject.SetActive(isNicknameEmpty || isEmailEmpty);

        // Phone selalu ditampilkan
        phoneProfileText.text = phone;
    }


}
