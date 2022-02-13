using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using System.Collections;

public class GamePlayer : NetworkBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Dictionary<ServerCommand, Action<MessageTo>> cmdCall = new Dictionary<ServerCommand, Action<MessageTo>>();
    [SyncVar] public uint id;
    [SyncVar] public string playerName = "";
    public PlayerData playerData = new PlayerData();

    UIManager uiManager;
    [SerializeField] Image PanelImage;
    [SerializeField] Text TextNickname;
    [SerializeField] Text TextAssociation;
    [SerializeField] Text TextOccupation;
    [SerializeField] Text TextObjectCount;
    [SerializeField] Slider Slider;
    [SerializeField] GameObject DesPanel;
    [SerializeField] Text DesText;
    GameObject HandCardPanel;
    GameObject ShowCardPanel;
    Text TextMessage;
    float SliderTime = 0;
    public List<uint> playerSets;
    public ClientState clientState;
    int deckObjectCount = 0;

    CardID selectCard;
    GamePlayer selectPlayer;
    TradeData tradeData;
    FightData fightData;

    void Start()
    {
        this.id = this.netId;
        GameManager.Instance.AddGamePlayer(this);

        if (isLocalPlayer)
        {
            cmdCall.Add(ServerCommand.PlayerSet, PlayerSetCmd);
            cmdCall.Add(ServerCommand.Licensing, LicensingCmd);
            cmdCall.Add(ServerCommand.LicensingBack, LicensingBackCmd);
            cmdCall.Add(ServerCommand.LicensingAssociation, LicensingAssociationCmd);
            cmdCall.Add(ServerCommand.LicensingOccupation, LicensingOccupationCmd);
            cmdCall.Add(ServerCommand.AssociationPublic, AssociationPublicCmd);
            cmdCall.Add(ServerCommand.SyncDeckObjectCount, SyncDeckObjectCountCmd);
            cmdCall.Add(ServerCommand.SyncPlayerHandCount, SyncPlayerHandCountCmd);
            cmdCall.Add(ServerCommand.OccupationPublic, OccupationPublicCmd);

            cmdCall.Add(ServerCommand.RoundStart, RoundStartCmd);

            cmdCall.Add(ServerCommand.Trade, TradeCmd);
            cmdCall.Add(ServerCommand.TradeRequest, TradeRequestCmd);
            cmdCall.Add(ServerCommand.TradeBack, TradeBackCmd);
            cmdCall.Add(ServerCommand.TradeEvent, TradeEventCmd);
            cmdCall.Add(ServerCommand.ClientMessage, ClientMessageCmd);
            cmdCall.Add(ServerCommand.TradeMonocle, TradeMonocleCmd);
            cmdCall.Add(ServerCommand.TradeCoat, TradeCoatCmd);
            cmdCall.Add(ServerCommand.TradeTome, TradeTomeCmd);
            cmdCall.Add(ServerCommand.TradePrivilege, TradePrivilegeCmd);
            cmdCall.Add(ServerCommand.TradeSextant, TradeSextantCmd);
            cmdCall.Add(ServerCommand.TradeSextantEvent, TradeSextantEventCmd);
            cmdCall.Add(ServerCommand.TradeSextantBack, TradeSextantBackCmd);

            cmdCall.Add(ServerCommand.DeclareVictory, DeclareVictoryCmd);
            cmdCall.Add(ServerCommand.DeclareVictorySelectPlayer, DeclareVictorySelectPlayerCmd);
            cmdCall.Add(ServerCommand.DeclareVictoryMessage, DeclareVictoryMessageCmd);

            cmdCall.Add(ServerCommand.Diplomat, DiplomatCmd);
            cmdCall.Add(ServerCommand.DiplomatSelect, DiplomatSelectCmd);
            cmdCall.Add(ServerCommand.DiplomatConfirm, DiplomatConfirmCmd);

            cmdCall.Add(ServerCommand.Clairvoyant, ClairvoyantCmd);
            cmdCall.Add(ServerCommand.ClairvoyantData, ClairvoyantDataCmd);
            cmdCall.Add(ServerCommand.ClairvoyantFrist, ClairvoyantFristCmd);
            cmdCall.Add(ServerCommand.ClairvoyantEnd, ClairvoyantEndCmd);

            cmdCall.Add(ServerCommand.FightEvent, FightEventCmd);
            cmdCall.Add(ServerCommand.FightEventConfirm, FightEventConfirmCmd);
            cmdCall.Add(ServerCommand.UpdateFightData, UpdateFightDataCmd);
            cmdCall.Add(ServerCommand.Attack, AttackCmd);
            cmdCall.Add(ServerCommand.AttackTarget, AttackTargetCmd);
            cmdCall.Add(ServerCommand.FightPriest, FightPriestCmd);
            cmdCall.Add(ServerCommand.SyncVoteList, SyncVoteListCmd);
            cmdCall.Add(ServerCommand.FightVote, FightVoteCmd);
            cmdCall.Add(ServerCommand.FightHypnotist, FightHypnotistCmd);
            cmdCall.Add(ServerCommand.FightSupport, FightSupportCmd);
            cmdCall.Add(ServerCommand.FightPoisonRing, FightPoisonRingCmd);
            cmdCall.Add(ServerCommand.FightEnd, FightEndCmd);
            cmdCall.Add(ServerCommand.WinnerTakeData, WinnerTakeDataCmd);
            cmdCall.Add(ServerCommand.WinnerCheck, WinnerCheckCmd);
            cmdCall.Add(ServerCommand.WinnerTakeConfirm, WinnerTakeConfirmCmd);
            cmdCall.Add(ServerCommand.WinnerCheckConfirm, WinnerCheckConfirmCmd);
            cmdCall.Add(ServerCommand.WinnerTakeGiveBack, WinnerTakeGiveBackCmd);

            cmdCall.Add(ServerCommand.VoteConfirm, VoteConfirmCmd);
            cmdCall.Add(ServerCommand.Hypnotist, HypnotistCmd);
            cmdCall.Add(ServerCommand.Priest, PriestCmd);
            cmdCall.Add(ServerCommand.PriestEffect, PriestEffectCmd);
            cmdCall.Add(ServerCommand.Docter, DocterCmd);
            cmdCall.Add(ServerCommand.UsePoisonRing, UsePoisonRingCmd);

            GameManager.Instance.localGamePlayer = this;
            NetworkClient.RegisterHandler<MessageTo>(handleMessage);

            uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
            HandCardPanel = GameObject.Find("HandCardPanel");
            ShowCardPanel = GameObject.Find("ShowCardPanel");
            TextMessage = GameObject.Find("TextMessage").GetComponent<Text>();

            SendClientReady();
        }
        SetPlayerName(GameManager.Instance.localPlayer.playerName);
        DesPanel.SetActive(false);
    }

    void UsePoisonRingCmd(MessageTo msg)
    {
        clientState = ClientState.AttackWait;
        TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 使用了毒戒指";
        uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 使用了毒戒指");
        ControllerSwitch();
    }

    void DocterCmd(MessageTo msg)
    {
        clientState = ClientState.WinnerCheck;
        TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID) + " 使用了医生, 争斗被打断";
        uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID) + " 使用了医生, 争斗被打断");
        ControllerSwitch();
    }

    void WinnerCheckCmd(MessageTo msg)
    {
        clientState = ClientState.WinnerCheck;
        for (int i = 0; i < msg.data.Length; i++)
        {
            CardID _thisCard = (CardID)msg.data[i];
            AddCard(_thisCard, false, true);
        }

        GamePlayer gamePlayer = GameManager.Instance.GetGamePlayer(fightData.loser);
        gamePlayer.playerData.association = (CardID)msg.data[0];
        gamePlayer.TextAssociation.text = new CardData((CardID)msg.data[0]).cardName;
        if (gamePlayer.playerData.association == CardID.TheBrotherhoodOfTrueLies)
        {
            gamePlayer.TextAssociation.color = new Color(244f / 255f, 66f / 255f, 54f / 255f, 255f / 255f);
        }
        else
        {
            gamePlayer.TextAssociation.color = new Color(33f / 255f, 149f / 255f, 243f / 255f, 255f / 255f);
        }

        gamePlayer.playerData.occupation = (CardID)msg.data[1];
        gamePlayer.TextOccupation.text = new CardData((CardID)msg.data[1]).cardName;
        gamePlayer.TextAssociation.color = new Color(244f / 255f, 66f / 255f, 54f / 255f, 255f / 255f);

        ControllerSwitch();
    }

    void WinnerTakeGiveBackCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            clientState = ClientState.WinnerGiveBack;
            TextMessage.text = "对方没有道具了, 选择一张交回的道具";
        }
        else
        {
            clientState = ClientState.AttackWait;
            TextMessage.text = "等待胜利方交还一张道具";
        }
        ControllerSwitch();
    }

    void WinnerCheckConfirmCmd(MessageTo msg)
    {
        clientState = ClientState.AttackWait;
        ClearShowCard();
        ControllerSwitch();
    }

    void WinnerTakeConfirmCmd(MessageTo msg)
    {
        clientState = ClientState.AttackWait;
        ClearShowCard();
        ControllerSwitch();
    }

    void WinnerTakeDataCmd(MessageTo msg)
    {
        clientState = ClientState.WinnerTake;
        for (int i = 0; i < msg.data.Length; i++)
        {
            CardID _thisCard = (CardID)msg.data[i];
            AddCard(_thisCard, false, true);
        }
        ControllerSwitch();
    }

    void SyncVoteListCmd(MessageTo msg)
    {
        fightData.PlayerVoteList = msg.data;
    }

    void FightEndCmd(MessageTo msg)
    {
        if (msg.data.Length == 2)
        {
            fightData.winner = msg.data[0];
            fightData.loser = msg.data[1];

            if (fightData.winner == id)
            {
                clientState = ClientState.Winner;
            }
            else
            {
                clientState = ClientState.AttackWait;
            }
            TextMessage.text = GameManager.Instance.GetGamePlayer(fightData.winner).playerName + " 获胜, 等待选择";
            uiManager.AddLog(GameManager.Instance.GetGamePlayer(fightData.winner).playerName + " 获胜, 等待选择");
        }
        ControllerSwitch();
    }

    void FightPoisonRingCmd(MessageTo msg)
    {
        if (GameManager.Instance.localGamePlayer.playerData.cardList.Contains(CardID.PoisonRing))
        {
            if (fightData.player1 == id || fightData.player2 == id)
            {
                clientState = ClientState.PoisonRing;
            }
        }
        if (clientState != ClientState.PoisonRing)
        {
            clientState = ClientState.AttackWait;
        }
        TextMessage.text = "正在确认 毒戒指";
        uiManager.AddLog("正在确认 毒戒指");
        ControllerSwitch();
    }

    void PriestEffectCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            clientState = ClientState.Payment;
            TextMessage.text = "你需要交出一张牌";
            uiManager.AddLog("你需要交出一张牌");
        }
        else
        {
            clientState = ClientState.AttackWait;
            TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 需要交出一张牌";
            uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 需要交出一张牌");
        }
        ControllerSwitch();
    }

    void PriestCmd(MessageTo msg)
    {
        clientState = ClientState.AttackWait;
        TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 使用牧师, 争斗结束";
        uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 使用牧师, 争斗结束");
        ControllerSwitch();
    }

    void FightPriestCmd(MessageTo msg)
    {
        if (GameManager.Instance.localGamePlayer.playerData.occupation == CardID.Priest)
        {
            if (!GameManager.Instance.localGamePlayer.playerData.occupationIsPublic)
            {
                if (fightData.player1 != id)
                {
                    clientState = ClientState.Priest;
                }
            }
        }
        if (clientState != ClientState.Priest)
        {
            clientState = ClientState.AttackWait;
        }
        TextMessage.text = "正在确认 牧师";
        uiManager.AddLog("正在确认 牧师");
        ControllerSwitch();
    }

    void FightSupportCmd(MessageTo msg)
    {
        if (((IList)fightData.PlayerVoteList).Contains(id) || fightData.player1 == id || fightData.player2 == id)
        {
            clientState = ClientState.Support;
            TextMessage.text = "请选择要使用的道具或职业";
            uiManager.AddLog("请选择要使用的道具或职业");
        }
        else
        {
            clientState = ClientState.AttackWait;
            TextMessage.text = "等待其他玩家操作";
            uiManager.AddLog("等待其他玩家操作");
        }
        ControllerSwitch();
    }

    void VoteConfirmCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            clientState = ClientState.AttackWait;
            uiManager.ReActive();
            uiManager.PanelFightActive();
        }
    }

    void HypnotistCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            clientState = ClientState.HypnotistTarget;
            TextMessage.text = "你使用催眠师, 请选择玩家";
            uiManager.AddLog("你使用催眠师, 请选择玩家");
        }
        else
        {
            clientState = ClientState.AttackWait;
            TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 使用催眠师";
            uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 使用催眠师");
        }
        ControllerSwitch();
    }

    void FightHypnotistCmd(MessageTo msg)
    {
        if (GameManager.Instance.localGamePlayer.playerData.occupation == CardID.Hypnotist)
        {
            if (!GameManager.Instance.localGamePlayer.playerData.occupationIsPublic)
            {
                if (fightData.player1 == id)
                {
                    clientState = ClientState.Hypnotist;
                }
            }
        }
        if (clientState != ClientState.Hypnotist)
        {
            clientState = ClientState.AttackWait;
        }
        TextMessage.text = "正在确认 催眠师";
        uiManager.AddLog("正在确认 催眠师");
        ControllerSwitch();
    }

    void FightVoteCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            clientState = ClientState.Vote;
            TextMessage.text = "轮到你投票了";
            uiManager.AddLog("轮到你投票了");
        }
        else
        {
            clientState = ClientState.AttackWait;
            TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 该投票了";
            uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 该投票了");
        }
        ControllerSwitch();
    }

    void FightEventConfirmCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            uiManager.ReActiveWithoutFightPanel();
            TextMessage.text = "等待其他玩家";
        }
    }

    void ClairvoyantEndCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            clientState = ClientState.RoundStart;
            TextMessage.text = "透视者发动结束, 发起争斗, 交易, 或宣告胜利";
            selectCard = CardID.Null;
            uiManager.AddLog("透视者发动结束, 你的回合");
        }
        else
        {
            clientState = ClientState.Wait;
            TextMessage.text = "透视者发动结束, " + GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 的回合";
            uiManager.AddLog("透视者发动结束, " + GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 的回合");
        }
        ControllerSwitch();
        ReSetAllPlayerPanel();
        GameManager.Instance.GetGamePlayer(msg.playerID).SetPlayerPanelActive();
    }

    void ClairvoyantFristCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            clientState = ClientState.ClairvoyantFrist;
            TextMessage.text = "选择放在顶部的牌";
            uiManager.AddLog("选择放在顶部的牌");
        }
        else
        {
            clientState = ClientState.Wait;
            TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 发动了 透视者, 等待选择第二张牌";
            uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 发动了 透视者, 等待选择第二张牌");
        }
        ControllerSwitch();
    }

    void ClairvoyantDataCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            for (int i = 0; i < msg.data.Length; i++)
            {
                CardID _thisCard = (CardID)msg.data[i];
                AddCard(_thisCard, false, true);
            }
        }
    }

    void ClairvoyantCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            if (msg.data[0] >= 2)
            {
                clientState = ClientState.ClairvoyantSecond;
                TextMessage.text = "选择放在第二张的牌";
                uiManager.AddLog("选择放在第二张的牌");
            }
            else if (msg.data[0] == 1)
            {
                clientState = ClientState.ClairvoyantFrist;
                TextMessage.text = "选择放在第一张的牌";
                uiManager.AddLog("选择放在第一张的牌");
            }
            else
            {
                clientState = ClientState.Wait;
                SendPlayerClairvoyantFrist(CardID.Null);
            }
        }
        else
        {
            clientState = ClientState.Wait;
            TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 发动了 透视者, 等待选牌";
            uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 发动了 透视者, 等待选牌");
        }
        ControllerSwitch();
    }

    void DiplomatConfirmCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            clientState = ClientState.DiplomatConfirm;
            TextMessage.text = "对方没有你要的牌";
            uiManager.AddLog("对方没有你要的牌");
            string tmp = "对方持有的道具: ";
            for (int i = 0; i < msg.data.Length; i++)
            {
                CardID _thisCard = (CardID)msg.data[i];
                AddCard(_thisCard, false);
                tmp += new CardData(_thisCard).cardName + " ";
            }
            uiManager.AddLog(tmp);
        }
        else
        {
            clientState = ClientState.Wait;
            TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 正在因 外交官 效果检查手牌";
            uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 正在因 外交官 效果检查手牌");
        }
        ControllerSwitch();
    }


    void DiplomatSelectCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            Client_TradeAction();
        }
        else
        {
            clientState = ClientState.Wait;
            TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 选择了 " + new CardData((CardID)msg.data[0]).cardName;
            uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 选择了 " + new CardData((CardID)msg.data[0]).cardName);
        }
        tradeData = new TradeData(msg.playerID);
        tradeData.WanttedCard = (CardID)msg.data[0];
        ControllerSwitch();
        GameManager.Instance.GetGamePlayer(msg.playerID).SetPlayerPanelActive();
    }

    void DiplomatCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            clientState = ClientState.Diplomat;
        }
        else
        {
            clientState = ClientState.Wait;
            TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 发动了 外交官, 等待选牌";
            uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 发动了 外交官, 等待选牌");
        }
        ControllerSwitch();
    }

    void AttackTargetCmd(MessageTo msg)
    {
        fightData.SelectPlayer(msg.data[0], playerSets);
        if (fightData.player1 == id)
        {
            clientState = ClientState.AttackAndDefend;
            TextMessage.text = "你发起攻击, 请选择要使用的牌";
            uiManager.AddLog("攻击了 " + GameManager.Instance.GetGamePlayer(fightData.player2).playerName);

        }
        else if (fightData.player2 == id)
        {
            clientState = ClientState.AttackAndDefend;
            TextMessage.text = "你被攻击了, 请选择要使用的牌";
            uiManager.AddLog("被 " + GameManager.Instance.GetGamePlayer(fightData.player1).playerName + "攻击了");
        }
        else
        {
            clientState = ClientState.AttackWait;
            TextMessage.text = GameManager.Instance.GetGamePlayer(fightData.player1).playerName + " 攻击了 " + GameManager.Instance.GetGamePlayer(fightData.player2).playerName + " 等待双方发动职业和道具";
            uiManager.AddLog(GameManager.Instance.GetGamePlayer(fightData.player1).playerName + " 攻击了 " + GameManager.Instance.GetGamePlayer(fightData.player2).playerName + " 等待双方发动职业和道具");
        }
        GameManager.Instance.GetGamePlayer(fightData.player2).SetPlayerPanelWait();
        ControllerSwitch();

    }

    void UpdateFightDataCmd(MessageTo msg)
    {
        if (msg.data.Length == 2)
        {
            uiManager.UpdateFightData((int)msg.data[0], (int)msg.data[1]);
        }
    }

    void FightEventCmd(MessageTo msg)
    {
        if (clientState == ClientState.AttackWait)
        {
            DOVirtual.DelayedCall(2.0f, () =>
            {
                FightEvent();
            });
        }
        ControllerSwitch();
    }

    void FightEvent()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.FightEvent;
        NetworkClient.Send(msg);
    }

    void AttackCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            clientState = ClientState.AttackTarget;
            TextMessage.text = "请选择攻击目标";
            uiManager.AddLog("你选择攻击");
        }
        else
        {
            clientState = ClientState.AttackWait;
            uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 选择攻击");
        }
        fightData = new FightData(msg.playerID);
        ControllerSwitch();
    }

    void DeclareVictoryMessageCmd(MessageTo msg)
    {
        clientState = ClientState.GameOver;
        TextMessage.text = "获胜阵营: " + new CardData((CardID)msg.data[0]).cardName + "\r\n" +
            GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 宣告时, 阵营持有 " + msg.data[1].ToString() + " 个胜利道具";

        uiManager.AddLog("获胜阵营: " + new CardData((CardID)msg.data[0]).cardName + "\r\n" +
            GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 宣告时, 阵营持有 " + msg.data[1].ToString() + " 个胜利道具");
        ControllerSwitch();
    }

    void AssociationPublicCmd(MessageTo msg)
    {
        GamePlayer gamePlayer = GameManager.Instance.GetGamePlayer(msg.playerID);
        gamePlayer.playerData.association = (CardID)msg.data[0];
        gamePlayer.TextAssociation.text = new CardData((CardID)msg.data[0]).cardName;
        if (gamePlayer.playerData.association == CardID.TheBrotherhoodOfTrueLies)
        {
            gamePlayer.TextAssociation.color = new Color(244f / 255f, 66f / 255f, 54f / 255f, 255f / 255f);
        }
        else
        {
            gamePlayer.TextAssociation.color = new Color(33f / 255f, 149f / 255f, 243f / 255f, 255f / 255f);
        }
    }

    void DeclareVictorySelectPlayerCmd(MessageTo msg)
    {
        TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 选择了 " + GameManager.Instance.GetGamePlayer(msg.data[0]).playerName;
        GameManager.Instance.GetGamePlayer(msg.data[0]).SetPlayerPanelActive();
        uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 选择了 " + GameManager.Instance.GetGamePlayer(msg.data[0]).playerName);
    }

    void DeclareVictoryCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            clientState = ClientState.DeclareVictory;
            TextMessage.text = "请选择你的队友";
        }
        uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 宣告胜利");
        ControllerSwitch();
    }

    void OccupationPublicCmd(MessageTo msg)
    {
        if (msg.data[0] == 0)
        {
            GameManager.Instance.GetGamePlayer(msg.playerID).playerData.occupationIsPublic = false;
            if (msg.playerID != id)
            {
                GameManager.Instance.GetGamePlayer(msg.playerID).TextOccupation.text = "未知职业";
                GameManager.Instance.GetGamePlayer(msg.playerID).playerData.occupation = CardID.Null;
            }
            GameManager.Instance.GetGamePlayer(msg.playerID).TextOccupation.color = new Color(244f / 255f, 66f / 255f, 54f / 255f, 255f / 255f);
        }
        else
        {
            GameManager.Instance.GetGamePlayer(msg.playerID).playerData.occupationIsPublic = true;
            GameManager.Instance.GetGamePlayer(msg.playerID).playerData.occupation = (CardID)msg.data[0];
            GameManager.Instance.GetGamePlayer(msg.playerID).TextOccupation.text = new CardData((CardID)msg.data[0]).cardName;
            GameManager.Instance.GetGamePlayer(msg.playerID).TextOccupation.color = new Color(33f / 255f, 149f / 255f, 243f / 255f, 255f / 255f);
        }
    }

    void Awake()
    {
        transform.SetParent(GameObject.Find("GamePlayerGroup").transform);
    }

    void TradeSextantBackCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            clientState = ClientState.TradeSextantBack;
            TextMessage.text = "请选择一张牌";
        }
        else
        {
            TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 正在选择交出的牌";
        }
        ControllerSwitch();
        // GameManager.Instance.GetGamePlayer(msg.playerID).SetPlayerPanelWait();
    }

    void TradeSextantCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            clientState = ClientState.TradeSextant;
        }
        ControllerSwitch();
    }

    void TradeTomeCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            clientState = ClientState.TradeTome;
        }
        ControllerSwitch();
    }

    void TradeCoatCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            clientState = ClientState.TradeCoat;
            for (int i = 0; i < msg.data.Length; i++)
            {
                AddCard((CardID)msg.data[i], false);
            }
        }
        ControllerSwitch();
    }

    void TradePrivilegeCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            clientState = ClientState.TradePrivilege;
            for (int i = 0; i < msg.data.Length; i++)
            {
                AddCard((CardID)msg.data[i], false);
            }
        }
        ControllerSwitch();
    }

    void TradeMonocleCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            clientState = ClientState.TradeMonocle;
            AddCard((CardID)msg.data[0], false);
            GamePlayer gamePlayer = GameManager.Instance.GetGamePlayer(msg.playerID == tradeData.player1 ? tradeData.player2 : tradeData.player1);

            gamePlayer.TextAssociation.text = new CardData((CardID)msg.data[0]).cardName;
            gamePlayer.playerData.association = (CardID)msg.data[0];
            if (gamePlayer.playerData.association == CardID.TheBrotherhoodOfTrueLies)
            {
                gamePlayer.TextAssociation.color = new Color(244f / 255f, 66f / 255f, 54f / 255f, 255f / 255f);
            }
            else
            {
                gamePlayer.TextAssociation.color = new Color(33f / 255f, 149f / 255f, 243f / 255f, 255f / 255f);
            }
        }
        ControllerSwitch();
    }

    void ClientMessageCmd(MessageTo msg)
    {
        switch ((ClientMessage)msg.data[0])
        {
            case ClientMessage.SecretBag:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 交易 神秘手提袋, 抽取一张道具";
                uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 交易 神秘手提袋, 抽取一张道具");
                break;
            case ClientMessage.CantTrade:
                if (tradeData.WanttedCard != CardID.Null)
                {
                    TextMessage.text = "不能交易这张卡, 因为指定了 " + new CardData(tradeData.WanttedCard).cardName;
                }
                else
                {
                    TextMessage.text = "不能交易这张卡";
                }
                break;
            case ClientMessage.Monocle:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 交易 单片眼镜, 正在查看对方阵营";
                uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 交易 单片眼镜");
                break;
            case ClientMessage.MonocleEnd:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 已经查看完毕";
                break;
            case ClientMessage.Coat:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 交易 大衣, 正在查看职业牌堆";
                break;
            case ClientMessage.CoatTrue:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 换了一张职业牌";
                uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 交易 大衣, 换了一张职业牌");
                break;
            case ClientMessage.CoatFalse:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 没有更换职业牌";
                uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 交易 大衣, 没有更换职业牌");
                break;
            case ClientMessage.CoatCant:
                TextMessage.text = "数据错误, 你选择了一张不应当被选的牌";
                break;
            case ClientMessage.Tome:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 交易 典籍, 正在等待选择是否交换职业";
                break;
            case ClientMessage.TomeTrue:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 互换了职业牌";
                uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 交易 典籍, 互换了职业牌");
                break;
            case ClientMessage.TomeFalse:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 没有交换职业牌";
                uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 交易 典籍, 没有交换职业牌");
                break;
            case ClientMessage.Privilege:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 交易 特权状, 正在查看对方道具";
                uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 交易 特权状");
                break;
            case ClientMessage.PrivilegeEnd:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 已经查看完毕";
                break;
            case ClientMessage.Sextant:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 交易 六分仪, 正在选择方向";
                break;
            case ClientMessage.SextantLeft:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 选择 左手边";
                uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 交易 六分仪, 选择左手边开始");
                break;
            case ClientMessage.SextantRight:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 选择 右手边";
                uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 交易 六分仪, 选择右手边开始");
                break;
            case ClientMessage.SextantCant:
                TextMessage.text = "数据错误, 你选择了一张不应当被选的牌";
                break;
            case ClientMessage.SextantNext:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 选择完毕";
                break;
            case ClientMessage.CantRefuse:
                if (tradeData.WanttedCard != CardID.Null)
                {
                    TextMessage.text = "不能拒绝这场交易, 因为对方指定了 " + new CardData(tradeData.WanttedCard).cardName;
                }
                else
                {
                    TextMessage.text = "不能拒绝这场交易";
                }

                break;
            case ClientMessage.CantDeclarePersonalVictory:
                TextMessage.text = "条件不符";
                break;
            case ClientMessage.DeclarePersonalVictory:
                clientState = ClientState.GameOver;
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 宣告了个人胜利, 游戏结束";
                uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 宣告了个人胜利");
                uiManager.ReActive();
                ControllerSwitch();
                break;
            case ClientMessage.DeclareVictory:
                clientState = ClientState.Wait;
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 宣告胜利, 等待指出队友";
                break;
            case ClientMessage.DeclareVictorySelected:
                TextMessage.text = "已经选择过他了";
                break;
            case ClientMessage.CantTradeSecretBag:
                TextMessage.text = "你不能在只有手提袋时交换手提袋";
                break;
            case ClientMessage.ClairvoyantCant:
                TextMessage.text = "无法选择这张牌";
                break;
            case ClientMessage.OccupationCant:
                TextMessage.text = "无法使用职业 或 已经用过了";
                break;
            case ClientMessage.Duelist:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 使用决斗者, 本场战斗没有支援";
                break;
            case ClientMessage.Vote:
                TextMessage.text = "等待 " + GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 投票";
                break;
            case ClientMessage.VoteEnd:
                TextMessage.text = "所有人都投过票了";
                break;
            case ClientMessage.VoteAttack:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 投了攻击票";
                break;
            case ClientMessage.VoteDefend:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 投了防守票";
                break;
            case ClientMessage.HypnotistTarget:
                clientState = ClientState.AttackWait;
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 催眠了 " + GameManager.Instance.GetGamePlayer(msg.data[1]).playerName;
                break;
            case ClientMessage.HypnotistTargetCant:
                TextMessage.text = "不能选择斗争双方";
                break;
            case ClientMessage.Priest:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 使用了 牧师";
                break;
            case ClientMessage.Hypnotist:
                break;
            case ClientMessage.Hypnotized:
                TextMessage.text = "你被催眠了";
                break;
            case ClientMessage.Draw:
                TextMessage.text = "平局";
                break;
            case ClientMessage.WinnerTake:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 选择 抢夺道具";
                break;
            case ClientMessage.WinnerCheck:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 选择 查看阵营和职业";
                break;
            case ClientMessage.FightTakeEnd:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 拿了一张牌";
                break;
            case ClientMessage.WinnerTakeGiveBack:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 需要交回一张牌";
                break;
            case ClientMessage.GiveBackEnd:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 交回了一张牌, 战斗结算结束了";
                break;
            case ClientMessage.WinnerCheckEnd:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 查看了阵营和职业";
                break;
            case ClientMessage.UseCard:
                TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 使用 " + new CardData((CardID)msg.data[1]).cardName;
                break;
            case ClientMessage.UsedCard:
                TextMessage.text = "这张牌无法使用 或 已经用过了";
                break;
        }
    }

    void TradeSextantEventCmd(MessageTo msg)
    {
        clientState = ClientState.Wait;
        ControllerSwitch();
        DOVirtual.DelayedCall(2.0f, () =>
        {
            TradeSextantEvent();
        });
    }

    void TradeEventCmd(MessageTo msg)
    {
        clientState = ClientState.Wait;
        ControllerSwitch();
        DOVirtual.DelayedCall(2.0f, () =>
        {
            TradeEvent();
        });
    }

    void TradeBackCmd(MessageTo msg)
    {
        clientState = ClientState.Wait;
        if (msg.data[0] == 1)
        {
            // 同意交易
            TextMessage.text = GameManager.Instance.GetGamePlayer(tradeData.player2).playerName + " 同意了 " + GameManager.Instance.GetGamePlayer(tradeData.player1).playerName + " 的交易请求";
            uiManager.AddLog(GameManager.Instance.GetGamePlayer(tradeData.player2).playerName + " 同意了 " + GameManager.Instance.GetGamePlayer(tradeData.player1).playerName + " 的交易请求");
        }
        else
        {
            // 拒绝交易
            TextMessage.text = GameManager.Instance.GetGamePlayer(tradeData.player2).playerName + " 拒绝了 " + GameManager.Instance.GetGamePlayer(tradeData.player1).playerName + " 的交易请求";
            uiManager.AddLog(GameManager.Instance.GetGamePlayer(tradeData.player2).playerName + " 拒绝了 " + GameManager.Instance.GetGamePlayer(tradeData.player1).playerName + " 的交易请求");
        }
        ClearShowCard();
        ControllerSwitch();
    }

    void TradeSextantEvent()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.TradeSextantEvent;
        NetworkClient.Send(msg);
    }

    void TradeEvent()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.TradeEvent;
        NetworkClient.Send(msg);
    }

    void ClearShowCard()
    {
        for (int i = 0; i < ShowCardPanel.transform.childCount; i++)
        {
            Destroy(ShowCardPanel.transform.GetChild(i).gameObject);
        }
    }

    void TradeRequestCmd(MessageTo msg)
    {
        uiManager.ReActive();
        tradeData.player2 = msg.playerID;
        if (msg.playerID == id)
        {
            clientState = ClientState.TradeRequest;
            TextMessage.text = GameManager.Instance.GetGamePlayer(tradeData.player1).playerName + " 请求交易";
            AddCard((CardID)msg.data[0], false);
            tradeData.card1 = (CardID)msg.data[0];
            uiManager.AddLog(GameManager.Instance.GetGamePlayer(tradeData.player1).playerName + " 请求交易 " + new CardData((CardID)msg.data[0]).cardName);
        }
        else
        {
            clientState = ClientState.Wait;
            TextMessage.text = GameManager.Instance.GetGamePlayer(tradeData.player1).playerName + " 等待 " + GameManager.Instance.GetGamePlayer(tradeData.player2).playerName + " 是否交易";
            uiManager.AddLog(GameManager.Instance.GetGamePlayer(tradeData.player1).playerName + " 等待 " + GameManager.Instance.GetGamePlayer(tradeData.player2).playerName + " 是否交易");
        }
        GameManager.Instance.GetGamePlayer(tradeData.player2).SetPlayerPanelWait();
        ControllerSwitch();
    }

    void TradeCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            Client_TradeAction();
        }
        else
        {
            clientState = ClientState.Wait;
            TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 选择交易";
            uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 选择交易");
        }
        tradeData = new TradeData(msg.playerID);
        ControllerSwitch();
        GameManager.Instance.GetGamePlayer(msg.playerID).SetPlayerPanelActive();
    }

    void Client_TradeAction()
    {
        GameManager.Instance.localGamePlayer.clientState = ClientState.Trade;
        TextMessage.text = "请选择一张牌交易";
        uiManager.AddLog("你选择交易");
    }

    void RoundStartCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            clientState = ClientState.RoundStart;
            TextMessage.text = "发起争斗, 交易, 或宣告胜利";
            selectCard = CardID.Null;
            uiManager.AddLog("你的回合");
        }
        else
        {
            clientState = ClientState.Wait;
            TextMessage.text = GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 的回合";
            uiManager.AddLog(GameManager.Instance.GetGamePlayer(msg.playerID).playerName + " 的回合");
        }
        ControllerSwitch();
        ReSetAllPlayerPanel();
        GameManager.Instance.GetGamePlayer(msg.playerID).SetPlayerPanelActive();
    }

    void ReSetAllPlayerPanel()
    {
        foreach (GamePlayer gamePlayer in GameManager.Instance.GetAllGamePlayer())
        {
            gamePlayer.ReSetPlayerPanel();
        }
    }

    void ControllerSwitch()
    {
        uiManager.ReActive();
        switch (GameManager.Instance.localGamePlayer.clientState)
        {
            case ClientState.Wait:
                ClearShowCard();
                break;
            case ClientState.RoundStart:
                ClearShowCard();
                uiManager.RoundStartActive();
                bool canVictory = true;
                for (int i = 0; i < HandCardPanel.transform.childCount; i++)
                {
                    CardID _thisCard = HandCardPanel.transform.GetChild(i).GetComponent<Card>().cardData.cardID;
                    if (_thisCard == CardID.BlackPeart)
                    {
                        uiManager.RoundStartActive(false);
                        canVictory = false;
                    }
                }
                if ((playerData.occupation == CardID.Clairvoyant || playerData.occupation == CardID.Diplomat) && playerData.occupationIsPublic == false)
                {
                    uiManager.OccupationActive();
                }
                if (canVictory)
                {
                    int Count = 0;
                    bool canPersonalVictory = false;
                    for (int i = 0; i < HandCardPanel.transform.childCount; i++)
                    {
                        CardID _thisCard = HandCardPanel.transform.GetChild(i).GetComponent<Card>().cardData.cardID;
                        if (_thisCard == CardID.TheCoatOfArmorOfTheLoge)
                        {
                            canPersonalVictory = true;
                        }
                        if (playerData.association == CardID.TheBrotherhoodOfTrueLies)
                        {
                            if ((deckObjectCount == 0 && _thisCard == CardID.SecretBagGoblet) || _thisCard == CardID.Goblet)
                            {
                                Count++;
                            }
                        }
                        else if (playerData.association == CardID.TheOrderOFOpenSecrets)
                        {
                            if ((deckObjectCount == 0 && _thisCard == CardID.SecretBagKey) || _thisCard == CardID.Key)
                            {
                                Count++;
                            }
                        }

                    }
                    if (canPersonalVictory && Count >= 3)
                    {
                        uiManager.PersonalActive();
                    }
                }
                break;
            case ClientState.Trade:
                break;
            case ClientState.TradeRequest:
                break;
            case ClientState.TradeMonocle:
                break;
            case ClientState.TradeCoat:
                uiManager.CancelActive();
                break;
            case ClientState.TradeTome:
                uiManager.TradeTomeActive();
                break;
            case ClientState.TradePrivilege:
                break;
            case ClientState.TradeSextant:
                uiManager.TradeSextantActive();
                break;
            case ClientState.TradeSextantBack:
                break;
            case ClientState.GameOver:
                uiManager.GameOverActive();
                break;
            case ClientState.DeclareVictory:
                uiManager.EndActive();
                break;
            case ClientState.Diplomat:
                uiManager.AllCardActive();
                break;
            case ClientState.DiplomatConfirm:
                break;
            case ClientState.ClairvoyantFrist:
                break;
            case ClientState.ClairvoyantSecond:
                break;
            case ClientState.AttackWait:
                uiManager.PanelFightActive();
                break;
            case ClientState.AttackTarget:
                break;
            case ClientState.AttackAndDefend:
                uiManager.PanelFightActive();
                uiManager.CancelActive();
                if (fightData.player1 == GameManager.Instance.localGamePlayer.id)
                {
                    // 进攻方
                    if (GameManager.Instance.localGamePlayer.playerData.occupation == CardID.Duelist ||
                        GameManager.Instance.localGamePlayer.playerData.occupation == CardID.Thug
                        )
                    {
                        if (!GameManager.Instance.localGamePlayer.playerData.occupationIsPublic)
                        {
                            uiManager.OccupationActive();
                        }
                    }
                }
                else if (fightData.player2 == GameManager.Instance.localGamePlayer.id)
                {
                    // 防守方
                    if (GameManager.Instance.localGamePlayer.playerData.occupation == CardID.Duelist ||
                        GameManager.Instance.localGamePlayer.playerData.occupation == CardID.GrandMaster
                        )
                    {
                        if (!GameManager.Instance.localGamePlayer.playerData.occupationIsPublic)
                        {
                            uiManager.OccupationActive();
                        }
                    }
                }
                else
                {
                    if (GameManager.Instance.localGamePlayer.playerData.occupation == CardID.PoisonMixer)
                    {
                        if (!GameManager.Instance.localGamePlayer.playerData.occupationIsPublic)
                        {
                            uiManager.OccupationActive();
                        }
                    }
                }
                break;
            case ClientState.Vote:
                uiManager.SupportActive();
                uiManager.PanelFightActive();
                break;
            case ClientState.Priest:
                uiManager.PanelFightActive();
                uiManager.OccupationActive();
                uiManager.CancelActive();
                break;
            case ClientState.Hypnotist:
                uiManager.PanelFightActive();
                uiManager.OccupationActive();
                uiManager.CancelActive();
                break;
            case ClientState.HypnotistTarget:
                uiManager.PanelFightActive();
                break;
            case ClientState.Support:
                uiManager.PanelFightActive();
                uiManager.CancelActive();
                if (fightData.player1 == GameManager.Instance.localGamePlayer.id)
                {
                    // 进攻方
                    if (GameManager.Instance.localGamePlayer.playerData.occupation == CardID.Duelist ||
                        GameManager.Instance.localGamePlayer.playerData.occupation == CardID.Thug
                        )
                    {
                        if (!GameManager.Instance.localGamePlayer.playerData.occupationIsPublic)
                        {
                            uiManager.OccupationActive();
                        }
                    }
                }
                else if (fightData.player2 == GameManager.Instance.localGamePlayer.id)
                {
                    // 防守方
                    if (GameManager.Instance.localGamePlayer.playerData.occupation == CardID.Duelist ||
                        GameManager.Instance.localGamePlayer.playerData.occupation == CardID.GrandMaster
                        )
                    {
                        if (!GameManager.Instance.localGamePlayer.playerData.occupationIsPublic)
                        {
                            uiManager.OccupationActive();
                        }
                    }
                }
                else
                {
                    // 支援方
                    if (GameManager.Instance.localGamePlayer.playerData.occupation == CardID.Doctor ||
                        GameManager.Instance.localGamePlayer.playerData.occupation == CardID.Bodyguard
                        )
                    {
                        if (!GameManager.Instance.localGamePlayer.playerData.occupationIsPublic)
                        {
                            uiManager.OccupationActive();
                        }
                    }
                }

                break;
            case ClientState.PoisonRing:
                uiManager.PanelFightActive();
                uiManager.CancelActive();
                break;
            case ClientState.Payment:
                break;
            case ClientState.Winner:
                uiManager.PanelFightActive();
                uiManager.WinnerActive();
                break;
            case ClientState.Loser:
                break;
            case ClientState.WinnerTake:
                break;
            case ClientState.WinnerCheck:
                uiManager.PanelFightActive();
                break;
            case ClientState.WinnerGiveBack:
                break;
        }
    }

    void LicensingCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            playerData.cardList.Add((CardID)msg.data[0]);
            AddCard((CardID)msg.data[0], true);
        }
    }

    void AddCard(CardID _cardid, bool isHand = true, bool isUI = false)
    {
        GameObject card = Instantiate((GameObject)Resources.Load("Prefabs/Card"));

        if (isHand)
        {
            card.transform.SetParent(HandCardPanel.transform);
        }
        else
        {
            card.transform.SetParent(ShowCardPanel.transform);
        }
        card.GetComponent<Card>().InitCardData(new CardData(_cardid), isHand, isUI);
    }


    void LicensingBackCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            playerData.cardList.Remove((CardID)msg.data[0]);
            for (int i = 0; i < HandCardPanel.transform.childCount; i++)
            {
                if (HandCardPanel.transform.GetChild(i).GetComponent<Card>().cardData.cardID == (CardID)msg.data[0])
                {
                    DestroyImmediate(HandCardPanel.transform.GetChild(i).gameObject);
                    GameObject.Find("CardManager").GetComponent<HandCardManager>().HandCardAnimation();
                    break;
                }
            }
        }
    }

    void LicensingAssociationCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            playerData.association = (CardID)msg.data[0];
            TextAssociation.text = new CardData(playerData.association).cardName;
            if (playerData.association == CardID.TheBrotherhoodOfTrueLies)
            {
                TextAssociation.color = new Color(244f / 255f, 66f / 255f, 54f / 255f, 255f / 255f);
            }
            else
            {
                TextAssociation.color = new Color(33f / 255f, 149f / 255f, 243f / 255f, 255f / 255f);
            }
        }
    }

    void LicensingOccupationCmd(MessageTo msg)
    {
        if (msg.playerID == id)
        {
            playerData.occupation = (CardID)msg.data[0];
            TextOccupation.text = new CardData(playerData.occupation).cardName;
            TextOccupation.color = new Color(244f / 255f, 66f / 255f, 54f / 255f, 255f / 255f);
            ControllerSwitch();
        }
    }

    void SyncDeckObjectCountCmd(MessageTo msg)
    {
        deckObjectCount = (int)msg.data[0];
        uiManager.DeckObjectCount.text = deckObjectCount.ToString();
    }

    void SyncPlayerHandCountCmd(MessageTo msg)
    {
        GameManager.Instance.GetGamePlayer(msg.playerID).TextObjectCount.text = msg.data[0].ToString();
    }

    void handleMessage(NetworkConnection conn, MessageTo msg)
    {
        ServerCommand servercmd = (ServerCommand)msg.messageCommand;
        if (cmdCall.ContainsKey(servercmd))
        {
            cmdCall[servercmd](msg);
        }
        else
        {
            Debug.Log("CommandError: " + (ServerCommand)msg.messageCommand);
        }
    }

    void SendClientReady()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.ClientReady;
        NetworkClient.Send(msg);
    }

    void PlayerSetCmd(MessageTo msg)
    {
        if (playerSets.Count == 0)
        {
            for (int i = 0; i < msg.data.Length; i++)
            {
                playerSets.Add(msg.data[i]);
            }
            RowPlayerUI();
            uiManager.AddLog("游戏开始");
        }
    }

    void RowPlayerUI()
    {
        GameObject ControllerPanelPostion = GameObject.Find("ControllerPanelPostion");
        transform.localScale = new Vector2(0.7f, 0.7f);
        transform.DOMove(ControllerPanelPostion.transform.position, 0.3f);
        var tmpPlayerSets = getOthers(playerSets, id);

        int[] setList = new int[0] { };
        if (tmpPlayerSets.Count == 2)
        {
            setList = new int[2] { 2, 1 };
        }
        else if (tmpPlayerSets.Count == 3)
        {
            setList = new int[3] { 2, 3, 1 };
        }
        else if (tmpPlayerSets.Count == 4)
        {
            setList = new int[4] { 2, 3, 1, 4 };
        }
        else if (tmpPlayerSets.Count == 5)
        {
            setList = new int[5] { 5, 2, 3, 1, 4 };
        }
        else if (tmpPlayerSets.Count == 6)
        {
            setList = new int[6] { 5, 2, 3, 1, 4, 6 };
        }
        else if (tmpPlayerSets.Count == 7)
        {
            setList = new int[7] { 7, 5, 2, 3, 1, 4, 6 };
        }
        else if (tmpPlayerSets.Count == 8)
        {
            setList = new int[8] { 7, 5, 2, 3, 1, 4, 6, 8 };
        }
        else if (tmpPlayerSets.Count == 9)
        {
            setList = new int[9] { 9, 7, 5, 2, 3, 1, 4, 6, 8 };
        }

        for (int i = 0; i < tmpPlayerSets.Count; i++)
        {
            Vector2 pos = GameObject.Find("PlayerPanelPostion" + setList[i].ToString()).transform.position;
            GamePlayer tempPlayer = GameManager.Instance.allGamePlayers[tmpPlayerSets[i]];
            tempPlayer.transform.localScale = new Vector2(0.7f, 0.7f);
            tempPlayer.transform.DOMove(pos, 0.3f);
        }
    }

    List<uint> getOthers(List<uint> rowList, uint myNetid)
    {
        int myNum = rowList.IndexOf(myNetid);
        if (myNum < 0) return null;
        var result = rowList.GetRange(0, myNum);
        result.Reverse();
        int right = rowList.Count - 1 - myNum;
        if (right > 0)
        {
            var rlist = rowList.GetRange(myNum + 1, right);
            rlist.Reverse();
            result.AddRange(rlist);
        }
        return result;
    }

    void Update()
    {
        TextNickname.text = playerName;

        if (id == GameManager.Instance.localGamePlayer.id)
        {
            if (Input.GetMouseButtonDown(1))
            {
                switch (GameManager.Instance.localGamePlayer.clientState)
                {
                    case ClientState.Wait:
                        break;
                    case ClientState.RoundStart:
                        break;
                    case ClientState.Trade:
                        Client_TradeAction();
                        break;
                }

                ControllerSwitch();
            }
        }
    }

    [ContextMenu("角色卡等待回应")]
    void SetPlayerPanelWait()
    {
        PanelImage.color = new Color(255f / 255f, 193f / 255f, 7f / 255f, 255f / 255f);
    }

    [ContextMenu("角色卡激活")]
    void SetPlayerPanelActive()
    {
        PanelImage.color = new Color(139f / 255f, 195f / 255f, 74f / 255f, 100f / 255f);
    }

    [ContextMenu("角色卡重置")]
    void ReSetPlayerPanel()
    {
        PanelImage.color = new Color(53f / 255f, 53f / 255f, 53f / 255f, 100f / 255f);
    }

    public void SetPlayerName(string playerName)
    {
        CmdChangeName(playerName);
    }

    [Command]
    void CmdChangeName(string name)
    {
        playerName = name;
    }

    private void OnDestroy()
    {
        GameManager.Instance.RemoveGamePlayer(id);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.Instance.localGamePlayer.clientState == ClientState.Trade)
        {
            if (id == GameManager.Instance.localGamePlayer.id) return;
            GameManager.Instance.localGamePlayer.uiManager.ArrowSetTarget(transform);
            GameManager.Instance.localGamePlayer.uiManager.ArrowTargetLock(true);
        }
        else
        {
            if (playerData.occupation == CardID.Null) return;
            DesText.text = new CardData(playerData.occupation).cardDes;
            DesPanel.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.Instance.localGamePlayer.clientState == ClientState.Trade)
        {
            GameManager.Instance.localGamePlayer.uiManager.ArrowTargetLock(false);
        }
        else
        {
            DesPanel.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (id == GameManager.Instance.localGamePlayer.id) return;
        GameManager.Instance.localGamePlayer.onClick_Player(this);
    }

    void SendPlayerTrade()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.TradeRequest;
        msg.data = new uint[2] { (uint)tradeData.player2, (uint)tradeData.card1 };
        NetworkClient.Send(msg);
    }

    /// <summary>
    /// 点击玩家
    /// </summary>
    /// <param name="_player"></param>
    public void onClick_Player(GamePlayer _player)
    {
        switch (GameManager.Instance.localGamePlayer.clientState)
        {
            case ClientState.Wait:
                break;
            case ClientState.RoundStart:
                break;
            case ClientState.Trade:
                if (tradeData.card1 == CardID.Null) return;
                tradeData.player2 = _player.id;
                SendPlayerTrade();
                break;
            case ClientState.DeclareVictory:
                if (_player.id == GameManager.Instance.localGamePlayer.id) return;
                DeclareVictorySelectPlayer(_player.id);
                break;
            case ClientState.AttackTarget:
                if (_player.id == GameManager.Instance.localGamePlayer.id) return;
                SendPlayerAttackTargetPlayer(_player.id);
                break;
            case ClientState.HypnotistTarget:
                if (_player.id == GameManager.Instance.localGamePlayer.id) return;
                SendPlayerHypnotistTarget(_player.id);
                break;
        }
    }

    void SendPlayerHypnotistTarget(uint _player)
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.UseHypnotistTarget;
        msg.data = new uint[1] { _player };
        NetworkClient.Send(msg);
    }

    void SendPlayerAttackTargetPlayer(uint _player)
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.AttackTarget;
        msg.data = new uint[1] { _player };
        NetworkClient.Send(msg);
    }

    void DeclareVictorySelectPlayer(uint _player)
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.DeclareVictorySelectPlayer;
        msg.data = new uint[1] { _player };
        NetworkClient.Send(msg);
    }

    void SendPlayerTradeBack()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.TradeBack;
        msg.data = new uint[1] { (uint)tradeData.card2 };
        NetworkClient.Send(msg);
    }

    /// <summary>
    /// 点击道具牌
    /// </summary>
    /// <param name="_card"></param>
    /// <param name="_transform"></param>
    public void onButtonClick_Card(Card _card, RectTransform _transform)
    {
        switch (GameManager.Instance.localGamePlayer.clientState)
        {
            case ClientState.Wait:
                break;
            case ClientState.RoundStart:
                break;
            case ClientState.Trade:
                tradeData.card1 = _card.cardData.cardID;
                uiManager.ArrowActive();
                uiManager.ArrowSetOrigin(_transform);
                uiManager.ArrowTargetLock(false);
                uiManager.ArrowSetTarget(_transform);
                TextMessage.text = "请选择交易对象";
                break;
            case ClientState.TradeRequest:
                if (_card.isHand)
                {
                    tradeData.card2 = _card.cardData.cardID;
                    SendPlayerTradeBack();
                }
                else
                {
                    tradeData.card2 = CardID.Null;
                    SendPlayerTradeBack();
                }
                break;
            case ClientState.TradeMonocle:
                if (!_card.isHand)
                {
                    SendPlayerTradeMonocle();
                }
                break;
            case ClientState.TradeCoat:
                if (!_card.isHand)
                {
                    SendPlayerTradeCoat(_card.cardData.cardID);
                }
                break;
            case ClientState.TradeTome:
                break;
            case ClientState.TradePrivilege:
                if (!_card.isHand)
                {
                    SendPlayerTradePrivilege();
                }
                break;
            case ClientState.TradeSextantBack:
                if (_card.isHand)
                {
                    SendPlayerTradeSextant(_card.cardData.cardID);
                }
                break;
            case ClientState.Diplomat:
                if (_card.isUI)
                {
                    SendPlayerDiplomatSelect(_card.cardData.cardID);
                }
                break;
            case ClientState.DiplomatConfirm:
                if (!_card.isHand)
                {
                    SendPlayerDiplomatConfirm();
                }
                break;
            case ClientState.TradeSextant:
                break;
            case ClientState.GameOver:
                break;
            case ClientState.DeclareVictory:
                break;
            case ClientState.ClairvoyantFrist:
                if (!_card.isHand)
                {
                    SendPlayerClairvoyantFrist(_card.cardData.cardID);
                }
                break;
            case ClientState.ClairvoyantSecond:
                if (!_card.isHand)
                {
                    SendPlayerClairvoyantSecond(_card.cardData.cardID);
                    Destroy(_card.gameObject);
                }
                break;
            case ClientState.AttackTarget:
                break;
            case ClientState.AttackWait:
                break;
            case ClientState.AttackAndDefend:
                if (_card.isHand)
                {
                    SendPlayerUseCard(_card.cardData.cardID);
                }
                break;
            case ClientState.Vote:
                break;
            case ClientState.Hypnotist:
                break;
            case ClientState.Priest:
                break;
            case ClientState.Support:
                if (_card.isHand)
                {
                    SendPlayerUseCard(_card.cardData.cardID);
                }
                break;
            case ClientState.PoisonRing:
                if (_card.cardData.cardID == CardID.PoisonRing)
                {
                    SendPlayerUseCard(_card.cardData.cardID);
                }
                break;
            case ClientState.Payment:
                if (_card.isHand)
                {
                    SendPlayerPayment(_card.cardData.cardID);
                    FightEvent();
                }
                break;
            case ClientState.HypnotistTarget:
                break;
            case ClientState.Winner:
                break;
            case ClientState.Loser:
                break;
            case ClientState.WinnerTake:
                if (!_card.isHand)
                {
                    SendPlayerWinnerTake(_card.cardData.cardID);
                }
                break;
            case ClientState.WinnerCheck:
                if (!_card.isHand)
                {
                    SendPlayerWinnerCheck();
                }
                break;
            case ClientState.WinnerGiveBack:
                if (_card.isHand)
                {
                    SendPlayerWinnerGiveBack(_card.cardData.cardID);
                }
                break;
            default:
                break;
        }
    }

    void SendPlayerWinnerCheck()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.WinnerCheckData;
        NetworkClient.Send(msg);
    }

    void SendPlayerWinnerGiveBack(CardID _card)
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.WinnerGiveBack;
        msg.data = new uint[1] { (uint)_card };
        NetworkClient.Send(msg);
    }


    void SendPlayerWinnerTake(CardID _card)
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.WinnerTakeData;
        msg.data = new uint[1] { (uint)_card };
        NetworkClient.Send(msg);
    }

    void SendPlayerPayment(CardID _card)
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.Payment;
        msg.data = new uint[1] { (uint)_card };
        NetworkClient.Send(msg);
    }

    void SendPlayerUseCard(CardID _card)
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.UseCard;
        msg.data = new uint[1] { (uint)_card };
        NetworkClient.Send(msg);
    }

    void SendPlayerClairvoyantFrist(CardID _card)
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.ClairvoyantFrist;
        msg.data = new uint[1] { (uint)_card };
        NetworkClient.Send(msg);
    }

    void SendPlayerClairvoyantSecond(CardID _card)
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.ClairvoyantSecond;
        msg.data = new uint[1] { (uint)_card };
        NetworkClient.Send(msg);
    }

    void SendPlayerDiplomatConfirm()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.DiplomatConfirm;
        NetworkClient.Send(msg);
    }

    void SendPlayerDiplomatSelect(CardID _card)
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.DiplomatSelect;
        msg.data = new uint[1] { (uint)_card };
        NetworkClient.Send(msg);
    }

    void SendPlayerTradeSextant(CardID _card)
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.TradeSextantBack;
        msg.data = new uint[1] { (uint)_card };
        NetworkClient.Send(msg);
    }

    void SendPlayerTradeCoat(CardID _card)
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.TradeCoat;
        msg.data = new uint[1] { (uint)_card };
        NetworkClient.Send(msg);
    }

    void SendPlayerTradeMonocle()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.TradeMonocle;
        NetworkClient.Send(msg);
    }

    void SendPlayerTradePrivilege()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.TradePrivilege;
        NetworkClient.Send(msg);
    }

    public void onButtonClick_UseOccupation()
    {
        if (playerData.occupation == CardID.Clairvoyant)
        {
            SendUseClairvoyant();
        }
        else if (playerData.occupation == CardID.Diplomat)
        {
            SendUseDiplomat();
        }
        else if (playerData.occupation == CardID.Duelist)
        {
            SendUseDuelist();
        }
        else if (playerData.occupation == CardID.Priest)
        {
            SendUsePriest();
        }
        else if (playerData.occupation == CardID.Hypnotist)
        {
            SendUseHypnotist();
        }
        else if (playerData.occupation == CardID.Thug)
        {
            SendUseThug();
        }
        else if (playerData.occupation == CardID.GrandMaster)
        {
            SendUseGrandMaster();
        }
        else if (playerData.occupation == CardID.Doctor)
        {
            SendUseDoctor();
        }
        else if (playerData.occupation == CardID.Bodyguard)
        {
            SendUseBodyguard();
        }
    }

    /// <summary>
    /// 医生
    /// </summary>
    void SendUseBodyguard()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.UseBodyguard;
        NetworkClient.Send(msg);
    }

    /// <summary>
    /// 医生
    /// </summary>
    void SendUseDoctor()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.UseDoctor;
        NetworkClient.Send(msg);
    }

    /// <summary>
    /// 武术宗师
    /// </summary>
    void SendUseGrandMaster()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.UseGrandMaster;
        NetworkClient.Send(msg);
    }

    /// <summary>
    /// 流氓
    /// </summary>
    void SendUseThug()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.UseThug;
        NetworkClient.Send(msg);
    }

    /// <summary>
    /// 牧师
    /// </summary>
    void SendUsePriest()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.UsePriest;
        NetworkClient.Send(msg);
    }

    /// <summary>
    /// 催眠师
    /// </summary>
    void SendUseHypnotist()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.UseHypnotist;
        NetworkClient.Send(msg);
    }

    /// <summary>
    /// 决斗者
    /// </summary>
    public void SendUseDuelist()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.UseDuelist;
        NetworkClient.Send(msg);
    }

    /// <summary>
    /// 透视者
    /// </summary>
    public void SendUseClairvoyant()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.UseClairvoyant;
        NetworkClient.Send(msg);
    }
    /// <summary>
    /// 外交官
    /// </summary>
    public void SendUseDiplomat()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.UseDiplomat;
        NetworkClient.Send(msg);
    }

    public void onButtonClick_End()
    {
        DeclareVictorySelectPlayer(0);
    }

    public void onButtonClick_Attack()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.Attack;
        NetworkClient.Send(msg);
    }

    public void onButtonClick_Trade()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.Trade;
        NetworkClient.Send(msg);
    }

    public void onButtonClick_DeclareVictory()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.DeclareVictory;
        NetworkClient.Send(msg);
    }

    public void onButtonClick_DeclarePersonalVictory()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.DeclarePersonalVictory;
        NetworkClient.Send(msg);
    }

    public void onButtonClick_Tome(bool _bool)
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.TradeTome;
        msg.data = new uint[1] { (uint)(_bool ? 1 : 0) };
        NetworkClient.Send(msg);
    }

    public void onButtonClick_Cancel()
    {
        switch (GameManager.Instance.localGamePlayer.clientState)
        {
            case ClientState.Wait:
                break;
            case ClientState.RoundStart:
                break;
            case ClientState.Trade:
                break;
            case ClientState.TradeRequest:
                break;
            case ClientState.TradeMonocle:
                break;
            case ClientState.TradeCoat:
                SendPlayerTradeCoat(CardID.Null);
                break;
            case ClientState.TradeTome:
                break;
            case ClientState.TradePrivilege:
                break;
            case ClientState.TradeSextant:
                break;
            case ClientState.TradeSextantBack:
                break;
            case ClientState.GameOver:
                break;
            case ClientState.DeclareVictory:
                break;
            case ClientState.Diplomat:
                break;
            case ClientState.DiplomatConfirm:
                break;
            case ClientState.ClairvoyantFrist:
                break;
            case ClientState.ClairvoyantSecond:
                break;
            case ClientState.AttackTarget:
                break;
            case ClientState.AttackWait:
                break;
            case ClientState.AttackAndDefend:
                FightEvent();
                break;
            case ClientState.Priest:
                FightEvent();
                break;
            case ClientState.Vote:
                break;
            case ClientState.Hypnotist:
                FightEvent();
                break;
            case ClientState.Support:
                FightEvent();
                break;
            case ClientState.PoisonRing:
                FightEvent();
                break;
        }
    }

    public void onButtonClick_Sextant(bool _bool)
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.TradeSextant;
        msg.data = new uint[1] { (uint)(_bool ? 1 : 0) };
        NetworkClient.Send(msg);
    }

    public void onButtonClick_LeaveRoom()
    {
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StopClient();
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StopHost();
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StopServer();
    }

    public void onButtonClick_SupportAttack()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.SupportAttack;
        NetworkClient.Send(msg);
    }

    public void onButtonClick_SupportDefend()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.SupportDefend;
        NetworkClient.Send(msg);
    }

    public void onButtonClick_FightWinner_Take()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.WinnerTake;
        NetworkClient.Send(msg);
    }

    public void onButtonClick_FightWinner_Check()
    {
        MessageTo msg = new MessageTo();
        msg.playerID = GameManager.Instance.localGamePlayer.id;
        msg.messageCommand = (int)ClientCommand.WinnerCheck;
        NetworkClient.Send(msg);
    }
}
