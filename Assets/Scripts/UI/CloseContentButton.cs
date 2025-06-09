using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseContentButton : MonoBehaviour
{
    [SerializeField] private GameEventSO onCloseContentPressed;

    private Button closeContentButton;

    private void Awake()
    {
        closeContentButton = GetComponent<Button>();

        if (closeContentButton != null)
            closeContentButton.onClick.AddListener(HandleCloseContentClicked);
    }

    private void HandleCloseContentClicked()
    {
        onCloseContentPressed?.Raise();
    }
}
