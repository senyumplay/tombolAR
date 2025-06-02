using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] private UniWebView uniWebView;

    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        shopButton.onClick.AddListener(OpenWebView);
        closeButton.onClick.AddListener(CloseWebView);
    }

    private void CloseWebView()
    {
        shopPanel.SetActive(false);
        uniWebView.Hide();
    }

    private void OpenWebView()
    {
        shopPanel.SetActive(true);
        uniWebView.Show();
    }
}
