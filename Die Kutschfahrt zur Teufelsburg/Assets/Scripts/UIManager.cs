using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] public Text DeckObjectCount;
    [SerializeField] public Arrow Arrow;
    [SerializeField] public GameObject Button_Attack;
    [SerializeField] public GameObject Button_Trade;
    [SerializeField] public GameObject Button_DeclareVictory;
    [SerializeField] public GameObject Button_DeclarePersonalVictory;
    [SerializeField] public GameObject UI_Arrow;
    [SerializeField] public GameObject Button_Cancel;
    [SerializeField] public GameObject Button_Tome_Yes;
    [SerializeField] public GameObject Button_Tome_No;
    [SerializeField] public GameObject Button_Sextant_Left;
    [SerializeField] public GameObject Button_Sextant_Right;
    [SerializeField] public GameObject Button_End;
    [SerializeField] public GameObject Panel_Log;
    [SerializeField] public GameObject Grid_Log;
    [SerializeField] public GameObject Panel_Fight;
    [SerializeField] public Text Panel_Fight_Attack;
    [SerializeField] public Text Panel_Fight_Defend;
    [SerializeField] public GameObject Button_UseOccupation;
    [SerializeField] public GameObject Grid_AllCard;
    [SerializeField] public GameObject Button_LeaveRoom;
    [SerializeField] public GameObject Button_SupportAttack;
    [SerializeField] public GameObject Button_SupportDefend;
    [SerializeField] public GameObject Button_FightWinner_Take;
    [SerializeField] public GameObject Button_FightWinner_Check;

    public void ReActive()
    {
        Button_Attack.SetActive(false);
        Button_Trade.SetActive(false);
        Button_DeclareVictory.SetActive(false);
        Button_DeclarePersonalVictory.SetActive(false);
        UI_Arrow.SetActive(false);
        Button_Cancel.SetActive(false);
        Button_Tome_Yes.SetActive(false);
        Button_Tome_No.SetActive(false);
        Button_Sextant_Left.SetActive(false);
        Button_Sextant_Right.SetActive(false);
        Button_End.SetActive(false);
        Panel_Fight.SetActive(false);
        Button_UseOccupation.SetActive(false);
        Grid_AllCard.SetActive(false);
        Button_LeaveRoom.SetActive(false);
        Button_SupportAttack.SetActive(false);
        Button_SupportDefend.SetActive(false);
        Button_FightWinner_Take.SetActive(false);
        Button_FightWinner_Check.SetActive(false);
    }
    public void ReActiveWithoutFightPanel()
    {
        ReActive();
        Panel_Fight.SetActive(true);
    }

    public void SupportActive()
    {
        Button_SupportAttack.SetActive(true);
        Button_SupportDefend.SetActive(true);
    }

    public void WinnerActive()
    {
        Button_FightWinner_Take.SetActive(true);
        Button_FightWinner_Check.SetActive(true);
    }

    public void GameOverActive()
    {
        Button_LeaveRoom.SetActive(true);
    }

    public void AllCardActive()
    {
        Grid_AllCard.SetActive(true);
    }

    public void Awake()
    {
        foreach (CardID cardID in Enum.GetValues(typeof(CardID)))
        {
            if (new CardData(cardID).cardType == CardType.Object)
            {
                GameObject card = Instantiate((GameObject)Resources.Load("Prefabs/Card"));
                card.transform.SetParent(Grid_AllCard.transform);
                card.GetComponent<Card>().InitCardDataMute(new CardData(cardID));
            }
        }
    }

    public void UseOccupation()
    {
        GameManager.Instance.localGamePlayer.onButtonClick_UseOccupation();
    }

    public void OccupationActive()
    {
        Button_UseOccupation.SetActive(true);
    }

    public void UpdateFightData(int _attack,  int _defend)
    {
        Panel_Fight_Attack.text = _attack.ToString();
        Panel_Fight_Defend.text = _defend.ToString();
    }

    public void PanelFightActive()
    {
        Panel_Fight.SetActive(true);
    }

    public void AddLog(string _text)
    {
        GameObject log = Instantiate((GameObject)Resources.Load("Prefabs/Panel_Log"));
        log.GetComponent<PanelLog>().SetLogText(_text);
        log.transform.SetParent(Grid_Log.transform);
        log.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void Log()
    {
        Panel_Log.SetActive(!Panel_Log.activeSelf);
    }

    public void ArrowSetOrigin(RectTransform _v2)
    {
        Arrow.origin = _v2;
    }

    public void RoundStartActive(bool _bool = true)
    {
        ReActive();
        Button_Attack.SetActive(true);
        Button_Trade.SetActive(true);
        if (_bool)
        {
            Button_DeclareVictory.SetActive(true);
        }
    }

    public void EndActive()
    {
        Button_End.SetActive(true);
    }

    public void CancelActive()
    {
        Button_Cancel.SetActive(true);
    }

    public void TradeSextantActive()
    {
        Button_Sextant_Left.SetActive(true);
        Button_Sextant_Right.SetActive(true);
    }

    public void TradeTomeActive()
    {
        Button_Tome_Yes.SetActive(true);
        Button_Tome_No.SetActive(true);
    }

    public void ArrowActive()
    {
        UI_Arrow.SetActive(true);
    }

    public void ArrowTargetLock(bool _bool)
    {
        Arrow.targetLock = _bool;
    }

    public void ArrowSetTarget(Transform _rt)
    {
        Arrow.target = _rt;
    }

    public void PersonalActive()
    {
        Button_DeclarePersonalVictory.SetActive(true);
    }

    public void onButtonClick_Attack()
    {
        GameManager.Instance.localGamePlayer.onButtonClick_Attack();
    }

    public void onButtonClick_Trade()
    {
        GameManager.Instance.localGamePlayer.onButtonClick_Trade();
    }

    public void onButtonClick_DeclareVictory()
    {
        GameManager.Instance.localGamePlayer.onButtonClick_DeclareVictory();
    }

    public void onButtonClick_DeclarePersonalVictory()
    {
        GameManager.Instance.localGamePlayer.onButtonClick_DeclarePersonalVictory();
    }

    public void onButtonClick_Cancel()
    {
        GameManager.Instance.localGamePlayer.onButtonClick_Cancel();
    }
    public void onButtonClick_End()
    {
        GameManager.Instance.localGamePlayer.onButtonClick_End();
    }

    public void onButtonClick_Tome_Yes()
    {
        GameManager.Instance.localGamePlayer.onButtonClick_Tome(true);
    }

    public void onButtonClick_Tome_No()
    {
        GameManager.Instance.localGamePlayer.onButtonClick_Tome(false);
    }

    public void onButtonClick_Sextant_Left()
    {
        GameManager.Instance.localGamePlayer.onButtonClick_Sextant(true);
    }

    public void onButtonClick_Sextant_Right()
    {
        GameManager.Instance.localGamePlayer.onButtonClick_Sextant(false);
    }

    public void onButtonClick_LeaveRoom()
    {
        GameManager.Instance.localGamePlayer.onButtonClick_LeaveRoom();
    }

    public void onButtonClick_SupportAttack()
    {
        GameManager.Instance.localGamePlayer.onButtonClick_SupportAttack();
    }

    public void onButtonClick_SupportDefend()
    {
        GameManager.Instance.localGamePlayer.onButtonClick_SupportDefend();
    }

    public void onButtonClick_FightWinner_Take()
    {
        GameManager.Instance.localGamePlayer.onButtonClick_FightWinner_Take();
    }

    public void onButtonClick_FightWinner_Check()
    {
        GameManager.Instance.localGamePlayer.onButtonClick_FightWinner_Check();
    }
}
