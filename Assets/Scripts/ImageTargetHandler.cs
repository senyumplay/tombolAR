using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTargetContentHandler : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private GameEventSO onImageTargetDetected;
    [SerializeField] private IntGameEventSO generatedRandomId;

    [Header("AR Components")]
    [SerializeField] private ARSession arSession;
    [SerializeField] private Camera arCamera;

    [Header("Target Content Map")]
    [Tooltip("Mapping nama image ke rentang ID")]
    [SerializeField] private List<TargetContentRange> targetContents = new List<TargetContentRange>();

    private int randomId;
    private ARTrackedImageManager trackedImageManager;
    private bool hasDetected = false;

    private Dictionary<string, Vector2Int> contentMap = new Dictionary<string, Vector2Int>();

    [System.Serializable]
    public class TargetContentRange
    {
        public string imageName;
        public int minId = 1;
        public int maxId = 5;
    }

    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();

        // Simpan rentang ID berdasarkan nama image
        foreach (var item in targetContents)
        {
            if (!contentMap.ContainsKey(item.imageName))
            {
                contentMap.Add(item.imageName, new Vector2Int(item.minId, item.maxId));
                Debug.Log($"Mapped {item.imageName} to ID range {item.minId}–{item.maxId}");
            }
        }
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        if (hasDetected) return;

        foreach (var trackedImage in args.added)
        {
            HandleImageDetected(trackedImage);
            break;
        }

        foreach (var trackedImage in args.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                HandleImageDetected(trackedImage);
                break;
            }
        }
    }

    private void HandleImageDetected(ARTrackedImage trackedImage)
    {
        if (hasDetected) return;

        string imageName = trackedImage.referenceImage.name;
        Debug.Log("Detected Image: " + imageName);

        hasDetected = true;

        // Vibrate
        Handheld.Vibrate();

        // Generate random ID dari rentang yang ditentukan
        if (contentMap.ContainsKey(imageName))
        {
            Vector2Int range = contentMap[imageName];
            randomId = Random.Range(range.x, range.y + 1);
            Debug.Log($"{imageName} terdeteksi, generate ID: {randomId}");

            generatedRandomId.Raise(randomId);
            // Simpan ke PlayerPrefs / kirim ke sistem lain jika perlu
            // PlayerPrefs.SetInt("GeneratedID", randomId);
        }
        else
        {
            Debug.LogWarning("No ID range mapped for image: " + imageName);
        }
        
        // Invoke event
        onImageTargetDetected.Raise();

        // Disable AR camera
        //DisableARCamera();
    }

    public void DisableARCamera()
    {
        /*if (arCamera != null)
            arCamera.gameObject.SetActive(false);*/

        if (arSession != null)
            arSession.enabled = false;

        Debug.Log("AR Session disabled");
    }

    public void EnableARCamera()
    {
        /*if (arCamera != null)
            arCamera.gameObject.SetActive(true);*/

        if (arSession != null)
            arSession.enabled = true;

        hasDetected = false;

        Debug.Log("AR Camera re-enabled");
    }
}
