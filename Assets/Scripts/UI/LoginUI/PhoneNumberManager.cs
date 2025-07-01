using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhoneNumberManager : MonoBehaviour
{
    [SerializeField] private BoolGameEventSO onPhoneNumberValid;

    private string currentCountryCode = "+62"; // default
    [SerializeField] private string fullPhoneNumber = "";

    public void SetCountryCode(string code)
    {
        currentCountryCode = code;
        ValidatePhoneNumber(fullPhoneNumber);
    }

    public void UpdatePhoneNumber(string input)
    {
        fullPhoneNumber = ConvertToInternationalFormat(input);
        ValidatePhoneNumber(fullPhoneNumber);
    }

    private void ValidatePhoneNumber(string number)
    {
        bool isValid = number.Length > 7;
        onPhoneNumberValid.Raise(isValid);
    }

    private string ConvertToInternationalFormat(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";

        if (input.StartsWith("0"))
            return currentCountryCode + input.Substring(1);
        if (input.StartsWith("+"))
            return input;

        return currentCountryCode + input;
    }

    public string GetPhoneNumber() => fullPhoneNumber;
}
