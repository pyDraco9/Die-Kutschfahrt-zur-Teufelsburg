using System.Collections.Generic;

public class FightData
{
    public uint player1;
    public uint player2;
    public int AttackPoint = 1;
    public int DefendPoint = 1;
    public int AttackAddPoint;
    public int DefendAddPoint;
    public uint[] PlayerVoteList;
    private int currentActionPlayer = 0;
    public FightRound fightRound = FightRound.Start;
    public Dictionary<uint, bool> fightDetails = new Dictionary<uint, bool>();
    public Dictionary<CardID, uint> useCardList = new Dictionary<CardID, uint>();
    public uint winner;
    public uint loser;
    public uint HypnotistTarget;


    public enum FightRound
    {
        /// <summary>
        /// 发起攻击
        /// </summary>
        Start,
        /// <summary>
        /// 设置目标
        /// </summary>
        TargetSet,
        /// <summary>
        /// 攻守双方操作
        /// </summary>
        AttackAndDefend,
        /// <summary>
        /// 判断是否使用牧师
        /// </summary>
        Priest,
        /// <summary>
        /// 全员投票 Gloves,Dagger,Grand Master,Thug,Duelist,Poison Mixer,Priest
        /// </summary>
        Vote,
        /// <summary>
        /// 投票后判断是否使用催眠师
        /// </summary>
        Hypnotist,
        /// <summary>
        /// 全员使用卡片 Gloves,Dagger,Grand Master,Thug,Duelist,Casting Knives,whip,Doctor,Bodyguard
        /// </summary>
        Support,
        /// <summary>
        /// 结算前最终结果 Poison Ring
        /// </summary>
        PoisonRing,
        /// <summary>
        /// 已完成战斗 仅用于服务器记录战斗状态
        /// </summary>
        End,
    }

    public void SetFightRound(FightRound _fr)
    {
        fightRound = _fr;
    }


    /// <summary>
    /// 支援数据记录
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="support">true = attack, false = defend</param>
    public void Support(uint _player, bool _support)
    {
        if (fightDetails.ContainsKey(_player))
        {
            fightDetails[_player] = _support;
        }
        else
        {
            fightDetails.Add(_player, _support);
        }
        if (_support)
        {
            AttackAddPoint++;
        }
        else
        {
            DefendAddPoint++;
        }
    }

    public FightData(uint _player)
    {
        player1 = _player;
    }

    public void SelectPlayer(uint _player, List<uint> _playerSet)
    {
        player2 = _player;
        fightRound = FightRound.TargetSet;

        // 旋转列表匹配到起始玩家 并剔除攻守双方
        List<uint> tmpSet = new List<uint>();
        for (int i = 0; i < _playerSet.Count; i++)
        {
            tmpSet.Add(_playerSet[i]);
        }

        int c = tmpSet.Count;
        for (int i = 0; i < c; i++)
        {
            uint tid = tmpSet[0];
            if (tid == player1 || tid == player2)
            {
                tmpSet.Remove(tid);
            }
            else
            {
                tmpSet.Remove(tid);
                tmpSet.Add(tid);
            }
        }
        tmpSet.Reverse();
        tmpSet.Add(0);
        PlayerVoteList = tmpSet.ToArray();
    }

    public bool UseCard(CardID _cardID, uint _player, uint[] _data = null)
    {
        if (useCardList.ContainsKey(_cardID)) return false;
        useCardList.Add(_cardID, _player);
        switch (_cardID)
        {
            case CardID.Gloves:
                DefendPoint++;
                break;
            case CardID.CastingKnives:
                AttackAddPoint++;
                break;
            case CardID.Dagger:
                AttackPoint++;
                break;
            case CardID.Whip:
                DefendAddPoint++;
                break;
            case CardID.GrandMaster:
                break;
            case CardID.Doctor:
                break;
            case CardID.Thug:
                break;
            case CardID.Hypnotist:
                List<uint> tmp = new List<uint>();
                for (int i = 0; i < PlayerVoteList.Length; i++)
                {
                    if (PlayerVoteList[i] != _data[0])
                    {
                        tmp.Add(PlayerVoteList[i]);
                    }
                }
                PlayerVoteList = tmp.ToArray();
                if (fightDetails[_data[0]])
                {
                    AttackAddPoint--;
                }
                else
                {
                    DefendAddPoint--;
                }
                HypnotistTarget = _data[0];
                break;
            case CardID.Duelist:
                PlayerVoteList = new uint[1] { 0 };
                AttackAddPoint = 0;
                DefendAddPoint = 0;
                if (player1 == _player)
                {
                    AttackPoint++;
                }
                else
                {
                    DefendPoint++;
                }
                break;
            case CardID.Bodyguard:
                break;
        }
        return true;
    }

    public uint getNextPlayer()
    {
        return PlayerVoteList[currentActionPlayer++];
    }
}
