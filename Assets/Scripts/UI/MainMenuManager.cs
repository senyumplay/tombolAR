using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private UniWebView uniWebView;

    [Header("UI Button")]
    [SerializeField] private Button scanButton;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button closeShopButton;
    [SerializeField] private Button contactButton;

    [Header("UI Panel")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject shopPanel;


    [Header("WhatsApp Setting")]
    [SerializeField] private string phoneNumber = "+6287825035164";
    [SerializeField][TextArea] private string message = "Assalamu'alaikum warahmatullahi wabarakatuh";
    private void Start()
    {
        scanButton.onClick.AddListener(LoadToARScene);
        loginButton.onClick.AddListener(ShowLoginPanel);
        shopButton.onClick.AddListener(OpenWebView);
        closeShopButton.onClick.AddListener(CloseWebView);
        contactButton.onClick.AddListener(SendWhatsApp);
    }
    

    private void SendWhatsApp()
    {
        // Hapus '+' jika ada dan encode pesan
        string cleanPhoneNumber = phoneNumber.Replace("0", "+").Trim();
        string encodedMessage = UnityEngine.Networking.UnityWebRequest.EscapeURL(message);

        // Format URL
        string url = $"https://wa.me/{cleanPhoneNumber}?text={encodedMessage}";

        // Buka WhatsApp
        Application.OpenURL(url);
    }
    private void ShowLoginPanel()
    { 
        mainMenuPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    private void LoadToARScene() {
        SceneManager.LoadScene("ARSimulator");
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
