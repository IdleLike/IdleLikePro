using SUIFW;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panel
{
    public class BattleRoomModel
    {
        [Serializable]
        public struct BattleCharacterModel
        {
            /// <summary>
            /// 角色ID
            /// </summary>
            public byte ID;
            /// <summary>
            /// 角色名称
            /// </summary>
            public string Name;
            /// <summary>
            /// 角色种族
            /// </summary>
            public string RocaName;
            /// <summary>
            /// 角色职业
            /// </summary>
            public string Career;
            /// <summary>
            /// 角色等级
            /// </summary>
            public byte Level;
            /// <summary>
            /// 角色血量
            /// </summary>
            public ushort CurrentHP;
            /// <summary>
            /// 角色最大血量
            /// </summary>
            public ushort MaxHP;
            /// <summary>
            /// 角色最大魔法
            /// </summary>
            public ushort MaxMP_Txt;
            /// <summary>
            /// 角色当前魔法
            /// </summary>
            public ushort CurrentMP_Txt;
        }
        [Serializable]
        public struct BattleEnemyModel
        {
            /// <summary>
            /// 敌人ID
            /// </summary>
            public byte ID;
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
            public ushort CurrentHP;
            /// <summary>
            /// 敌人血量
            /// </summary>
            public ushort MaxHP;
            /// <summary>
            /// 敌人技能悬停显示
            /// </summary>
            public Sprite Ability_sprite;
            /// <summary>
            /// 技能回调
            /// </summary>
            public Action<string> AbilityCallback;
        }
        public enum ReportType
        {
            Attack,
            Rest,
            Result
        }
        //public enum AdditionalEntry
        //{
        //    AOE,
        //    Equipment,
        //    Buff
        //}
        [Serializable]
        public class Equipment
        {
            public string name;
        }
        [Serializable]
        public class Buff
        {
            public string name;
        }
        public enum AttackHandleType
        {
            Dodge,
            Deathblow,
            Attack
        }
        public enum AOEType
        {
            Attack,
            Recovery
        }
        [Serializable]
        public class BattleReportModel
        {
            public string TeamName;
            public bool IsDerateDamage;
            public ushort DerateDamage;
            public byte Attacker;
            public byte AnAttacker;
            public string AbilityName;
            public ReportType ReportType;
            //public AdditionalEntry AdditionalEntry;
            public AttackHandleType AttackHandleType;
            public AOEType AOEType;
            public bool IsAOE;
            public bool IsGroupAttack;
            public bool IsSussces;
            public ushort ReportNum;
            public ushort Gold;
            public ushort Damage;
            public byte RestCountdown;
            public List<Buff> BuffList;
            public List<Equipment> EquipmentList;
            public ushort Recovery;
            public uint EXP;
        }

        /// <summary>
        /// 地图名称
        /// </summary>
        public string MapName;
        /// <summary>
        /// 房间名称
        /// </summary>
        public string RoomName;


        /// <summary>
        /// 角色集合
        /// </summary>
        //public List<BattleCharacterModel> characterList { get; set; }
        public List<HeroEntity> characterList { get; set; }
        /// <summary>
        /// 敌人集合
        /// </summary>
        public List<HeroEntity> enemyList { get; set; }
        /// <summary>
        /// 战报集合
        /// </summary>
        public Queue<BattleReportModel> ReportQueue { get; set; }
    }
    public class BattleRoomPanel : BaseUIForm
    {
        #region
        ///// <summary>
        ///// 角色一名称
        ///// </summary>
        //public Text CharacterOneName_Txt;
        ///// <summary>
        ///// 角色一种族
        ///// </summary>
        //public Text CharacterOneRocaName_Txt;
        ///// <summary>
        ///// 角色一职业
        ///// </summary>
        //public Text CharacterOneCareer_Txt;
        ///// <summary>
        ///// 角色一等级
        ///// </summary>
        //public Text CharacterOneLevel_Txt;
        ///// <summary>
        ///// 角色一血量
        ///// </summary>
        //public Text CharacterOneHP_Txt;
        ///// <summary>
        ///// 角色一血条
        ///// </summary>
        //public Slider CharacterOneHP_Sli;
        ///// <summary>
        ///// 角色一蓝条
        ///// </summary>
        //public Slider CharacterOneMP_Sli;

        ///// <summary>
        ///// 角色二名称
        ///// </summary>
        //public Text CharacterTwoName_Txt;
        ///// <summary>
        ///// 角色二种族
        ///// </summary>
        //public Text CharacterTwoRocaName_Txt;
        ///// <summary>
        ///// 角色二职业
        ///// </summary>
        //public Text CharacterTwoCareer_Txt;
        ///// <summary>
        ///// 角色二等级
        ///// </summary>
        //public Text CharacterTwoLevel_Txt;
        ///// <summary>
        ///// 角色二血量
        ///// </summary>
        //public Text CharacterTwoHP_Txt;
        ///// <summary>
        ///// 角色二血条
        ///// </summary>
        //public Slider CharacterTwoHP_Sli;
        ///// <summary>
        ///// 角色二蓝条
        ///// </summary>
        //public Slider CharacterTwoMP_Sli;

        ///// <summary>
        ///// 角色三名称
        ///// </summary>
        //public Text CharacterThreeName_Txt;
        ///// <summary>
        ///// 角色三种族
        ///// </summary>
        //public Text CharacterThreeRocaName_Txt;
        ///// <summary>
        ///// 角色三职业
        ///// </summary>
        //public Text CharacterThreeCareer_Txt;
        ///// <summary>
        ///// 角色三等级
        ///// </summary>
        //public Text CharacterThreeLevel_Txt;
        ///// <summary>
        ///// 角色三血量
        ///// </summary>
        //public Text CharacterThreeHP_Txt;
        ///// <summary>
        ///// 角色三血条
        ///// </summary>
        //public Slider CharacterThreeHP_Sli;
        ///// <summary>
        ///// 角色三蓝条
        ///// </summary>
        //public Slider CharacterThreeMP_Sli;

        ///// <summary>
        ///// 敌人一名称
        ///// </summary>
        //public Text EnemyOneName_Txt;
        ///// <summary>
        ///// 敌人一等级
        ///// </summary>
        //public Text EnemyOneLevel_Txt;
        ///// <summary>
        ///// 敌人一血量
        ///// </summary>
        //public Text EnemyOneHP_Txt;
        ///// <summary>
        ///// 敌人一血条
        ///// </summary>
        //public Slider EnemyOneHP_Sli;
        ///// <summary>
        ///// 敌人一技能悬停显示
        ///// </summary>
        //public Image EnemyOneAbility_Img;

        ///// <summary>
        ///// 敌人二名称
        ///// </summary>
        //public Text EnemyTwoName_Txt;
        ///// <summary>
        ///// 敌人二等级
        ///// </summary>
        //public Text EnemyTwoLevel_Txt;
        ///// <summary>
        ///// 敌人二血量
        ///// </summary>
        //public Text EnemyTwoHP_Txt;
        ///// <summary>
        ///// 敌人二血条
        ///// </summary>
        //public Slider EnemyTwoHP_Sli;
        ///// <summary>
        ///// 敌人二技能悬停显示
        ///// </summary>
        //public Image EnemyTwoAbility_Img;

        ///// <summary>
        ///// 敌人三名称
        ///// </summary>
        //public Text EnemyThreeName_Txt;
        ///// <summary>
        ///// 敌人三等级
        ///// </summary>
        //public Text EnemyThreeLevel_Txt;
        ///// <summary>
        ///// 敌人三血量
        ///// </summary>
        //public Text EnemyThreeHP_Txt;
        ///// <summary>
        ///// 敌人三血条
        ///// </summary>
        //public Slider EnemyThreeHP_Sli;
        ///// <summary>
        ///// 敌人三技能悬停显示
        ///// </summary>
        //public Image EnemyThreeAbility_Img;
        #endregion

        /// <summary>
        /// 地图名称
        /// </summary>
        public Text MapName_Txt;
        /// <summary>
        /// 房间名称
        /// </summary>
        public Text RoomName_Txt;
        /// <summary>
        /// 战报容器(父物体)
        /// </summary>
        public Transform Content_Tra;

        public List<CharacterUIInfo> CharacterUIInfoList;
        [Serializable]
        public class CharacterUIInfo
        {
            /// <summary>
            /// 角色ID
            /// </summary>
            public byte ID;
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
            /// 角色最大魔法
            /// </summary>
            public ushort MaxMP_Txt;
            /// <summary>
            /// 角色当前魔法
            /// </summary>
            public ushort CurrentMP_Txt;
            /// <summary>
            /// 角色血条
            /// </summary>
            public Slider HP_Sli;
            /// <summary>
            /// 角色蓝条
            /// </summary>
            public Slider MP_Sli;
        }

        public List<EnemyUIInfo> EnemyUIInfoList;
        [Serializable]
        public class EnemyUIInfo
        {
            /// <summary>
            /// 敌人ID
            /// </summary>
            public byte ID;
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
            ///// 敌人技能悬停显示
            ///// </summary>
            public Image Ability_Img;
        }

        private bool IsInit = false;


        private string m_DerateDamageInfoTemplete = "，对方减免了{0}伤害";
        private string m_NormalAttackInfoTemplete = "{0}.{1}攻击了{2}";
        private string m_SkillInfoTemplete = "{0}.{1}使用技能{2}，攻击了{3}";
        private string m_AOEInfoTemplete = "\n{0}受到{1}AOE伤害";
        private string m_DodgeInfoTemplete = "，但被对方闪避，{0}没有受到直接伤害";
        private string m_RestInfoTemplete = "战斗刚结束，正在休息恢复中，并搜寻敌人...({0})";
        private string m_SuccessResultInfoTemplete = "\n敌方被击败，战斗结束...\n";
        private string m_GetGoldInfoTemplete = "\n团队收获金币{0}g\n\n";
        private string m_GetEquipmentInfoTemplete = "得到装备.[{0}]\n";
        private string m_GetEXPInfoTemplete = "{0}获得了经验{1}Exp\n";
        private string m_FailureResultInfoTemplete = "{0}被击败，战斗结束...";
        private string m_DeathblowInfoTemplete = "，触发了致命一击";
        private string m_DamageInfoTemplete = "，造成了{0}伤害";


        private string m_BuffInfoTemplete = "{0} {1}，受到伤害{2}，持续{3}轮";
        private string m_BeatInfoTemplete = "{0}被击败了";

        private string m_RecoveryInfoTemplete = "{0}.{1}使用技能{2}，恢复了自己{3}HP，{4}没有受到直接伤害";
        private string m_GroupRecoveryInfoTemplete = "{0}恢复了{1}HP";




        /// <summary>
        /// 初始化战斗信息
        /// </summary>
        /// <param name="viewModel"></param>
        private void InitBattleInfo(object viewModel)
        {
            if (viewModel == null)
            {
                return;
            }
            BattleRoomModel BattleModel = (BattleRoomModel)viewModel;
            BattleRoomModel.BattleReportModel CurrentBattleReport;

            if (IsInit)
            {
                Debug.Log("已经初始化");
                if (BattleModel.ReportQueue != null && BattleModel.ReportQueue.Count > 0)
                {
                    Debug.Log("战报不等于空");
                    CurrentBattleReport = BattleModel.ReportQueue.Dequeue();
                    InitReportInfo(BattleModel, CurrentBattleReport);
                }
                else
                {
                    return;
                }
            }
            else
            {
                Debug.Log("第一次初始化");
                MapName_Txt.text = BattleModel.MapName;
                RoomName_Txt.text = BattleModel.RoomName;

                InitCharcterInfo(BattleModel);
                //TODO 初始化怪物
                //InitEnemyInfo(BattleModel);
            }
        }

        private void InitReportInfo(BattleRoomModel battleModel, BattleRoomModel.BattleReportModel CurrentBattleReport)
        {

            switch (CurrentBattleReport.ReportType)
            {
                case BattleRoomModel.ReportType.Attack:
                    Debug.Log("攻击战报");
                    if (Content_Tra != null)
                    {

                        GameObject m_ReportGoParent = ResourcesMgr.GetInstance().LoadResource<GameObject>("Prefab/UI/Battle/AttackReport", false);

                        GameObject m_ReportParent = Instantiate(m_ReportGoParent);
                        Text m_Report = UnityHelper.GetTheChildNodeComponetScripts<Text>(m_ReportParent, "AttackReport_Txt");
                        string m_Info;
                        if (!CurrentBattleReport.IsAOE)
                        {
                            switch (CurrentBattleReport.AttackHandleType)
                            {
                                case BattleRoomModel.AttackHandleType.Dodge:
                                    m_Report.text = string.Format(m_NormalAttackInfoTemplete,
                                                          CurrentBattleReport.ReportNum,
                                                          GetAttacker(battleModel, CurrentBattleReport),
                                                          GetAnAttacker(battleModel, CurrentBattleReport)) +
                                                          string.Format(m_DodgeInfoTemplete, GetAnAttacker(battleModel, CurrentBattleReport));
                                    break;
                                case BattleRoomModel.AttackHandleType.Deathblow:
                                    m_Info = string.Format(m_NormalAttackInfoTemplete,
                                                         CurrentBattleReport.ReportNum,
                                                         GetAttacker(battleModel, CurrentBattleReport),
                                                         GetAnAttacker(battleModel, CurrentBattleReport)) +
                                                         string.Format(m_DeathblowInfoTemplete) +
                                                         string.Format(m_DamageInfoTemplete,
                                                                          CurrentBattleReport.Damage);
                                    m_Report.text = m_Info;
                                    if (CurrentBattleReport.IsDerateDamage)
                                    {
                                        m_Report.text = m_Info + string.Format(m_DerateDamageInfoTemplete, CurrentBattleReport.DerateDamage);
                                    }
                                    break;
                                case BattleRoomModel.AttackHandleType.Attack:
                                    m_Info = string.Format(m_NormalAttackInfoTemplete,
                                                          CurrentBattleReport.ReportNum,
                                                          GetAttacker(battleModel, CurrentBattleReport),
                                                          GetAnAttacker(battleModel, CurrentBattleReport)) +
                                                          string.Format(m_DamageInfoTemplete,
                                                                          CurrentBattleReport.Damage);
                                    m_Report.text = m_Info;
                                    if (CurrentBattleReport.IsDerateDamage)
                                    {
                                        m_Report.text = m_Info + string.Format(m_DerateDamageInfoTemplete, CurrentBattleReport.DerateDamage);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            StringBuilder sb = new StringBuilder();
                            switch (CurrentBattleReport.AttackHandleType)
                            {
                                case BattleRoomModel.AttackHandleType.Dodge:
                                    m_Report.text = string.Format(m_SkillInfoTemplete,
                                                          CurrentBattleReport.ReportNum,
                                                          GetAttacker(battleModel, CurrentBattleReport),
                                                          CurrentBattleReport.AbilityName,
                                                          GetAnAttacker(battleModel, CurrentBattleReport)) +
                                                          string.Format(m_DodgeInfoTemplete, GetAnAttacker(battleModel, CurrentBattleReport));
                                    break;
                                case BattleRoomModel.AttackHandleType.Deathblow:
                        
                                    sb.Append(string.Format(m_SkillInfoTemplete,
                                                         CurrentBattleReport.ReportNum,
                                                         GetAttacker(battleModel, CurrentBattleReport),
                                                         CurrentBattleReport.AbilityName,
                                                         GetAnAttacker(battleModel, CurrentBattleReport)) +
                                                         string.Format(m_DeathblowInfoTemplete) +
                                                         string.Format(m_DamageInfoTemplete,
                                                                          CurrentBattleReport.Damage));
                                    m_Report.text = sb.ToString();
                                    if (CurrentBattleReport.IsDerateDamage)
                                    {
                                        m_Report.text = sb.Append(string.Format(m_DerateDamageInfoTemplete, CurrentBattleReport.DerateDamage)).ToString();
                                    }
                                    if (CurrentBattleReport.IsGroupAttack)
                                    {
                                        //List<BattleRoomModel.BattleCharacterModel> CharacterModelList = GetTeam(battleModel, battleModel.characterList, CurrentBattleReport);
                                        List<HeroEntity> CharacterModelList = GetTeam(battleModel, battleModel.characterList, CurrentBattleReport);

                                        List<HeroEntity> EnemyModelList = GetTeam(battleModel, battleModel.enemyList, CurrentBattleReport);
                                        if (CharacterModelList.Count > 0 && CharacterModelList != null)
                                        {
                                            for (int i = 0; i < CharacterModelList.Count; i++)
                                            {
                                                Debug.Log(CharacterModelList[i].Name);
                                                m_Report.text = sb.Append(
                                                    string.Format(m_AOEInfoTemplete, CharacterModelList[i].Name, CurrentBattleReport.Damage)).ToString();
                                            }
                                        }
                                        else if (EnemyModelList.Count > 0 && EnemyModelList != null)
                                        {
                                            for (int i = 0; i < EnemyModelList.Count; i++)
                                            {
                                                m_Report.text = sb.Append(
                                                    string.Format(m_AOEInfoTemplete, EnemyModelList[i].Name, CurrentBattleReport.Damage)).ToString();
                                            }
                                        }
                                    }

                                    break;
                                case BattleRoomModel.AttackHandleType.Attack:
  
                                    switch (CurrentBattleReport.AOEType)
                                    {
                                        case BattleRoomModel.AOEType.Attack:
                                            sb.Append(string.Format(m_SkillInfoTemplete,
                                                        CurrentBattleReport.ReportNum,
                                                        GetAttacker(battleModel, CurrentBattleReport),
                                                        CurrentBattleReport.AbilityName,
                                                        GetAnAttacker(battleModel, CurrentBattleReport)) +
                                                        string.Format(m_DamageInfoTemplete,
                                                                        CurrentBattleReport.Damage));
                                            m_Report.text = sb.ToString();
                                            if (CurrentBattleReport.IsDerateDamage)
                                            {
                                                m_Report.text = sb.Append(string.Format(m_DerateDamageInfoTemplete, CurrentBattleReport.DerateDamage)).ToString(); ;
                                            }
                                            if (CurrentBattleReport.IsGroupAttack)
                                            {
                                                List<HeroEntity> CharacterModelList = GetTeam(battleModel, battleModel.characterList, CurrentBattleReport);
                                                List<HeroEntity> EnemyModelList = GetTeam(battleModel, battleModel.enemyList, CurrentBattleReport);
                                                if (CharacterModelList.Count > 0 && CharacterModelList != null)
                                                {
                                                    for (int i = 0; i < CharacterModelList.Count; i++)
                                                    {
                                                        Debug.Log(CharacterModelList[i].Name);
                                                        m_Report.text = sb.Append(
                                                            string.Format(m_AOEInfoTemplete, CharacterModelList[i].Name, CurrentBattleReport.Damage)).ToString();
                                                    }
                                                }
                                                else if (EnemyModelList.Count > 0 && EnemyModelList != null)
                                                {
                                                    for (int i = 0; i < EnemyModelList.Count; i++)
                                                    {
                                                        m_Report.text = sb.Append(
                                                            string.Format(m_AOEInfoTemplete, EnemyModelList[i].Name, CurrentBattleReport.Damage)).ToString();
                                                    }
                                                }
                                            }
                                            break;
                                        case BattleRoomModel.AOEType.Recovery:
                                            m_Report.text = string.Format(m_RecoveryInfoTemplete,
                                                        CurrentBattleReport.ReportNum,
                                                        GetAttacker(battleModel, CurrentBattleReport),
                                                        CurrentBattleReport.AbilityName,
                                                        CurrentBattleReport.Recovery,
                                                        GetAnAttacker(battleModel, CurrentBattleReport));
                                            break;
                                        default:
                                            break;
                                    }

                                    break;
                                default:
                                    break;
                            }
                        }
                        Text Data = UnityHelper.GetTheChildNodeComponetScripts<Text>(m_ReportParent, "Data_Txt");
                        Data.text = DateTime.Now.ToString();
                        //扣血
                        m_ReportParent.GetComponent<RectTransform>().sizeDelta = new Vector2(m_Report.rectTransform.rect.width, m_Report.preferredHeight + Data.preferredHeight);
                        m_ReportParent.transform.parent = Content_Tra.transform;

                        m_ReportParent.transform.localScale = Content_Tra.transform.localScale;
                        m_ReportParent.transform.SetAsFirstSibling();
                    }
                    break;
                case BattleRoomModel.ReportType.Result:
                    Debug.Log("结束战报");
                    if (Content_Tra != null)
                    {
                        GameObject m_ReportGoParent = ResourcesMgr.GetInstance().LoadResource<GameObject>("Prefab/UI/Battle/ResultReport", false);
                        GameObject m_ReportParent = Instantiate(m_ReportGoParent);
                        Text m_Report = UnityHelper.GetTheChildNodeComponetScripts<Text>(m_ReportParent, "ResultReport_Txt");
                        if (CurrentBattleReport.IsSussces)
                        {
                            //扣血
                            StringBuilder sb = new StringBuilder();

                            if (CurrentBattleReport.EquipmentList.Count > 0)
                            {
                                for (int i = 0; i < CurrentBattleReport.EquipmentList.Count; i++)
                                {
                                    sb.Append(string.Format(m_GetEquipmentInfoTemplete, CurrentBattleReport.EquipmentList[i].name));
                                }
                            }
                            if (CurrentBattleReport.Gold > 0)
                            {
                                sb.Append(string.Format(m_GetGoldInfoTemplete, CurrentBattleReport.Gold));
                            }
                            if (battleModel.characterList != null)
                            {
                                for (int i = 0; i < battleModel.characterList.Count; i++)
                                {
                                    sb.Append(string.Format(m_GetEXPInfoTemplete, battleModel.characterList[i].Name, CurrentBattleReport.EXP));
                                }
                            }
                            sb.Append(m_SuccessResultInfoTemplete);
                            m_Report.text = sb.ToString();
                        }
                        else
                        {
                            m_Report.text = string.Format(m_FailureResultInfoTemplete, CurrentBattleReport.TeamName);
                        }
                        Debug.Log("m_Report.rectTransform.rect.height = " + m_Report.preferredHeight);
                        Debug.Log("m_ReportParent.GetComponent<RectTransform>().rect.height) = " + m_ReportParent.GetComponent<RectTransform>().rect.height);

                        m_ReportParent.transform.parent = Content_Tra.transform;
                        m_ReportParent.transform.localScale = m_ReportParent.transform.parent.transform.localScale;
                        m_ReportParent.GetComponent<RectTransform>().sizeDelta = new Vector2(m_Report.rectTransform.rect.width, m_Report.preferredHeight);
                        m_ReportParent.transform.SetAsFirstSibling();
                        Debug.Log("m_Report.rectTransform.rect.height = " + m_Report.rectTransform.rect.height + m_Report.name);
                        Debug.Log("m_ReportParent.GetComponent<RectTransform>().rect.height) = " + m_ReportParent.GetComponent<RectTransform>().rect.height + m_ReportParent.name);
                    }
                    break;
                case BattleRoomModel.ReportType.Rest:
                    Debug.Log("休息战报");
                    if (Content_Tra != null)
                    {
                        GameObject m_ReportGoParent = ResourcesMgr.GetInstance().LoadResource<GameObject>("Prefab/UI/Battle/RestReport", false);
                        GameObject m_ReportParent = Instantiate(m_ReportGoParent);
                        Text m_Report = UnityHelper.GetTheChildNodeComponetScripts<Text>(m_ReportParent, "RestReport_Txt");

                        m_Report.text = string.Format(m_RestInfoTemplete, CurrentBattleReport.RestCountdown);

                        //扣血
                        m_ReportParent.GetComponent<RectTransform>().sizeDelta = new Vector2(m_Report.rectTransform.rect.width, m_Report.rectTransform.rect.height);
                        m_ReportParent.transform.parent = Content_Tra.transform;
                        m_ReportParent.transform.localScale = m_ReportParent.transform.parent.transform.localScale;
                        m_ReportParent.transform.SetAsFirstSibling();
                    }
                    break;
                default:
                    Debug.LogError("战报类型错误:" + CurrentBattleReport.ReportType);
                    break;
            }
        }

        private string GetAnAttacker(BattleRoomModel battleModel, BattleRoomModel.BattleReportModel CurrentBattleReport)
        {
            foreach (var item in battleModel.characterList)
            {
                if (item.ID == CurrentBattleReport.AnAttacker)
                {
                    return item.Name;
                }
            }
            foreach (var item in battleModel.enemyList)
            {
                if (item.ID == CurrentBattleReport.AnAttacker)
                {
                    return item.Name;
                }
            }
            return String.Empty;
        }
        private List<HeroEntity> GetTeam(BattleRoomModel battleModle, List<HeroEntity> list, BattleRoomModel.BattleReportModel CurrentBattleReport)
        {

            List<HeroEntity> m_CharacterModelList = list.FindAll(p => p.ID == CurrentBattleReport.AnAttacker);
            if (m_CharacterModelList != null && m_CharacterModelList.Count > 0)
            {
                List<HeroEntity> modle = battleModle.characterList;
                for (int i = 0; i < m_CharacterModelList.Count; i++)
                {
                    modle.Remove(m_CharacterModelList[i]);
                }
                
                Debug.Log(modle.Count);
                foreach (var item in modle)
                {
                    Debug.Log(item.Name);
                }
                return modle;
            }
            return null;
        }
        //private List<HeroEntity> GetTeam(BattleRoomModel battleModle, List<HeroEntity> list, BattleRoomModel.BattleReportModel CurrentBattleReport)
        //{

        //    List<HeroEntity> m_EnemyModelList = list.FindAll(p => p.ID == CurrentBattleReport.AnAttacker);
        //    if (m_EnemyModelList != null && m_EnemyModelList.Count > 0)
        //    {
        //        List<HeroEntity> modle = battleModle.enemyList;
        //        for (int i = 0; i < m_EnemyModelList.Count; i++)
        //        {
        //            modle.Remove(m_EnemyModelList[i]);
        //        }

        //        Debug.Log(modle.Count);
        //        foreach (var item in modle)
        //        {
        //            Debug.Log(item.Name);
        //        }
        //        return modle;
        //    }
        //    return null;
        //}
        private string GetAttacker(BattleRoomModel battleModel, BattleRoomModel.BattleReportModel CurrentBattleReport)
        {
            foreach (var item in battleModel.characterList)
            {
                if (item.ID == CurrentBattleReport.Attacker)
                {
                    return item.Name;
                }
            }
            foreach (var item in battleModel.enemyList)
            {
                if (item.ID == CurrentBattleReport.Attacker)
                {
                    return item.Name;
                }
            }
            return String.Empty;
        }



        /// <summary>
        /// 角色初始化
        /// </summary>
        /// <param name="model"></param>
        private void InitCharcterInfo(BattleRoomModel model)
        {
            #region
            //CharacterOneName_Txt.text = model.characterList[0].Name;
            //CharacterOneRocaName_Txt.text = model.characterList[0].RocaName;
            //CharacterOneCareer_Txt.text = model.characterList[0].Career;
            //CharacterOneLevel_Txt.text = model.characterList[0].Level.ToString();
            //CharacterOneHP_Txt.text = model.characterList[0].HP.ToString();
            //CharacterOneHP_Sli.value = 1;
            //CharacterOneMP_Sli.value = 1;

            //CharacterTwoName_Txt.text = model.characterList[1].Name;
            //CharacterTwoRocaName_Txt.text = model.characterList[1].RocaName;
            //CharacterTwoCareer_Txt.text = model.characterList[1].Career;
            //CharacterTwoLevel_Txt.text = model.characterList[1].Level.ToString();
            //CharacterTwoHP_Txt.text = model.characterList[1].HP.ToString();
            //CharacterTwoHP_Sli.value = 1;
            //CharacterTwoMP_Sli.value = 1;

            //CharacterThreeName_Txt.text = model.characterList[2].Name;
            //CharacterThreeRocaName_Txt.text = model.characterList[2].RocaName;
            //CharacterThreeCareer_Txt.text = model.characterList[2].Career;
            //CharacterThreeLevel_Txt.text = model.characterList[2].Level.ToString();
            //CharacterThreeHP_Txt.text = model.characterList[2].HP.ToString();
            //CharacterThreeHP_Sli.value = 1;
            //CharacterThreeMP_Sli.value = 1;
            #endregion
            if (model == null || model.characterList.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < model.characterList.Count; i++)
            {
                //TODO 当前HP
                CharacterUIInfoList[i].Name_Txt.text = model.characterList[i].Name;
                CharacterUIInfoList[i].RocaName_Txt.text = model.characterList[i].RaceData.Name;
                CharacterUIInfoList[i].Career_Txt.text = model.characterList[i].CareerData.Name;
                CharacterUIInfoList[i].Level_Txt.text = model.characterList[i].Level.ToString();
                CharacterUIInfoList[i].HP_Sli.value = Mathf.Clamp(model.characterList[i].MaxHP, 0, model.characterList[i].MaxHP) / (float)model.characterList[i].MaxHP;
                CharacterUIInfoList[i].MP_Sli.value = Mathf.Clamp(model.characterList[i].MaxMP, 0, model.characterList[i].MaxMP) / (float)model.characterList[i].MaxMP;
                CharacterUIInfoList[i].CurrentHP_Txt.text = model.characterList[i].MaxHP.ToString();
                CharacterUIInfoList[i].MaxHP_Txt.text = model.characterList[i].MaxMP.ToString();

                Log("玩家名称：" + model.characterList[i].Name + "\n" +
                    "种族名称：" + model.characterList[i].RaceData.Name + "\n" +
                    "职业名称：" + model.characterList[i].CareerData.Name + "\n" +
                    "等级：" + model.characterList[i].Level.ToString() + "\n" +
                    "最大红量：" + model.characterList[i].MaxHP + "\n" + 
                    "最大蓝量：" + model.characterList[i].MaxMP + "\n" + 
                    "当前红量：" + CharacterUIInfoList[i].CurrentHP_Txt.text + "\n" + 
                    "当前蓝量：" + CharacterUIInfoList[i].MaxHP_Txt.text
                    );
            }
            IsInit = true;
        }
        /// <summary>
        /// 敌人初始化
        /// </summary>
        /// <param name="model"></param>
        private void InitEnemyInfo(BattleRoomModel model)
        {
            if (model == null || model.enemyList.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < model.enemyList.Count; i++)
            {
                //TODO 当前HP
                EnemyUIInfoList[i].Name_Txt.text = model.enemyList[i].Name;
                EnemyUIInfoList[i].Level_Txt.text = model.enemyList[i].Level.ToString();
                EnemyUIInfoList[i].HP_Sli.value = Mathf.Clamp(model.enemyList[i].MaxHP, 0, model.enemyList[i].MaxHP) / (float)model.enemyList[i].MaxHP; ;
                EnemyUIInfoList[i].CurrentHP_Txt.text = model.enemyList[i].MaxHP.ToString();
                EnemyUIInfoList[i].MaxHP_Txt.text = model.enemyList[i].MaxHP.ToString();
                EnemyUIInfoList[i].Ability_Img.sprite = ResourcesMgr.GetInstance().LoadResource<Sprite>( model.enemyList[i].AbilityIco,false);
            }
            IsInit = true;
        }

        BattleRoomModel battleRoomModel;
        public override void UpdatePanel(object viewModel)
        {
            battleRoomModel = (BattleRoomModel)viewModel;
            InitBattleInfo(viewModel);
        }

        public void Test()
        {
            StartCoroutine(Execute());

            //赋值数据


        }

        private IEnumerator Execute()
        {
            yield return new WaitForSeconds(1);
            InitBattleInfo(battleRoomModel);
        }
    }
}
