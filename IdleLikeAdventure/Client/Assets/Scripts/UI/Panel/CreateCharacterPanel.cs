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
        public uint rocaOneType { get; set; }
        public uint rocaTwoType { get; set; }
        public uint rocaThreeType { get; set; }
    }

    public Button m_CreateBtn;
    public InputField m_PlayerNameInput;
    public InputField m_ActorOneNameInput;
    public InputField m_ActorTwoNameInput;
    public InputField m_ActorThreeNameInput;
    public InputField m_TeamNameInput;
    public Dropdown m_RocaOneTypeDropdown;
    public Dropdown m_RocaTwoTypeDropdown;
    public Dropdown m_RocaThreeTypeDropdown;
    public Text m_RocaNameText;
    public Text m_RocaDesText;
    public Text m_InitDesText;
    public Text m_GrowthDesText;
    public Text m_AbilityOneDesText;
    public Text m_AbilityTowDesText;
    public Text m_PlayerNameIsRepeatOrNullText;
    public Text m_ActorOneIsRepeatOrNullText;
    public Text m_ActorTwoIsRepeatOrNullText;
    public Text m_ActorThreeIsRepeatOrNullText;
    public Text m_TeamNameIsRepeatOrNullText;

    private CreateCharacterViewModel m_CreateCharacterViewModel = new CreateCharacterViewModel();
    private string m_ErrorMessage = "";

    private string m_InfoInitValueDes = "HP{0} Pow{1} Dex{2} Con{3}";
    private void Awake()
    {
        ReceiveMessage("Create", ReceiveCreateMessage);


        m_CreateBtn.onClick.AddListener(OnCreateClick);

        m_RocaOneTypeDropdown.onValueChanged.AddListener(OnRocaTypeValueChanged);
        m_RocaTwoTypeDropdown.onValueChanged.AddListener(OnRocaTypeValueChanged);
        m_RocaThreeTypeDropdown.onValueChanged.AddListener(OnRocaTypeValueChanged);

        m_PlayerNameInput.onValueChanged.AddListener(OnNameIsNull);
        m_ActorOneNameInput.onValueChanged.AddListener(OnActorOneIsRepeat);
        m_ActorTwoNameInput.onValueChanged.AddListener(OnActorTwoIsRepeat);
        m_ActorThreeNameInput.onValueChanged.AddListener(OnActorThreeIsRepeat);
        m_TeamNameInput.onValueChanged.AddListener(OnTeamNameIsNull);
    }


    private void ReceiveCreateMessage(KeyValuesUpdate kv)
    {
        m_ErrorMessage = kv.Values.ToString();
        switch (kv.Key)
        {
            case "CreateNameError":
                m_PlayerNameIsRepeatOrNullText.text = m_ErrorMessage;
                DisableAfterTwoSeconds(m_PlayerNameIsRepeatOrNullText);
                break;
            case "CreateTeamNameExist":
                m_TeamNameIsRepeatOrNullText.text = m_ErrorMessage;
                DisableAfterTwoSeconds(m_TeamNameIsRepeatOrNullText);
                break;

            default:
                break;
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
                OnRocaTypeValueChanged(m_RocaOneTypeDropdown.value);
                break;
            case 2:
                OnRocaTypeValueChanged(m_RocaTwoTypeDropdown.value);
                break;
            case 3:
                OnRocaTypeValueChanged(m_RocaThreeTypeDropdown.value);
                break;
        }
    }
    public void OnRocaTypeValueChanged(int index)
    {
        m_RocaNameText.text = m_CreateCharacterViewModel.createCharacterViewModels[index].Name;
        m_RocaDesText.text = m_CreateCharacterViewModel.createCharacterViewModels[index].Describe;

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
    }

    private bool m_IsCreatePlayer = false;

    public void OnCreateClick()
    {
        CheckedInputInfo(m_PlayerNameIsRepeatOrNullText, m_PlayerNameInput.text, string.Empty, string.Empty);
        CheckedInputInfo(m_TeamNameIsRepeatOrNullText, m_TeamNameInput.text, string.Empty, string.Empty);
        CheckedInputInfo(m_ActorOneIsRepeatOrNullText, m_ActorOneNameInput.text, m_ActorTwoNameInput.text, m_ActorThreeNameInput.text);
        CheckedInputInfo(m_ActorTwoIsRepeatOrNullText, m_ActorTwoNameInput.text, m_ActorOneNameInput.text, m_ActorThreeNameInput.text);
        CheckedInputInfo(m_ActorThreeIsRepeatOrNullText, m_ActorThreeNameInput.text, m_ActorOneNameInput.text, m_ActorTwoNameInput.text);

        if (!m_IsCreatePlayer)
        {
            Debug.Log("创建角色失败");
            return;
        }

        //TODO 设置Error
        m_CreateCharacterViewModel.CreateCharacterCallback(CreateDataCallback());

        if (m_ErrorMessage != "" || m_ErrorMessage != string.Empty)
        {
            Log("登录失败");
            return;
        }

        Log("成功创建角色 —— " +
            "玩家名称：" + m_CreateData.playerName +
            "玩家队伍名称：" + m_CreateData.teamName + 
            "角色一名称：" + m_CreateData.actorOneName +
            "角色二名称：" + m_CreateData.actorTwoName +
            "角色三名称：" + m_CreateData.actorThreeName +
            "种族一类型：" + m_CreateData.rocaOneType +
            "种族二类型：" + m_CreateData.rocaTwoType +
            "种族三类型：" + m_CreateData.rocaThreeType);
    }

    private void CheckedInputInfo(Text go,string nameOne,string nameTwo, string nameThree)
    {
        if (nameOne == "" || nameOne == String.Empty)
        {
            go.text = "名称为空！";
            go.gameObject.SetActive(true);
            m_IsCreatePlayer = false;
        }
        else if (nameOne == nameTwo || nameOne == nameThree)
        {
            go.text = "英雄名称重复！";
            go.gameObject.SetActive(true);
            m_IsCreatePlayer = false;
        }
        else
        {
            go.gameObject.SetActive(false);
            m_IsCreatePlayer = true;
        }
        if (!m_IsCreatePlayer)
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
        m_CreateData.rocaOneType = (uint)m_RocaOneTypeDropdown.value + 1;
        m_CreateData.rocaTwoType = (uint)m_RocaTwoTypeDropdown.value + 1;
        m_CreateData.rocaThreeType = (uint)m_RocaThreeTypeDropdown.value + 1;

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
            m_RocaOneTypeDropdown.options.Add(op);
            m_RocaTwoTypeDropdown.options.Add(op);
            m_RocaThreeTypeDropdown.options.Add(op);
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
        yield return new WaitForSeconds(2);
        go.gameObject.SetActive(false);
    }
}
