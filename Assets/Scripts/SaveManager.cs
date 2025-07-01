using UnityEngine;

/// <summary>
/// Helper static class untuk menyimpan & mengambil data user di PlayerPrefs.
/// Semua perubahan langsung dipanggil PlayerPrefs.Save() agar persist‑storage segera ditulis.
/// </summary>
public static class SaveManager
{
    /*─────── KEY CONSTANTS ───────*/
    private const string FirstLaunchKey = "FirstLaunch";
    private const string LoginSessionKey = "LoginSession";   
    private const string RefreshTokenKey = "RefreshToken";
    private const string UserNameKey = "UserName";
    private const string UserEmailKey = "UserEmail";
    private const string UserPhoneNumberKey = "UserPhoneNumber";

    private const string TextSizeKey = "TextSize";
    private const string TextColorKey = "TextColor";

    /*─────── FIRST‑LAUNCH FLAG ───────*/
    public static bool IsFirstLaunch() => PlayerPrefs.GetInt(FirstLaunchKey, 1) == 1;
    public static void SetFirstLaunchCompleted() => SaveInt(FirstLaunchKey, 0);

    /*─────── LOGIN SESSION DATA ───────*/
    public static void SaveUserData(string sessionKey, string userName, string userEmail, string phoneNumber)
    {
        SaveString(LoginSessionKey, sessionKey);
        SaveString(UserNameKey, userName);
        SaveString(UserEmailKey, userEmail);
        SaveString(UserPhoneNumberKey, phoneNumber);
    }

    /* Refresh Token tersimpan terpisah agar dapat diperbarui berkala */
    public static void SaveRefreshToken(string refreshToken) => SaveString(RefreshTokenKey, refreshToken);

    public static string GetAccessToken() => PlayerPrefs.GetString(LoginSessionKey, "");
    public static string GetRefreshToken() => PlayerPrefs.GetString(RefreshTokenKey, "");
    public static string GetUserId() => PlayerPrefs.GetString(LoginSessionKey, "");
    public static string GetUserName() => PlayerPrefs.GetString(UserNameKey, "Unknown");
    public static string GetUserEmail() => PlayerPrefs.GetString(UserEmailKey, "Unknown");
    public static string GetUserPhone() => PlayerPrefs.GetString(UserPhoneNumberKey, "Unknown");

    public static bool IsUserLoggedIn() => PlayerPrefs.HasKey(LoginSessionKey);

    public static void ClearLoginSession()
    {
        PlayerPrefs.DeleteKey(LoginSessionKey);
        PlayerPrefs.DeleteKey(RefreshTokenKey);
        PlayerPrefs.DeleteKey(UserNameKey);
        PlayerPrefs.DeleteKey(UserEmailKey);
        PlayerPrefs.DeleteKey(UserPhoneNumberKey);
        PlayerPrefs.Save();
        Debug.Log("Login session cleared.");
    }

    /*─────── UI SETTINGS (tetap) ───────*/
    public static void SaveTextSize(float size) => SaveFloat(TextSizeKey, size);
    public static float GetTextSize() => PlayerPrefs.GetFloat(TextSizeKey, 14f);

    public static void SaveTextColor(Color c) => SaveString(TextColorKey, ColorUtility.ToHtmlStringRGB(c));
    public static Color GetTextColor()
    {
        var hex = PlayerPrefs.GetString(TextColorKey, "FFFFFF");
        ColorUtility.TryParseHtmlString("#" + hex, out var c);
        return c;
    }

    /*─────── WRAPPERS ───────*/
    private static void SaveInt(string k, int v) { PlayerPrefs.SetInt(k, v); PlayerPrefs.Save(); }
    private static void SaveFloat(string k, float v) { PlayerPrefs.SetFloat(k, v); PlayerPrefs.Save(); }
    private static void SaveString(string k, string v) { PlayerPrefs.SetString(k, v); PlayerPrefs.Save(); }
}
