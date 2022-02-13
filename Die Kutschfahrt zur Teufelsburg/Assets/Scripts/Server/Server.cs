using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Linq;
using DG.Tweening;

public class Server : NetworkBehaviour
{
    Dictionary<ClientCommand, Action<MessageTo>> cmdCall = new Dictionary<ClientCommand, Action<MessageTo>>();
    int readyCount;
    List<uint> playerSet = new List<uint>();
    /// <summary>
    /// 职业
    /// </summary>
    List<CardID> Occupation = new List<CardID>();
    /// <summary>
    /// 阵营
    /// </summary>
    List<CardID> Association = new List<CardID>();
    /// <summary>
    /// 道具
    /// </summary>
    List<CardID> Object = new List<CardID>();

    Dictionary<uint, PlayerData> playerDatas = new Dictionary<uint, PlayerData>();
    int controllPlayerSet;
    uint controllPlayer;
    List<uint> currentEventPlayer = new List<uint>();
    DeclareVictoryData declareVictoryData;

    TradeData tradeData;
    SextantData sextantData;
    FightData fightData;

    void Start()
    {
        if (isServer)
        {
            cmdCall.Add(ClientCommand.ClientReady, ClientReadyCmd);

            cmdCall.Add(ClientCommand.Trade, TradeCmd);
            cmdCall.Add(ClientCommand.TradeRequest, TradeRequestCmd);
            cmdCall.Add(ClientCommand.TradeBack, TradeBackCmd);
            cmdCall.Add(ClientCommand.TradeEvent, TradeEventCmd);
            cmdCall.Add(ClientCommand.TradeMonocle, TradeMonocleCmd);
            cmdCall.Add(ClientCommand.TradeCoat, TradeCoatCmd);
            cmdCall.Add(ClientCommand.TradeTome, TradeTomeCmd);
            cmdCall.Add(ClientCommand.TradePrivilege, TradePrivilegeCmd);
            cmdCall.Add(ClientCommand.TradeSextant, TradeSextantCmd);
            cmdCall.Add(ClientCommand.TradeSextantEvent, TradeSextantEventCmd);
            cmdCall.Add(ClientCommand.TradeSextantBack, TradeSextantBackCmd);

            cmdCall.Add(ClientCommand.UseDiplomat, UseDiplomatCmd);
            cmdCall.Add(ClientCommand.DiplomatSelect, DiplomatSelectCmd);
            cmdCall.Add(ClientCommand.DiplomatConfirm, DiplomatConfirmCmd);

            cmdCall.Add(ClientCommand.UseClairvoyant, UseClairvoyantCmd);
            cmdCall.Add(ClientCommand.ClairvoyantSecond, ClairvoyantSecondCmd);
            cmdCall.Add(ClientCommand.ClairvoyantFrist, ClairvoyantFristCmd);

            cmdCall.Add(ClientCommand.DeclareVictory, DeclareVictoryCmd);
            cmdCall.Add(ClientCommand.DeclareVictorySelectPlayer, DeclareVictorySelectPlayerCmd);
            cmdCall.Add(ClientCommand.DeclarePersonalVictory, DeclarePersonalVictoryCmd);

            cmdCall.Add(ClientCommand.UseDuelist, UseDuelistCmd);
            cmdCall.Add(ClientCommand.FightEvent, FightEventCmd);
            cmdCall.Add(ClientCommand.Attack, AttackCmd);
            cmdCall.Add(ClientCommand.AttackTarget, AttackTargetCmd);
            cmdCall.Add(ClientCommand.SupportAttack, SupportAttackCmd);
            cmdCall.Add(ClientCommand.SupportDefend, SupportDefendCmd);
            cmdCall.Add(ClientCommand.UsePriest, UsePriestCmd);
            cmdCall.Add(ClientCommand.UseThug, UseThugCmd);
            cmdCall.Add(ClientCommand.UseGrandMaster, UseGrandMasterCmd);
            cmdCall.Add(ClientCommand.UseDoctor, UseDoctorCmd);
            cmdCall.Add(ClientCommand.UseBodyguard, UseBodyguardCmd);
            cmdCall.Add(ClientCommand.UseHypnotist, UseHypnotistCmd);
            cmdCall.Add(ClientCommand.UseHypnotistTarget, UseHypnotistTargetCmd);
            cmdCall.Add(ClientCommand.UseCard, UseCardCmd);
            cmdCall.Add(ClientCommand.Payment, PaymentCmd);
            cmdCall.Add(ClientCommand.WinnerTake, FightWinnerTakeCmd);
            cmdCall.Add(ClientCommand.WinnerTakeData, WinnerTakeDataCmd);
            cmdCall.Add(ClientCommand.WinnerCheck, FightWinnerCheckCmd);
            cmdCall.Add(ClientCommand.WinnerCheckData, FightWinnerCheckDataCmd);
            cmdCall.Add(ClientCommand.WinnerGiveBack, WinnerGiveBackCmd);

            NetworkServer.RegisterHandler<MessageTo>(handleMessage);
            readyCount = 0;
        }
    }

    void UseThugCmd(MessageTo msg)
    {
        if (fightData.player1 == msg.playerID || fightData.player2 == msg.playerID || fightData.PlayerVoteList.Contains(msg.playerID))
        {
            CardID _thisCard = CardID.Thug;
            if (fightData.UseCard(_thisCard, msg.playerID))
            {
                SendPlayerClientMessage(msg.playerID, ClientMessage.UseCard, new uint[1] { (uint)_thisCard });
                SendPlayerOccupationPublic(GameManager.Instance.GetGamePlayer(msg.playerID), true);
            }
            else
            {
                SendToPlayerClientMessage(msg.playerID, ClientMessage.UsedCard);
            }
            SendPlayerUpdateFightData();
        }
    }

    void UseGrandMasterCmd(MessageTo msg)
    {
        if (fightData.player1 == msg.playerID || fightData.player2 == msg.playerID || fightData.PlayerVoteList.Contains(msg.playerID))
        {
            CardID _thisCard = CardID.GrandMaster;
            if (fightData.UseCard(_thisCard, msg.playerID))
            {
                SendPlayerClientMessage(msg.playerID, ClientMessage.UseCard, new uint[1] { (uint)_thisCard });
                SendPlayerOccupationPublic(GameManager.Instance.GetGamePlayer(msg.playerID), true);
            }
            else
            {
                SendToPlayerClientMessage(msg.playerID, ClientMessage.UsedCard);
            }
            SendPlayerUpdateFightData();
        }
    }

    void UseDoctorCmd(MessageTo msg)
    {
        if (fightData.player1 == msg.playerID || fightData.player2 == msg.playerID || fightData.PlayerVoteList.Contains(msg.playerID))
        {
            fightData.SetFightRound(FightData.FightRound.End);
            SendPlayerDocter(msg.playerID);
            SendPlayerOccupationPublic(GameManager.Instance.GetGamePlayer(msg.playerID), true);
            InitEventList();
            SendPlayerFightEvent();
        }
    }

