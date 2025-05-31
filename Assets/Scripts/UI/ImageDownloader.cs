using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ImageDownloader : MonoBehaviour
{
    [SerializeField] private string imageUrl = "https://senyumplay.com/tombol/img_background/";
    [SerializeField] private string cacheFolder = "background";
    [SerializeField] private List<string> imageFileNames = new List<string>(); // Daftar nama file di server

    public System.Action<List<Texture2D>> OnDownloadComplete;
    private List<Texture2D> downloadedTextures = new List<Texture2D>();

    public string CacheFolderPath => Path.Combine(Application.persistentDataPath, cacheFolder);


    private void Start()
    {
        string cachePath = Path.Combine(Application.persistentDataPath, cacheFolder);
        Debug.Log(cachePath);
        if (!Directory.Exists(cachePath))
        {
            Directory.CreateDirectory(cachePath);
        }

        StartCoroutine(DownloadAllImages());
    }

    private IEnumerator DownloadAllImages()
    {
        downloadedTextures.Clear();

        foreach (var fileName in imageFileNames)
        {
            string localPath = Path.Combine(Application.persistentDataPath, cacheFolder, fileName);
            string fullUrl = imageUrl + fileName;

            // Check if file exists and is valid
            if (File.Exists(localPath))
            {
                byte[] fileData = File.ReadAllBytes(localPath);
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(fileData))
                {
                    downloadedTextures.Add(texture);
                    continue;
                }
                else
                {
                    File.Delete(localPath); // Delete corrupt file
                }
            }

            // Download file
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(fullUrl))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(request);
                    File.WriteAllBytes(localPath, request.downloadHandler.data); // Save to cache
                    downloadedTextures.Add(texture);
                }
                else
                {
                    Debug.LogError($"Failed to download {fileName}: {request.error}");
                }
            }
        }

        // Notify when all downloads are complete
        OnDownloadComplete?.Invoke(downloadedTextures);
    }
}
