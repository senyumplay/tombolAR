using UnityEngine;

public static class IntroPrefs
{
    private const string IntroKey = "HasSeenIntro";

    public static bool HasSeenIntro()
    {
        return PlayerPrefs.GetInt(IntroKey, 0) == 1;
    }

    public static void SetIntroCompleted()
    {
        PlayerPrefs.SetInt(IntroKey, 1);
        PlayerPrefs.Save();
    }

    public static void SetIntroUncomplete() {
        PlayerPrefs.DeleteKey(IntroKey);
    }
}
