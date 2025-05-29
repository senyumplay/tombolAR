using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class CountryCodeData
{
    public string label; 
    public string code;  
}

public class PhoneNumManager : MonoBehaviour
{
    [SerializeField] private RectTransform phoneNumberParentTransform;
    [SerializeField] private TMP_InputField phoneNumber_Input;
    [SerializeField] private TMP_Dropdown codeCountry_Dropdown;
    [SerializeField] private TMP_Text codeCountry_Text;

    [SerializeField] private List<CountryCodeData> countryCodes = new List<CountryCodeData>();

    [SerializeField] private string phoneNumber;

    public event Action<bool> OnPhoneNumberEligible;

    [SerializeField] private float moveYAmount = 100f;
    [SerializeField] private float moveDuration = 0.3f;
    private Vector2 originalPosPrent;
    private void Start()
    {
        
        SetCodeCountry(0); // default ke index 0

        codeCountry_Dropdown.onValueChanged.AddListener(SetCodeCountry);

        phoneNumber_Input.onValueChanged.AddListener(UpdatePhoneNumber);
        phoneNumber_Input.onSelect.AddListener(MovingUp);
        phoneNumber_Input.onDeselect.AddListener(MovingDown);

        if (phoneNumberParentTransform != null)
            originalPosPrent = phoneNumberParentTransform.anchoredPosition;
    }

    private void MovingUp(string _)
    {
        if (phoneNumberParentTransform != null)
        {
            phoneNumberParentTransform.DOAnchorPos(originalPosPrent + new Vector2(0, moveYAmount), moveDuration)
                                      .SetEase(Ease.OutQuad);
        }
    }

    private void MovingDown(string _)
    {
        if (phoneNumberParentTransform != null)
        {
            phoneNumberParentTransform.DOAnchorPos(originalPosPrent, moveDuration)
                                      .SetEase(Ease.OutQuad);
        }
    }

    private void SetCodeCountry(int index)
    {
        if (index >= 0 && index < countryCodes.Count)
        {
            string selectedCode = countryCodes[index].code;
            codeCountry_Text.text = selectedCode;

            // Update ulang nomor telepon dengan kode baru
            UpdatePhoneNumber(phoneNumber_Input.text);
        }
        else
        {
            Debug.LogWarning("Index country code tidak valid.");
        }
    }

    private void UpdatePhoneNumber(string value)
    {
        phoneNumber = ConvertLocalToInternational(value);


        if (phoneNumber.Length > 7)
            OnPhoneNumberEligible?.Invoke(true);
        else
            OnPhoneNumberEligible?.Invoke(false);

    }

    private string ConvertLocalToInternational(string localNumber)
    {
        if (string.IsNullOrEmpty(localNumber)) return "";

        string code = codeCountry_Text.text;

        if (localNumber.StartsWith("0"))
        {
            return code + localNumber.Substring(1);
        }

        if (localNumber.StartsWith("+"))
        {
            return localNumber;
        }

        return code + localNumber;
    }

    public string GetFullPhoneNumber()
    {
        return phoneNumber;
    }
}
