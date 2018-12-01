using StaticData;
using SUIFW;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateCharacterPanel : BaseUIForm
{
    public struct CreateData
    {
        public string actorOneName { get; set; }
        public string actorTwoName { get; set; }
        public string actorThreeName { get; set; }
        public string playerName { get; set; }
        public string teamName { get; set; }
        public uint raceOneType { get; set; }
        public uint raceTwoType { get; set; }
        public uint raceThreeType { get; set; }
    }

    public Button m_CreateBtn;
    public InputField m_PlayerNameInput;
    public InputField m_ActorOneNameInput;
    public InputField m_ActorTwoNameInput;
    public InputField m_ActorThreeNameInput;
    public InputField m_TeamNameInput;
    public Dropdown m_RaceOneTypeDropdown;
    public Dropdown m_RaceTwoTypeDropdown;
    public Dropdown m_RaceThreeTypeDropdown;
    public Text m_RaceNameText;
    public Text m_RaceDesText;
    public Text m_InitDesText;
    public Text m_GrowthDesText;
    public Text m_AbilityOneDesText;
    public Text m_AbilityTwoDesText;
    public Text m_PlayerNameIsRepeatOrNullText;
    public Text m_ActorOneIsRepeatOrNullText;
    public Text m_ActorTwoIsRepeatOrNullText;
    public Text m_ActorThreeIsRepeatOrNullText;
    public Text m_TeamNameIsRepeatOrNullText;

    private CreateCharacterViewModel m_CreateCharacterViewModel = new CreateCharacterViewModel();

    private string m_InfoInitValueDes = "HP{0} Pow{1} Dex{2} Con{3}";
    private bool m_IsCreatePlayerSuccess = false;

    private void Awake()
    {
        ReceiveMessage("Create", ReceiveCreateMessage);


        m_CreateBtn.onClick.AddListener(OnCreateClick);

        m_RaceOneTypeDropdown.onValueChanged.AddListener(OnRocaTypeValueChanged);
        m_RaceTwoTypeDropdown.onValueChanged.AddListener(OnRocaTypeValueChanged);
        m_RaceThreeTypeDropdown.onValueChanged.AddListener(OnRocaTypeValueChanged);

        m_PlayerNameInput.onValueChanged.AddListener(OnNameIsNull);
        m_ActorOneNameInput.onValueChanged.AddListener(OnActorOneIsRepeat);
        m_ActorTwoNameInput.onValueChanged.AddListener(OnActorTwoIsRepeat);
        m_ActorThreeNameInput.onValueChanged.AddListener(OnActorThreeIsRepeat);
        m_TeamNameInput.onValueChanged.AddListener(OnTeamNameIsNull);
    }


    private void ReceiveCreateMessage(KeyValuesUpdate kv)
    {
        ArrayList m_ListMessage = kv.Values as ArrayList;
        Log("m_IsRegisterSuccess = " + (Convert.ToBoolean(m_ListMessage[1])) + m_ListMessage[0].GetType() + m_ListMessage[1].GetType());
        m_IsCreatePlayerSuccess = !Convert.ToBoolean(m_ListMessage[1]);

        if (!m_IsCreatePlayerSuccess)
        {
            switch (kv.Key)
            {
                case "CreatePlayerError":
                case "CreatePlayerNameExit":
                    m_PlayerNameIsRepeatOrNullText.text = m_ListMessage[0].ToString();
                    StartCoroutine(DisableAfterTwoSeconds(m_PlayerNameIsRepeatOrNullText));
                    break;
                case "CreateTeamNameExit":
                    m_TeamNameIsRepeatOrNullText.text = m_ListMessage[0].ToString();
                    StartCoroutine(DisableAfterTwoSeconds(m_TeamNameIsRepeatOrNullText));
                    break;
                case "CreateActorNameExit":
                case "CreateActorRaceIDNonExit":
                case "CreateActorCareerNonExit":
                    m_ActorOneIsRepeatOrNullText.text = m_ListMessage[0].ToString();
                    StartCoroutine(DisableAfterTwoSeconds(m_PlayerNameIsRepeatOrNullText));
                    break;
                default:
                    break;
            }
        }
    }

    private void OnNameIsNull(string name)
    {
        CheckedInputInfo(m_PlayerNameIsRepeatOrNullText, name,"","");
    }
    private void OnTeamNameIsNull(string name)
    {
        CheckedInputInfo(m_TeamNameIsRepeatOrNullText, name,"","");
    }
    private void OnActorOneIsRepeat(string name)
    {
        CheckedInputInfo(m_ActorOneIsRepeatOrNullText, name, m_ActorTwoNameInput.text, m_ActorThreeNameInput.text);
    }
    private void OnActorTwoIsRepeat(string name)
    {
        CheckedInputInfo(m_ActorTwoIsRepeatOrNullText, name, m_ActorOneNameInput.text, m_ActorThreeNameInput.text);
    }
    private void OnActorThreeIsRepeat(string name)
    {
        CheckedInputInfo(m_ActorThreeIsRepeatOrNullText, name, m_ActorOneNameInput.text, m_ActorTwoNameInput.text);
    }

    public void OnRacaTypeOnClick(int index)
    {
        switch (index)
        {
            case 1:
                OnRocaTypeValueChanged(m_RaceOneTypeDropdown.value);
                break;
            case 2:
                OnRocaTypeValueChanged(m_RaceTwoTypeDropdown.value);
                break;
            case 3:
                OnRocaTypeValueChanged(m_RaceThreeTypeDropdown.value);
                break;
        }
    }
    public void OnRocaTypeValueChanged(int index)
    {
        m_RaceNameText.text = m_CreateCharacterViewModel.createCharacterViewModels[index].Name;
        m_RaceDesText.text = m_CreateCharacterViewModel.createCharacterViewModels[index].Describe;

        m_InitDesText.text = String.Format(m_InfoInitValueDes,
            m_CreateCharacterViewModel.createCharacterViewModels[index].InitHP,
            m_CreateCharacterViewModel.createCharacterViewModels[index].InitPow,
            m_CreateCharacterViewModel.createCharacterViewModels[index].InitDex,
            m_CreateCharacterViewModel.createCharacterViewModels[index].InitCon);
        m_GrowthDesText.text = String.Format(m_InfoInitValueDes,
            m_CreateCharacterViewModel.createCharacterViewModels[index].HPGrowth,
            m_CreateCharacterViewModel.createCharacterViewModels[index].PowGrowth,
            m_CreateCharacterViewModel.createCharacterViewModels[index].DexGrowth,
            m_CreateCharacterViewModel.createCharacterViewModels[index].ConGrowth);
        //通过技能ID寻找技能描述
        m_AbilityOneDesText.text = StaticDataHelper.GetRaceAbilityByID(m_CreateCharacterViewModel.createCharacterViewModels[index].AbilityOneID).AbilityDescribe;
        m_AbilityTwoDesText.text = StaticDataHelper.GetRaceAbilityByID(m_CreateCharacterViewModel.createCharacterViewModels[index].AbilityTwoID).AbilityDescribe;
    }


    public void OnCreateClick()
    {
        CheckedInputInfo(m_PlayerNameIsRepeatOrNullText, m_PlayerNameInput.text, string.Empty, string.Empty);
        CheckedInputInfo(m_TeamNameIsRepeatOrNullText, m_TeamNameInput.text, string.Empty, string.Empty);
        CheckedInputInfo(m_ActorOneIsRepeatOrNullText, m_ActorOneNameInput.text, m_ActorTwoNameInput.text, m_ActorThreeNameInput.text);
        CheckedInputInfo(m_ActorTwoIsRepeatOrNullText, m_ActorTwoNameInput.text, m_ActorOneNameInput.text, m_ActorThreeNameInput.text);
        CheckedInputInfo(m_ActorThreeIsRepeatOrNullText, m_ActorThreeNameInput.text, m_ActorOneNameInput.text, m_ActorTwoNameInput.text);

        if (!m_IsCreatePlayerSuccess)
        {
            Log("创建角色失败——客户端检查");
            UpdateData();
            return;
        }

        //TODO 设置Error
        m_CreateCharacterViewModel.CreateCharacterCallback(CreateDataCallback());

        if (!m_IsCreatePlayerSuccess)
        {
            Log("创建角色失败——服务端检查");
            UpdateData();
            return;
        }

        Log("成功创建角色 —— " +
            "玩家名称：" + m_CreateData.playerName +
            "玩家队伍名称：" + m_CreateData.teamName +
            "角色一名称：" + m_CreateData.actorOneName +
            "角色二名称：" + m_CreateData.actorTwoName +
            "角色三名称：" + m_CreateData.actorThreeName +
            "种族一类型：" + m_CreateData.raceOneType +
            "种族二类型：" + m_CreateData.raceTwoType +
            "种族三类型：" + m_CreateData.raceThreeType);
    }
    void UpdateData()
    {
        m_PlayerNameInput.text = "";
        m_ActorOneNameInput.text = "";
        m_ActorTwoNameInput.text = "";
        m_ActorThreeNameInput.text = "";
        m_TeamNameInput.text = "";
        InitModel(m_CreateCharacterViewModel);
    //m_RocaOneTypeDropdown;
    //m_RocaTwoTypeDropdown;
    //m_RocaThreeTypeDropdown;
    //m_RocaNameText;
    //m_RocaDesText;
    //m_InitDesText;
    //m_GrowthDesText;
    //m_AbilityOneDesText;
    //m_AbilityTowDesText;
}

    private void CheckedInputInfo(Text go,string nameOne,string nameTwo, string nameThree)
    {
        if (nameOne == "" || nameOne == String.Empty)
        {
            go.text = "名称为空！";
            go.gameObject.SetActive(true);
            m_IsCreatePlayerSuccess = false;
        }
        else if (nameOne == nameTwo || nameOne == nameThree)
        {
            go.text = "英雄名称重复！";
            go.gameObject.SetActive(true);
            m_IsCreatePlayerSuccess = false;
        }
        else
        {
            go.gameObject.SetActive(false);
            m_IsCreatePlayerSuccess = true;
        }
        if (!m_IsCreatePlayerSuccess)
        {
            StartCoroutine(DisableAfterTwoSeconds(go));
        }
    }

    private CreateData m_CreateData;
    private CreateData CreateDataCallback()
    {
        m_CreateData = new CreateData();
        m_CreateData.playerName = m_PlayerNameInput.text;
        m_CreateData.teamName = m_TeamNameInput.text;
        m_CreateData.actorOneName = m_ActorOneNameInput.text;
        m_CreateData.actorTwoName = m_ActorTwoNameInput.text;
        m_CreateData.actorThreeName = m_ActorThreeNameInput.text;


        //StaticData.Data.RaceData raceData = new StaticData.Data.RaceData();
        //raceData.AbilityOneID = 1001;
        //raceData.AbilityTwoID = 1002;
        //raceData.ConGrowth = 1;
        //raceData.Describe = "啊撒旦水水CDC";
        //raceData.DexGrowth = 2;
        //raceData.HPGrowth = 3;
        //raceData.MPGrowth = 4;
        //raceData.ID = 1;
        //raceData.InitCon = 100;
        //raceData.InitDex = 200;
        //raceData.InitHP = 130;
        //raceData.InitMP = 140;
        //raceData.Name = "人类";
        //raceData.PowGrowth = 5;

        //TODO ++
        m_CreateData.raceOneType = StaticDataHelper.GetRaceIDByName(m_RaceOneTypeDropdown.captionText.text);
        m_CreateData.raceTwoType = StaticDataHelper.GetRaceIDByName(m_RaceTwoTypeDropdown.captionText.text);
        m_CreateData.raceThreeType = StaticDataHelper.GetRaceIDByName(m_RaceThreeTypeDropdown.captionText.text);

        return m_CreateData;
    }
    private bool m_IsInitModel = false;
    private void InitModel(object viewModel)
    {
        if (viewModel == null || !(viewModel is CreateCharacterViewModel) || m_IsInitModel)
        {
            return;
        }
        m_CreateCharacterViewModel = (CreateCharacterViewModel)viewModel;

        for (int i = 0; i < m_CreateCharacterViewModel.createCharacterViewModels.Count; i++)
        {
            Dropdown.OptionData op = new Dropdown.OptionData();
            op.text = m_CreateCharacterViewModel.createCharacterViewModels[i].Name;
            m_RaceOneTypeDropdown.options.Add(op);
            m_RaceTwoTypeDropdown.options.Add(op);
            m_RaceThreeTypeDropdown.options.Add(op);
        }

        OnRocaTypeValueChanged(0);

        m_PlayerNameIsRepeatOrNullText.gameObject.SetActive(false);
        m_ActorOneIsRepeatOrNullText.gameObject.SetActive(false);
        m_ActorTwoIsRepeatOrNullText.gameObject.SetActive(false);
        m_ActorThreeIsRepeatOrNullText.gameObject.SetActive(false);
        m_IsInitModel = true;
    }
    public override void UpdatePanel(object viewModel)
    {
        InitModel(viewModel);
        Debug.Log("11111111111111");
    }



    /// <summary>
    /// 两秒后禁用
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    private IEnumerator DisableAfterTwoSeconds(Text go)
    {
        go.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        go.gameObject.SetActive(false);
    }
}
