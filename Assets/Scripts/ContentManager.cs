using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ContentManager : MonoBehaviour
{
    [Header("Hadith Content Data")]
    public HadithDatabase contentData;

    [Header("UI Components")]
    public GameObject contentPanel;
    public TextMeshProUGUI judulText;
    public Image isiHaditsImage;
    public TextMeshProUGUI arabicText;
    public TextMeshProUGUI translateText;

    [Header("Settings")]
    private bool isTranslateID = true;

    public static Action OnContentShow;
    public static Action<AudioClip> OnSoundPlay;
    public static Action OnContentClose;


    private List<int> shuffledIndexes = new List<int>();
    private int currentIndex = 0;

    private void Start()
    {
        GenerateShuffledIndexes();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowContentPanel();
        }
    }

    public void ShowContentPanel()
    {
        if (contentData == null)
        {
            Debug.LogWarning("Hadith content data is not assigned.");
            return;
        }

        contentPanel.SetActive(true);

        //isiHaditsImage.sprite = contentData.isiHaditsImage;
        //isiHaditsImage.SetNativeSize();

        UpdateTranslation();

        OnContentShow?.Invoke();

        if (contentData.hadithList[currentIndex].audioClips != null)
        {
            OnSoundPlay?.Invoke(contentData.hadithList[currentIndex].audioClips[0]); //play default
        }
    }

    public void CloseContentPanel()
    {
        contentPanel.SetActive(false);
        OnContentClose?.Invoke();
    }

    public void ToggleTranslation()
    {
        isTranslateID = !isTranslateID;
        UpdateTranslation();
    }

    private void UpdateTranslation()
    {
        if (isTranslateID)
        {
            judulText.text = contentData.hadithList[currentIndex].judul;
            arabicText.text = contentData.hadithList[currentIndex].arab;
            translateText.text = contentData.hadithList[currentIndex].indo;
        }
        else
        {
            judulText.text = contentData.hadithList[currentIndex].judul;
            arabicText.text = contentData.hadithList[currentIndex].arab;
            translateText.text = contentData.hadithList[currentIndex].inggris;
        }
    }

    private void GenerateShuffledIndexes()
    {
        shuffledIndexes.Clear();

        for (int i = 0; i < contentData.hadithList.Count; i++)
        {
            shuffledIndexes.Add(i);
        }

        // Fisher-Yates Shuffle
        for (int i = 0; i < shuffledIndexes.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledIndexes.Count);
            int temp = shuffledIndexes[i];
            shuffledIndexes[i] = shuffledIndexes[randomIndex];
            shuffledIndexes[randomIndex] = temp;
        }

        currentIndex = 0;
    }

    public int GetNextIndex()
    {
        if (currentIndex >= shuffledIndexes.Count)
        {
            Debug.Log("Semua index sudah dipakai. Regenerate ulang.");
            GenerateShuffledIndexes(); // bisa direset jika perlu mengulang
        }

        int next = shuffledIndexes[currentIndex];
        currentIndex++;
        return next;
    }
}
