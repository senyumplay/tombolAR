using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseSettingButtonUI : MonoBehaviour
{
    [SerializeField] private GameEventSO onCloseSettingPanelPressed;

    private Button closeSettingPanelButton;

    private void Awake()
    {
        closeSettingPanelButton = GetComponent<Button>();

        if (closeSettingPanelButton != null)
            closeSettingPanelButton.onClick.AddListener(HandleCloseSettingClicked);
    }

    private void HandleCloseSettingClicked()
    {
        onCloseSettingPanelPressed.Raise();
    }
}
