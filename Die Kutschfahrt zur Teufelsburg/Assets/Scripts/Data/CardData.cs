using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData
{
    public CardID cardID;
    public CardType cardType;
    public string cardRes;
    public string cardName;
    public string cardDes;

    public CardData(CardID _cardID)
    {
        switch (_cardID)
        {
            case CardID.Null:
                cardID = _cardID;
                cardType = CardType.Null;
                cardRes = "";
                cardName = "未知";
                cardDes = "";
                break;
            case CardID.TheOrderOFOpenSecrets:
                cardID = _cardID;
                cardType = CardType.Association;
                cardRes = "阵营-公示机密";
                cardName = "公示机密";
                cardDes = "";
                break;
            case CardID.TheBrotherhoodOfTrueLies:
                cardID = _cardID;
                cardType = CardType.Association;
                cardRes = "阵营-真实幻言";
                cardName = "真实幻言";
                cardDes = "";
                break;
            case CardID.Monocle:
                cardID = _cardID;
                cardType = CardType.Object;
                cardRes = "道具-单片眼镜";
                cardName = "单片眼镜";
                cardDes = "";
                break;
            case CardID.Key:
                cardID = _cardID;
                cardType = CardType.Object;
                cardRes = "道具-银钥匙";
                cardName = "钥匙";
                cardDes = "";
                break;
            case CardID.Goblet:
                cardID = _cardID;
                cardType = CardType.Object;
                cardRes = "道具-高脚杯";
                cardName = "圣杯";
                cardDes = "";
                break;
            case CardID.Gloves:
                cardID = _cardID;
                cardType = CardType.Object;
                cardRes = "道具-手套";
                cardName = "手套";
                cardDes = "";
                break;
            case CardID.Coat:
                cardID = _cardID;
                cardType = CardType.Object;
                cardRes = "道具-大衣";
                cardName = "大衣";
                cardDes = "";
                break;
            case CardID.SecretBagGoblet:
                cardID = _cardID;
                cardType = CardType.Object;
                cardRes = "道具-神秘手袋（杯子）";
                cardName = "神秘手提袋";
                cardDes = "";
                break;
            case CardID.SecretBagKey:
                cardID = _cardID;
                cardType = CardType.Object;
                cardRes = "道具-神秘手袋（钥匙）";
                cardName = "神秘手提袋";
                cardDes = "";
                break;
            case CardID.Tome:
                cardID = _cardID;
                cardType = CardType.Object;
                cardRes = "道具-典籍";
                cardName = "典籍";
                cardDes = "";
                break;
            case CardID.CastingKnives:
                cardID = _cardID;
                cardType = CardType.Object;
                cardRes = "道具-飞刀";
                cardName = "飞刀";
                cardDes = "";
                break;
            case CardID.Dagger:
                cardID = _cardID;
                cardType = CardType.Object;
                cardRes = "道具-匕首";
                cardName = "匕首";
                cardDes = "";
                break;
            case CardID.Privilege:
                cardID = _cardID;
                cardType = CardType.Object;
                cardRes = "道具-特权文书";
                cardName = "特权状";
                cardDes = "";
                break;
            case CardID.PoisonRing:
                cardID = _cardID;
                cardType = CardType.Object;
                cardRes = "道具-毒戒指";
                cardName = "毒戒指";
                cardDes = "";
                break;
            case CardID.BlackPeart:
                cardID = _cardID;
                cardType = CardType.Object;
                cardRes = "道具-黑珍珠";
                cardName = "厄运黑珍珠";
                cardDes = "";
                break;
            case CardID.Whip:
                cardID = _cardID;
                cardType = CardType.Object;
                cardRes = "道具-长鞭";
                cardName = "长鞭";
                cardDes = "";
                break;
            case CardID.TheCoatOfArmorOfTheLoge:
                cardID = _cardID;
                cardType = CardType.Object;
                cardRes = "道具-盔甲勋章";
                cardName = "铁甲徽章";
                cardDes = "";
                break;
            case CardID.BrokeMirror:
                cardID = _cardID;
                cardType = CardType.Object;
                cardRes = "道具-破镜子";
                cardName = "破碎的镜子";
                cardDes = "";
                break;
            case CardID.Sextant:
                cardID = _cardID;
                cardType = CardType.Object;
                cardRes = "道具-六分仪";
                cardName = "六分仪";
                cardDes = "";
                break;
            case CardID.GrandMaster:
                cardID = _cardID;
                cardType = CardType.Occupation;
                cardRes = "职业-武术宗师";
                cardName = "武术宗师";
                cardDes = "如果你是防御者，可获得+1的争斗结算。即使其他玩家已经提出支援，使用物品、职业能力，也能产生效果。";
                break;
            case CardID.Doctor:
                cardID = _cardID;
                cardType = CardType.Occupation;
                cardRes = "职业-医生";
                cardName = "医生";
                cardDes = "游戏中限定一次。在争斗之后，你能立刻阻止争斗效果。";
                break;
            case CardID.Clairvoyant:
                cardID = _cardID;
                cardType = CardType.Occupation;
                cardRes = "职业-预言家";
                cardName = "透视者";
                cardDes = "游戏中限定一次。在你的回合，你可查看物品牌堆并且取出两样物品，将其他牌洗匀后，将这两张牌牌面向下，以任意顺序放在物品牌堆顶端。";
                break;
            case CardID.Thug:
                cardID = _cardID;
                cardType = CardType.Occupation;
                cardRes = "职业-流氓";
                cardName = "流氓";
                cardDes = "如果你是攻击者，可获得+1的争斗结算。在防御或支援的时候没有效果。";
                break;
            case CardID.Hypnotist:
                cardID = _cardID;
                cardType = CardType.Occupation;
                cardRes = "职业-催眠师";
                cardName = "催眠师";
                cardDes = "若你是攻击者，你可以指定某个玩家不能支援争斗。即使在他宣告支援之后。这个特权只能在玩家宣告支援时立刻使用，也就是玩家使用物品、职业之前。被指定的玩家在这场争斗中的支援无效，并且不可以使用职业或者物品。";
                break;
            case CardID.PoisonMixer:
                cardID = _cardID;
                cardType = CardType.Occupation;
                cardRes = "职业-调毒师";
                cardName = "毒药调和者";
                cardDes = "游戏中限定一次。你可以指定争斗中的胜利者，但你不可以是这场争斗的任何一方。即使在其他玩家已经提出支援，使用职业或者物品的能力，也能生效。";
                break;
            case CardID.Priest:
                cardID = _cardID;
                cardType = CardType.Occupation;
                cardRes = "职业-牧师";
                cardName = "牧师";
                cardDes = "限定一次。在其他玩家宣告支援前阻止争斗。如果攻击者有两件或以上的物品，则选择一件给你。然后攻击者回合结束。";
                break;
            case CardID.Duelist:
                cardID = _cardID;
                cardType = CardType.Occupation;
                cardRes = "职业-决斗者";
                cardName = "决斗者";
                cardDes = "游戏中限定一次，作为攻击者或防御者，你可以指定这场争斗没有任何支援者，并可以获得+1的争斗结算。即使其他玩家已经提出支援，以及使用职业、物品能力，也可以产生效果。";
                break;
            case CardID.Bodyguard:
                cardID = _cardID;
                cardType = CardType.Occupation;
                cardRes = "职业-保镖";
                cardName = "保镖";
                cardDes = "如果你在争斗中支援其他玩家，他将获得+1的争斗结算。这个效果对自身没有作用。";
                break;
            case CardID.Diplomat:
                cardID = _cardID;
                cardType = CardType.Occupation;
                cardRes = "职业-外交官";
                cardName = "外交官";
                cardDes = "游戏中限定一次。你能在你的回合指定其他玩家和你交易某个物品。若该玩家有这个物品，必须交易；若没有，则该玩家把物品牌给你看以证明他没有该物品，随后你的回合结束。";
                break;
            case CardID.TheDrinkOfPower:
                cardID = _cardID;
                cardType = CardType.DrinkOfPower;
                cardRes = "特殊-权利的美酒";
                cardName = "权利的美酒";
                cardDes = "";
                break;
        }
    }
    public CardData()
    {
    }
}

