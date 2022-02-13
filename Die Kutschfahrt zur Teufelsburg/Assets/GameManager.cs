using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public RoomPlayer localPlayer;
    public GamePlayer localGamePlayer;
    private Dictionary<uint, RoomPlayer> allPlayers;
    public Dictionary<uint, GamePlayer> allGamePlayers;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddPlayer(RoomPlayer player)
    {
        if (allPlayers == null)
        {
            allPlayers = new Dictionary<uint, RoomPlayer>();
        }
        if (!allPlayers.ContainsKey(player.id))
        {
            allPlayers.Add(player.id, player);
        }
        else
        {
            allPlayers[player.id] = player;
        }

    }

    public void AddGamePlayer(GamePlayer gamePlayer)
    {
        if (allGamePlayers == null)
        {
            allGamePlayers = new Dictionary<uint, GamePlayer>();
        }
        if (!allGamePlayers.ContainsKey(gamePlayer.id))
        {
            allGamePlayers.Add(gamePlayer.id, gamePlayer);
        }
        else
        {
            allGamePlayers[gamePlayer.id] = gamePlayer;
        }

    }

    public bool RemovePlayer(uint playerId)
    {
        if (allPlayers == null)
        {
            allPlayers = new Dictionary<uint, RoomPlayer>();
        }

        if (!allPlayers.ContainsKey(playerId))
        {
            return false;
        }
        return allPlayers.Remove(playerId);
    }

    public bool RemoveGamePlayer(uint gamePlayerId)
    {
        if (allGamePlayers == null)
        {
            allGamePlayers = new Dictionary<uint, GamePlayer>();
        }

        if (!allGamePlayers.ContainsKey(gamePlayerId))
        {
            return false;
        }
        return allGamePlayers.Remove(gamePlayerId);
    }

    public RoomPlayer GetPlayer(uint playerId)
    {
        if (allPlayers == null)
        {
            allPlayers = new Dictionary<uint, RoomPlayer>();
        }

        if (!allPlayers.ContainsKey(playerId))
        {
            return null;
        }
        return allPlayers[playerId];
    }

    public GamePlayer GetGamePlayer(uint gamePlayerId)
    {
        if (allGamePlayers == null)
        {
            allGamePlayers = new Dictionary<uint, GamePlayer>();
        }

        if (!allGamePlayers.ContainsKey(gamePlayerId))
        {
            return null;
        }
        return allGamePlayers[gamePlayerId];
    }

    public List<RoomPlayer> GetAllPlayer()
    {
        return allPlayers != null ? allPlayers.Values.ToList() : new List<RoomPlayer>();
    }

    public List<GamePlayer> GetAllGamePlayer()
    {
        return allGamePlayers != null ? allGamePlayers.Values.ToList() : new List<GamePlayer>();
    }
}
