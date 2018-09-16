using System.Collections;
using System.Collections.Generic;

namespace GlobalDefine
{

    /// <summary>
    /// 生成物体类型
    /// </summary>
    public enum GenerateType
    {
        Assets,
        Neutral,
        Monster,
        Bullet,
        Effect,
        Hero
    }

    //解锁的类型
    public enum ELockType
    {
        Level, //关卡
        Item,//道具
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
    }
    public enum ECharacterType
    {
        Hero,
        Neutral,
        TombStone
    }
    public enum EItemType
    {
        Currency,
        Experience,
        Fighting,
        Food,
        FoodMaterial,
        BuildMaterial,
        UpgradeMaterial,
        Chip,
        Seed, //种子
        Collection, //收藏品
    }
    public enum EWeaponType
    {
        Melee,
        Bow,
        Spells
    }
    public enum EAttackType
    {
        OnePlayer,
        Range
    }
    public enum EAttackTarget
    {
        Self,
        Enemy
    }
    public enum EBallisticType
    {
        Track,
        Line,
        Parabola
    }
    public enum EDeathExplosion
    {
        False6,
        True
    }
    #region 任务系统中用到的枚举
    /// <summary>
    /// 任务 
    /// </summary>
    public enum TaskType
    {
        StageTask,//关卡任务
        DailyTask,//日常任务（每日任务）
        AchievementTask,//成就任务（累计）

    }
    /// <summary>
    /// 任务目标类型
    /// </summary>
    public enum TaskObjectType
    {
        PassSpecifyLevel,//通过指定关卡  1
        KilledEnemyCount,//杀死敌人数量  2
        KilledSpecifyEnemyCount,//杀死指定a敌人数量b   3
        CollectedSpecifyItems,//收集指定资源A的数量b  4
        LineLength,//队伍长度（队伍长度达到多长B） 5
        RecruitSpecifyHore,//招募指定A英雄  6
        Deaths,//英雄死亡次数  7
        Hp,//指定英雄A剩余血量  8
        TotalDamage,//总伤害 9
        HPR,//回血量 10
        UseSpecificSkillCount,//使用指定技能A的次数B  11
        PassTime,// 通过关卡时间A*60  12


    }
    /// <summary>
    /// 任务完成判定
    /// </summary>
    public enum TaskFinshedJudge
    {
        Forthwith,//即时判断
        PassedThisLevel,//过关判断

    }
    #endregion
    public enum Label {
        Story,
        Activity,
        Expert
    }
    public enum Classify {

        Chapter,
        Stage
    }
    /// <summary>
    /// 背包内的显示分类
    /// </summary>
    public enum BagViewType
    {
        All,//全部
        Food,//食物
        Material,//材料
        UpgradeMaterial,//强化材料
        Chips,//碎片
        Other,//其他
    }

}
