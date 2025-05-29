using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonAction : MonoBehaviour
{
    [Header("Action Settings")]
    public bool isLoadScene = true;
    public string sceneToLoad; // Akan diisi lewat dropdown

    public UnityEvent onClickCustomAction;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(HandleButtonClick);
        }
    }

    private void HandleButtonClick()
    {
        if (isLoadScene && !string.IsNullOrEmpty(sceneToLoad))
        {
            FindObjectOfType<UIManager>()?.LoadScene(sceneToLoad);
        }
        else
        {
            onClickCustomAction?.Invoke();
        }
    }

    /*public void LoginWithGoogle()
    {
        string callbackURL;

#if UNITY_ANDROID
        callbackURL = "tombol://login";
#elif UNITY_WEBGL
        callbackURL = Application.absoluteURL;
#else
        callbackURL = "https://localhost/login"; // fallback
#endif

        Application.OpenURL($"{MyUtils.hostedUIDomain}/oauth2/authorize?identity_provider=Google&client_id={MyUtils.appClientID}&response_type=token&scope=aws.cognito.signin.user.admin+openid+email&redirect_uri={callbackURL}");
    }*/
}
