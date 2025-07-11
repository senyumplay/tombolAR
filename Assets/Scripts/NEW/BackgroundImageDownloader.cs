using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using DG.Tweening;

public class BackgroundImageLoader : MonoBehaviour
{
    // -------------------------------- mapping gambar ↔ backsound ----------------------------
    [System.Serializable]
    public class ImageAudioPair
    {
        public string fileName;
        public AudioClip[] clips;
    }

    // ---------------------------------------------------------------------------------------
    [Header("Download Settings")]
    [SerializeField] private string imageUrl = "https://senyumplay.com/tombol/img_background/";
    [SerializeField] private string cacheFolder = "background";

    [Header("Assets List (file ↔ audio)")]
    [SerializeField] private List<ImageAudioPair> assets = new();

    [Header("UI")]
    [SerializeField] private GameObject imagePrefab;
    [SerializeField] private Transform parentPanel;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float displayTime = 4f;
    [SerializeField] private float zoomScale = 1.08f;
    [SerializeField] private float zoomDuration = 5f;

    [Header("Audio")]
    [SerializeField] private float audioVolume = 1f;
    [SerializeField] private float pauseBetweenRepeats = 2f;   // NEW — jeda antar‐ulang clip yg sama

    // ---------------------------------------------------------------------------------------
    private readonly List<Image> images = new();
    private int currentIndex;
    private string CachePath => Path.Combine(Application.persistentDataPath, cacheFolder);

    // ---------------------------------------------------------------------------------------

    private void Start()
    {
        if (!Directory.Exists(CachePath))
            Directory.CreateDirectory(CachePath);

        StartCoroutine(LoadOrDownloadImages());
    }

    // ---------------------------------------------------------------------------------------

    private IEnumerator LoadOrDownloadImages()
    {
        foreach (var asset in assets)
        {
            string file = asset.fileName;
            string local = Path.Combine(CachePath, file);
            Texture2D tex = null;

            // -- cache
            if (File.Exists(local))
            {
                tex = new Texture2D(2, 2);
                if (!tex.LoadImage(File.ReadAllBytes(local)))
                {
                    File.Delete(local);
                    tex = null;
                }
            }

            // -- download
            if (tex == null)
            {
                using var req = UnityWebRequestTexture.GetTexture(imageUrl + file);
                yield return req.SendWebRequest();

                if (req.result == UnityWebRequest.Result.Success)
                {
                    tex = DownloadHandlerTexture.GetContent(req);
                    File.WriteAllBytes(local, req.downloadHandler.data);
                }
                else
                {
                    Debug.LogError($"Download failed {file}: {req.error}");
                    continue;
                }
            }

            // -- buat UI
            if (tex != null)
            {
                var go = Instantiate(imagePrefab, parentPanel);
                var img = go.GetComponent<Image>();
                img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * .5f);
                img.color = new Color(1, 1, 1, 0);
                go.transform.localScale = Vector3.one;

                var bgCmp = go.GetComponent<BackgroundImage>();
                bgCmp.SetBacksounds(asset.clips);

                var src = go.GetComponent<AudioSource>();
                src.playOnAwake = false;
                src.loop = false;            // NEW — disable loop
                src.volume = 0;

                images.Add(img);
            }
        }

        if (images.Count == 0)
        {
            Debug.LogWarning("No images loaded.");
            yield break;
        }

        ShowNext();
    }

    // ---------------------------------------------------------------------------------------
    // slideshow + audio cross-fade

    private void ShowNext()
    {
        Image img = images[currentIndex];
        GameObject go = img.gameObject;
        BackgroundImage bg = go.GetComponent<BackgroundImage>();
        AudioSource src = go.GetComponent<AudioSource>();

        // pilih clip
        AudioClip clip = bg.GetRandomClip();
        if (clip != null)
        {
            src.clip = clip;
            src.volume = 0;
            src.Play();
            StartCoroutine(RepeatClipWithGap(src, clip.length, pauseBetweenRepeats, currentIndex));   // NEW
        }

        // reset visual
        img.color = new Color(1, 1, 1, 0);
        img.transform.localScale = Vector3.one;

        // tween
        Sequence seq = DOTween.Sequence();
        seq.Append(img.DOFade(1, fadeDuration).SetEase(Ease.InOutSine));
        if (clip != null)
            seq.Join(DOTween.To(() => src.volume, v => src.volume = v, audioVolume, fadeDuration));

        seq.Join(img.transform.DOScale(zoomScale, zoomDuration).SetEase(Ease.InOutSine));
        seq.AppendInterval(displayTime);

        seq.Append(img.DOFade(0, fadeDuration).SetEase(Ease.InOutSine));
        if (clip != null)
            seq.Join(DOTween.To(() => src.volume, v => src.volume = v, 0f, fadeDuration)
                       .OnComplete(() => src.Stop()));

        seq.OnComplete(() =>
        {
            currentIndex = (currentIndex + 1) % images.Count;
            ShowNext();
        });
    }

    // ---------------------------------------------------------------------------------------
    // NEW — coroutine untuk memberi jeda antar-ulang clip yang sama

    private IEnumerator RepeatClipWithGap(AudioSource src, float clipLen, float gap, int ownerIndex)
    {
        while (currentIndex == ownerIndex)          // masih gambar yang sama?
        {
            yield return new WaitForSeconds(clipLen + gap);
            if (currentIndex != ownerIndex) break;  // sudah ganti slide, stop
            src.time = 0f;
            src.Play();
        }
    }
}
