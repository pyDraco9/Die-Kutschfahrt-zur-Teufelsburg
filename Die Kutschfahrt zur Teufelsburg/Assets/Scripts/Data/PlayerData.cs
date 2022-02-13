using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public List<CardID> cardList = new List<CardID>();
    /// <summary>
    /// 职业
    /// </summary>
    public CardID occupation = CardID.Null;
    /// <summary>
    /// 阵营
    /// </summary>
    public CardID association = CardID.Null;
    /// <summary>
    /// 职业是否公开
    /// </summary>
    public bool occupationIsPublic;

    public PlayerData()
    {

    }
}
