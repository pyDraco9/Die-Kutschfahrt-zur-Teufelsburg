public enum ClientState
{
    /// <summary>
    /// 等待
    /// </summary>
    Wait,
    /// <summary>
    /// 新回合开始
    /// </summary>
    RoundStart,
    /// <summary>
    /// 准备交易数据
    /// </summary>
    Trade,
    /// <summary>
    /// 收到交易请求 等待处理
    /// </summary>
    TradeRequest,
    /// <summary>
    /// 正在查看 单片眼镜
    /// </summary>
    TradeMonocle,
    /// <summary>
    /// 正在查看 大衣
    /// </summary>
    TradeCoat,
    /// <summary>
    /// 正在决定 典籍
    /// </summary>
    TradeTome,
    /// <summary>
    /// 正在查看 特权状
    /// </summary>
    TradePrivilege,
    /// <summary>
    /// 正在决定 六分仪
    /// </summary>
    TradeSextant,
    /// <summary>
    /// 正在选牌 其他玩家 六分仪
    /// </summary>
    TradeSextantBack,
    GameOver,
    /// <summary>
    /// 正在选择 宣告胜利的队友
    /// </summary>
    DeclareVictory,
    /// <summary>
    /// 准备外交官数据
    /// </summary>
    Diplomat,
    /// <summary>
    /// 外交官 查看对方手牌
    /// </summary>
    DiplomatConfirm,
    /// <summary>
    /// 准备透视者数据
    /// </summary>
    ClairvoyantFrist,
    ClairvoyantSecond,
    /// <summary>
    /// 选定攻击目标
    /// </summary>
    AttackTarget,
    /// <summary>
    /// 等待其他人的攻击流程
    /// </summary>
    AttackWait,
    /// <summary>
    /// 投票回合
    /// </summary>
    Vote,
    /// <summary>
    /// 准备催眠师数据
    /// </summary>
    Hypnotist,
    HypnotistTarget,
    /// <summary>
    /// 攻守双方操作
    /// </summary>
    AttackAndDefend,
    /// <summary>
    /// 等待牧师
    /// </summary>
    Priest,
    /// <summary>
    /// 支援阶段, 所有人操作
    /// </summary>
    Support,
    /// <summary>
    /// 等待毒戒指
    /// </summary>
    PoisonRing,
    /// <summary>
    /// 败者支付
    /// </summary>
    Payment,
    Winner,
    Loser,
    WinnerTake,
    WinnerCheck,
    WinnerGiveBack,
}