    void SendPlayerDocter(uint _player)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = _player;
        msgSend.messageCommand = (int)ServerCommand.Docter;
        NetworkServer.SendToAll(msgSend);
    }

    void UseBodyguardCmd(MessageTo msg)
    {
        if (fightData.player1 == msg.playerID || fightData.player2 == msg.playerID || fightData.PlayerVoteList.Contains(msg.playerID))
        {
            CardID _thisCard = CardID.Bodyguard;
            if (fightData.UseCard(_thisCard, msg.playerID))
            {
                SendPlayerClientMessage(msg.playerID, ClientMessage.UseCard, new uint[1] { (uint)_thisCard });
                SendPlayerOccupationPublic(GameManager.Instance.GetGamePlayer(msg.playerID), true);
            }
            else
            {
                SendToPlayerClientMessage(msg.playerID, ClientMessage.UsedCard);
            }
            SendPlayerUpdateFightData();
        }
    }

    void FightWinnerCheckDataCmd(MessageTo msg)
    {
        if (controllPlayer == msg.playerID && fightData.winner == msg.playerID)
        {
            SendPlayerWinnerCheckConfirm();
            SendPlayerClientMessage(fightData.winner, ClientMessage.WinnerCheckEnd);
            InitEventList();
            SendPlayerFightEvent();
        }
    }

    void WinnerGiveBackCmd(MessageTo msg)
    {
        if (controllPlayer == msg.playerID && fightData.winner == msg.playerID)
        {
            SendPlayerWinnerTakeConfirm();
            CardID _thisCard = (CardID)msg.data[0];
            if (!playerDatas[fightData.winner].cardList.Contains(_thisCard)) return;
            SendRemovePlayerHandCard(GameManager.Instance.GetGamePlayer(fightData.winner), _thisCard);
            SendAddPlayerHandCard(GameManager.Instance.GetGamePlayer(fightData.loser), _thisCard);
            SendPlayerClientMessage(fightData.winner, ClientMessage.GiveBackEnd);
            InitEventList();
            SendPlayerFightEvent();
        }
    }

    void WinnerTakeDataCmd(MessageTo msg)
    {
        if (controllPlayer == msg.playerID && fightData.winner == msg.playerID)
        {
            SendPlayerWinnerTakeConfirm();
            CardID _thisCard = (CardID)msg.data[0];
            if (!playerDatas[fightData.loser].cardList.Contains(_thisCard)) return;
            SendRemovePlayerHandCard(GameManager.Instance.GetGamePlayer(fightData.loser), _thisCard);
            SendAddPlayerHandCard(GameManager.Instance.GetGamePlayer(fightData.winner), _thisCard);
            if (playerDatas[fightData.loser].cardList.Count == 0)
            {
                SendPlayerrWinnerTakeGiveBack();
            }
            else
            {
                SendPlayerClientMessage(fightData.winner, ClientMessage.FightTakeEnd);
                InitEventList();
                SendPlayerFightEvent();
            }
        }
    }

    void SendPlayerrWinnerTakeGiveBack()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = fightData.winner;
        msgSend.messageCommand = (int)ServerCommand.WinnerTakeGiveBack;
        NetworkServer.SendToAll(msgSend);
    }

    void SendPlayerWinnerCheckConfirm()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = controllPlayer;
        msgSend.messageCommand = (int)ServerCommand.WinnerCheckConfirm;
        NetworkServer.SendToAll(msgSend);
    }

    void SendPlayerWinnerTakeConfirm()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = controllPlayer;
        msgSend.messageCommand = (int)ServerCommand.WinnerTakeConfirm;
        NetworkServer.SendToAll(msgSend);
    }

    void FightWinnerTakeCmd(MessageTo msg)
    {
        if (controllPlayer == msg.playerID && fightData.winner == msg.playerID)
        {
            SendPlayerClientMessage(fightData.winner, ClientMessage.WinnerTake);
            SendPlayerLoserCard();
        }
    }

    void SendPlayerLoserCard()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.messageCommand = (int)ServerCommand.WinnerTakeData;
        msgSend.data = playerDatas[fightData.loser].cardList.ConvertAll<uint>(x => (uint)x).ToArray();
        NetworkServer.SendToClientOfPlayer(GameManager.Instance.GetGamePlayer(fightData.winner).netIdentity, msgSend);
    }

    void FightWinnerCheckCmd(MessageTo msg)
    {
        if (controllPlayer == msg.playerID && fightData.winner == msg.playerID)
        {
            SendPlayerClientMessage(fightData.winner, ClientMessage.WinnerCheck);
            SendPlayerWinnerCheck();
        }
    }

    void SendPlayerWinnerCheck()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.messageCommand = (int)ServerCommand.WinnerCheck;
        msgSend.data = new uint[2] { (uint)playerDatas[fightData.loser].association, (uint)playerDatas[fightData.loser].occupation };
        NetworkServer.SendToClientOfPlayer(GameManager.Instance.GetGamePlayer(fightData.winner).netIdentity, msgSend);
    }

    void SendPlayerSyncVoteList()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.messageCommand = (int)ServerCommand.SyncVoteList;
        msgSend.data = fightData.PlayerVoteList;
        NetworkServer.SendToAll(msgSend);
    }

    void PaymentCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            if (!playerDatas[msg.playerID].cardList.Contains((CardID)msg.data[0]))
            {
                return;
            }
            CardID _thisCard = (CardID)msg.data[0];
            SendRemovePlayerHandCard(GameManager.Instance.GetGamePlayer(msg.playerID), _thisCard);
            if (fightData.useCardList.ContainsKey(CardID.Priest))
            {
                SendAddPlayerHandCard(GameManager.Instance.GetGamePlayer(fightData.useCardList[CardID.Priest]), _thisCard);
            }
            else
            {
                SendAddPlayerHandCard(GameManager.Instance.GetGamePlayer(fightData.player2), _thisCard);
            }
            InitEventList();
            SendPlayerFightEvent();
        }
    }

    void UseCardCmd(MessageTo msg)
    {
        if (fightData.fightRound == FightData.FightRound.AttackAndDefend)
        {
            if (msg.playerID == fightData.player1 && (CardID)msg.data[0] == CardID.Dagger) 
            {
                UseCardSwitch(msg.playerID, (CardID)msg.data[0]);
            }
            else if (msg.playerID == fightData.player2 && (CardID)msg.data[0] == CardID.Gloves)
            {
                UseCardSwitch(msg.playerID, (CardID)msg.data[0]);
            }
        }
        else if (fightData.fightRound == FightData.FightRound.Support)
        {
            if (fightData.useCardList.ContainsKey(CardID.Hypnotist))
            {
                if (fightData.useCardList[CardID.Hypnotist] == fightData.HypnotistTarget)
                {
                    SendToPlayerClientMessage(msg.playerID, ClientMessage.Hypnotized);
                    return;
                }
            }
            if (msg.playerID == fightData.player1 && (CardID)msg.data[0] == CardID.Dagger)
            {
                UseCardSwitch(msg.playerID, (CardID)msg.data[0]);
            }
            else if (msg.playerID == fightData.player2 && (CardID)msg.data[0] == CardID.Gloves)
            {
                UseCardSwitch(msg.playerID, (CardID)msg.data[0]);
            }
            else
            {
                if (fightData.fightDetails.ContainsKey(msg.playerID))
                {
                    if (fightData.fightDetails[msg.playerID] == true && (CardID)msg.data[0] == CardID.CastingKnives)
                    {
                        UseCardSwitch(msg.playerID, (CardID)msg.data[0]);
                    }
                    else if (fightData.fightDetails[msg.playerID] == false && (CardID)msg.data[0] == CardID.Whip)
                    {
                        UseCardSwitch(msg.playerID, (CardID)msg.data[0]);
                    }
                }
            }
        }
        else if (fightData.fightRound == FightData.FightRound.PoisonRing)
        {
            if (playerDatas[msg.playerID].cardList.Contains(CardID.PoisonRing))
            {
                UseCardSwitch(msg.playerID, CardID.PoisonRing);
                SendPlayerUsePoisonRing(msg.playerID);
                InitEventList();
                SendPlayerFightEvent();
            }
        }
    }

    void SendPlayerUsePoisonRing(uint _player)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = _player;
        msgSend.messageCommand = (int)ServerCommand.UsePoisonRing;
        NetworkServer.SendToAll(msgSend);
    }

    void UseCardSwitch(uint _player ,CardID _cardID)
    {
        if (fightData.UseCard(_cardID, _player))
        {
            SendPlayerClientMessage(_player, ClientMessage.UseCard, new uint[1] { (uint)_cardID });
            SendPlayerUpdateFightData();
        }
        else
        {
            SendToPlayerClientMessage(_player, ClientMessage.UsedCard);
        }
    }

    void UseHypnotistTargetCmd(MessageTo msg)
    {
        if (fightData.fightRound != FightData.FightRound.Hypnotist) return;
        if (msg.data[0] == fightData.player1 || msg.data[0] == fightData.player2)
        {
            SendPlayerClientMessage(msg.playerID, ClientMessage.HypnotistTargetCant);
            return;
        }
        if (fightData.UseCard(CardID.Hypnotist, msg.playerID, msg.data))
        {
            SendPlayerClientMessage(msg.playerID, ClientMessage.HypnotistTarget, msg.data);
            InitEventList();
            SendPlayerFightEvent();
            SendPlayerUpdateFightData();
            SendPlayerSyncVoteList();
        }
    }

    void UsePriestCmd(MessageTo msg)
    {
        if (fightData.fightRound != FightData.FightRound.Priest) return;
        if (playerDatas[msg.playerID].occupation != CardID.Priest || playerDatas[msg.playerID].occupationIsPublic) return;
        if (fightData.UseCard(CardID.Priest, msg.playerID))
        {
            SendPlayerOccupationPublic(GameManager.Instance.GetGamePlayer(msg.playerID), true);
            SendPlayerPriest(msg.playerID);
            SendPlayerClientMessage(msg.playerID, ClientMessage.Priest);
            fightData.SetFightRound(FightData.FightRound.End);
            if (playerDatas[fightData.player1].cardList.Count > 1)
            {
                controllPlayer = fightData.player1;
                SendPlayerPriestEffect(fightData.player1);
            }
            else
            {
                InitEventList();
                SendPlayerFightEvent();
            }
        }
    }
    /// <summary>
    /// 因牧师效果 需要交出牌的玩家
    /// </summary>
    /// <param name="_player"></param>
    void SendPlayerPriestEffect(uint _player)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = _player;
        msgSend.messageCommand = (int)ServerCommand.PriestEffect;
        NetworkServer.SendToAll(msgSend);
    }

    void SendPlayerPriest(uint _player)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = _player;
        msgSend.messageCommand = (int)ServerCommand.Priest;
        NetworkServer.SendToAll(msgSend);
    }

    void UseHypnotistCmd(MessageTo msg)
    {
        if (fightData.fightRound != FightData.FightRound.Hypnotist) return;
        if (playerDatas[msg.playerID].occupation != CardID.Hypnotist || playerDatas[msg.playerID].occupationIsPublic) return;
        SendPlayerOccupationPublic(GameManager.Instance.GetGamePlayer(msg.playerID), true);
        SendPlayerHypnotist(msg.playerID);
    }

    void SendPlayerHypnotist(uint _player)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = _player;
        msgSend.messageCommand = (int)ServerCommand.Hypnotist;
        NetworkServer.SendToAll(msgSend);
    }

    void SendPlayerVoteConfirm()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = controllPlayer;
        msgSend.messageCommand = (int)ServerCommand.VoteConfirm;
        NetworkServer.SendToAll(msgSend);
    }

    void SupportAttackCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            SendPlayerVoteConfirm();
            fightData.Support(msg.playerID, true);
            SendPlayerClientMessage(msg.playerID, ClientMessage.VoteAttack);
            InitEventList();
            SendPlayerFightEvent();
            SendPlayerUpdateFightData();
        }
    }

    void SupportDefendCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            SendPlayerVoteConfirm();
            fightData.Support(msg.playerID, false);
            SendPlayerClientMessage(msg.playerID, ClientMessage.VoteDefend);
            InitEventList();
            SendPlayerFightEvent();
            SendPlayerUpdateFightData();
        }
    }

    void SendPlayerUpdateFightData()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.messageCommand = (int)ServerCommand.UpdateFightData;
        msgSend.data = new uint[2] { (uint)(fightData.AttackPoint + fightData.AttackAddPoint), (uint)(fightData.DefendPoint + fightData.DefendAddPoint) };
        NetworkServer.SendToAll(msgSend);
    }

    void FightEventCmd(MessageTo msg)
    {
        if (currentEventPlayer.Contains(msg.playerID))
        {
            SendToPlayerFightEventConfirm(msg.playerID);
            currentEventPlayer.Remove(msg.playerID);
            if (currentEventPlayer.Count == 0)
            {
                // 所有的判断都是"当前是什么状态 处理下一个流程" 所以流程中"FightData.FightRound.AttackAndDefend" 实际处理的是下一阶段"Priest"
                switch (fightData.fightRound)
                {
                    case FightData.FightRound.AttackAndDefend:
                        // 攻守双方操作完毕 判断牧师
                        fightData.SetFightRound(FightData.FightRound.Priest);
                        SendPlayerChangePriest();
                        InitEventList();
                        SendPlayerFightEvent();
                        break;
                    case FightData.FightRound.Priest:
                        uint player = fightData.getNextPlayer();
                        if (player == 0)
                        {
                            fightData.SetFightRound(FightData.FightRound.Vote);
                            SendPlayerClientMessage(0, ClientMessage.VoteEnd);
                            InitEventList();
                            SendPlayerFightEvent();
                        }
                        else
                        {
                            controllPlayer = player;
                            SendPlayerSyncVoteList();
                            SendPlayerVote(player);
                        }
                        break;
                    case FightData.FightRound.Vote:
                        fightData.SetFightRound(FightData.FightRound.Hypnotist);
                        SendPlayerChangeHypnotist();
                        InitEventList();
                        SendPlayerFightEvent();
                        break;
                    case FightData.FightRound.Hypnotist:
                        fightData.SetFightRound(FightData.FightRound.Support);
                        SendPlayerChangeSupport();
                        InitEventList();
                        SendPlayerUpdateFightData();
                        SendPlayerFightEvent();
                        break;
                    case FightData.FightRound.Support:
                        fightData.SetFightRound(FightData.FightRound.PoisonRing);
                        SendPlayerChangePoisonRing();
                        InitEventList();
                        SendPlayerFightEvent();
                        break;
                    case FightData.FightRound.PoisonRing:
                        fightData.SetFightRound(FightData.FightRound.End);
                        if (fightData.AttackPoint + fightData.AttackAddPoint == fightData.DefendPoint + fightData.DefendAddPoint)
                        {
                            if (fightData.useCardList.ContainsKey(CardID.PoisonRing))
                            {
                                if (fightData.useCardList[CardID.PoisonRing] == fightData.player1)
                                {
                                    fightData.winner = fightData.player1;
                                    fightData.loser = fightData.player2;
                                }
                                else
                                {
                                    fightData.winner = fightData.player2;
                                    fightData.loser = fightData.player1;
                                }
                            }
                            else
                            {
                                SendPlayerClientMessage(0, ClientMessage.Draw);
                                SendAddPlayerHandCard(GameManager.Instance.GetGamePlayer(fightData.player1), PickCardFrom(Object));
                                InitEventList();
                                SendPlayerFightEvent();
                                return;
                            }
                        }
                        else
                        {
                            if (fightData.AttackPoint + fightData.AttackAddPoint > fightData.DefendPoint + fightData.DefendAddPoint)
                            {
                                fightData.winner = fightData.player1;
                                fightData.loser = fightData.player2;
                            }
                            else
                            {
                                fightData.winner = fightData.player2;
                                fightData.loser = fightData.player1;
                            }
                        }
                        controllPlayer = fightData.winner;
                        SendPlayerChangeEnd();
                        break;
                    case FightData.FightRound.End:
                        RoundStart();
                        break;
                }
            }
        }
    }

    void SendPlayerVote(uint _player)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = _player;
        msgSend.messageCommand = (int)ServerCommand.FightVote;
        NetworkServer.SendToAll(msgSend);
    }

    void SendPlayerChangeEnd()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.messageCommand = (int)ServerCommand.FightEnd;
        msgSend.data = new uint[2] { fightData.winner, fightData.loser };
        NetworkServer.SendToAll(msgSend);
    }

    void SendPlayerChangePoisonRing()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.messageCommand = (int)ServerCommand.FightPoisonRing;
        NetworkServer.SendToAll(msgSend);
    }

    void SendPlayerChangeSupport()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.messageCommand = (int)ServerCommand.FightSupport;
        NetworkServer.SendToAll(msgSend);
    }

    void SendPlayerChangePriest()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.messageCommand = (int)ServerCommand.FightPriest;
        NetworkServer.SendToAll(msgSend);
    }

    void SendPlayerChangeHypnotist()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.messageCommand = (int)ServerCommand.FightHypnotist;
        NetworkServer.SendToAll(msgSend);
    }

    void SendToPlayerFightEventConfirm(uint _player)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = _player;
        msgSend.messageCommand = (int)ServerCommand.FightEventConfirm;
        NetworkServer.SendToAll(msgSend);
    }

    void UseDuelistCmd(MessageTo msg)
    {
        if (msg.playerID == fightData.player1 || msg.playerID == fightData.player2)
        {
            if (playerDatas[msg.playerID].occupationIsPublic == false)
            {
                if (fightData.UseCard(CardID.Duelist, msg.playerID))
                {
                    SendPlayerClientMessage(msg.playerID, ClientMessage.Duelist);
                    SendPlayerOccupationPublic(GameManager.Instance.GetGamePlayer(msg.playerID), true);
                }
                else
                {
                    SendToPlayerClientMessage(msg.playerID, ClientMessage.OccupationCant);
                }
                SendPlayerUpdateFightData();
            }
        }
    }

    void ClairvoyantFristCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            if ((CardID)msg.data[0] == CardID.Null)
            {
                SendPlayerClairvoyantEnd();
                return;
            }
            if (!Object.Contains((CardID)msg.data[0]))
            {
                SendToPlayerClientMessage(msg.playerID, ClientMessage.ClairvoyantCant);
                return;
            }
            Object.Remove((CardID)msg.data[0]);
            Object.Add((CardID)msg.data[0]);
            Object.Reverse();
            SendPlayerClairvoyantEnd();
        }
    }

    void ClairvoyantSecondCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            if (!Object.Contains((CardID)msg.data[0]))
            {
                SendToPlayerClientMessage(msg.playerID, ClientMessage.ClairvoyantCant);
                return;
            }
            Object = FlushList(Object);
            Object.Remove((CardID)msg.data[0]);
            Object.Add((CardID)msg.data[0]);
            SendPlayerClairvoyantFrist();
        }
    }

    void SendPlayerClairvoyantEnd()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = controllPlayer;
        msgSend.messageCommand = (int)ServerCommand.ClairvoyantEnd;
        NetworkServer.SendToAll(msgSend);
    }

    void SendPlayerClairvoyantFrist()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = controllPlayer;
        msgSend.messageCommand = (int)ServerCommand.ClairvoyantFrist;
        NetworkServer.SendToAll(msgSend);
    }

    /// <summary>
    /// 透视者
    /// </summary>
    /// <param name="msg"></param>
    void UseClairvoyantCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            if (playerDatas[msg.playerID].occupationIsPublic) return;
            SendPlayerUseClairvoyant();
            SendPlayerClairvoyantData(msg.playerID);
        }
        SendPlayerOccupationPublic(GameManager.Instance.GetGamePlayer(msg.playerID), true);
    }

    void SendPlayerClairvoyantData(uint _player)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = _player;
        msgSend.messageCommand = (int)ServerCommand.ClairvoyantData;
        msgSend.data = Object.ConvertAll<uint>(x => (uint)x).ToArray();
        NetworkServer.SendToClientOfPlayer(GameManager.Instance.GetGamePlayer(_player).netIdentity, msgSend);
    }

    void SendPlayerUseClairvoyant()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = controllPlayer;
        msgSend.messageCommand = (int)ServerCommand.Clairvoyant;
        msgSend.data = new uint[1] { (uint)Object.Count };
        NetworkServer.SendToAll(msgSend);
    }

    void DiplomatConfirmCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            RoundStart();
        }
    }

    void DiplomatSelectCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            bool cant = false;
            if ((CardID)msg.data[0] == CardID.SecretBagGoblet || (CardID)msg.data[0] == CardID.SecretBagKey)
            {
                cant = true;
                for (int i = 0; i < playerDatas[controllPlayer].cardList.Count; i++)
                {
                    if (playerDatas[controllPlayer].cardList[i] != CardID.SecretBagGoblet && playerDatas[controllPlayer].cardList[i] != CardID.SecretBagKey)
                    {
                        cant = false;
                        break;
                    }
                }
            }
            if (cant)
            {
                SendToPlayerClientMessage(controllPlayer, ClientMessage.CantTradeSecretBag);
                return;
            }
            tradeData = new TradeData(controllPlayer);
            tradeData.WanttedCard = (CardID)msg.data[0];
            SendPlayerDiplomatSelect();
        }
    }

    /// <summary>
    /// 外交官
    /// </summary>
    /// <param name="msg"></param>
    void UseDiplomatCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            if (playerDatas[msg.playerID].occupationIsPublic) return;
            SendPlayerDiplomat();
        }
        SendPlayerOccupationPublic(GameManager.Instance.GetGamePlayer(msg.playerID), true);
    }

    void SendPlayerDiplomatSelect()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = controllPlayer;
        msgSend.messageCommand = (int)ServerCommand.DiplomatSelect;
        msgSend.data = new uint[1] { (uint)tradeData.WanttedCard };
        NetworkServer.SendToAll(msgSend);
    }

    void SendPlayerDiplomat()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = controllPlayer;
        msgSend.messageCommand = (int)ServerCommand.Diplomat;
        NetworkServer.SendToAll(msgSend);
    }

    void AttackTargetCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            fightData.SelectPlayer(msg.data[0], playerSet);
            SendPlayerAttackTarget(msg.data[0]);
            fightData.SetFightRound(FightData.FightRound.AttackAndDefend);
            SendPlayerUpdateFightData();
            InitEventList();
            SendPlayerFightEvent();
        }
    }

    void SendPlayerAttackTarget(uint _player)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = controllPlayer;
        msgSend.messageCommand = (int)ServerCommand.AttackTarget;
        msgSend.data = new uint[1] { _player };
        NetworkServer.SendToAll(msgSend);
    }

    void AttackCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            fightData = new FightData(msg.playerID);
            SendPlayerAttack(msg.playerID);


        }
    }

    void SendPlayerAttack(uint _player)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = _player;
        msgSend.messageCommand = (int)ServerCommand.Attack;
        NetworkServer.SendToAll(msgSend);
    }

    void DeclareVictorySelectPlayerCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            if (msg.data[0] == 0)
            {
                if (declareVictoryData.FinalResult())
                {
                    // 胜利
                    SendPlayerDeclareVictoryMessage(declareVictoryData.DeclareAssociation, declareVictoryData.ObjectCount);
                }
                else
                {
                    // 失败
                    SendPlayerDeclareVictoryMessage(declareVictoryData.OtherAssociation, declareVictoryData.ObjectCount);
                }
                SendAllPlayerPublicAssociation();
            }
            else
            {
                if (declareVictoryData.SelectPlayers.Contains(msg.data[0]))
                {
                    SendToPlayerClientMessage(msg.playerID, ClientMessage.DeclareVictorySelected);
                    return;
                }
                declareVictoryData.SelectPlayer(msg.data[0]);
                SendPlayerDeclareVictorySelectPlayer(msg.data[0]);
            }
        }
    }

    void SendPlayerDeclareVictoryMessage(CardID _card, int _objectCount)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = controllPlayer;
        msgSend.messageCommand = (int)ServerCommand.DeclareVictoryMessage;
        msgSend.data = new uint[2] { (uint)_card, (uint)_objectCount };
        NetworkServer.SendToAll(msgSend);
    }

    void SendPlayerDeclareVictorySelectPlayer(uint _player)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = controllPlayer;
        msgSend.messageCommand = (int)ServerCommand.DeclareVictorySelectPlayer;
        msgSend.data = new uint[1] { _player };
        NetworkServer.SendToAll(msgSend);
    }

    void DeclareVictoryCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            declareVictoryData.DeclareVictory(msg.playerID, Object.Count, playerDatas);
            SendToPlayerClientMessage(msg.playerID, ClientMessage.DeclareVictory);
            SendPlayerDeclareVictory(msg.playerID);
        }
    }

    void SendPlayerDeclareVictory(uint _player)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = _player;
        msgSend.messageCommand = (int)ServerCommand.DeclareVictory;
        NetworkServer.SendToClientOfPlayer(GameManager.Instance.GetGamePlayer(_player).netIdentity, msgSend);
    }

    void DeclarePersonalVictoryCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            if (!playerDatas[controllPlayer].cardList.Contains(CardID.TheCoatOfArmorOfTheLoge))
            {
                SendToPlayerClientMessage(msg.playerID, ClientMessage.CantDeclarePersonalVictory);
                return;
            }
            int Count = 0;
            for (int i = 0; i < playerDatas[controllPlayer].cardList.Count; i++)
            {
                CardID _thisCard = playerDatas[controllPlayer].cardList[i];
                if (playerDatas[controllPlayer].association == CardID.TheBrotherhoodOfTrueLies)
                {
                    if ((Object.Count == 0 && _thisCard == CardID.SecretBagGoblet) || _thisCard == CardID.Goblet)
                    {
                        Count++;
                    }
                }
                else if (playerDatas[controllPlayer].association == CardID.TheOrderOFOpenSecrets)
                {
                    if ((Object.Count == 0 && _thisCard == CardID.SecretBagKey) || _thisCard == CardID.Key)
                    {
                        Count++;
                    }
                }
            }
            if (Count >= 3)
            {
                // 宣告成功
                SendPlayerClientMessage(msg.playerID, ClientMessage.DeclarePersonalVictory);
                SendAllPlayerPublicAssociation();
            }
            else
            {
                SendToPlayerClientMessage(msg.playerID, ClientMessage.CantDeclarePersonalVictory);
                return;
            }
        }
    }

    void TradeSextantBackCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            if (!playerDatas[msg.playerID].cardList.Contains((CardID)msg.data[0]))
            {
                // 这张牌不在手牌中
                SendToPlayerClientMessage(msg.playerID, ClientMessage.SextantCant);
                return;
            }
            sextantData.cardList.Add((CardID)msg.data[0]);
            sextantData.nextPlayer = sextantData.getNextPlayer();
            SendRemovePlayerHandCard(GameManager.Instance.GetGamePlayer(msg.playerID), (CardID)msg.data[0]);
            if (sextantData.nextPlayer == 0)
            {
                sextantData.cardListReverse();
                for (int i = 0; i < sextantData.playerList.Length; i++)
                {
                    if (sextantData.playerList[i] == 0) break;
                    SendAddPlayerHandCard(GameManager.Instance.GetGamePlayer(sextantData.playerList[i]), sextantData.cardList[i]);
                }
                RoundStart();
                return;
            }
            SendPlayerClientMessage(msg.playerID, ClientMessage.SextantNext);
            InitEventList();
            SendPlayerTradeSextantEvent();
        }
    }

    void TradeSextantCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            sextantData = new SextantData(msg.playerID, msg.data[0], playerSet);
            sextantData.nextPlayer = sextantData.getNextPlayer();
            if (msg.data[0] == 1)
            {
                // 左
                SendPlayerClientMessage(msg.playerID, ClientMessage.SextantLeft);
            }
            else
            {
                // 右
                SendPlayerClientMessage(msg.playerID, ClientMessage.SextantRight);
            }

            InitEventList();
            SendPlayerTradeSextantEvent();
        }
    }

    void TradeSextantEventCmd(MessageTo msg)
    {
        if (currentEventPlayer.Contains(msg.playerID))
        {
            currentEventPlayer.Remove(msg.playerID);
            if (currentEventPlayer.Count == 0)
            {
                if (sextantData.nextPlayer == 0)
                {
                    RoundStart();
                }
                else
                {
                    controllPlayer = sextantData.nextPlayer;
                    SendPlayerTradeSextantBack(sextantData.nextPlayer);
                }
            }
        }

    }

    /// <summary>
    /// 交易效果事件
    /// </summary>
    /// <param name="msg"></param>
    void TradeEventCmd(MessageTo msg)
    {
        if (currentEventPlayer.Contains(msg.playerID))
        {
            currentEventPlayer.Remove(msg.playerID);
            if (currentEventPlayer.Count == 0)
            {
                uint EffectPlayer = 0;
                CardID EffectCard = tradeData.getNextEffectCard(ref EffectPlayer);
                if ((EffectCard == CardID.SecretBagGoblet || EffectCard == CardID.SecretBagKey) && Object.Count == 0)
                {
                    EffectCard = tradeData.getNextEffectCard(ref EffectPlayer);
                }

                if (EffectCard == CardID.Null) // 不发动特效，进入下回合
                {
                    RoundStart();
                }
                else
                {
                    if (EffectCard == CardID.SecretBagGoblet || EffectCard == CardID.SecretBagKey)
                    {
                        SendPlayerClientMessage(EffectPlayer, ClientMessage.SecretBag);
                        SendAddPlayerHandCard(GameManager.Instance.GetGamePlayer(EffectPlayer), PickCardFrom(Object));
                        InitEventList();
                        SendPlayerTradeEvent();
                    }
                    else if (EffectCard == CardID.Monocle)
                    {
                        controllPlayer = EffectPlayer;
                        SendPlayerClientMessage(EffectPlayer, ClientMessage.Monocle);
                        SendPlayerTradeMonocle(EffectPlayer);
                    }
                    else if (EffectCard == CardID.Coat)
                    {
                        controllPlayer = EffectPlayer;
                        SendPlayerClientMessage(EffectPlayer, ClientMessage.Coat);
                        SendPlayerTradeCoat(EffectPlayer);
                    }
                    else if (EffectCard == CardID.Tome)
                    {

                        controllPlayer = EffectPlayer;
                        SendPlayerClientMessage(EffectPlayer, ClientMessage.Tome);
                        SendPlayerTradeTome(EffectPlayer);
                    }
                    else if (EffectCard == CardID.Privilege)
                    {
                        controllPlayer = EffectPlayer;
                        SendPlayerClientMessage(EffectPlayer, ClientMessage.Privilege);
                        SendPlayerTradePrivilege(EffectPlayer);
                    }
                    else if (EffectCard == CardID.Sextant)
                    {
                        controllPlayer = EffectPlayer;
                        SendPlayerClientMessage(EffectPlayer, ClientMessage.Sextant);
                        SendPlayerTradeSextant(EffectPlayer);
                    }
                }
            }
        }
    }

    void TradeTomeCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            if (msg.data[0] == 1)
            {
                // 交换职业
                SendPlayerClientMessage(msg.playerID, ClientMessage.TomeTrue);
                SendPlayerOccupationPublic(GameManager.Instance.GetGamePlayer(tradeData.player1));
                SendPlayerOccupationPublic(GameManager.Instance.GetGamePlayer(tradeData.player2));
                CardID tempPlayerOccupation1 = playerDatas[tradeData.player1].occupation;
                CardID tempPlayerOccupation2 = playerDatas[tradeData.player2].occupation;
                SendPlayerOccupation(GameManager.Instance.GetGamePlayer(tradeData.player1), tempPlayerOccupation2, false);
                SendPlayerOccupation(GameManager.Instance.GetGamePlayer(tradeData.player2), tempPlayerOccupation1, false);
            }
            else
            {
                // 不交换职业
                SendPlayerClientMessage(msg.playerID, ClientMessage.TomeFalse);
            }

            InitEventList();
            SendPlayerTradeEvent();
        }
    }

    void TradeCoatCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            if ((CardID)msg.data[0] == CardID.Null)
            {
                SendPlayerClientMessage(msg.playerID, ClientMessage.CoatFalse);
            }
            else
            {
                if (!Occupation.Contains((CardID)msg.data[0]) || new CardData((CardID)msg.data[0]).cardType != CardType.Occupation)
                {
                    // 不是牌组中的牌 或 不是职业牌 直接报错
                    SendPlayerClientMessage(msg.playerID, ClientMessage.CoatCant);
                    return;
                }
                SendPlayerClientMessage(msg.playerID, ClientMessage.CoatTrue);
                SendPlayerOccupationPublic(GameManager.Instance.GetGamePlayer(msg.playerID));
                SendPlayerOccupation(GameManager.Instance.GetGamePlayer(msg.playerID), (CardID)msg.data[0]);
                Occupation.Remove((CardID)msg.data[0]);
            }

            InitEventList();
            SendPlayerTradeEvent();
        }
    }

    void TradeMonocleCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            SendPlayerClientMessage(msg.playerID, ClientMessage.MonocleEnd);
            InitEventList();
            SendPlayerTradeEvent();
        }
    }

    void TradePrivilegeCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            SendPlayerClientMessage(msg.playerID, ClientMessage.PrivilegeEnd);
            InitEventList();
            SendPlayerTradeEvent();
        }
    }

    void SendPlayerTradeSextantBack(uint _player)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = _player;
        msgSend.messageCommand = (int)ServerCommand.TradeSextantBack;
        NetworkServer.SendToAll(msgSend);
    }

    void SendPlayerTradeSextant(uint _player)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = _player;
        msgSend.messageCommand = (int)ServerCommand.TradeSextant;
        NetworkServer.SendToClientOfPlayer(GameManager.Instance.GetGamePlayer(_player).netIdentity, msgSend);
    }

    void SendPlayerTradeTome(uint _player)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = _player;
        msgSend.messageCommand = (int)ServerCommand.TradeTome;
        NetworkServer.SendToClientOfPlayer(GameManager.Instance.GetGamePlayer(_player).netIdentity, msgSend);
    }

    void SendPlayerTradeCoat(uint _player)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = _player;
        msgSend.messageCommand = (int)ServerCommand.TradeCoat;
        uint[] arr = Occupation.ConvertAll<uint>(x => (uint)x).ToArray();
        msgSend.data = arr;
        NetworkServer.SendToClientOfPlayer(GameManager.Instance.GetGamePlayer(_player).netIdentity, msgSend);
    }

    void SendPlayerTradeMonocle(uint _player)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = _player;
        msgSend.messageCommand = (int)ServerCommand.TradeMonocle;
        msgSend.data = new uint[1] { (uint)playerDatas[_player == tradeData.player1 ? tradeData.player2 : tradeData.player1].association };
        NetworkServer.SendToClientOfPlayer(GameManager.Instance.GetGamePlayer(_player).netIdentity, msgSend);
    }

    void SendPlayerTradePrivilege(uint _player)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = _player;
        msgSend.messageCommand = (int)ServerCommand.TradePrivilege;
        uint[] arr = playerDatas[_player == tradeData.player1 ? tradeData.player2 : tradeData.player1].cardList.ConvertAll<uint>(x => (uint)x).ToArray();
        msgSend.data = arr;
        NetworkServer.SendToClientOfPlayer(GameManager.Instance.GetGamePlayer(_player).netIdentity, msgSend);
    }

    void SendPlayerClientMessage(uint _player, ClientMessage _ClientMessage, uint[] _data = null)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = _player;
        msgSend.messageCommand = (int)ServerCommand.ClientMessage;
        if (_data == null)
        {
            msgSend.data = new uint[1] { (uint)_ClientMessage };
        }
        else
        {
            msgSend.data = new uint[_data.Length + 1];
            msgSend.data[0] = (uint)_ClientMessage;
            for (int i = 0; i < _data.Length; i++)
            {
                msgSend.data[i + 1] = _data[i];
            }
        }
        NetworkServer.SendToAll(msgSend);
    }

    void SendToPlayerClientMessage(uint _player, ClientMessage _ClientMessage)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = _player;
        msgSend.messageCommand = (int)ServerCommand.ClientMessage;
        msgSend.data = new uint[1] { (uint)_ClientMessage };
        NetworkServer.SendToClientOfPlayer(GameManager.Instance.GetGamePlayer(_player).netIdentity, msgSend);
    }
    void SendPlayerTradeSextantEvent()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.messageCommand = (int)ServerCommand.TradeSextantEvent;
        NetworkServer.SendToAll(msgSend);
    }

    void SendPlayerTradeEvent()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.messageCommand = (int)ServerCommand.TradeEvent;
        NetworkServer.SendToAll(msgSend);
    }

    void SendPlayerFightEvent()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.messageCommand = (int)ServerCommand.FightEvent;
        NetworkServer.SendToAll(msgSend);
    }

    void TradeBackCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            tradeData.card2 = (CardID)msg.data[0];
            if (tradeData.card2 == CardID.Null)
            {
                // 拒绝交易
                if (tradeData.card1 == CardID.BrokeMirror || tradeData.card1 == CardID.BlackPeart)
                {
                    SendToPlayerClientMessage(msg.playerID, ClientMessage.CantRefuse);
                    return;
                }
                if (tradeData.WanttedCard != CardID.Null)
                {
                    if (playerDatas[tradeData.player2].cardList.Contains(tradeData.WanttedCard))
                    {
                        SendToPlayerClientMessage(msg.playerID, ClientMessage.CantRefuse);
                        return;
                    }
                    else
                    {
                        controllPlayer = tradeData.player1;
                        SendPlayerTradeBack(false);
                        DiplomatConfirm();
                    }
                }
                else
                {
                    SendPlayerTradeBack(false);
                    InitEventList();
                    SendPlayerTradeEvent();
                }

            }
            else
            {
                // 同意交易
                if (tradeData.WanttedCard != CardID.Null && tradeData.WanttedCard != tradeData.card2)
                {
                    // 如果有指定的牌 却没交易 返回失败
                    SendToPlayerClientMessage(msg.playerID, ClientMessage.CantTrade);
                    return;
                }
                if (tradeData.card1 == CardID.SecretBagGoblet || tradeData.card1 == CardID.SecretBagKey)
                {
                    if (tradeData.card2 == CardID.SecretBagGoblet || tradeData.card2 == CardID.SecretBagKey)
                    {
                        SendToPlayerClientMessage(msg.playerID, ClientMessage.CantTrade);
                        return;
                    }
                }
                SendRemovePlayerHandCard(GameManager.Instance.GetGamePlayer(tradeData.player1), tradeData.card1);
                SendRemovePlayerHandCard(GameManager.Instance.GetGamePlayer(tradeData.player2), tradeData.card2);
                SendAddPlayerHandCard(GameManager.Instance.GetGamePlayer(tradeData.player1), tradeData.card2);
                SendAddPlayerHandCard(GameManager.Instance.GetGamePlayer(tradeData.player2), tradeData.card1);
                tradeData.InitTradeEffect();
                SendPlayerTradeBack(true);
                InitEventList();
                SendPlayerTradeEvent();
            }
        }
    }

    /// <summary>
    /// 外交官拒绝后 发送手牌
    /// </summary>
    void DiplomatConfirm()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = tradeData.player1;
        msgSend.messageCommand = (int)ServerCommand.DiplomatConfirm;
        msgSend.data = playerDatas[tradeData.player2].cardList.ConvertAll<uint>(x => (uint)x).ToArray();
        NetworkServer.SendToAll(msgSend);
    }

    void InitEventList()
    {
        currentEventPlayer.Clear();
        foreach (GamePlayer gamePlayer in GameManager.Instance.GetAllGamePlayer())
        {
            currentEventPlayer.Add(gamePlayer.id);
        }
    }

    void SendPlayerTradeBack(bool _bool)
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = tradeData.player2;
        msgSend.messageCommand = (int)ServerCommand.TradeBack;
        if (_bool)
        {
            msgSend.data = new uint[1] { 1 };
        }
        else
        {
            msgSend.data = new uint[1] { 0 };
        }
        NetworkServer.SendToAll(msgSend);

    }

    void TradeRequestCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            if (((CardID)msg.data[1] == CardID.SecretBagGoblet || (CardID)msg.data[1] == CardID.SecretBagKey) && (tradeData.WanttedCard == CardID.SecretBagGoblet || tradeData.WanttedCard == CardID.SecretBagKey))
            {
                SendToPlayerClientMessage(msg.playerID, ClientMessage.CantTrade);
                return;
            }
            tradeData.player2 = msg.data[0];
            tradeData.card1 = (CardID)msg.data[1];
            controllPlayer = tradeData.player2;

            MessageTo msgSend = new MessageTo();
            msgSend.playerID = tradeData.player2;
            msgSend.messageCommand = (int)ServerCommand.TradeRequest;
            foreach (GamePlayer gamePlayer in GameManager.Instance.GetAllGamePlayer())
            {
                msgSend.data = new uint[0];
                if (gamePlayer.id == tradeData.player2)
                {
                    msgSend.data = new uint[1] { (uint)tradeData.card1 }; 
                }
                NetworkServer.SendToClientOfPlayer(gamePlayer.netIdentity, msgSend);
            }
        }
    }

    void TradeCmd(MessageTo msg)
    {
        if (msg.playerID == controllPlayer)
        {
            tradeData = new TradeData(controllPlayer);

            MessageTo msgSend = new MessageTo();
            msgSend.playerID = controllPlayer;
            msgSend.messageCommand = (int)ServerCommand.Trade;
            NetworkServer.SendToAll(msgSend);
        }
    }

    void ClientReadyCmd(MessageTo msg)
    {
        readyCount += 1;
        InitPlayerData(msg.playerID);
        if (readyCount == GameManager.Instance.GetAllPlayer().Count)
        {
            RowPlayer();
            InitCardList(GameManager.Instance.GetAllPlayer().Count);
            SendPlayerSet();

            InitPlayerHandCard();
            RoundStart();
        }
    }

    void RoundStart()
    {
        MessageTo msgSend = new MessageTo();
        msgSend.playerID = NextRoundPlayer();
        msgSend.messageCommand = (int)ServerCommand.RoundStart;
        NetworkServer.SendToAll(msgSend);
    }

    uint NextRoundPlayer()
    {
        if (controllPlayerSet == 0)
        {
            controllPlayerSet = playerSet.Count;
        }
        controllPlayerSet -= 1;
        controllPlayer = playerSet[controllPlayerSet];
        return playerSet[controllPlayerSet];
    }

    void InitPlayerData(uint _player)
    {
        if (playerDatas == null)
        {
            playerDatas = new Dictionary<uint, PlayerData>();
        }
        if (!playerDatas.ContainsKey(_player))
        {
            playerDatas.Add(_player, new PlayerData());
        }
    }
    
    void InitPlayerHandCard()
    {
        declareVictoryData = new DeclareVictoryData();
        for (int i = 0; i < playerSet.Count; i++)
        {
            var gamePlayer = GameManager.Instance.GetGamePlayer(playerSet[i]);
            SendPlayerAssociation(gamePlayer, PickCardFrom(Association));
            SendPlayerOccupation(gamePlayer, PickCardFrom(Occupation));
            SendAddPlayerHandCard(gamePlayer, PickCardFrom(Object));
        }
    }

    [ContextMenu("随机更换职业")]
    void Debug_SendAllPlayerRandomOccupation()
    {
        for (int i = 0; i < playerSet.Count; i++)
        {
            var gamePlayer = GameManager.Instance.GetGamePlayer(playerSet[i]);
            SendPlayerOccupation(gamePlayer, PickCardFrom(Occupation));
            SendPlayerOccupationPublic(gamePlayer, false);
        }
    }

    [ContextMenu("翻开全部职业牌")]
    void Debug_SendAllPlayerPublicOccupation()
    {
        for (int i = 0; i < playerSet.Count; i++)
        {
            var gamePlayer = GameManager.Instance.GetGamePlayer(playerSet[i]);
            SendPlayerOccupationPublic(gamePlayer, true);
        }
    }

    [ContextMenu("翻开全部阵营牌")]
    void SendAllPlayerPublicAssociation()
    {
        for (int i = 0; i < playerSet.Count; i++)
        {
            var gamePlayer = GameManager.Instance.GetGamePlayer(playerSet[i]);
            SendPlayerAssociationPublic(gamePlayer);
        }
    }


    void SendPlayerAssociationPublic(GamePlayer _player)
    {
        MessageTo msgClient = new MessageTo();
        msgClient.playerID = _player.id;
        msgClient.messageCommand = (int)ServerCommand.AssociationPublic;
        msgClient.data = new uint[1] { (uint)playerDatas[_player.id].association };
        NetworkServer.SendToAll(msgClient);
    }

    void SendPlayerOccupationPublic(GamePlayer _player, bool isPublic = false)
    {
        playerDatas[_player.id].occupationIsPublic = isPublic;
        MessageTo msgClient = new MessageTo();
        msgClient.playerID = _player.id;
        msgClient.messageCommand = (int)ServerCommand.OccupationPublic;
        if (isPublic)
        {
            msgClient.data = new uint[1] { (uint)playerDatas[_player.id].occupation };
        }
        else
        {
            msgClient.data = new uint[1] { 0 };
        }
        NetworkServer.SendToAll(msgClient);
    }

    [ContextMenu("调试输出_所有职业牌")]
    void Debug_PrintAllOccupation()
    {
        for (int i = 0; i < Occupation.Count; i++)
        {
            Debug.Log(new CardData(Occupation[i]).cardName);
        }
    }

    [ContextMenu("发放全部道具")]
    void Debug_SendPlayerCard()
    {
        while(Object.Count > 0)
        {
            for (int i = 0; i < playerSet.Count; i++)
            {
                var gamePlayer = GameManager.Instance.GetGamePlayer(playerSet[i]);
                SendAddPlayerHandCard(gamePlayer, PickCardFrom(Object));
            }
        }
    }

    [ContextMenu("作弊_发放胜利牌")]
    void Debug_SendPlayerVictoryCard()
    {
        for (int i = 0; i < playerSet.Count; i++)
        {
            var gamePlayer = GameManager.Instance.GetGamePlayer(playerSet[i]);
            if (playerDatas[gamePlayer.id].association == CardID.TheBrotherhoodOfTrueLies)
            {
                SendAddPlayerHandCard(gamePlayer, CardID.Goblet);
                SendAddPlayerHandCard(gamePlayer, CardID.Goblet);
                SendAddPlayerHandCard(gamePlayer, CardID.Goblet);
            }
            else
            {
                SendAddPlayerHandCard(gamePlayer, CardID.Key);
                SendAddPlayerHandCard(gamePlayer, CardID.Key);
                SendAddPlayerHandCard(gamePlayer, CardID.Key);
            }
        }
    }

    /// <summary>
    /// 发送玩家职业牌
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_card"></param>
    /// <param name="_putBack">是否放回到牌组</param>

    void SendPlayerOccupation(GamePlayer _player, CardID _card, bool _putBack = true)
    {
        if (_card == (uint)CardID.Null) return;
        if (playerDatas[_player.id].occupation != CardID.Null && _putBack)
        {
            Occupation.Add(playerDatas[_player.id].occupation);
        }
        playerDatas[_player.id].occupation = _card;
        playerDatas[_player.id].occupationIsPublic = false;

        MessageTo msgClient = new MessageTo();
        msgClient.playerID = _player.id;
        msgClient.messageCommand = (int)ServerCommand.LicensingOccupation;
        msgClient.data = new uint[1] { (uint)_card };
        NetworkServer.SendToClientOfPlayer(_player.netIdentity, msgClient);
    }

    /// <summary>
    /// 发送玩家阵营牌
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_card"></param>
    
    void SendPlayerAssociation(GamePlayer _player, CardID _card)
    {
        if (_card == (uint)CardID.Null) return;
        if (playerDatas[_player.id].association != CardID.Null)
        {
            Association.Add(playerDatas[_player.id].association);
        }
        playerDatas[_player.id].association = _card;
        declareVictoryData.AddPlayerToList(_player.id, _card);
        MessageTo msgClient = new MessageTo();
        msgClient.playerID = _player.id;
        msgClient.messageCommand = (int)ServerCommand.LicensingAssociation;
        msgClient.data = new uint[1] { (uint)_card };
        NetworkServer.SendToClientOfPlayer(_player.netIdentity, msgClient);
    }

    CardID PickCardFrom(List<CardID> _cardDataList)
    {
        if (_cardDataList.Count > 0)
        {
            var ret = _cardDataList[0];
            _cardDataList.RemoveAt(0);
            return ret;
        }
        return CardID.Null;
    }
    
    void SendRemovePlayerHandCard(GamePlayer _player, CardID _card)
    {
        if (_card == (uint)CardID.Null) return;
        RemovePlayerHandCard(_player.id, _card);
        MessageTo msgClient = new MessageTo();
        msgClient.playerID = _player.id;
        msgClient.messageCommand = (int)ServerCommand.LicensingBack;
        msgClient.data = new uint[1] { (uint)_card };
        NetworkServer.SendToClientOfPlayer(_player.netIdentity, msgClient);
        SyncData();
    }

    void SendAddPlayerHandCard(GamePlayer _player, CardID _card)
    {
        if (_card == (uint)CardID.Null) return;
        AddPlayerHandCard(_player.id, _card);
        MessageTo msgClient = new MessageTo();
        msgClient.playerID = _player.id;
        msgClient.messageCommand = (int)ServerCommand.Licensing;
        msgClient.data = new uint[1] { (uint)_card };
        NetworkServer.SendToClientOfPlayer(_player.netIdentity, msgClient);
        SyncData();
    }

    void SyncData()
    {
        SendSyncPlayerHandCount();
        SendSyncDeckObjectCount();
    }

    void SendSyncDeckObjectCount()
    {
        MessageTo msgClient = new MessageTo();
        msgClient.messageCommand = (int)ServerCommand.SyncDeckObjectCount;
        msgClient.data = new uint[1] { (uint)Object.Count() };
        NetworkServer.SendToAll(msgClient);
    }

    void SendSyncPlayerHandCount()
    {
        MessageTo msgClient = new MessageTo();
        msgClient.messageCommand = (int)ServerCommand.SyncPlayerHandCount;
        for (int i = 0; i < playerSet.Count; i++)
        {
            msgClient.playerID = playerSet[i];
            msgClient.data = new uint[1] { (uint)playerDatas[playerSet[i]].cardList.Count };
            NetworkServer.SendToAll(msgClient);
        }
    }

    public void AddPlayerHandCard(uint _playerID, CardID _cardID)
    {
        playerDatas[_playerID].cardList.Add(_cardID);
    }

    public bool RemovePlayerHandCard(uint _playerID, CardID _cardID)
    {
        return playerDatas[_playerID].cardList.Remove(_cardID);
    }

    void InitCardList(int playerNumber)
    {
        Occupation.Add(CardID.GrandMaster);
        Occupation.Add(CardID.Doctor);
        Occupation.Add(CardID.Clairvoyant);
        Occupation.Add(CardID.Thug);
        Occupation.Add(CardID.Hypnotist);
        Occupation.Add(CardID.PoisonMixer);
        Occupation.Add(CardID.Priest);
        Occupation.Add(CardID.Duelist);
        Occupation.Add(CardID.Bodyguard);
        Occupation.Add(CardID.Diplomat);
        Occupation = FlushList(Occupation);

        Object.Add(CardID.Monocle);
        Object.Add(CardID.Gloves);
        if (playerNumber < 10)
        {
            Object.Add(CardID.Coat);
        }
        Object.Add(CardID.Tome);
        Object.Add(CardID.CastingKnives);
        Object.Add(CardID.Dagger);
        Object.Add(CardID.Privilege);
        Object.Add(CardID.PoisonRing);
        Object.Add(CardID.BrokeMirror);
        if (playerNumber > 3)
        {
            Object.Add(CardID.BlackPeart);
        }
        Object.Add(CardID.Whip);
        Object.Add(CardID.TheCoatOfArmorOfTheLoge);
        Object.Add(CardID.Sextant);

        int c = 0;
        if (playerNumber % 2 == 0)
        {
            c = playerNumber / 2;
        }
        else
        {
            c = (playerNumber + 1) / 2;
        }

        for (int i = 0; i < c; i++)
        {
            Association.Add(CardID.TheOrderOFOpenSecrets);
            Association.Add(CardID.TheBrotherhoodOfTrueLies);
        }

        for (int i = 0; i < 3; i++)
        {
            Object.Add(CardID.Key);
            Object.Add(CardID.Goblet);
        }

        Object = FlushList(Object);
        Object.Insert(UnityEngine.Random.Range(0, playerNumber - 1), CardID.SecretBagGoblet);
        Object.Insert(UnityEngine.Random.Range(0, playerNumber), CardID.SecretBagKey);

        Association = FlushList(Association);
    }
    
    void handleMessage(NetworkConnection conn, MessageTo msg)
    {
        ClientCommand clientcmd = (ClientCommand)msg.messageCommand;
        if (cmdCall.ContainsKey(clientcmd))
        {
            cmdCall[clientcmd](msg);
        }
        else
        {
            Debug.Log("CommandError! " + clientcmd.ToString());
        }
    }

    void RowPlayer()
    {
        playerSet = GameManager.Instance.allGamePlayers.Keys.ToList();
        playerSet = FlushList(playerSet);
    }

    private List<T> FlushList<T>(List<T> list)
    {
        var random = new System.Random();
        var newList = new List<T>();
        foreach (var item in list)
        {
            newList.Insert(random.Next(newList.Count), item);
        }
        return newList;
    }

    void SendPlayerSet()
    {
        MessageTo msgClient = new MessageTo();
        msgClient.messageCommand = (int)ServerCommand.PlayerSet;
        msgClient.data = new uint[playerSet.Count];
        for (int i = 0; i < playerSet.Count; i++)
        {
            msgClient.data[i] = playerSet[i];
        }
        NetworkServer.SendToAll(msgClient);
    }
}
