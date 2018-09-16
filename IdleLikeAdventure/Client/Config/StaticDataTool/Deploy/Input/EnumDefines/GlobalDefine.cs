using System.Collections;
using System.Collections.Generic;

namespace GlobalDefine {
    //游戏模式
    public enum EGameMode
    {
        Classic,//经典模式
        Timed,//计时模式
        Ordered,//顺序模式
    }

    public enum EDifficulty
    {
        Easy,
        Normal,
        Hard,
    }
    
    //渠道
    public enum EChannel
    {
        AppStore, //苹果商店
        GooglePlay,//谷歌商店
        Max
    }
    //IAP的功能类型
    public enum EIAPFunction
    {
        BuyItem, //购买道具
        RemoveAd, //移除广告
    }
}
