using System.Runtime.Serialization;

public enum ClientCommand
{
    /// <summary>
    /// 客户端 准备就绪
    /// </summary>
    ClientReady,
    /// <summary>
    /// 客户端 发起交易请求
    /// </summary>
    Trade,
    /// <summary>
    /// 客户端 交易请求细节
    /// </summary>
    TradeRequest,
    /// <summary>
    /// 客户端 交易回应细节
    /// </summary>
    TradeBack,
    /// <summary>
    /// 客户端 交易event响应
    /// </summary>
    TradeEvent,
    /// <summary>
    /// 客户端 单片眼镜 查看完毕
    /// </summary>
    TradeMonocle,
    /// <summary>
    /// 客户端 大衣效果选择完毕
    /// </summary>
    TradeCoat,
    /// <summary>
    /// 客户端 典籍 选择完毕
    /// </summary>
    TradeTome,
    /// <summary>
    /// 客户端 特权状 查看完毕
    /// </summary>
    TradePrivilege,
    /// <summary>
    /// 客户端 六分仪 选择方向完毕
    /// </summary>
    TradeSextant,
    /// <summary>
    /// 客户端 六分仪 event响应
    /// </summary>
    TradeSextantEvent,
    /// <summary>
    /// 客户端 六分仪 玩家做出选择
    /// </summary>
    TradeSextantBack,
    /// <summary>
    /// 宣告胜利
    /// </summary>
    DeclareVictory,
    /// <summary>
    /// 宣告胜利时选择玩家
    /// </summary>
    DeclareVictorySelectPlayer,
    /// <summary>
    /// 宣告个人胜利
    /// </summary>
    DeclarePersonalVictory,
    /// <summary>
    /// 使用透视者
    /// </summary>
    UseClairvoyant,
    /// <summary>
    /// 使用外交官
    /// </summary>
    UseDiplomat,
    /// <summary>
    /// 使用决斗者
    /// </summary>
    UseDuelist,
    UseThug,
    UseGrandMaster,
    UseDoctor,
    UseBodyguard,
    UsePriest,
    /// <summary>
    /// 使用催眠师
    /// </summary>
    UseHypnotist,
    UseHypnotistTarget,
    /// <summary>
    /// 外交官选卡
    /// </summary>
    DiplomatSelect,
    /// <summary>
    /// 外交官查看结束
    /// </summary>
    DiplomatConfirm,
    ClairvoyantFrist,
    ClairvoyantSecond,
    /// <summary>
    /// 客户端 发起攻击请求
    /// </summary>
    Attack,
    /// <summary>
    /// 客户端 决定攻击目标
    /// </summary>
    AttackTarget,
    /// <summary>
    /// 攻击前攻守双方操作完毕
    /// </summary>
    AttackStep1,
    /// <summary>
    /// 战斗事件响应
    /// </summary>
    FightEvent,
    /// <summary>
    /// 支持攻击
    /// </summary>
    SupportAttack,
    /// <summary>
    /// 支持防守
    /// </summary>
    SupportDefend,
    UseCard,
    Payment,
    WinnerTake,
    WinnerTakeData,
    WinnerCheck,
    WinnerCheckData,
    WinnerGiveBack,
}