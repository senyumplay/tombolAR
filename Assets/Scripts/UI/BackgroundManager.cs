using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;
using UnityEngine.Networking;

public class BackgroundManager : MonoBehaviour
{
    public enum BackgroundSource { Local, Download }

    [Header("Source Settings")]
    [SerializeField] private BackgroundSource source = BackgroundSource.Download;
    [SerializeField] private string[] streamingAssetImageNames; // Nama file di StreamingAssets

    [Header("Download Dependencies")]
    [SerializeField] private ImageDownloader imageDownloader;

    [Header("UI & Animation Settings")]
    [SerializeField] private GameObject imagePrefab; // Prefab UI Image
    [SerializeField] private Transform parentPanel;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float displayDuration = 5f;
    [SerializeField] private float zoomScale = 1.1f;
    [SerializeField] private float zoomDuration = 5f;

    private List<Image> imageInstances = new List<Image>();
    private int currentIndex = 0;

    private void Start()
    {
        if (source == BackgroundSource.Local)
        {
            StartCoroutine(LoadSpritesFromStreamingAssets());
        }
        else if (source == BackgroundSource.Download)
        {
            LoadCachedImages();
            if (imageDownloader != null)
                imageDownloader.OnDownloadComplete += OnImagesDownloaded;
        }
    }

    private IEnumerator LoadSpritesFromStreamingAssets()
    {
        foreach (string fileName in streamingAssetImageNames)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

#if UNITY_ANDROID && !UNITY_EDITOR
            string uri = "jar:file://" + Application.dataPath + "!/assets/" + fileName;
#else
            string uri = "file://" + filePath;
#endif

            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogWarning("Failed to load from StreamingAssets: " + www.error);
                    continue;
                }

                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

                GameObject instance = Instantiate(imagePrefab, parentPanel);
                Image image = instance.GetComponent<Image>();
                image.sprite = sprite;
                image.color = new Color(1, 1, 1, 0);
                image.transform.localScale = Vector3.one;
                imageInstances.Add(image);
            }
        }

        if (imageInstances.Count > 0)
        {
            StartCoroutine(ShowImagesSequentially());
        }
        else
        {
            Debug.LogWarning("No images found in StreamingAssets.");
        }
    }

    private void LoadCachedImages()
    {
        if (imageDownloader == null)
        {
            Debug.LogError("ImageDownloader not assigned.");
            return;
        }

        string cacheFolder = imageDownloader.CacheFolderPath;

        if (!Directory.Exists(cacheFolder))
        {
            Debug.LogWarning("Cache folder not found: " + cacheFolder);
            return;
        }

        foreach (string filePath in Directory.GetFiles(cacheFolder))
        {
            Texture2D texture = LoadTextureFromFile(filePath);
            if (texture != null)
            {
                GameObject instance = Instantiate(imagePrefab, parentPanel);
                Image image = instance.GetComponent<Image>();
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                image.color = new Color(1, 1, 1, 0);
                image.transform.localScale = Vector3.one;
                imageInstances.Add(image);
            }
        }

        if (imageInstances.Count > 0)
        {
            StartCoroutine(ShowImagesSequentially());
        }
        else
        {
            Debug.LogWarning("No cached images found.");
        }
    }

    private Texture2D LoadTextureFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(fileData))
                return texture;
        }
        return null;
    }

    private void OnImagesDownloaded(List<Texture2D> textures)
    {
        foreach (var texture in textures)
        {
            GameObject instance = Instantiate(imagePrefab, parentPanel);
            Image image = instance.GetComponent<Image>();
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            image.color = new Color(1, 1, 1, 0);
            image.transform.localScale = Vector3.one;
            imageInstances.Add(image);
        }

        if (imageInstances.Count > 0)
        {
            StartCoroutine(ShowImagesSequentially());
        }
    }

    private IEnumerator ShowImagesSequentially()
    {
        while (true)
        {
            Image currentImage = imageInstances[currentIndex];
            currentImage.transform.localScale = Vector3.one;

            currentImage.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
            currentImage.transform.DOScale(zoomScale, zoomDuration).SetEase(Ease.InOutSine);

            yield return new WaitForSeconds(displayDuration);

            currentImage.DOFade(0f, fadeDuration).SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(fadeDuration);

            currentIndex = (currentIndex + 1) % imageInstances.Count;
        }
    }
}
