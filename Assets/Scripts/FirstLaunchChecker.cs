using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstLaunchChecker : MonoBehaviour
{
    private void Start()
    {
        if (!SaveManager.IsFirstLaunch())
        {
            SceneManager.LoadScene("MainMenu");
        }
        else {
            SceneManager.LoadScene("Intro");
        }
    }
}
