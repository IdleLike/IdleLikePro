using SUIFW;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionHeroInfoViewModel
{
    public string Name;
    public string Race;
    public string Career;
    public string Level;
    public string CareerPoint;
    public string HP;
    public string Pow;
    public string Dex;
    public string Con;
    public string Attack;
    public string Defense;
    public int MinExp;
    public int MaxExp;

    public class WearerEquipment
    {
        public string Name;
        public string SpriteName;
    }
    public List<WearerEquipment> WearerEquipmentList = new List<WearerEquipment>();

    public List<string> AllPassiveAbilityList = new List<string>();

    public List<string> AbilityGroupList = new List<string>();
}

public class FunctionHeroInfoPanel : BaseUIForm
{
    #region UI控件 

    public Text m_Name_Txt;
    public Text m_Race_Txt;
    public Text m_Career_Txt;
    public Text m_Level_Txt;
    public Text m_CareerPoint_Txt;
    public Text m_HP_Txt;
    public Text m_Pow_Txt;
    public Text m_Dex_Txt;
    public Text m_Con_Txt;
    public Text m_Attack_Txt;
    public Text m_Defense_Txt;
    public Text m_Exp_Txt;
    //public List<Text> m_AbilityGroup_List_Txt;
    public List<Text> m_AllPassiveAbility_List_Txt;
    public List<Text> m_WearerEquipment_List_Txt;
    public List<Image> m_WearerEquipment_List_Sprite;

    public Slider m_Exp_Slider;

    public Text m_PassiveAbility_Txt;
    public Text m_ActiveAbility_Txt;
    public Toggle m_AbilityGroup_Toggle;

    #endregion
    FunctionHeroInfoViewModel m_ViewModel = new FunctionHeroInfoViewModel();

    public override void UpdatePanel(object viewModel)
    {
        if (viewModel != null)
        {
            m_ViewModel = viewModel as FunctionHeroInfoViewModel;
        }
        else
        {
            Log("viewModel == null");
        }
        if (m_ViewModel != null)
        {
            m_Name_Txt.text = m_ViewModel.Name;
            m_Race_Txt.text = m_ViewModel.Race;
            m_Career_Txt.text = m_ViewModel.Career;
            m_Level_Txt.text = m_ViewModel.Level;
            m_CareerPoint_Txt.text = m_ViewModel.CareerPoint;
            m_HP_Txt.text = m_ViewModel.HP;
            m_Pow_Txt.text = m_ViewModel.Pow;
            m_Dex_Txt.text = m_ViewModel.Dex;
            m_Con_Txt.text = m_ViewModel.Con;
            m_Attack_Txt.text = m_ViewModel.Attack;
            m_Defense_Txt.text = m_ViewModel.Defense;
            m_Exp_Txt.text = m_ViewModel.MinExp + "/" + m_ViewModel.MaxExp;

            m_Exp_Slider.value = Mathf.Clamp(m_ViewModel.MinExp,0,m_ViewModel.MaxExp) / m_ViewModel.MaxExp;

            
            for (int i = 0; i < m_ViewModel.AbilityGroupList.Count; i++)
            {
                GameObject go = CreateGameObject(m_AbilityGroup_Toggle.gameObject, m_AbilityGroup_Toggle.transform.parent.gameObject);
                Toggle toggle = go.GetComponent<Toggle>();
                GameObject go_Txt = CreateGameObject(m_ActiveAbility_Txt.gameObject, m_ActiveAbility_Txt.transform.parent.gameObject);
                Text text = go_Txt.GetComponent<Text>();
                text.text = m_ViewModel.AbilityGroupList[i];
                toggle.graphic = text;
            }

            for (int i = 0; i < m_ViewModel.WearerEquipmentList.Count; i++)
            {
                m_WearerEquipment_List_Sprite[i].sprite.name = m_ViewModel.WearerEquipmentList[i].SpriteName;
                m_WearerEquipment_List_Txt[i].text = m_ViewModel.WearerEquipmentList[i].Name;
                m_WearerEquipment_List_Sprite[i].gameObject.SetActive(true);
            }

            if (m_AllPassiveAbility_List_Txt.Count < m_ViewModel.AllPassiveAbilityList.Count)
            {
                for (int i = 0; i < m_ViewModel.AllPassiveAbilityList.Count; i++)
                {
                    GameObject go = CreateGameObject(m_PassiveAbility_Txt.gameObject, m_PassiveAbility_Txt.transform.parent.gameObject);
                    Text text = go.GetComponent<Text>();
                    m_AllPassiveAbility_List_Txt.Add(text);
                    m_AllPassiveAbility_List_Txt[i].text = m_ViewModel.AllPassiveAbilityList[i];
                }
            }
            else if (m_AllPassiveAbility_List_Txt.Count > m_ViewModel.AllPassiveAbilityList.Count)
            {
                m_AllPassiveAbility_List_Txt.ForEach(p => p.gameObject.SetActive(false));
                for (int i = 0; i < m_ViewModel.AllPassiveAbilityList.Count; i++)
                {
                    m_AllPassiveAbility_List_Txt[i].text = m_ViewModel.AllPassiveAbilityList[i];
                    m_AllPassiveAbility_List_Txt[i].gameObject.SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < m_ViewModel.AllPassiveAbilityList.Count; i++)
                {
                    m_AllPassiveAbility_List_Txt[i].text = m_ViewModel.AllPassiveAbilityList[i];
                    m_AllPassiveAbility_List_Txt[i].gameObject.SetActive(true);
                }
            }
        }
    }
    GameObject CreateGameObject(GameObject go,GameObject parent)
    {
        GameObject m_Go = Instantiate(go);
        m_Go.transform.parent = parent.transform;
        m_Go.transform.localScale = Vector3.one;
        m_Go.transform.SetAsFirstSibling();
        m_Go.SetActive(true);
        return m_Go;
    }

}
