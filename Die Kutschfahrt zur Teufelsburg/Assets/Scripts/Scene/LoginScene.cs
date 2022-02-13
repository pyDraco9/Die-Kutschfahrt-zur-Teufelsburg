using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour
{
    NetworkManager manager;
    [SerializeField] InputField text_Nickname;
    [SerializeField] InputField text_IP;
    [SerializeField] Text text_versionInfomation;
    [SerializeField] Text contentButtonText;
    [SerializeField] Button contentButton;
    // Start is called before the first frame update
    void Start()
    {
        text_Nickname.text = PlayerPrefs.GetString("playerName");
        text_IP.text = PlayerPrefs.GetString("IP");
        text_versionInfomation.text = "魔城马车 Ver." + Application.version.ToString();
    }

    void Awake()
    {
        manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    private void Update()
    {
        if (!NetworkClient.active)
        {
            contentButton.interactable = true;
            contentButtonText.text = "连接";
        }
        else
        {
            contentButton.interactable = false;
            contentButtonText.text = "正在连接...";
        }
    }

    public void onButtonStartHost()
    {
        manager.StopHost();
        if (text_Nickname.text.Length == 0)
        {
            text_Nickname.text = "Furry Guy " + new System.Random().Next(1000, 9999).ToString();
        }
        PlayerPrefs.SetString("playerName", text_Nickname.text);
        manager.StartHost();
    }

    public void onButtonConnect()
    {
        if (text_IP.text.Length > 0)
        {
            if (text_Nickname.text.Length == 0)
            {
                text_Nickname.text = "Furry Guy " + new System.Random().Next(1000, 9999).ToString();
            }
            PlayerPrefs.SetString("playerName", text_Nickname.text);
            PlayerPrefs.SetString("IP", text_IP.text);
            manager.StopClient();
            if (text_IP.text == "")
                text_IP.text = "localhost";
            manager.networkAddress = text_IP.text;
            manager.StartClient();
        }
    }
}
