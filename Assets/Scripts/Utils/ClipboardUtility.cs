using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClipboardUtility : MonoBehaviour
{
    [SerializeField] private GameEventSO onCopyButtonPressed;
    [SerializeField] private BoolGameEventSO onCanCopyText;

    [SerializeField] private Button copyButton;

    [SerializeField] private string title;
    [SerializeField] private string arabic;
    [SerializeField] private string translate;
    [SerializeField] private string footer;
    private void OnEnable()
    {
        onCopyButtonPressed.Register(HandleCopyButtonPressed);
        onCanCopyText.Register(HandleOnCanCopyText);

        ContentManager.onLoadContentCompleted += UpdateCopyText;
    }
    private void OnDisable()
    {
        onCopyButtonPressed.Unregister(HandleCopyButtonPressed);
        onCanCopyText.Unregister(HandleOnCanCopyText);

        ContentManager.onLoadContentCompleted -= UpdateCopyText;
    }

    private void UpdateCopyText(int arg1, string arg2, string arg3, string arg4, string arg5)
    {
        title = arg2;
        arabic = arg3;
        translate = arg4;
        footer = $"selengkapnya kunjungi website kami di https://www.tombol.com";
    }

    private void HandleOnCanCopyText(bool canCopy)
    {
        copyButton.interactable = canCopy?true:false;
    }

    private void HandleCopyButtonPressed()
    {
        CopyFormattedText(title, arabic, translate, footer);
    }

    public static void CopyFormattedText(string title, string arabic, string translate, string footer)
    {
        string formattedText = $"Judul: {title}\n----------------\n{arabic}\n----------------\n{translate}\n----------------\n{footer}";
        GUIUtility.systemCopyBuffer = formattedText;
        Debug.Log("Formatted text copied to clipboard.");
    }
}
