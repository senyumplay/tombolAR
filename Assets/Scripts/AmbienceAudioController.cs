using System.Collections;
using UnityEngine;
using DG.Tweening;

public class AmbienceAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;

    [SerializeField] private float minVolume = 0.5f;
    [SerializeField] private float maxVolume = 1f;
    [SerializeField] private float minFadeTime = 1f;
    [SerializeField] private float maxFadeTime = 3f;
    [SerializeField] private float minPauseTime = 2f;
    [SerializeField] private float maxPauseTime = 5f;

    private Tween currentTween;
    private Coroutine playCoroutine;

    private void OnEnable()
    {
        /*UIEvents.OnShowHomePanel += HandleOnPlaySoundEffect;
        UIEvents.OnShowLoginPanel += HandleOnStopSoundEffect;*/
    }

    private void OnDisable()
    {
        /*UIEvents.OnShowHomePanel -= HandleOnPlaySoundEffect;
        UIEvents.OnShowLoginPanel -= HandleOnStopSoundEffect;*/
    }

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        HandleOnPlaySoundEffect();
    }

    private void HandleOnPlaySoundEffect()
    {
        if (audioSource.isPlaying) return;
        playCoroutine = StartCoroutine(PlayAudioAmbient());
    }

    private void HandleOnStopSoundEffect()
    {
        StopAllCoroutines();
        playCoroutine = null;

        currentTween?.Kill(true);

        audioSource.DOFade(0f, 0.5f).OnComplete(() =>
        {
            audioSource.Stop();
        });

        Debug.Log("Audio stopped");
    }

    private IEnumerator PlayAudioAmbient()
    {
        if (audioClips == null || audioClips.Length == 0)
        {
            Debug.LogWarning("Audio clips list is empty!");
            yield break;
        }

        while (gameObject.activeInHierarchy)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.volume = 0f;
                audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];

                float fadeInTime = Random.Range(minFadeTime, maxFadeTime);
                currentTween = audioSource.DOFade(Random.Range(minVolume, maxVolume), fadeInTime);
                audioSource.Play();
            }

            yield return new WaitForSeconds(Random.Range(minPauseTime, maxPauseTime));

            float fadeOutTime = Random.Range(minFadeTime, maxFadeTime);
            currentTween = audioSource.DOFade(0f, fadeOutTime);

            yield return new WaitForSeconds(fadeOutTime);
            audioSource.Stop();

            yield return new WaitForSeconds(Random.Range(minPauseTime, maxPauseTime));
        }
    }
}
