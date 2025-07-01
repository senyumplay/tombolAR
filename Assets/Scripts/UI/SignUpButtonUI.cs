using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignUpButtonUI : MonoBehaviour
{
    [SerializeField] private GameEventSO onButtonPressed;

    private Button thisButton;

    private void Awake()
    {
        thisButton = GetComponent<Button>();

        if (thisButton != null)
            thisButton.onClick.AddListener(HandlePlayClicked);
    }

    private void HandlePlayClicked()
    {
        onButtonPressed?.Raise();
    }

}
