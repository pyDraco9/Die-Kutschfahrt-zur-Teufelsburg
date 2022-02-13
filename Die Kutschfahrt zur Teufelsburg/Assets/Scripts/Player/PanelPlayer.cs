using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PanelPlayer : NetworkBehaviour
{
    public uint id;
    [SerializeField] Text text_PlayerName;
    [SerializeField] Text text_State;
    void Update()
    {
        RoomPlayer player = GameManager.Instance.GetPlayer(id);
        text_PlayerName.text = player.playerName;
        if (player.GetComponent<NetworkRoomPlayer>().readyToBegin)
        {
            text_State.text = "准备";
            text_State.color = new Color(39f / 255f, 83f / 255f, 150f / 255f);
        }
        else
        {
            text_State.text = "未准备";
            text_State.color = Color.gray;
        }
    }
}
