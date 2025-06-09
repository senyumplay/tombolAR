using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BottomMenuController : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button recentButton;
    [SerializeField] private Button closeContentButton;
    [SerializeField] private Button playSoundButton;
    [SerializeField] private Button pauseSoundButton;

    [Space(5)]
    [Header("Events")]
    [SerializeField] private GameEventSO onHomeButtonPressed;
    [SerializeField] private GameEventSO onRecentButtonPressed;
    [SerializeField] private GameEventSO onCloseContentButtonPressed;

    private void OnEnable()
    {
        
        onHomeButtonPressed?.Register(HandleHomeButtonPressed);
        onRecentButtonPressed?.Register(HandleRecentButtonPressed);
        onCloseContentButtonPressed?.Register(HandleCloseContentButtonPressed);
    }

    private void OnDisable()
    {
        
        onHomeButtonPressed?.Unregister(HandleHomeButtonPressed);
        onRecentButtonPressed?.Unregister(HandleRecentButtonPressed);
        onCloseContentButtonPressed?.Unregister(HandleCloseContentButtonPressed);
    }

    private void HandleHomeButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void HandleRecentButtonPressed()
    {

        recentButton.gameObject.SetActive(false);
        closeContentButton.gameObject.SetActive(true);

        
    }
    private void HandleCloseContentButtonPressed()
    {
        recentButton.gameObject.SetActive(true);
        closeContentButton.gameObject.SetActive(false);

        
    }
}
