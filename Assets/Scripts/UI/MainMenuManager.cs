using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private GameEventSO onScanButtonPressed;
    [SerializeField] private GameEventSO onLoginButtonPressed;
    [SerializeField] private GameEventSO onProfileButtonPressed;
    [SerializeField] private GameEventSO onOpenShopButtonPressed;
    [SerializeField] private GameEventSO onCloseShopButtonPressed;
    [SerializeField] private GameEventSO onContactUsButtonPressed;
    [SerializeField] private BoolGameEventSO OnSignUpSuccess;
    [SerializeField] private BoolGameEventSO OnSignInSuccess;

    [SerializeField] private GameEventSO onBackButtonPressed;

    [SerializeField] private BoolGameEventSO IsSigningSuccess;

    [Header("UI Panel")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject profilePanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject shopPanel;

    [Header("Buttons")]
    [SerializeField] private Button loginButton;
    [SerializeField] private Button profileButton;


    [Header("WhatsApp Setting")]
    [SerializeField] private string phoneNumber = "+6287825035164";
    [SerializeField][TextArea] private string message = "Assalamu'alaikum warahmatullahi wabarakatuh";

    private void OnEnable()
    {
        onScanButtonPressed.Register(HandleScanButtonPressed);
        onLoginButtonPressed.Register(HandleLoginButtonPressed);
        onProfileButtonPressed.Register(HandleProfileButtonPressed);

        onOpenShopButtonPressed.Register(HandleOpenShopButtonPressed);

        onContactUsButtonPressed.Register(HandleContactUsButtonPressed);

        onBackButtonPressed.Register(HandlebackButtonPressed);

        IsSigningSuccess.Register(HandleOnSignUpOrSignInSuccess);

        OnSignUpSuccess.Register(HandleOnSignUpOrSignInSuccess);
        OnSignInSuccess.Register(HandleOnSignUpOrSignInSuccess);
    }
    private void OnDisable()
    {
        onScanButtonPressed.Unregister(HandleScanButtonPressed);
        onLoginButtonPressed.Unregister(HandleLoginButtonPressed);
        onProfileButtonPressed.Unregister(HandleProfileButtonPressed);

        onOpenShopButtonPressed.Unregister(HandleOpenShopButtonPressed);

        onContactUsButtonPressed.Unregister(HandleContactUsButtonPressed);

        onBackButtonPressed.Unregister(HandlebackButtonPressed);

        IsSigningSuccess.Unregister(HandleOnSignUpOrSignInSuccess);

        OnSignUpSuccess.Unregister(HandleOnSignUpOrSignInSuccess);
        OnSignInSuccess.Unregister(HandleOnSignUpOrSignInSuccess);
    }
    private void Start()
    {
        SetDefaultpanel();

        
    }
    private void HandleOnSignUpOrSignInSuccess(bool value)
    {
        SetDefaultpanel();

        if (value)
        {
            loginButton.gameObject.SetActive(false);
            profileButton.gameObject.SetActive(true);
        }
        else {
            loginButton.gameObject.SetActive(true);
            profileButton.gameObject.SetActive(false);
        }
    }
    private void InitiateButton()
    {
        bool alreadyLoggedIn = SaveManager.IsUserLoggedIn();

        if (alreadyLoggedIn)
        {
            loginButton.gameObject.SetActive(false);
            profileButton.gameObject.SetActive(true);
        }
        else
        {
            loginButton.gameObject.SetActive(true);
            profileButton.gameObject.SetActive(false);
        }
    }

    private void SetDefaultpanel() { 
        mainMenuPanel.SetActive(true);
        loginPanel.SetActive(false);
        profilePanel.SetActive(false);
        shopPanel.SetActive(false);
    }
    private void HandleProfileButtonPressed() {
        mainMenuPanel.SetActive(false);
        profilePanel.SetActive(true);
    }
    private void HandleIsSignUpSuccessful(bool isSuccess)
    {
        if (isSuccess)
        {
            mainMenuPanel.SetActive(true);
            loginPanel.SetActive(false);

        }
    }

    private void HandleContactUsButtonPressed()
    {
        // Hapus '+' jika ada dan encode pesan
        string cleanPhoneNumber = phoneNumber.Replace("0", "+").Trim();
        string encodedMessage = UnityEngine.Networking.UnityWebRequest.EscapeURL(message);

        // Format URL
        string url = $"https://wa.me/{cleanPhoneNumber}?text={encodedMessage}";

        // Buka WhatsApp
        Application.OpenURL(url);
    }
    private void HandleLoginButtonPressed()
    { 
        mainMenuPanel.SetActive(false);
        loginPanel.SetActive(true);
    }
    private void HandlebackButtonPressed() {
        SetDefaultpanel();
    }

    private void HandleScanButtonPressed() {
        SceneManager.LoadScene("ARSimulator");
    }
    private void HandleOpenShopButtonPressed()
    {
        string url = "https://amrullahstudio.com";
        Application.OpenURL($"{url}");
    }
}
