using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagerHelper
{
    /// <summary>
    /// Load scene by name.
    /// </summary>
    public static void LoadScene(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' not found in Build Settings.");
        }
    }

    /// <summary>
    /// Load scene by index.
    /// </summary>
    public static void LoadScene(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError($"Scene index {sceneIndex} is out of range.");
        }
    }

    /// <summary>
    /// Reload the current scene.
    /// </summary>
    public static void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Load next scene (based on Build Settings order).
    /// </summary>
    public static void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogError("Next scene index is out of range.");
        }
    }
}
