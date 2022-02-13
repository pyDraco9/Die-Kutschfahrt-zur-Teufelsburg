using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomScene : MonoBehaviour
{
    [SerializeField] Text textReady;
    [SerializeField] GameObject buttonReady;
    bool lastState;

    void Start()
    {

    }

    void Update()
    {
        if (GameManager.Instance.localPlayer != null)
        {
            if (GameManager.Instance.localPlayer.GetComponent<NetworkRoomPlayer>().readyToBegin != lastState)
            {
                lastState = GameManager.Instance.localPlayer.GetComponent<NetworkRoomPlayer>().readyToBegin;
                onButtonFlush();
            }
        }
    }

    public void onButtonReady()
    {
        if (GameManager.Instance.localPlayer.GetComponent<NetworkRoomPlayer>().readyToBegin)
        {
            GameManager.Instance.localPlayer.GetComponent<NetworkRoomPlayer>().CmdChangeReadyState(false);
        }
        else
        {
            GameManager.Instance.localPlayer.GetComponent<NetworkRoomPlayer>().CmdChangeReadyState(true);
        }
    }

    public void onButtonFlush()
    {
        if (GameManager.Instance.localPlayer.GetComponent<NetworkRoomPlayer>().readyToBegin)
        {
            textReady.text = "取消";
        }
        else
        {
            textReady.text = "准备";
        }
    }

    public void onButtonDisconnect()
    {
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StopClient();
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StopHost();
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StopServer();
    }
}
