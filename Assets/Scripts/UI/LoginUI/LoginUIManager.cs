using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUIManager : MonoBehaviour
{
    [SerializeField] private Button google_Button;
    [SerializeField] private Button apple_Button;

    [SerializeField] private PhoneNumManager phoneNumManager;
    private void OnEnable()
    {
        phoneNumManager.OnPhoneNumberEligible += ValidatePhoneNumber;
    }

    private void OnDisable()
    {
        phoneNumManager.OnPhoneNumberEligible -= ValidatePhoneNumber;
    }
    private void Start()
    {
        
        CheckPlatform();
    }

    private void CheckPlatform()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            google_Button.gameObject.SetActive(true);
            apple_Button.gameObject.SetActive(false);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            google_Button.gameObject.SetActive(false);
            apple_Button.gameObject.SetActive(true);
        }
        else {
            google_Button.gameObject.SetActive(true);
            apple_Button.gameObject.SetActive(true);
        }
    }

    private void ValidatePhoneNumber(bool isValid) {
        if (isValid)
        {
            google_Button.interactable = true;
            apple_Button.interactable = true;
        }
        else {
            google_Button.interactable = false;
            apple_Button.interactable = false;
        }

    }
}
