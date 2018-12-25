using NetData.Enumeration;
using SUIFW;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIBattleRoomView : BaseUIForm
{
    private int m_AttackNum = 0;
    private int m_RoundNum = 0;
    private string m_AttackName = "";
    private string m_DefendsName = "";
    private int m_AbiilityEffectValue = 0;
    private string m_AbilityName = "";

    private class BattleReportInfo
    {
        public string m_DateTime;
        public string m_Info;
        public int m_RoundNum;
    }
    private List<BattleReportInfo> m_BattleReportInfo_List = new List<BattleReportInfo>();

    public UIBattleRoomViewModel m_ViewModel = new UIBattleRoomViewModel();
    
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
                GameObject m_ReportGo = Instantiate(m_AttackBattleReport.gameObject);
                UIBattleReportScript m_ReportScript = m_ReportGo.GetComponent<UIBattleReportScript>();
                m_ReportScript.m_DateTime_Txt.text = "";
                m_ReportGo.transform.parent = m_AttackBattleReport.transform.parent;
                m_ReportGo.transform.localScale = Vector3.one;
                m_ReportGo.SetActive(true);
                StartCoroutine(CountDown(FindEnemyTime, m_ReportScript.m_Info_Txt));
                //while (FindEnemyTime > 0)
                //{
                //    yield return new WaitForSeconds(1);
                //    m_ReportScript.m_Info_Txt.text = string.Format(m_FindEnemy, FindEnemyTime);
                //    FindEnemyTime--;
                //}
            }
            else
            {
                InitEnemyInfo();
                List<UIBattleRoomViewModel.AttackInfos> AttackInfos = m_ViewModel.BattleReportList.AttackInfoList;
                StartCoroutine(PlayAttackInfo(AttackInfos));
            }
        
            //while (m_BattleReportInfo_List.Count > 0)
            //{
            //    m_BattleReportInfo_List.RemoveAt(0);
            //    StartCoroutine(BattleReportUpdate(m_AttackNum));
            //    m_AttackNum++;
            //}

        }
    }

    private IEnumerator PlayAttackInfo(List<UIBattleRoomViewModel.AttackInfos> AttackInfos)
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < AttackInfos.Count; i++)
        {

            GameObject m_ReportGo = Instantiate(m_AttackBattleReport.gameObject);
            m_ReportGo.transform.parent = m_AttackBattleReport.gameObject.transform;
            UIBattleReportScript m_ReportScript = m_ReportGo.GetComponent<UIBattleReportScript>();

            m_BattleReportInfo_List = new List<BattleReportInfo>();

            UIBattleRoomViewModel.BattleCharacter AttackCharacter = null;
            UIBattleRoomViewModel.BattleCharacter DefendsCharacter = null;
            UIBattleRoomViewModel.BattleEnemy AttackEnemy = null;
            UIBattleRoomViewModel.BattleEnemy DefendsEnemy = null;
            CharacterUIInfo UIAttackCharacter = null;
            CharacterUIInfo UIDefendsCharacter = null;
            EnemyUIInfo UIAttackEnemy = null;
            EnemyUIInfo UIDefendsEnemy = null;
            Debug.Log("AttackInfos[i].AttakPos" + AttackInfos[i].AttakPos);
            Debug.Log("AttackInfos[i].DefendsPos" + AttackInfos[i].DefendsPos);
            if (AttackInfos[i].AttakPos % 2 == 0)
            {
                AttackEnemy = m_ViewModel.enemyList.Find(p => p.ID == AttackInfos[i].AttakPos);
                UIAttackEnemy = m_EnemyUIInfoList.Find(p => p.ID == AttackInfos[i].AttakPos);
            }
            else
            {
                AttackCharacter = m_ViewModel.characterList.Find(p => p.HeroID == AttackInfos[i].AttakPos);
                UIAttackCharacter = m_CharacterUIInfoList.Find(p => p.ID == AttackInfos[i].AttakPos);
            }
            if (AttackInfos[i].DefendsPos % 2 == 0)
            {
                DefendsEnemy = m_ViewModel.enemyList.Find(p => p.ID == AttackInfos[i].DefendsPos);
                UIDefendsEnemy = m_EnemyUIInfoList.Find(p => p.ID == AttackInfos[i].DefendsPos);
            }
            else
            {
                DefendsCharacter = m_ViewModel.characterList.Find(p => p.HeroID == AttackInfos[i].DefendsPos);
                UIDefendsCharacter = m_CharacterUIInfoList.Find(p => p.ID == AttackInfos[i].DefendsPos);
            }


            //TODO 明天

            m_AttackName = AttackCharacter != null ? AttackCharacter.Name : AttackEnemy.Name;
            m_DefendsName = DefendsCharacter != null ? DefendsCharacter.Name : DefendsEnemy.Name;
            m_AbiilityEffectValue = AttackInfos[i].AbiilityEffectValue;
            m_AbilityName = AttackInfos[i].AbilityName;

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
                        DefendsCharacter.CurrentHP -= m_AbiilityEffectValue;

                        if (DefendsCharacter.CurrentHP < 0)
                        {
                            DefendsCharacter.CurrentHP = 0;
                        }
                        UIDefendsCharacter.CurrentHP_Txt.text = DefendsCharacter.CurrentHP.ToString();
                    }
                    else
                    {
                        DefendsEnemy.CurrentHP -= m_AbiilityEffectValue;

                        if (DefendsEnemy.CurrentHP < 0)
                        {
                            DefendsEnemy.CurrentHP = 0;
                        }
                        UIDefendsEnemy.CurrentHP_Txt.text = DefendsEnemy.CurrentHP.ToString();
                    }
      
                    m_ReportScript.m_DateTime_Txt.text = DateTime.Now.ToString();
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
                        AttackCharacter.CurrentHP += m_AbiilityEffectValue;
                        if (AttackCharacter.CurrentHP > AttackCharacter.MaxHP)
                        {
                            AttackCharacter.CurrentHP = AttackCharacter.MaxHP;
                        }
                        UIAttackCharacter.CurrentHP_Txt.text = AttackCharacter.CurrentHP.ToString();
                    }
                    else
                    {
                        AttackEnemy.CurrentHP += m_AbiilityEffectValue;
                        if (AttackEnemy.CurrentHP > AttackEnemy.MaxHP)
                        {
                            AttackEnemy.CurrentHP = AttackEnemy.MaxHP;
                        }
                        UIAttackEnemy.CurrentHP_Txt.text = AttackEnemy.CurrentHP.ToString();
                    }
                    
                    m_ReportScript.m_DateTime_Txt.text = DateTime.Now.ToString();

                    m_ReportGo.SetActive(true);
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
                        DefendsCharacter.CurrentHP -= m_AbiilityEffectValue;
                        if (DefendsCharacter.CurrentHP < 0)
                        {
                            DefendsCharacter.CurrentHP = 0;
                        }
                        UIDefendsCharacter.CurrentHP_Txt.text = DefendsCharacter.CurrentHP.ToString();
                    }
                    else
                    {
                        DefendsEnemy.CurrentHP -= m_AbiilityEffectValue;
                        if (DefendsEnemy.CurrentHP < 0)
                        {
                            DefendsEnemy.CurrentHP = 0;
                        }
                        UIDefendsEnemy.CurrentHP_Txt.text = DefendsEnemy.CurrentHP.ToString();
                    }
              
                    m_ReportScript.m_DateTime_Txt.text = DateTime.Now.ToString();
                    break;
            }
            m_ReportGo.SetActive(true);
        }
        yield return new WaitForSeconds(3f);

        var m_ResultReport = Instantiate(m_BattleReport_Txt);
        m_ResultReport.text = "";

        if (m_ViewModel.BattleReportList.IsWin)
        {
            m_ResultReport.text = m_SuccessResult;
            for (int i = 0; i < m_ViewModel.BattleReportList.ItemRewards.Count; i++)
            {
                m_ResultReport.text += m_ViewModel.BattleReportList.ItemRewards[i] + "\n";
            }
        }
        else
        {
            m_ResultReport.text = string.Format(m_FailureResult,
                                                   m_ViewModel.BattleReportList.TeamName);
        }
        m_ResultReport.gameObject.SetActive(true);
    }

    private IEnumerator CountDown(float time,Text txt)
    {
        while (time > 0)
        {
            yield return null;
            txt.text = string.Format(m_FindEnemy, (int)time);
            time-= Time.deltaTime;
        }
        InitEnemyInfo();
        List<UIBattleRoomViewModel.AttackInfos> AttackInfos = m_ViewModel.BattleReportList.AttackInfoList;
        StartCoroutine(PlayAttackInfo(AttackInfos));
    }
    private IEnumerator BattleReportUpdate(int index)
    {
        yield return new WaitForSeconds(4);
        GameObject m_ReportGo = Instantiate(m_AttackBattleReport.gameObject);
        UIBattleReportScript m_ReportScript = m_ReportGo.GetComponent<UIBattleReportScript>();
        m_ReportScript.m_DateTime_Txt.text = m_BattleReportInfo_List[index].m_DateTime;
        m_ReportScript.m_Info_Txt.text = m_BattleReportInfo_List[index].m_Info;
        m_ReportGo.transform.parent = m_AttackBattleReport.gameObject.transform;
        m_ReportGo.SetActive(true);
    }

    /// <summary>
    /// 角色初始化
    /// </summary>
    /// <param name="model"></param>
    private void InitCharcterInfo()
    {
        //if (m_ViewModel.characterList.Count <= 0)
        //{
        //    return;
        //}
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
            //m_CharacterUIInfoList[i].CurrentEXP_Txt.text = m_ViewModel.characterList[i].CurrentEXP.ToString();
            //m_CharacterUIInfoList[i].MaxEXP_Txt.text = m_ViewModel.characterList[i].MaxEXP.ToString();

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
            for (int j = 0; j < m_ViewModel.enemyList[i].AbilityInfoList.Count; j++)
            {
                //m_EnemyUIInfoList[i].AbilityNameAndType.text += m_ViewModel.enemyList[i].AbilityInfoList[i].AbilityType + m_ViewModel.enemyList[i].AbilityInfoList[i].Name;
                if (j < m_ViewModel.enemyList[i].AbilityInfoList.Count)
                {
                    //m_EnemyUIInfoList[i].AbilityNameAndType.text += "\n";
                }
            }
            m_EnemyUIInfoList[i].ID = i * 2 + 2;
            m_ViewModel.enemyList[i].ID = i * 2 + 2;

        }
    }

    #region
    //public override void UpdatePanel(object viewModel)
    //{
    //    m_ViewModel = viewModel as UIBattleRoomViewModel;
    //    if (m_ViewModel != null)
    //    {
    //        InitCharcterInfo();

    //        for (int i = 0; i < m_ViewModel.BattleReportList.Count; i++)
    //        {
    //            List<UIBattleRoomViewModel.AttackInfos> infos = m_ViewModel.BattleReportList[i].AttackInfoList;
    //            int FindEnemyTime = m_ViewModel.BattleReportList[i].FindEnemyTime;
    //            for (int j = 0; j < infos.Count; j++)
    //            {
    //                if (FindEnemyTime > 0)
    //                {
    //                    m_BattleReportInfo_List[j].m_Info = string.Format(m_FindEnemy, FindEnemyTime);
    //                    m_BattleReportInfo_List[j].m_DateTime = "";
    //                    m_BattleReportInfo_List[j].m_RoundNum = i;
    //                    continue;
    //                }
    //                if (m_RoundNum != 0 && m_RoundNum == i + 1 && i + 1 <= m_ViewModel.BattleReportList.Count)
    //                {
    //                    if (m_ViewModel.BattleReportList[i].IsWin)
    //                    {
    //                        if (m_ViewModel.BattleReportList[i].ItemRewards.Count > 0)
    //                        {
    //                            for (int reward = 0; reward < m_ViewModel.BattleReportList[reward].ItemRewards.Count; reward++)
    //                            {
    //                                m_BattleReportInfo_List[j].m_Info = m_ViewModel.BattleReportList[reward].ItemRewards[i] + "\n";
    //                            }
    //                            m_BattleReportInfo_List[j].m_Info += m_SuccessResult;
    //                        }
    //                        m_BattleReportInfo_List[j].m_Info = m_SuccessResult;
    //                    }
    //                    else
    //                    {
    //                        m_BattleReportInfo_List[j].m_Info = string.Format(m_FailureResult,
    //                                                               m_ViewModel.BattleReportList[i].TeamName);
    //                    }
    //                    continue;
    //                }
    //                m_BattleReportInfo_List = new List<BattleReportInfo>();
    //                m_AttackName = m_ViewModel.characterList[infos[j].AttakPos].Name;
    //                m_DefendsName = m_ViewModel.characterList[infos[j].DefendsPos].Name;
    //                m_AbiilityEffectValue = infos[j].AbiilityEffectValue;
    //                m_AbilityName = infos[j].AbilityName;
    //                if (m_AbiilityEffectValue > 0)
    //                {
    //                    if (m_AbilityName == "" || m_AbilityName == String.Empty)
    //                    {
    //                        m_BattleReportInfo_List[j].m_Info = string.Format(m_NormalAttack,
    //                                                                        i,
    //                                                                        m_AttackName,
    //                                                                        m_DefendsName);
    //                        m_BattleReportInfo_List[j].m_Info += string.Format(m_Damage,
    //                                                                        m_AbiilityEffectValue);
    //                        m_BattleReportInfo_List[j].m_DateTime = DateTime.Now.ToString();
    //                        m_BattleReportInfo_List[j].m_RoundNum = i;
    //                    }
    //                    else
    //                    {
    //                        m_BattleReportInfo_List[j].m_Info = string.Format(m_AbilityAttack,
    //                                             i,
    //                                             m_AttackName,
    //                                             m_AbilityName,
    //                                             m_DefendsName);
    //                        m_BattleReportInfo_List[j].m_Info += string.Format(m_Damage,
    //                                                                        m_AbiilityEffectValue);
    //                        m_BattleReportInfo_List[j].m_DateTime = DateTime.Now.ToString();
    //                        m_BattleReportInfo_List[j].m_RoundNum = i;
    //                    }
    //                }
    //                else if (m_AbiilityEffectValue < 0)
    //                {
    //                    m_BattleReportInfo_List[j].m_Info = string.Format(m_Recovery,
    //                    i,
    //                    m_AttackName,
    //                    m_AbilityName,
    //                    m_AbiilityEffectValue,
    //                    m_DefendsName);
    //                    m_BattleReportInfo_List[j].m_DateTime = DateTime.Now.ToString();
    //                    m_BattleReportInfo_List[j].m_RoundNum = i;
    //                }
    //                else
    //                {
    //                    if (m_AbilityName == "" || m_AbilityName == String.Empty)
    //                    {
    //                        m_BattleReportInfo_List[j].m_Info = string.Format(m_NormalAttack,
    //                                                                        i,
    //                                                                        m_AttackName,
    //                                                                        m_DefendsName);
    //                        m_BattleReportInfo_List[j].m_Info += string.Format(m_Dodge,
    //                                                                        m_DefendsName);
    //                        m_BattleReportInfo_List[j].m_DateTime = DateTime.Now.ToString();
    //                        m_BattleReportInfo_List[j].m_RoundNum = i;
    //                    }
    //                    else
    //                    {
    //                        m_BattleReportInfo_List[j].m_Info = string.Format(m_AbilityAttack,
    //                                             i,
    //                                             m_AttackName,
    //                                             m_AbilityName,
    //                                             m_DefendsName);
    //                        m_BattleReportInfo_List[j].m_Info += string.Format(m_Dodge,
    //                                                                        m_DefendsName);
    //                        m_BattleReportInfo_List[j].m_DateTime = DateTime.Now.ToString();
    //                        m_BattleReportInfo_List[j].m_RoundNum = i;
    //                    }
    //                }
    //                m_RoundNum = i;
    //            }
    //        }
    //        while (m_BattleReportInfo_List.Count > 0)
    //        {
    //            m_BattleReportInfo_List.RemoveAt(0);
    //            StartCoroutine(BattleReportUpdate(m_AttackNum));
    //            m_AttackNum++;
    //        }
    //        InitEnemyInfo();

    //    }
    //}
    #endregion
}
public class UIBattleRoomViewModel
{
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

        public int CurrentHP;
        /// <summary>
        /// 敌人技能信息
        /// </summary>
        public List<AbilityInfo> AbilityInfoList = new List<AbilityInfo>();
    }

    public enum ReportType
    {
        Attack,
        Rest,
        Result
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
