using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuProfile : MonoBehaviour
{
    public static Action<Texture2D> onPhotoTaken;

    [Header("UI")]
    [SerializeField] private RawImage previewImage;  // UI untuk menampilkan hasil foto

    private void Start()
    {
        // Subscribing internal method to static event (optional)
        onPhotoTaken += ShowPhotoInUI;
    }

    public void TakePictureButtonCallback()
    {
        NativeCamera.TakePicture((path) =>
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning("No image path returned");
                return;
            }

            Texture2D texture = NativeCamera.LoadImageAtPath(path, 1024, false);
            if (texture == null)
            {
                Debug.LogError("Couldn't load the texture");
                return;
            }

            Texture2D squareTexture = SquareTexture(texture);
            onPhotoTaken?.Invoke(squareTexture);

        }, maxSize: 1024);
    }


    private void StartCameraCapture()
    {
        NativeCamera.TakePicture((path) =>
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning("No image path returned");
                return;
            }

            Texture2D texture = NativeCamera.LoadImageAtPath(path, 1024, false);
            if (texture == null)
            {
                Debug.LogError("Couldn't load the texture from: " + path);
                return;
            }

            Texture2D squareTexture = SquareTexture(texture);
            onPhotoTaken?.Invoke(squareTexture);

        }, maxSize: 1024);
    }

    private Texture2D SquareTexture(Texture2D original)
    {
        int size = Mathf.Min(original.width, original.height);
        int x = (original.width - size) / 2;
        int y = (original.height - size) / 2;

        Color[] pixels = original.GetPixels(x, y, size, size);
        Texture2D square = new Texture2D(size, size);
        square.SetPixels(pixels);
        square.Apply();

        return square;
    }

    private void ShowPhotoInUI(Texture2D photo)
    {
        if (previewImage != null)
        {
            previewImage.texture = photo;
            previewImage.color = Color.white; // make sure it's visible
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        onPhotoTaken -= ShowPhotoInUI;
    }
}
