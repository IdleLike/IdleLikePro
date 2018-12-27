using NetData.Enumeration;
using SUIFW;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIBattleRoomView : BaseUIForm
{
    #region UI组件
    public List<CharacterUIInfo> m_CharacterUIInfoList = new List<CharacterUIInfo>();
    [Serializable]
    public class CharacterUIInfo
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int ID;
        /// <summary>
        /// 角色名称
        /// </summary>
        public Text Name_Txt;
        /// <summary>
        /// 角色种族
        /// </summary>
        public Text RocaName_Txt;
        /// <summary>
        /// 角色职业
        /// </summary>
        public Text Career_Txt;
        /// <summary>
        /// 角色等级
        /// </summary>
        public Text Level_Txt;
        /// <summary>
        /// 角色最大血量
        /// </summary>
        public Text MaxHP_Txt;
        /// <summary>
        /// 角色当前血量
        /// </summary>
        public Text CurrentHP_Txt;
        /// <summary>
        /// 角色最大经验
        /// </summary>
        public Text MaxEXP_Txt;
        /// <summary>
        /// 角色当前经验
        /// </summary>
        public Text CurrentEXP_Txt;
        /// <summary>
        /// 角色血条
        /// </summary>
        public Slider HP_Sli;
        /// <summary>
        /// 角色蓝条
        /// </summary>
        public Slider EXP_Sli;
    }
    public List<EnemyUIInfo> m_EnemyUIInfoList = new List<EnemyUIInfo>();
    [Serializable]
    public class EnemyUIInfo
    {
        /// <summary>
        /// 敌人ID
        /// </summary>
        public int ID;
        /// <summary>
        /// 敌人名称
        /// </summary>
        public Text Name_Txt;
        /// <summary>
        /// 敌人等级
        /// </summary>
        public Text Level_Txt;
        /// <summary>
        /// 敌人当前血量
        /// </summary>
        public Text CurrentHP_Txt;
        /// <summary>
        /// 敌人最大血量
        /// </summary>
        public Text MaxHP_Txt;
        /// <summary>
        /// 敌人血条
        /// </summary>
        public Slider HP_Sli;
        ///// <summary>
        ///// 敌人技能信息
        ///// </summary>
        public Text AbilityNameAndType;
    }

    public Text m_BattleReport_Txt;
    public UIBattleReportScript m_AttackBattleReport;
    public GameObject m_EnemyGo;
    #endregion

    #region 战报信息
    string m_FindEnemy = "战斗刚结束，正在休息恢复中，并搜寻敌人...({0})";
    private string m_NormalAttack = "{0}.{1}攻击了{2}";
    private string m_Damage = "，造成了{0}伤害";
    private string m_Recovery = "{0}.{1}使用技能{2}，恢复了自己{3}HP，{4}没有受到直接伤害";
    private string m_AbilityAttack = "{0}.{1}使用技能{2}，攻击了{3}";
    private string m_Dodge = "，但被对方闪避，{0}没有受到直接伤害";
    private string m_FailureResult = "{0}被击败，战斗结束...";
    private string m_SuccessResult = "\n敌方被击败，战斗结束...\n";
    private string m_Beat = "{0}被击败了";

    #endregion

    #region 战报临时数据
    private string m_AttackName = "";
    private string m_DefendsName = "";
    private string m_AbilityName = "";
    private int m_AbiilityEffectValue = 0;
    private UIBattleRoomViewModel.BattleCharacter AttackCharacter = null;
    private UIBattleRoomViewModel.BattleCharacter DefendsCharacter = null;
    private UIBattleRoomViewModel.BattleEnemy AttackEnemy = null;
    private UIBattleRoomViewModel.BattleEnemy DefendsEnemy = null;
    private CharacterUIInfo UIAttackCharacter = null;
    private CharacterUIInfo UIDefendsCharacter = null;
    private EnemyUIInfo UIAttackEnemy = null;
    private EnemyUIInfo UIDefendsEnemy = null;
    #endregion

    public UIBattleRoomViewModel m_ViewModel = new UIBattleRoomViewModel();
    
    public override void UpdatePanel(object viewModel)
    {
        m_ViewModel = viewModel as UIBattleRoomViewModel;
        if (m_ViewModel != null)
        {
            InitCharcterInfo();
            int FindEnemyTime = m_ViewModel.BattleReportList.FindEnemyTime;
            Debug.Log("FindEnemyTime : " + FindEnemyTime);
            if (FindEnemyTime > 0)
            {
                //更新倒计时
                UIBattleReportScript m_ReportScript = CreateReport().GetComponent<UIBattleReportScript>();
                m_ReportScript.m_DateTime_Txt.text = "";
                m_EnemyGo.SetActive(false);
                StartCoroutine(CountDown(FindEnemyTime, m_ReportScript.m_Info_Txt));
            }
            else
            {
                PrepareBattle();
            }
        }
    }
    /// <summary>
    /// 初始化敌人，战斗准备
    /// </summary>
    void PrepareBattle()
    {
        InitEnemyInfo();
        m_EnemyGo.SetActive(true);
        List<UIBattleRoomViewModel.AttackInfos> AttackInfos = m_ViewModel.BattleReportList.AttackInfoList;
        StartCoroutine(PlayAttackInfo(AttackInfos));
    }
    /// <summary>
    /// HP改变
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="maxhp"></param>
    /// <param name="effectValue"></param>
    /// <param name="text"></param>
    /// <param name="slider"></param>
    /// <param name="type"></param>
    void HPValueChanged(ref int hp,int maxhp,int effectValue,Text text,Slider slider, EnumAbilityEffect type)
    {
        switch (type)
        {
            case EnumAbilityEffect.Damage:
                hp -= effectValue;
                break;
            case EnumAbilityEffect.Recovery:
                hp += effectValue;
                break;
            default:
                break;
        }
        hp = hp < 0 ? 0 : hp;
        hp = hp > maxhp ? maxhp : hp; 
        text.text = hp.ToString();
        slider.value = Mathf.Clamp(hp, 0, maxhp) / (float)maxhp;
    }
    /// <summary>
    /// 获取战报信息
    /// </summary>
    /// <param name="info"></param>
    void GetAttackInfo(UIBattleRoomViewModel.AttackInfos info)
    {
        if (info.AttakPos % 2 == 0)
        {
            AttackEnemy = m_ViewModel.enemyList.Find(p => p.ID == info.AttakPos);
            UIAttackEnemy = m_EnemyUIInfoList.Find(p => p.ID == info.AttakPos);
        }
        else
        {
            AttackCharacter = m_ViewModel.characterList.Find(p => p.HeroID == info.AttakPos);
            UIAttackCharacter = m_CharacterUIInfoList.Find(p => p.ID == info.AttakPos);
        }
        if (info.DefendsPos % 2 == 0)
        {
            DefendsEnemy = m_ViewModel.enemyList.Find(p => p.ID == info.DefendsPos);
            UIDefendsEnemy = m_EnemyUIInfoList.Find(p => p.ID == info.DefendsPos);
        }
        else
        {
            DefendsCharacter = m_ViewModel.characterList.Find(p => p.HeroID == info.DefendsPos);
            UIDefendsCharacter = m_CharacterUIInfoList.Find(p => p.ID == info.DefendsPos);
        }
        m_AttackName = AttackCharacter != null ? AttackCharacter.Name : AttackEnemy.Name;
        m_DefendsName = DefendsCharacter != null ? DefendsCharacter.Name : DefendsEnemy.Name;
        m_AbiilityEffectValue = info.AbiilityEffectValue;
        m_AbilityName = info.AbilityName;
    }
    /// <summary>
    /// 创建一条战报
    /// </summary>
    /// <returns></returns>
    GameObject CreateReport()
    {
        GameObject m_ReportGo = Instantiate(m_AttackBattleReport.gameObject);
        m_ReportGo.transform.parent = m_AttackBattleReport.transform.parent;
        m_ReportGo.transform.localScale = Vector3.one;
        m_ReportGo.transform.SetAsFirstSibling();
        m_ReportGo.SetActive(true);
        return m_ReportGo;
    }
    /// <summary>
    /// 开始播放战报
    /// </summary>
    /// <param name="AttackInfos"></param>
    /// <returns></returns>
    private IEnumerator PlayAttackInfo(List<UIBattleRoomViewModel.AttackInfos> AttackInfos)
    {
        Log("播放战报");
        for (int i = 0; i < AttackInfos.Count; i++)
        {
            UIBattleReportScript m_ReportScript = CreateReport().GetComponent<UIBattleReportScript>();

            Debug.Log("AttackInfos[i].AttakPos" + AttackInfos[i].AttakPos);
            Debug.Log("AttackInfos[i].DefendsPos" + AttackInfos[i].DefendsPos);
            //获取战报信息
            GetAttackInfo(AttackInfos[i]);

            switch (AttackInfos[i].EffectType)
            {
                case EnumAbilityEffect.Damage:
                    if (m_AbilityName == "" || m_AbilityName == String.Empty)
                    {
                        m_ReportScript.m_Info_Txt.text = string.Format(m_NormalAttack,
                                                                        i,
                                                                        m_AttackName,
                                                                        m_DefendsName);
                        m_ReportScript.m_Info_Txt.text += string.Format(m_Damage,
                                                                        m_AbiilityEffectValue);
                    }
                    else
                    {
                        m_ReportScript.m_Info_Txt.text = string.Format(m_AbilityAttack,
                                             i,
                                             m_AttackName,
                                             m_AbilityName,
                                             m_DefendsName);
                        m_ReportScript.m_Info_Txt.text += string.Format(m_Damage,
                                                                        m_AbiilityEffectValue);
                    }
                    if (DefendsCharacter != null)
                    {
                        HPValueChanged(ref DefendsCharacter.CurrentHP, DefendsCharacter.MaxHP, m_AbiilityEffectValue, UIDefendsCharacter.CurrentHP_Txt, UIDefendsCharacter.HP_Sli, AttackInfos[i].EffectType);
                    }
                    else
                    {
                        HPValueChanged(ref DefendsEnemy.CurrentHP, DefendsEnemy.MaxHP, m_AbiilityEffectValue, UIDefendsEnemy.CurrentHP_Txt, UIDefendsEnemy.HP_Sli, AttackInfos[i].EffectType);
                    }
                    break;
                case EnumAbilityEffect.Recovery:
                    m_ReportScript.m_Info_Txt.text = string.Format(m_Recovery,
                    i,
                    m_AttackName,
                    m_AbilityName,
                    m_AbiilityEffectValue,
                    m_DefendsName);

                    if (AttackCharacter != null)
                    {
                        HPValueChanged(ref AttackCharacter.CurrentHP, AttackCharacter.MaxHP, m_AbiilityEffectValue, UIAttackCharacter.CurrentHP_Txt, UIAttackCharacter.HP_Sli, AttackInfos[i].EffectType);
                    }
                    else
                    {
                        HPValueChanged(ref AttackEnemy.CurrentHP, AttackEnemy.MaxHP, m_AbiilityEffectValue, UIAttackEnemy.CurrentHP_Txt, UIAttackEnemy.HP_Sli, AttackInfos[i].EffectType);
                    }
                    break;
                default:
                    if (m_AbilityName == "" || m_AbilityName == String.Empty)
                    {
                        m_ReportScript.m_Info_Txt.text = string.Format(m_NormalAttack,
                                                                        i,
                                                                        m_AttackName,
                                                                        m_DefendsName);
                        m_ReportScript.m_Info_Txt.text += string.Format(m_Dodge,
                                                                        m_DefendsName);
                    }
                    else
                    {
                        m_ReportScript.m_Info_Txt.text = string.Format(m_AbilityAttack,
                                             i,
                                             m_AttackName,
                                             m_AbilityName,
                                             m_DefendsName);
                        m_ReportScript.m_Info_Txt.text += string.Format(m_Dodge,
                                                                        m_DefendsName);
                    }
                    if (DefendsCharacter != null)
                    {
                        HPValueChanged(ref DefendsCharacter.CurrentHP, DefendsCharacter.MaxHP, m_AbiilityEffectValue, UIDefendsCharacter.CurrentHP_Txt, UIDefendsCharacter.HP_Sli, AttackInfos[i].EffectType);
                    }
                    else
                    {
                        HPValueChanged(ref DefendsEnemy.CurrentHP, DefendsEnemy.MaxHP, m_AbiilityEffectValue, UIDefendsEnemy.CurrentHP_Txt, UIDefendsEnemy.HP_Sli, AttackInfos[i].EffectType);
                    }
                    break;

            }
            m_ReportScript.m_DateTime_Txt.text = DateTime.Now.ToString();
            yield return new WaitForSeconds(3f);
        }
    }
    /// <summary>
    /// 倒计时
    /// </summary>
    /// <param name="time"></param>
    /// <param name="txt"></param>
    /// <returns></returns>
    private IEnumerator CountDown(float time,Text txt)
    {
        while (time > 0)
        {
            yield return null;
            txt.text = string.Format(m_FindEnemy, (int)time);
            time-= Time.deltaTime;
        }
        Log("倒计时结束");
        yield return new WaitForSeconds(1f);
        PrepareBattle();
    }
    /// <summary>
    /// 角色初始化
    /// </summary>
    /// <param name="model"></param>
    private void InitCharcterInfo()
    {
        if (m_ViewModel.characterList.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < m_ViewModel.characterList.Count; i++)
        {
            m_CharacterUIInfoList[i].Name_Txt.text = m_ViewModel.characterList[i].Name;
            m_CharacterUIInfoList[i].RocaName_Txt.text = m_ViewModel.characterList[i].RaceName;
            m_CharacterUIInfoList[i].Career_Txt.text = m_ViewModel.characterList[i].CareerName;
            m_CharacterUIInfoList[i].Level_Txt.text = m_ViewModel.characterList[i].Level.ToString();
            m_CharacterUIInfoList[i].HP_Sli.value = Mathf.Clamp(m_ViewModel.characterList[i].MaxHP, 0, m_ViewModel.characterList[i].MaxHP) / (float)m_ViewModel.characterList[i].MaxHP;
            m_CharacterUIInfoList[i].EXP_Sli.value = Mathf.Clamp(m_ViewModel.characterList[i].MaxEXP, 0, m_ViewModel.characterList[i].MaxEXP) / (float)m_ViewModel.characterList[i].MaxEXP;
            m_CharacterUIInfoList[i].CurrentHP_Txt.text = m_ViewModel.characterList[i].MaxHP.ToString();
            m_CharacterUIInfoList[i].MaxHP_Txt.text = m_ViewModel.characterList[i].MaxHP.ToString();
            m_CharacterUIInfoList[i].ID = i * 2 + 1;
            m_ViewModel.characterList[i].HeroID = i * 2 + 1;
            Log("玩家名称：" + m_ViewModel.characterList[i].Name + "\n" +
                "种族名称：" + m_ViewModel.characterList[i].RaceName + "\n" +
                "职业名称：" + m_ViewModel.characterList[i].CareerName + "\n" +
                "等级：" + m_ViewModel.characterList[i].Level.ToString() + "\n" +
                "最大红量：" + m_ViewModel.characterList[i].MaxHP + "\n" +
                "当前红量：" + m_CharacterUIInfoList[i].CurrentHP_Txt.text + "\n"
                );
        }
    }
    /// <summary>
    /// 敌人初始化
    /// </summary>
    /// <param name="model"></param>
    private void InitEnemyInfo()
    {
        if (m_ViewModel == null || m_ViewModel.enemyList.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < m_ViewModel.enemyList.Count; i++)
        {
            m_EnemyUIInfoList[i].Name_Txt.text = m_ViewModel.enemyList[i].Name;
            m_EnemyUIInfoList[i].Level_Txt.text = m_ViewModel.enemyList[i].Level.ToString();
            m_EnemyUIInfoList[i].HP_Sli.value = Mathf.Clamp(m_ViewModel.enemyList[i].MaxHP, 0, m_ViewModel.enemyList[i].MaxHP) / (float)m_ViewModel.enemyList[i].MaxHP; ;
            m_EnemyUIInfoList[i].CurrentHP_Txt.text = m_ViewModel.enemyList[i].MaxHP.ToString();
            m_EnemyUIInfoList[i].MaxHP_Txt.text = m_ViewModel.enemyList[i].MaxHP.ToString();
            m_EnemyUIInfoList[i].ID = i * 2 + 2;
            m_ViewModel.enemyList[i].ID = i * 2 + 2;
        }
    }
}
public class UIBattleRoomViewModel
{
    /// <summary>
    /// 战斗角色信息
    /// </summary>
    public class BattleCharacter
    {
        /// <summary>
        /// 玩家ID
        /// </summary>
        public int HeroID;
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 种族名称
        /// </summary>
        public string RaceName;
        /// <summary>
        /// 职业名称
        /// </summary>
        public string CareerName;
        /// <summary>
        /// 角色等级
        /// </summary>
        public uint Level;
        /// <summary>
        /// 角色最大血量
        /// </summary>
        public int MaxHP;
        /// <summary>
        /// 角色当前血量
        /// </summary>
        public int CurrentHP;
        /// <summary>
        /// 角色最大经验
        /// </summary>
        public uint MaxEXP;
        /// <summary>
        /// 角色当前经验
        /// </summary>
        public uint CurrentEXP;
    }
    /// <summary>
    /// 战斗敌人信息
    /// </summary>
    public class BattleEnemy
    {
        public class AbilityInfo
        {
            public string Name;
            public byte AbilityType;
        }
        /// <summary>
        /// 敌人ID
        /// </summary>
        public int ID;
        /// <summary>
        /// 敌人名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 敌人等级
        /// </summary>
        public ushort Level;
        /// <summary>
        /// 敌人血量
        /// </summary>
        public int MaxHP;
        /// <summary>
        /// 当前血量
        /// </summary>
        public int CurrentHP;
        /// <summary>
        /// 敌人技能信息
        /// </summary>
        public List<AbilityInfo> AbilityInfoList = new List<AbilityInfo>();
    }
    /// <summary>
    /// 一条战报信息
    /// </summary>
    public class AttackInfos
    {
        public byte AttakPos;
        public byte DefendsPos;
        public string AbilityName;
        public int AbiilityEffectValue;
        public EnumAbilityEffect EffectType;
    }
    /// <summary>
    /// 战报信息列表
    /// </summary>
    public class BattleReportModel
    {
        /// <summary>
        /// 休息时间
        /// </summary>
        public int RestTime;
        /// <summary>
        /// 获取金币
        /// </summary>
        public int GoldRewards;
        /// <summary>
        /// 输赢
        /// </summary>
        public bool IsWin;
        /// <summary>
        /// 寻找怪物的时间
        /// </summary>
        public int FindEnemyTime;
        /// <summary>
        /// 地图名称
        /// </summary>
        public string MapName;
        ///// <summary>
        ///// 队伍名称
        ///// </summary>
        public string TeamName;
        /// <summary>
        /// 物品奖励
        /// </summary>
        public List<string> ItemRewards = new List<string>();
        /// <summary>
        /// 战报信息
        /// </summary>
        public List<AttackInfos> AttackInfoList = new List<AttackInfos>();
    }
    /// <summary>
    /// 角色集合
    /// </summary>
    public List<BattleCharacter> characterList = new List<BattleCharacter>();
    /// <summary>
    /// 敌人集合
    /// </summary>
    public List<BattleEnemy> enemyList = new List<BattleEnemy>();
    /// <summary>
    /// 战报集合
    /// </summary>
    public BattleReportModel BattleReportList = new BattleReportModel();
}
