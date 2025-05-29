using UnityEngine;

public static class SaveManager
{
    private const string FirstLaunchKey = "FirstLaunch";
    private const string GoogleLoginSessionKey = "GoogleLoginSession";

    private const string TextSizeKey = "TextSize";
    private const string TextColorKey = "TextColor";

    private const string UserNameKey = "UserName";
    private const string UserEmailKey = "UserEmail";
    private const string UserPhoneNumberKey = "UserPhoneNumber";

    public static bool IsFirstLaunch() => PlayerPrefs.GetInt(FirstLaunchKey, 1) == 1;
    public static void SetFirstLaunchCompleted() => SaveInt(FirstLaunchKey, 0);

    public static void SaveUserPhoneNumber(string phoneNumber) => SaveString(UserPhoneNumberKey, phoneNumber);
    public static string GetUserPhoneNumber() => PlayerPrefs.GetString(UserPhoneNumberKey, "");

    public static void SaveUserData(string userId, string userName, string userEmail)
    {
        SaveString(GoogleLoginSessionKey, userId);
        SaveString(UserNameKey, userName);
        SaveString(UserEmailKey, userEmail);
    }

    public static string GetUserId() => PlayerPrefs.GetString(GoogleLoginSessionKey, "");
    public static string GetUserName() => PlayerPrefs.GetString(UserNameKey, "Unknown");
    public static string GetUserEmail() => PlayerPrefs.GetString(UserEmailKey, "Unknown");

    public static bool IsUserLoggedIn() => PlayerPrefs.HasKey(GoogleLoginSessionKey);

    public static void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Semua data telah dihapus.");
    }

    public static void SaveTextSize(float size) => SaveFloat(TextSizeKey, size);
    public static float GetTextSize() => PlayerPrefs.GetFloat(TextSizeKey, 14f);

    public static void SaveTextColor(Color color) => SaveString(TextColorKey, ColorUtility.ToHtmlStringRGB(color));
    public static Color GetTextColor()
    {
        string colorHex = PlayerPrefs.GetString(TextColorKey, "FFFFFF");
        ColorUtility.TryParseHtmlString("#" + colorHex, out Color color);
        return color;
    }

    private static void SaveInt(string key, int value) { PlayerPrefs.SetInt(key, value); PlayerPrefs.Save(); }
    private static void SaveFloat(string key, float value) { PlayerPrefs.SetFloat(key, value); PlayerPrefs.Save(); }
    private static void SaveString(string key, string value) { PlayerPrefs.SetString(key, value); PlayerPrefs.Save(); }
}
