using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CognitoAttribute {

    public string Name;
    public string Value;
}
[System.Serializable]
public class CognitoSignUpRequest { 
    public string Username;
    public string Password;
    public string ClientId;
    public List<CognitoAttribute> UserAttributes;
}
[System.Serializable]
public class CognitoHostedUIUser { 
    public string nickname;
    public string email;
    public string phone_number;
}