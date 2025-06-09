using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
/// <summary>
/// Manage Data Content (Non UI)
/// </summary>
public class ContentManager : MonoBehaviour
{
    [SerializeField] private HaditsCollection contentCollection;

    [SerializeField] private GameEventSO onRecentButtonPressed;
    [SerializeField] private BoolGameEventSO onCanCopyContent;

    private List<int> remainingIndices;
    private int index;

    public static event Action<int, string, string, string, string> onLoadContentCompleted;


    private void OnEnable()
    {
        onRecentButtonPressed?.Register(HandleShowContentPanel);

    }
    private void OnDisable()
    {
        onRecentButtonPressed?.Unregister(HandleShowContentPanel);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {

            index = GetNextIndex();
            Debug.Log($"Index terpilih :{index}");

            HandleShowContentPanel();
            LoadLocalContent();

        }
    }
    private void HandleShowContentPanel()
    {
        

    }

    private void LoadLocalContent()
    {
        int id = contentCollection.haditsList[index].id;
        string title  = contentCollection.haditsList[index].judul;
        string arabic_text = contentCollection.haditsList[index].arab;
        string indo_text = contentCollection.haditsList[index].indo;
        string english_text = contentCollection.haditsList[index].inggris;

        onLoadContentCompleted?.Invoke(id, title, arabic_text, indo_text, english_text);
        onCanCopyContent.Raise(true);
    }

    #region Random Index
    private void InitializeIndices()
    {
        remainingIndices = new List<int>();
        for (int i = 0; i < contentCollection.haditsList.Count; i++)
        {
            remainingIndices.Add(i);
        }
    }
    public int GetNextIndex()
    {
        if (remainingIndices == null || remainingIndices.Count == 0)
        {
            Debug.LogWarning("All indices have been used. Reinitializing...");
            InitializeIndices(); 
        }

        int randomIndex = Random.Range(0, remainingIndices.Count);
        int value = remainingIndices[randomIndex];
        remainingIndices.RemoveAt(randomIndex);
        return value;
    }
    public void ResetGenerator()
    {
        InitializeIndices();
    }
    public int RemainingCount => remainingIndices.Count;
    #endregion
}
