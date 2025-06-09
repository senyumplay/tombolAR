using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyButtonUI : MonoBehaviour
{
    [SerializeField] private GameEventSO onCopyButtonPressed;

    private Button copyButton;

    private void Awake()
    {
        copyButton = GetComponent<Button>();

        if (copyButton != null)
            copyButton.onClick.AddListener(HandlePlayClicked);
    }

    private void HandlePlayClicked()
    {
        onCopyButtonPressed?.Raise();
    }
}
