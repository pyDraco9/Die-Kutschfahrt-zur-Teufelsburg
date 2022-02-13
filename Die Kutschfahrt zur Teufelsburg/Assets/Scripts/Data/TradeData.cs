public class TradeData
{
    public uint player1;
    public uint player2;
    public CardID card1;
    public CardID card2;
    public CardID WanttedCard = CardID.Null;

    private CardID[] CardOrderList;
    private uint[] PlayerOrderList;
    private int CurrentActionCard;


    public TradeData (uint _player)
    {
        player1 = _player;

        CurrentActionCard = 0;
        CardOrderList = new CardID[3] { CardID.Null, CardID.Null, CardID.Null };
        PlayerOrderList = new uint[3] { 0, 0, 0 };
    }

    public void InitTradeEffect()
    {
        if (card1 == CardID.BrokeMirror || card2 == CardID.BrokeMirror)
        {
            return;
        }
        int tmp = isTradeEffectCard(card1, card2);
        int i = 0;
        if ((tmp & 1) > 0)
        {
            CardOrderList[i] = card1;
            PlayerOrderList[i] = player1;
            i++;
        }
        if ((tmp & 2) > 0)
        {
            CardOrderList[i] = card2;
            PlayerOrderList[i] = player2;
        }
        if (tmp == 7)
        {
            SwapVal(ref CardOrderList);
            SwapVal(ref PlayerOrderList);
        }
    }

    void SwapVal<T>(ref T[] val)
    {
        T a = val[0];
        val[0] = val[1];
        val[1] = a;
    }

    public int isTradeEffectCard(CardID _cardID1, CardID _cardID2)
    {
        int ret = 0;
        if (_cardID1 == CardID.Monocle || _cardID1 == CardID.Coat || _cardID1 == CardID.SecretBagGoblet || _cardID1 == CardID.SecretBagKey || _cardID1 == CardID.Tome || _cardID1 == CardID.Privilege || _cardID1 == CardID.Sextant)
        {
            ret += 1;
        }
        if (_cardID2 == CardID.Monocle || _cardID2 == CardID.Coat || _cardID2 == CardID.SecretBagGoblet || _cardID2 == CardID.SecretBagKey || _cardID2 == CardID.Tome || _cardID2 == CardID.Privilege || _cardID2 == CardID.Sextant)
        {
            ret += 2;
        }
        if (_cardID1 == CardID.Sextant)
        {
            ret += 4;
        }
        return ret;
    }

    public CardID getNextEffectCard(ref uint effectPlayer)
    {
        effectPlayer = PlayerOrderList[CurrentActionCard];
        return CardOrderList[CurrentActionCard++];
    }
}