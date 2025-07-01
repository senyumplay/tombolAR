using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CognitoHostedUIController : MonoBehaviour
{
    [Header("Sign In Event")]
    [SerializeField] private GameEventSO onSignInWithGoogleButtonPressed;
    [SerializeField] private BoolGameEventSO onSignInSuccess;

    private void OnEnable() {
        onSignInWithGoogleButtonPressed.Register(HandleSignInWithGooglePressed);
        onSignInSuccess.Register(HandleOnSignInSuccess);
    }
    private void OnDisable()
    {
        onSignInWithGoogleButtonPressed.Unregister(HandleSignInWithGooglePressed);
        onSignInSuccess.Unregister(HandleOnSignInSuccess);
    }

    private void HandleSignInWithGooglePressed() {
        Application.OpenURL("https://tombol-domain.auth.ap-southeast-2.amazoncognito.com/login?client_id=64dgol95pt7a8dfher4tlluqgq&response_type=token&scope=aws.cognito.signin.user.admin+email+openid+phone&redirect_uri=https%3A%2F%2Fd84l1y8p4kdic.cloudfront.net");
    }
    private void HandleOnSignInSuccess(bool isSuccess) { 
    
    }
}
