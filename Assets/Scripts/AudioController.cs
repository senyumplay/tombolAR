using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void OnEnable()
    {
        ContentManager.OnSoundPlay += AutoPlaySoundContent;
        ContentManager.OnContentClose += StopAudio;
    }

    private void OnDisable()
    {
        ContentManager.OnSoundPlay -= AutoPlaySoundContent;
        ContentManager.OnContentClose -= StopAudio;
    }

    private void AutoPlaySoundContent(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void StopAudio()
    {
        audioSource.Stop();
    }

    public void PlaySound()
    {
        audioSource.Play();
    }

    public void PauseSound()
    {
        audioSource.Pause();
    }
}
