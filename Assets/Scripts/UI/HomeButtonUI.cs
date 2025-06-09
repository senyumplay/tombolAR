using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeButton : MonoBehaviour
{
    [SerializeField] private GameEventSO onPlayPressed;

    private Button playButton;

    private void Awake()
    {
        playButton = GetComponent<Button>();

        if (playButton != null)
            playButton.onClick.AddListener(HandlePlayClicked);
    }

    private void HandlePlayClicked()
    {
        onPlayPressed?.Raise();
    }
}
