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
    [SerializeField] private GameEventSO onSettingButtonPressed;

    [SerializeField] private GameEventSO onImageTargetDetected;
    [SerializeField] private BoolGameEventSO onAudioReciterPlay;

    private void OnEnable()
    {
        
        onHomeButtonPressed.Register(HandleHomeButtonPressed);
        onRecentButtonPressed.Register(HandleRecentButtonPressed);
        onCloseContentButtonPressed.Register(HandleCloseContentButtonPressed);
        onSettingButtonPressed.Register(HandleSettingButtonPressed);

        onImageTargetDetected.Register(HandleImageTargetDetected);
        onAudioReciterPlay.Register(HandleAudioReciterPlay);
    }

    private void OnDisable()
    {
        
        onHomeButtonPressed.Unregister(HandleHomeButtonPressed);
        onRecentButtonPressed.Unregister(HandleRecentButtonPressed);
        onCloseContentButtonPressed.Unregister(HandleCloseContentButtonPressed);
        onSettingButtonPressed.Unregister(HandleSettingButtonPressed);

        onImageTargetDetected.Unregister(HandleImageTargetDetected);
        onAudioReciterPlay.Unregister(HandleAudioReciterPlay);
    }
    private void HandleAudioReciterPlay(bool isPlayed) {
        if (isPlayed)
        {
            OnAudioReciterPlay();
        }
        else {
            OnAudioReciterPause();
        }
    }
    private void HandleImageTargetDetected() {
        OnShowContent();
        OnAudioReciterPlay();
    }
    private void HandleHomeButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void HandleRecentButtonPressed()
    {
        OnShowContent();
        OnAudioReciterPlay();
    }
    private void HandleCloseContentButtonPressed()
    {
        OnHideContent();
    }
    private void HandleSettingButtonPressed() { 
        
    }

    //Logic
    private void OnShowContent() {
        recentButton.gameObject.SetActive(false);
        closeContentButton.gameObject.SetActive(true);
    }
    private void OnHideContent() {
        recentButton.gameObject.SetActive(true);
        closeContentButton.gameObject.SetActive(false);
    }
    private void OnAudioReciterPlay() { 
        playSoundButton.gameObject.SetActive(false);
        pauseSoundButton.gameObject.SetActive(true);
    }
    private void OnAudioReciterPause() {
        playSoundButton.gameObject.SetActive(true);
        pauseSoundButton.gameObject.SetActive(false);
    }
}
