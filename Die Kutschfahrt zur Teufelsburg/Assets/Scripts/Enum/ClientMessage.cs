public enum ClientMessage
{
    /// <summary>
    /// 神秘手提袋
    /// </summary>
    SecretBag,
    /// <summary>
    /// 无法交易
    /// </summary>
    CantTrade,
    /// <summary>
    /// 你不能在只有手提袋时交换手提袋
    /// </summary>
    CantTradeSecretBag,
    /// <summary>
    /// 无法拒绝
    /// </summary>
    CantRefuse,
    /// <summary>
    /// 单片眼镜
    /// </summary>
    Monocle,
    /// <summary>
    /// 单片眼镜查看结束
    /// </summary>
    MonocleEnd,
    /// <summary>
    /// 大衣效果
    /// </summary>
    Coat,
    /// <summary>
    /// 大衣 选择交换
    /// </summary>
    CoatTrue,
    /// <summary>
    /// 大衣 选择不换
    /// </summary>
    CoatFalse,
    /// <summary>
    /// 数据错误 这张牌不是职业牌 或 不在牌组中
    /// </summary>
    CoatCant,
    /// <summary>
    /// 典籍效果
    /// </summary>
    Tome,
    /// <summary>
    /// 典籍 选择交换
    /// </summary>
    TomeTrue,
    /// <summary>
    /// 典籍 选择不换
    /// </summary>
    TomeFalse,
    /// <summary>
    /// 特权状
    /// </summary>
    Privilege,
    /// <summary>
    /// 特权状结束
    /// </summary>
    PrivilegeEnd,
    /// <summary>
    /// 六分仪效果
    /// </summary>
    Sextant,
    /// <summary>
    /// 六分仪向左
    /// </summary>
    SextantLeft,
    /// <summary>
    /// 六分仪向右
    /// </summary>
    SextantRight,
    /// <summary>
    /// 数据错误 这张牌不在手牌中
    /// </summary>
    SextantCant,
    /// <summary>
    /// 六分仪 下一个玩家操作
    /// </summary>
    SextantNext,
    /// <summary>
    /// 不能宣告个人胜利
    /// </summary>
    CantDeclarePersonalVictory,
    /// <summary>
    /// 个人胜利
    /// </summary>
    DeclarePersonalVictory,
    /// <summary>
    /// 宣告胜利
    /// </summary>
    DeclareVictory,
    /// <summary>
    /// 该玩家已经选择过了
    /// </summary>
    DeclareVictorySelected,
    /// <summary>
    /// 透视者 无法选择这张牌
    /// </summary>
    ClairvoyantCant,
    /// <summary>
    /// 无法使用职业
    /// </summary>
    OccupationCant,
    /// <summary>
    /// 发动决斗者 没有支援
    /// </summary>
    Duelist,
    /// <summary>
    /// 指定玩家投票
    /// </summary>
    Vote,
    /// <summary>
    /// 所有投票完毕
    /// </summary>
    VoteEnd,
    /// <summary>
    /// 玩家投给了攻击方
    /// </summary>
    VoteAttack,
    /// <summary>
    /// 玩家投给了防守方
    /// </summary>
    VoteDefend,
    Priest,
    Hypnotist,
    HypnotistTarget,
    HypnotistTargetCant,
    Hypnotized,
    Draw,
    WinnerTake,
    WinnerCheck,
    FightTakeEnd,
    WinnerTakeGiveBack,
    GiveBackEnd,
    WinnerCheckEnd,
    UseCard,
    UsedCard,
}