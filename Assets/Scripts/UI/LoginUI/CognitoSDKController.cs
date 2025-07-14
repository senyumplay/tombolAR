using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using Amazon;
using Amazon.Runtime;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;


public class CognitoSDKController : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private GameEventSO OnContinueButtonPressed;
    [SerializeField] private GameEventSO OnSubmitButtonPressed;
    [SerializeField] private GameEventSO OnUpdateProfile;
    [SerializeField] private GameEventSO OnLogoutButtonPressed;
    [SerializeField] private GameEventSO OnDeleteButtonPressed;
    [SerializeField] private BoolGameEventSO OnSignUpSuccess;
    [SerializeField] private BoolGameEventSO OnSignInSuccess;
    [SerializeField] private StringGameEventSO OnStatusMessageReceived;

    [SerializeField] private LoginForm loginForm;
    [SerializeField] private ProfileManager profileManager;

    private AmazonCognitoIdentityProviderClient _cognito;


    private void OnEnable() {
        OnContinueButtonPressed.Register(HandleSignUpButtonPressed);
        OnSubmitButtonPressed.Register(HandleSubmitButtonPressed);
        OnLogoutButtonPressed.Register(HandleLogoutButtonPressed);
        OnDeleteButtonPressed.Register(HandleDeleteButtonPressed);
    }
    private void OnDisable() {
        OnContinueButtonPressed.Unregister(HandleSignUpButtonPressed);
        OnSubmitButtonPressed.Unregister(HandleSubmitButtonPressed);
        OnLogoutButtonPressed.Unregister(HandleLogoutButtonPressed);
        OnDeleteButtonPressed.Unregister(HandleDeleteButtonPressed);
    }

    public String userNickname;
    public String userEmail;
    public String userPhone;

    [SerializeField] private string refreshToken;
    [SerializeField] private string accessToken;
    [SerializeField] private string idToken;

    private async void Start()
    {
        _cognito = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(), RegionEndpoint.APSoutheast2);

        if (SaveManager.IsUserLoggedIn())
        {
            accessToken = SaveManager.GetAccessToken();
            userNickname = SaveManager.GetUserName();
            userEmail = SaveManager.GetUserEmail();
            userPhone = SaveManager.GetUserPhone();

            bool refreshed = await RefreshTokenAsync();
            if (refreshed)
            {
                await GetUserAsync();
                OnSignInSuccess.Raise(true);
            }
            else
            {
                SaveManager.ClearLoginSession();
                OnSignInSuccess.Raise(false);
                Debug.Log("Session expired – please login again.");
            }
        }
    }

    #region Log out
    private async void HandleLogoutButtonPressed()
    {
        bool success = await LogoutAsync();

        if (success)
        {
            OnStatusMessageReceived.Raise("You have been logged out.");
            OnSignInSuccess.Raise(false);           
        }
    }

    private async Task<bool> LogoutAsync()
    {
        // 1) Sign‑out di sisi Cognito (invalidasi accessToken)
        try
        {
            var signOutReq = new GlobalSignOutRequest
            {
                AccessToken = accessToken
            };

            var resp = await _cognito.GlobalSignOutAsync(signOutReq);
            bool ok = resp.HttpStatusCode == HttpStatusCode.OK;

            // 2) Hapus semua sesi lokal jika berhasil (atau paksa bersih jika token sudah kadaluarsa)
            if (ok || resp.HttpStatusCode == HttpStatusCode.Unauthorized)
            {
                SaveManager.ClearLoginSession();
                accessToken = null;
                idToken = null;
                refreshToken = null;
            }

            return ok;
        }
        catch (Exception e)
        {
            Debug.LogError($"Logout failed: {e.Message}");
            OnStatusMessageReceived.Raise("Logout error, please try again.");
            return false;
        }
    }

    #endregion

    #region SignUp
    private async void HandleSignUpButtonPressed()
    {
        try
        {
            bool ok = await SignUpAsync();

            if (ok)
            {
                OnStatusMessageReceived.Raise("Sign‑up success");

                bool signInSUccess = await InitiateAuthAsync();
                if (signInSUccess) {
                    OnSignInSuccess.Raise(true);

                    await GetUserAsync();
                }

                OnSignUpSuccess.Raise(true);
            }
            else {
                OnStatusMessageReceived.Raise("Sign‑up failed");

                OnSignUpSuccess.Raise(false); 
            }
        }
        catch (UsernameExistsException)
        {
            OnStatusMessageReceived.Raise("Account exists – trying login …");

            bool signInSuccess = await InitiateAuthAsync();
            if (signInSuccess)
            {

                OnSignInSuccess.Raise(signInSuccess);
                await GetUserAsync();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Sign‑up failed: {e.Message}");
            OnStatusMessageReceived.Raise("Please complete all required fields.");
            OnSignUpSuccess.Raise(false);
        }
    }
    private async Task<bool> SignUpAsync()
    {
        var userPhoneNumber = new AttributeType
        {
            Name = "phone_number",
            Value = loginForm.GetPhoneNumber()
        };
        var userNickname = new AttributeType
        {
            Name = "nickname",
            Value = loginForm.GetNickname()
        };
        var userEmail = new AttributeType
        {
            Name = "email",
            Value = loginForm?.GetEmail()
        };
        var userAttrsList = new List<AttributeType>();

        userAttrsList.Add(userPhoneNumber);
        userAttrsList.Add(userEmail);
        userAttrsList.Add(userNickname);

        var signUpRequest = new SignUpRequest
        {
            UserAttributes = userAttrsList,
            ClientId = MyUtils.awsClientID,
            Username = loginForm.GetEmail(),
            Password = loginForm.GetPassword(),
            
        };
        var response = await _cognito.SignUpAsync(signUpRequest);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }
    #endregion

    #region Sign In
    private async Task<bool> InitiateAuthAsync()
    {
        var authParameters = new Dictionary<string, string>
    {
        { "USERNAME", loginForm.GetPhoneNumber() },
        { "PASSWORD", loginForm.GetPassword() }
    };

        var authRequest = new InitiateAuthRequest
        {
            ClientId = MyUtils.awsClientID,
            AuthParameters = authParameters,
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH
        };

        try
        {
            var response = await _cognito.InitiateAuthAsync(authRequest);

            // Tangani kemungkinan challenge dari Cognito (opsional)
            if (response.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
            {
                Debug.LogWarning("User requires a new password.");
                OnStatusMessageReceived.Raise("Your account requires password reset. Please contact admin.");
                return false;
            }

            if (response.AuthenticationResult == null)
            {
                Debug.LogError("Login failed: No auth result.");
                OnStatusMessageReceived.Raise("Login failed. Please try again.");
                return false;
            }

            // Simpan token dan sesi
            refreshToken = response.AuthenticationResult.RefreshToken;
            accessToken = response.AuthenticationResult.AccessToken;
            idToken = response.AuthenticationResult.IdToken;

            if (!string.IsNullOrEmpty(refreshToken))
                SaveManager.SaveRefreshToken(refreshToken);

            return response.HttpStatusCode == HttpStatusCode.OK;
        }
        catch (NotAuthorizedException)
        {
            Debug.LogWarning("Incorrect phone number or password.");
            OnStatusMessageReceived.Raise("Incorrect phone number or password.");
            return false;
        }
        catch (UserNotFoundException)
        {
            Debug.LogWarning("Phone number is not registered.");
            OnStatusMessageReceived.Raise("Phone number is not registered.");
            return false;
        }
        catch (Exception e)
        {
            Debug.LogError($"Login failed: {e.Message}");
            OnStatusMessageReceived.Raise("Login failed. Please try again later.");
            return false;
        }
    }

    #endregion

    #region Get user
    public async Task<bool> GetUserAsync()
    {
        var getUserRequest = new GetUserRequest
        {
            AccessToken = accessToken
        };

        var response = await _cognito.GetUserAsync(getUserRequest);

        // Helper lokal untuk aman mengambil atribut
        string GetAttr(string name)
        {
            var attr = response.UserAttributes.Find(a => a.Name == name);
            return string.IsNullOrEmpty(attr?.Value) ? "" : attr.Value;
        }

        userPhone = GetAttr("phone_number");          
        userEmail = GetAttr("email");                 
        userNickname = GetAttr("custom:nickname");

        SaveManager.SaveUserData(accessToken, userNickname, userEmail, userPhone);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }
    #endregion

    #region Token Cycle
    private async Task<bool> RefreshTokenAsync()
    {
        var refreshTokenStored = SaveManager.GetRefreshToken();

        if (string.IsNullOrEmpty(refreshTokenStored))
        {
            Debug.LogWarning("No refresh token found.");
            return false;
        }

        var refreshRequest = new InitiateAuthRequest
        {
            ClientId = MyUtils.awsClientID,
            AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                { "REFRESH_TOKEN", refreshTokenStored }
            }
        };

        try
        {
            var response = await _cognito.InitiateAuthAsync(refreshRequest);

            accessToken = response.AuthenticationResult.AccessToken;
            idToken = response.AuthenticationResult.IdToken;
            refreshToken = refreshTokenStored; // Tetap gunakan refresh token lama

            Debug.Log("Access token refreshed successfully.");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Refresh token failed: {e.Message}");
            return false;
        }
    }
    #endregion

    #region Update Info
    private async void HandleSubmitButtonPressed()
    {
        try
        {
            bool success = await UpdateInfoUserAsync();

            if (success)
            {
                OnUpdateProfile.Raise();        // event sukses update
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"HandleSubmitButtonPressed error: {e.Message}");
            OnStatusMessageReceived.Raise("Unable to update profile right now.");
        }
    }

    private async Task<bool> UpdateInfoUserAsync()
    {
        // Ambil data baru dari manager UI Anda
        string newNickname = profileManager.GetNickname()?.Trim();
        string newEmail = profileManager.GetEmail()?.Trim();

        var updateRequest = new UpdateUserAttributesRequest
        {
            AccessToken = accessToken,
            UserAttributes = new List<AttributeType>
        {
            new AttributeType { Name = "custom:nickname", Value = newNickname },
            new AttributeType { Name = "email",           Value = newEmail    }
        }
        };

        try
        {
            var response = await _cognito.UpdateUserAttributesAsync(updateRequest);
            bool ok = response.HttpStatusCode == HttpStatusCode.OK;

            if (ok)
            {
                // Simpan ke memori & PlayerPrefs
                userNickname = newNickname;
                userEmail = newEmail;
                SaveManager.SaveUserData(accessToken, userNickname, userEmail, userPhone);

                OnStatusMessageReceived.Raise("Profile updated successfully.");
            }
            else
            {
                OnStatusMessageReceived.Raise("Failed to update profile.");
            }

            return ok;
        }
        catch (Exception e)
        {
            Debug.LogError($"UpdateUserAttributes failed: {e.Message}");
            OnStatusMessageReceived.Raise("Error updating user info.");
            return false;
        }
    }
    #endregion

    #region Delete
    private async void HandleDeleteButtonPressed()
    {
        bool success = await DeleteUserAsync();

        if (success)
        {
            OnStatusMessageReceived.Raise("Your account has been deleted");
            OnSignInSuccess.Raise(false);
        }
    }
    private async Task<bool> DeleteUserAsync()
    {
        var deleteUserRequest = new DeleteUserRequest
        {
            AccessToken = accessToken
        };

        var response = await _cognito.DeleteUserAsync(deleteUserRequest);
        SaveManager.ClearLoginSession();
        return response.HttpStatusCode == HttpStatusCode.OK;
    }
    #endregion
}
