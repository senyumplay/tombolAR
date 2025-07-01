using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CountryCode
{
    public string countryName;
    public string countryCode;

    public CountryCode(string name, string code)
    {
        countryName = name;
        countryCode = code;
    }
}

[CreateAssetMenu(fileName = "SoutheastAsiaCountryCodes", menuName = "Data/CountryCodes/SoutheastAsia")]
public class SoutheastAsiaCountryCodes : ScriptableObject
{
    public List<CountryCode> countryCodes = new List<CountryCode>()
{
    new CountryCode("Afghanistan", "+93"),
    new CountryCode("Argentina", "+54"),
    new CountryCode("Australia", "+61"),
    new CountryCode("Austria", "+43"),
    new CountryCode("Bangladesh", "+880"),
    new CountryCode("Belgium", "+32"),
    new CountryCode("Brazil", "+55"),
    new CountryCode("Brunei", "+673"),
    new CountryCode("Cambodia", "+855"),
    new CountryCode("Canada", "+1"),
    new CountryCode("China", "+86"),
    new CountryCode("Czech Republic", "+420"),
    new CountryCode("Denmark", "+45"),
    new CountryCode("Egypt", "+20"),
    new CountryCode("Finland", "+358"),
    new CountryCode("France", "+33"),
    new CountryCode("Germany", "+49"),
    new CountryCode("Greece", "+30"),
    new CountryCode("India", "+91"),
    new CountryCode("Indonesia", "+62"),
    new CountryCode("Iran", "+98"),
    new CountryCode("Iraq", "+964"),
    new CountryCode("Ireland", "+353"),
    new CountryCode("Israel", "+972"),
    new CountryCode("Italy", "+39"),
    new CountryCode("Japan", "+81"),
    new CountryCode("Jordan", "+962"),
    new CountryCode("Kazakhstan", "+7"),
    new CountryCode("Kuwait", "+965"),
    new CountryCode("Laos", "+856"),
    new CountryCode("Lebanon", "+961"),
    new CountryCode("Malaysia", "+60"),
    new CountryCode("Mexico", "+52"),
    new CountryCode("Myanmar (Burma)", "+95"),
    new CountryCode("Nepal", "+977"),
    new CountryCode("Netherlands", "+31"),
    new CountryCode("New Zealand", "+64"),
    new CountryCode("North Korea", "+850"),
    new CountryCode("Norway", "+47"),
    new CountryCode("Oman", "+968"),
    new CountryCode("Pakistan", "+92"),
    new CountryCode("Palestine", "+970"),
    new CountryCode("Philippines", "+63"),
    new CountryCode("Poland", "+48"),
    new CountryCode("Portugal", "+351"),
    new CountryCode("Qatar", "+974"),
    new CountryCode("Russia", "+7"),
    new CountryCode("Saudi Arabia", "+966"),
    new CountryCode("Singapore", "+65"),
    new CountryCode("South Korea", "+82"),
    new CountryCode("Spain", "+34"),
    new CountryCode("Sri Lanka", "+94"),
    new CountryCode("Sweden", "+46"),
    new CountryCode("Switzerland", "+41"),
    new CountryCode("Syria", "+963"),
    new CountryCode("Taiwan", "+886"),
    new CountryCode("Thailand", "+66"),
    new CountryCode("Timor-Leste", "+670"),
    new CountryCode("Turkey", "+90"),
    new CountryCode("Ukraine", "+380"),
    new CountryCode("United Arab Emirates", "+971"),
    new CountryCode("United Kingdom", "+44"),
    new CountryCode("United States", "+1"),
    new CountryCode("Uruguay", "+598"),
    new CountryCode("Uzbekistan", "+998"),
    new CountryCode("Vietnam", "+84"),
    new CountryCode("Yemen", "+967")
};

}
