using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class CognitoHostedUIController : MonoBehaviour
{
    [Header("Sign In Event")]
    [SerializeField] private GameEventSO onContinueButtonPressed;
    [SerializeField] private GameEventSO onSignInWithGoogleButtonPressed;
    [SerializeField] private StringGameEventSO OnStatusMessageReceived;
    [SerializeField] private BoolGameEventSO InSigningSuccess;

    [SerializeField] private CognitoSDKController cognitoSDKController;
    [SerializeField] private TMP_Text statusText;
    public static CognitoHostedUIController Instance { get; private set; }
    [Header("Token's")]
    [SerializeField] private string refreshToken;
    [SerializeField] private string idToken; 
    [SerializeField] private string accessToken;

    /*[Header("User Profile")]
    [SerializeField] private String userNickname;
    public string GetUserNickname() => userNickname;
    [SerializeField] private String userEmail;
    public string GetUserEmail() => userEmail;
    [SerializeField] private String userPhone;
    public string GetUserPhone() => userPhone;*/
    
    private void OnEnable()
    {
        //onContinueButtonPressed.Register(HandleOnContinueButtonPressed);
        onSignInWithGoogleButtonPressed.Register(HandleSignInWithGooglePressed);
    }
    private void OnDisable()
    {
        //onContinueButtonPressed.Unregister(HandleOnContinueButtonPressed);
        onSignInWithGoogleButtonPressed.Unregister(HandleSignInWithGooglePressed);
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Application.deepLinkActivated += onDeepLinkActivated;
            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                // Cold start and Application.absoluteURL not null so process Deep Link.
                onDeepLinkActivated(Application.absoluteURL);
            }
            // Initialize DeepLink Manager global variable.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void onDeepLinkActivated(string url)
    {
        string[] splittedURL = url.Split('#');

        if (splittedURL.Length == 2) {
            string urlParams = splittedURL[1];

            if (!string.IsNullOrEmpty(urlParams)) {

                accessToken = HttpUtility.ParseQueryString(urlParams).Get("access_token");

                if (!string.IsNullOrEmpty(accessToken))
                {
                    StartCoroutine(GetUserInfo());
                    
                }
            }
        }
        
    }
    
    IEnumerator GetUserInfo()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get($"{MyUtils.hostedUIDomain}/oauth2/userInfo"))
        {

            webRequest.SetRequestHeader("Content-Type", "application/x-amz-json-1.1; charset=UTF-8");
            webRequest.SetRequestHeader("Authorization", $"Bearer {accessToken}");

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success) {
                CognitoHostedUIUser user = JsonUtility.FromJson<CognitoHostedUIUser>(webRequest.downloadHandler.text);
                
                //userNickname = user.nickname;
                //userEmail = user.email;
                //userPhone = user.phone_number;
                cognitoSDKController.userNickname = user.nickname;
                cognitoSDKController.userEmail = user.email;
                cognitoSDKController.userPhone = user.phone_number;


                SaveManager.SaveUserData(accessToken, cognitoSDKController.userNickname, cognitoSDKController.userEmail, cognitoSDKController.userPhone);
                statusText.text = $"Hi, {cognitoSDKController.userEmail}, {cognitoSDKController.userNickname}, {cognitoSDKController.userPhone}";

                InSigningSuccess.Raise(true);
            }
        }
    }
    public void HandleOnContinueButtonPressed() {
        string callbackURL;
#if UNITY_ANDROID
        callbackURL = "tombol://login";
#elif UNITY_WEBGL
        callbackURL = Application.absoluteURL;
#endif
        Application.OpenURL($"{MyUtils.hostedUIDomain}/oauth2/authorize?client_id={MyUtils.awsClientID}&response_type=token&scope=aws.cognito.signin.user.admin+openid&redirect_uri={callbackURL}");
    }
    public void HandleSignInWithGooglePressed() {
        string callbackURL;
#if UNITY_ANDROID
        callbackURL = "tombol://login";
#elif UNITY_WEBGL
        callbackURL = Application.absoluteURL;
#endif
        Application.OpenURL($"{MyUtils.hostedUIDomain}/oauth2/authorize?client_id={MyUtils.awsClientID}&response_type=token&scope=aws.cognito.signin.user.admin+openid&redirect_uri={callbackURL}");
        //Application.OpenURL($"{MyUtils.hostedUIDomain}/oauth2/authorize?identity_provider=Google&client_id={MyUtils.awsClientID}&response_type=token&scope=aws.cognito.signin.user.admin+openid&redirect_uri={callbackURL}");
    }
    
}
