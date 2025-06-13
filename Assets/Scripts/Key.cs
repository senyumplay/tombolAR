using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TMP_Text keyText;
    private char key;

    [Header("Settings")]
    [SerializeField] private bool isBackspace;
    public void SetKey(char _key) {
        this.key = _key; 
        keyText.text = _key.ToString(); 

    }
    public Button GetButton() {
        return GetComponent<Button>();
    }
    public bool IsBackspace() { 
        return isBackspace;
    }
}
