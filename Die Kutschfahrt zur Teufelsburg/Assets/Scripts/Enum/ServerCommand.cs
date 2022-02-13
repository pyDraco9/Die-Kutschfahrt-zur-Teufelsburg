using System.Runtime.Serialization;

public enum ServerCommand
{
    /// <summary>
    /// 服务器 安排座位号
    /// </summary>
    PlayerSet,
    /// <summary>
    /// 服务器 发牌
    /// </summary>
    Licensing,
    /// <summary>
    /// 服务器 收牌
    /// </summary>
    LicensingBack,
    /// <summary>
    /// 服务器 玩家手牌数量
    /// </summary>
    SyncPlayerHandCount,
    /// <summary>
    /// 服务器 道具牌剩余数量
    /// </summary>
    SyncDeckObjectCount,
    /// <summary>
    /// 公布阵营
    /// </summary>
    AssociationPublic,
    /// <summary>
    /// 服务器 职业通知
    /// </summary>
    LicensingOccupation,
    /// <summary>
    /// 服务器 阵营通知
    /// </summary>
    LicensingAssociation,
    /// <summary>
    /// 服务器 通知 回合开始通知
    /// </summary>
    RoundStart,
    /// <summary>
    /// 职业公开通知
    /// </summary>
    OccupationPublic,
    /// <summary>
    /// 服务器 通知 玩家发起交易 
    /// </summary>
    Trade,
    /// <summary>
    /// 服务器 通知 玩家发起交易数据 
    /// </summary>
    TradeRequest,
    /// <summary>
    /// 服务器 通知 玩家回应交易数据
    /// </summary>
    TradeBack,
    /// <summary>
    /// 服务器 通知 等待玩家回应event
    /// </summary>
    TradeEvent,
    /// <summary>
    /// 服务器 通知
    /// </summary>
    ClientMessage,
    /// <summary>
    /// 服务器 下发 单片眼镜 数据
    /// </summary>
    TradeMonocle,
    /// <summary>
    /// 服务器 下发 大衣 数据
    /// </summary>
    TradeCoat,
    /// <summary>
    /// 服务器 下发 典籍 (空数据)
    /// </summary>
    TradeTome,
    /// <summary>
    /// 服务器 下发 特权状数据
    /// </summary>
    TradePrivilege,
    /// <summary>
    /// 服务器 下发 六分仪 发动
    /// </summary>
    TradeSextant,
    /// <summary>
    /// 服务器 下发 六分仪 回应event
    /// </summary>
    TradeSextantEvent,
    /// <summary>
    /// 服务器 下发 六分仪 单个玩家回应完毕
    /// </summary>
    TradeSextantBack,
    /// <summary>
    /// 服务器 下发 等待选择队友
    /// </summary>
    DeclareVictory,
    /// <summary>
    /// 服务器 下发 选择的玩家
    /// </summary>
    DeclareVictorySelectPlayer,
    /// <summary>
    /// 服务器 下发 最终宣告胜利结果
    /// </summary>
    DeclareVictoryMessage,
    /// <summary>
    /// 服务器 通知 玩家发起攻击
    /// </summary>
    Attack,
    /// <summary>
    /// 服务器 通知 玩家攻击对象
    /// </summary>
    AttackTarget,
    /// <summary>
    /// 服务器 通知 使用外交官
    /// </summary>
    Diplomat,
    /// <summary>
    /// 服务器 通知 外交官选卡完毕 进入交易流程
    /// </summary>
    DiplomatSelect,
    /// <summary>
    /// 服务器通知 外交官拒绝交易 查看对方手牌
    /// </summary>
    DiplomatConfirm,
    /// <summary>
    /// 服务器 通知 使用透视者
    /// </summary>
    Clairvoyant,
    /// <summary>
    /// 服务器 通知 使用决斗者
    /// </summary>
    Duelist,
    /// <summary>
    /// 服务器 下发 透视者数据
    /// </summary>
    ClairvoyantData,
    ClairvoyantFrist,
    ClairvoyantEnd,
    UpdateFightData,
    /// <summary>
    /// 要求客户端进行FightEvent
    /// </summary>
    FightEvent,
    /// <summary>
    /// 已收到FightEvent消息确认
    /// </summary>
    FightEventConfirm,
    VoteConfirm,
    /// <summary>
    /// 使用牧师
    /// </summary>
    Priest,
    /// <summary>
    /// 牧师效果 负责给牌的玩家
    /// </summary>
    PriestEffect,
    /// <summary>
    /// 使用催眠师
    /// </summary>
    Hypnotist,
    /// <summary>
    /// 使用毒戒指
    /// </summary>
    PoisonRing,
    /// <summary>
    /// 服务器 通知 攻守双方操作
    /// </summary>
    FightAttackAndDefend,
    /// <summary>
    /// 服务器 通知 使用牧师
    /// </summary>
    FightPriest,
    /// <summary>
    /// 服务器 通知 投票
    /// </summary>
    FightVote,
    /// <summary>
    /// 服务器 同步 投票玩家列表
    /// </summary>
    SyncVoteList,
    /// <summary>
    /// 服务器 通知 使用催眠师
    /// </summary>
    FightHypnotist,
    /// <summary>
    /// 服务器 通知 支援者使用道具
    /// </summary>
    FightSupport,
    /// <summary>
    /// 服务器通知 使用 毒戒指
    /// </summary>
    FightPoisonRing,
    /// <summary>
    /// 服务器通知 战斗结束
    /// </summary>
    FightEnd,
    WinnerTakeData,
    WinnerTakeGiveBack,
    WinnerTakeConfirm,
    WinnerCheck,
    WinnerCheckConfirm,
    Docter,
    UsePoisonRing,
}