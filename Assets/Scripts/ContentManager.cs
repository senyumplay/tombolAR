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

    [SerializeField] private BoolGameEventSO onCanCopyContent;
    [SerializeField] private IntGameEventSO generatedRandomId;

    public static event Action<int, string, string, string, string> onLoadContentCompleted;

    private List<int> remainingIndices;

    private void OnEnable()
    {
        generatedRandomId.Register(HandleGeneratedRandomId);

    }
    private void OnDisable()
    {
        generatedRandomId.Unregister(HandleGeneratedRandomId);
    }
    private void Update()
    {
        
    }
    private void HandleGeneratedRandomId(int index) {
        LoadLocalContent(index);
    }
    private void LoadLocalContent(int index)
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
