using System.Collections.Generic;
using System.Diagnostics;

public class DeclareVictoryData
{
    public List<uint> TheBrotherhoodOfTrueLies = new List<uint>();
    public List<uint> TheOrderOFOpenSecrets = new List<uint>();
    public List<uint> SelectPlayers = new List<uint>();
    public bool Victory = true;
    public CardID DeclareAssociation;
    public CardID OtherAssociation;
    public int ObjectCount = 0;

    public DeclareVictoryData()
    {
        
    }

    public void AddPlayerToList(uint _player, CardID _association)
    {
        if (_association == CardID.TheBrotherhoodOfTrueLies)
        {
            TheBrotherhoodOfTrueLies.Add(_player);
        }
        else if (_association == CardID.TheOrderOFOpenSecrets)
        {
            TheOrderOFOpenSecrets.Add(_player);
        }
    }

    public void DeclareVictory(uint _player, int deckObjectCount, Dictionary<uint, PlayerData> playerData)
    {
        CardID _association = playerData[_player].association;
        DeclareAssociation = _association;
        // 初始化胜利时 计算阵营全员 是否有足够的道具
        if (_association == CardID.TheBrotherhoodOfTrueLies)
        {
            OtherAssociation = CardID.TheOrderOFOpenSecrets;
            foreach (uint playerID in TheBrotherhoodOfTrueLies)
            {
                PlayerData gamePlayer = playerData[playerID];
                for (int i = 0; i < gamePlayer.cardList.Count; i++)
                {
                    CardID _thisCard = gamePlayer.cardList[i];
                    if ((deckObjectCount == 0 && _thisCard == CardID.SecretBagGoblet) || _thisCard == CardID.Goblet)
                    {
                        ObjectCount++;
                    }
                }
            }
        }
        else if (_association == CardID.TheOrderOFOpenSecrets)
        {
            OtherAssociation = CardID.TheBrotherhoodOfTrueLies;
            foreach (uint playerID in TheOrderOFOpenSecrets)
            {
                PlayerData gamePlayer = playerData[playerID];
                for (int i = 0; i < gamePlayer.cardList.Count; i++)
                {
                    CardID _thisCard = gamePlayer.cardList[i];
                    if ((deckObjectCount == 0 && _thisCard == CardID.SecretBagKey) || _thisCard == CardID.Key)
                    {
                        ObjectCount++;
                    }
                }
            }
        }

        if (ObjectCount < 3)
        {
            Victory = false;
        }

        SelectPlayer(_player);
    }

    public void SelectPlayer(uint _player)
    {
        // 检查该玩家是否在这个阵营中 并且修改当前胜利状态
        SelectPlayers.Add(_player);
        if (Victory)
        {
            if (DeclareAssociation == CardID.TheBrotherhoodOfTrueLies)
            {
                if (TheBrotherhoodOfTrueLies.Contains(_player))
                {
                    TheBrotherhoodOfTrueLies.Remove(_player);
                }
                else
                {
                    Victory = false;
                }
            }
            else if (DeclareAssociation == CardID.TheOrderOFOpenSecrets)
            {
                if (TheOrderOFOpenSecrets.Contains(_player))
                {
                    TheOrderOFOpenSecrets.Remove(_player);
                }
                else
                {
                    Victory = false;
                }
            }
        }
    }

    public bool FinalResult()
    {
        if (Victory)
        {
            if (DeclareAssociation == CardID.TheBrotherhoodOfTrueLies && TheBrotherhoodOfTrueLies.Count == 0)
            {
                return true;
            }
            else if (DeclareAssociation == CardID.TheOrderOFOpenSecrets && TheOrderOFOpenSecrets.Count == 0)
            {
                return true;
            }
        }

        return false;
    }
}
