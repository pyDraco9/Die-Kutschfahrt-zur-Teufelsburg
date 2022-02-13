using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayer : NetworkBehaviour
{
    [SyncVar] public uint id;
    [SyncVar] public string playerName = "";
    GameObject panelPlayer;
    [Scene] public string RoomScene;

    [Client]
    void Start()
    {
        this.id = this.netId;
        GameManager.Instance.AddPlayer(this);
        if (hasAuthority)
        {
            GameManager.Instance.localPlayer = this;
        }
        SetPlayerName(PlayerPrefs.GetString("playerName"));
        gameObject.transform.position = new Vector2(-7, -3);
    }

    public override void OnStartClient()
    {
        this.id = this.netId;
        if (NetworkManager.IsSceneActive(RoomScene))
        {
            panelPlayer = Instantiate((GameObject)Resources.Load("Prefabs/PanelPlayer"));
            panelPlayer.transform.SetParent(GameObject.Find("Grid_Player").transform);
            panelPlayer.name = "Player " + id.ToString();
            panelPlayer.transform.localScale = new Vector3(1, 1, 1);
            panelPlayer.GetComponent<PanelPlayer>().id = id;
        }
    }

    public override void OnStopClient()
    {
        Destroy(panelPlayer);
    }

    private void OnDestroy()
    {
        GameManager.Instance.RemovePlayer(id);
    }

    public void SetPlayerName(string playerName)
    {
        CmdChangeName(playerName);
    }


    [Command]
    public void CmdChangeName(string name)
    {
        playerName = name;
    }

}