using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecentButton : MonoBehaviour
{
    [SerializeField] private GameEventSO onRecentPressed;

    private Button recentButton;

    private void Awake()
    {
        recentButton = GetComponent<Button>();

        if (recentButton != null)
            recentButton.onClick.AddListener(HandleRecentClicked);
    }

    private void HandleRecentClicked()
    {
        onRecentPressed?.Raise();
    }
}
