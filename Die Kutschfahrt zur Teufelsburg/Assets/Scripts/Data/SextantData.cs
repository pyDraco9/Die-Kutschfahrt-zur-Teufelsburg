using System.Collections.Generic;

public class SextantData
{
    public uint[] playerList;
    private int currentActionPlayer = 0;
    public uint direction;
    public uint nextPlayer;
    public List<CardID> cardList = new List<CardID>();

    public SextantData(uint _player, uint _dirction, List<uint> _playerSet)
    {
        // 旋转列表匹配到起始玩家
        List<uint> tmpSet = new List<uint>();
        for (int i = 0; i < _playerSet.Count; i++)
        {
            tmpSet.Add(_playerSet[i]);
        }

        for (int i = 0; i < tmpSet.Count; i++)
        {
            uint tid = tmpSet[0];
            if (tid == _player)
            {
                tmpSet.Remove(tid);
                break;
            }
            else
            {
                tmpSet.Remove(tid);
                tmpSet.Add(tid);
            }
        }
        if (_dirction == 1)
        {
            tmpSet.Add(_player);
            direction = 1;
        }
        else
        {
            tmpSet.Reverse();
            tmpSet.Add(_player);
            direction = 0;
        }

        tmpSet.Add(0);
        playerList = tmpSet.ToArray();
    }

    public uint getNextPlayer()
    {
        return playerList[currentActionPlayer++];
    }

    public void cardListReverse()
    {
        cardList.Reverse();
        CardID _thisCard = cardList[0];
        cardList.RemoveAt(0);
        cardList.Add(_thisCard);
        cardList.Reverse();
    }
}
