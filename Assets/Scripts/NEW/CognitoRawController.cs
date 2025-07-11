using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class CognitoRawController : MonoBehaviour
{
    [SerializeField] private LoginForm loginForm;
    [Space(5)]
    [Header("Events")]
    [SerializeField] private BoolGameEventSO OnSignUpSuccess;
    [SerializeField] private StringGameEventSO OnStatusMessageReceived;
    [Header("Event Button")]
    [SerializeField] private GameEventSO OnContinueButtonPressed;

    private void OnEnable()
    {
        OnContinueButtonPressed.Register(HandleOnContinueButtonPressed);
    }
    private void OnDisable()
    {
        OnContinueButtonPressed.Unregister(HandleOnContinueButtonPressed);
    }
    private void HandleOnContinueButtonPressed() {
        StartCoroutine(SignUpAsync());
    }
    private IEnumerator SignUpAsync()
    {
        var userPhoneNumber = new CognitoAttribute
        {
            Name = "phone_number",
            Value = loginForm.GetPhoneNumber()
        };
        var userNickname = new CognitoAttribute
        {
            Name = "custom:nickname",
            Value = "-"
        };
        var userEmail = new CognitoAttribute
        {
            Name = "email",
            Value = ""
        };
        var userAttrsList = new List<CognitoAttribute>();

        userAttrsList.Add(userPhoneNumber);
        userAttrsList.Add(userNickname);
        userAttrsList.Add(userEmail);

        var signUpRequest = new CognitoSignUpRequest
        {
            UserAttributes = userAttrsList,
            Username = loginForm.GetPhoneNumber(),
            ClientId = MyUtils.awsClientID,
            Password = loginForm.GetPassword(),

        };
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(signUpRequest));
        using (UnityWebRequest www = UnityWebRequest.Put("https://cognito-idp.ap-southeast-2.amazonaws.com", myData))
        {
            www.method = "POST";
            www.SetRequestHeader("Content-Type", "application/x-amz-json-1.1; charset=UTF-8");
            www.SetRequestHeader("X-Amz-Target", "AWSCognitoIdentityProviderService.SignUp");

            yield return www.SendWebRequest();

            bool success = www.result == UnityWebRequest.Result.Success;
            try
            {
                if (success) {
                    OnStatusMessageReceived.Raise("Sign‑up success");
                    OnSignUpSuccess.Raise(true);

                    //langsung auto sign in dan get user info.
                }
            }
            catch (UsernameExistsException) {
                OnStatusMessageReceived.Raise("Account exists – trying login …");

                //langsung sign in dan get info
            }
            catch (Exception e)
            {
                Debug.LogError($"Sign‑up failed: {e.Message}");
                OnStatusMessageReceived.Raise("Please complete all required fields.");
                OnSignUpSuccess.Raise(false);
            }
        }
    }
}
