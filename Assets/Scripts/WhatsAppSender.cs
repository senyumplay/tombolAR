using UnityEngine;
using UnityEngine.UI;

public class WhatsAppSender : MonoBehaviour
{
    [SerializeField] private string phoneNumber = "6281234567890";
    [SerializeField][TextArea] private string message = "Halo! Saya tertarik dengan produk Anda.";

    public void SendMessageToWhatsApp()
    {
        // Hapus '+' jika ada dan encode pesan
        string cleanPhoneNumber = phoneNumber.Replace("+", "").Trim();
        string encodedMessage = UnityEngine.Networking.UnityWebRequest.EscapeURL(message);

        // Format URL
        string url = $"https://wa.me/{cleanPhoneNumber}?text={encodedMessage}";

        // Buka WhatsApp
        Application.OpenURL(url);
    }
}
