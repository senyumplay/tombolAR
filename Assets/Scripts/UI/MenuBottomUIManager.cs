using UnityEngine;
using UnityEngine.UI;

public class MenuBottomUIManager : MonoBehaviour
{
    [SerializeField] private Button home_Button;
    [SerializeField] private Button recent_Button;
    [SerializeField] private Button closeContent_Button;
    [SerializeField] private Button playSound_Button;
    [SerializeField] private Button pauseSound_Button;
    [SerializeField] private Button setting_Button;

    private void OnEnable()
    {
        ContentManager.OnContentShow += HandleOnContentShow;
        ContentManager.OnContentClose += HandleOnContentClose;
        ContentManager.OnSoundPlay += HandleOnSoundPlay;

        //closeContent_Button.onClick.AddListener(() => FindObjectOfType<ContentManager>().CloseContentPanel());
    }

    private void OnDisable()
    {
        ContentManager.OnContentShow -= HandleOnContentShow;
        ContentManager.OnContentClose -= HandleOnContentClose;
        ContentManager.OnSoundPlay -= HandleOnSoundPlay;
    }

    private void HandleOnContentShow()
    {
        ShowCloseContentButton();
    }

    private void HandleOnContentClose()
    {
        ShowRecentContentButton();
        ShowPlaySoundButton(); // Reset tombol ke default
    }

    private void HandleOnSoundPlay(AudioClip clip)
    {
        ShowPauseSoundButton();
    }

    public void ShowPlaySoundButton()
    {
        playSound_Button.gameObject.SetActive(true);
        pauseSound_Button.gameObject.SetActive(false);
    }

    public void ShowPauseSoundButton()
    {
        playSound_Button.gameObject.SetActive(false);
        pauseSound_Button.gameObject.SetActive(true);
    }

    public void ShowRecentContentButton()
    {
        recent_Button.gameObject.SetActive(true);
        closeContent_Button.gameObject.SetActive(false);
    }

    public void ShowCloseContentButton()
    {
        recent_Button.gameObject.SetActive(false);
        closeContent_Button.gameObject.SetActive(true);
    }
}