public enum CardID
{
    Null,
    /// <summary>
    /// 揭秘修道会
    /// </summary>
    TheOrderOFOpenSecrets,
    /// <summary>
    /// 真实谎言兄弟会
    /// </summary>
    TheBrotherhoodOfTrueLies,
    /// <summary>
    /// 单片眼镜
    /// </summary>
    Monocle,
    /// <summary>
    /// 钥匙
    /// </summary>
    Key,
    /// <summary>
    /// 圣杯
    /// </summary>
    Goblet,
    /// <summary>
    /// 手套
    /// </summary>
    Gloves,
    /// <summary>
    /// 大衣
    /// </summary>
    Coat,
    /// <summary>
    /// 神秘手袋（杯子）
    /// </summary>
    SecretBagGoblet,
    /// <summary>
    /// 神秘手袋（钥匙）
    /// </summary>
    SecretBagKey,
    /// <summary>
    /// 典籍
    /// </summary>
    Tome,
    /// <summary>
    /// 飞刀
    /// </summary>
    CastingKnives,
    /// <summary>
    /// 匕首
    /// </summary>
    Dagger,
    /// <summary>
    /// 特权状
    /// </summary>
    Privilege,
    /// <summary>
    /// 毒戒指
    /// </summary>
    PoisonRing,
    /// <summary>
    /// 厄运黑珍珠
    /// </summary>
    BlackPeart,
    /// <summary>
    /// 长鞭
    /// </summary>
    Whip,
    /// <summary>
    /// 铁甲徽章
    /// </summary>
    TheCoatOfArmorOfTheLoge,
    /// <summary>
    /// 破碎的镜子
    /// </summary>
    BrokeMirror,
    /// <summary>
    /// 六分仪
    /// </summary>
    Sextant,
    /// <summary>
    /// 武术宗师
    /// </summary>
    GrandMaster,
    /// <summary>
    /// 医生
    /// </summary>
    Doctor,
    /// <summary>
    /// 透视者
    /// </summary>
    Clairvoyant,
    /// <summary>
    /// 流氓
    /// </summary>
    Thug,
    /// <summary>
    /// 催眠师
    /// </summary>
    Hypnotist,
    /// <summary>
    /// 毒药调和者
    /// </summary>
    PoisonMixer,
    /// <summary>
    /// 牧师
    /// </summary>
    Priest,
    /// <summary>
    /// 决斗者
    /// </summary>
    Duelist,
    /// <summary>
    /// 保镖
    /// </summary>
    Bodyguard,
    /// <summary>
    /// 外交官
    /// </summary>
    Diplomat,
    /// <summary>
    /// 权利的美酒
    /// </summary>
    TheDrinkOfPower
}

public enum CardType
{
    Null,
    Character,
    Occupation,
    Association,
    Object,
    DrinkOfPower,
}