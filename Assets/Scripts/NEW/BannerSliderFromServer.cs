using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using DG.Tweening;

public class BannerSliderFromServer : MonoBehaviour
{
    [System.Serializable]
    public class BannerAsset
    {
        public string fileName;
    }

    [Header("Server Settings")]
    [SerializeField] private string imageBaseUrl = "https://yourdomain.com/banners/";
    [SerializeField] private string cacheFolderName = "banners";

    [Header("Banner Assets")]
    [SerializeField] private List<BannerAsset> bannerAssets;

    [Header("UI Settings")]
    [SerializeField] private GameObject imagePrefab;
    [SerializeField] private RectTransform bannerContainer;
    [SerializeField] private float slideInterval = 3f;
    [SerializeField] private float slideDuration = 0.5f;

    private List<RectTransform> bannerRects = new();
    private int currentIndex = 0;
    private string CachePath => Path.Combine(Application.persistentDataPath, cacheFolderName);
    private float bannerWidth;

    private void Start()
    {
        if (!Directory.Exists(CachePath))
            Directory.CreateDirectory(CachePath);

        StartCoroutine(LoadAllBanners());
    }

    private IEnumerator LoadAllBanners()
    {
        foreach (var banner in bannerAssets)
        {
            string file = banner.fileName;
            string localPath = Path.Combine(CachePath, file);
            Texture2D texture = null;

            // Load from cache
            if (File.Exists(localPath))
            {
                texture = new Texture2D(2, 2);
                if (!texture.LoadImage(File.ReadAllBytes(localPath)))
                {
                    File.Delete(localPath);
                    texture = null;
                }
            }

            // Download if needed
            if (texture == null)
            {
                using var request = UnityWebRequestTexture.GetTexture(imageBaseUrl + file);
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    texture = DownloadHandlerTexture.GetContent(request);
                    File.WriteAllBytes(localPath, request.downloadHandler.data);
                }
                else
                {
                    Debug.LogError("Download failed: " + request.error);
                    continue;
                }
            }

            // Create UI
            if (texture != null)
            {
                var go = Instantiate(imagePrefab, bannerContainer);
                var img = go.GetComponent<Image>();
                img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

                RectTransform rt = go.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(bannerRects.Count * bannerContainer.rect.width, 0);
                rt.sizeDelta = bannerContainer.rect.size;
                bannerRects.Add(rt);
            }
        }

        if (bannerRects.Count > 0)
        {
            bannerWidth = bannerContainer.rect.width;
            InvokeRepeating(nameof(PlayNextBanner), slideInterval, slideInterval);
        }
    }

    private void PlayNextBanner()
    {
        currentIndex = (currentIndex + 1) % bannerRects.Count;
        float targetX = -currentIndex * bannerWidth;

        bannerContainer.DOAnchorPosX(targetX, slideDuration).SetEase(Ease.InOutSine);
    }
}
