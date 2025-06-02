using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContactusUIManager : MonoBehaviour
{
    public Button sendWA_Button;
    public WhatsAppSender sender;

    void Start()
    {
        sendWA_Button.onClick.AddListener(sender.SendMessageToWhatsApp);
    }

}
