using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButtonUI : MonoBehaviour
{
    [SerializeField] private GameEventSO onSettingPressed;

    private Button settingButton;

    private void Awake()
    {
        settingButton = GetComponent<Button>();

        if (settingButton != null)
            settingButton.onClick.AddListener(HandleSettingClicked);
    }

    private void HandleSettingClicked()
    {
        onSettingPressed?.Raise();
    }

}
