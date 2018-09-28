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
        public string characterOneName { get; set; }
        public string characterTwoName { get; set; }
        public string characterThreeName { get; set; }
        public string playerName { get; set; }
        public string rocaOneType { get; set; }
        public string rocaTwoType { get; set; }
        public string rocaThreeType { get; set; }
    }

    public Button Create_Btn;
    public InputField PlayerName;
    public InputField CharacterOneName;
    public InputField CharacterTwoName;
    public InputField CharacterThreeName;
    public Dropdown RocaOneType;
    public Dropdown RocaTwoType;
    public Dropdown RocaThreeType;
    public Text RocaName;
    public Text RocaDes;
    public Text InitDes;
    public Text GrowthDes;
    public Text AbilityOneDes;
    public Text AbilityTowDes;
    public Text PlayerNameIsRepeatOrNull;
    public Text CharacterOneIsRepeatOrNull;
    public Text CharacterTwoIsRepeatOrNull;
    public Text CharacterThreeIsRepeatOrNull;

    private CreateCharacterViewModel m_CreateCharacterModel;

    BaseEventData m_BaseEventData;

    private string m_InfoInitValueDes = "HP{0} Pow{1} Dex{2} Con{3}";
    private void Awake()
    {
        Create_Btn.onClick.AddListener(OnCreateClick);

        RocaOneType.onValueChanged.AddListener(OnRocaTypeValueChanged);
        RocaTwoType.onValueChanged.AddListener(OnRocaTypeValueChanged);
        RocaThreeType.onValueChanged.AddListener(OnRocaTypeValueChanged);

        PlayerName.onValueChanged.AddListener(OnPlayerNameIsRepeat);
        CharacterOneName.onValueChanged.AddListener(OnCharacterOneIsRepeat);
        CharacterTwoName.onValueChanged.AddListener(OnCharacterTwoIsRepeat);
        CharacterThreeName.onValueChanged.AddListener(OnCharacterThreeIsRepeat);
    }

    private void OnPlayerNameIsRepeat(string name)
    {
        StartCoroutine(GameObjectSetActive(PlayerNameIsRepeatOrNull, name,string.Empty,string.Empty));
    }
    private void OnCharacterOneIsRepeat(string name)
    {
        StartCoroutine(GameObjectSetActive(CharacterOneIsRepeatOrNull, name, CharacterTwoName.text, CharacterThreeName.text));
    }
    private void OnCharacterTwoIsRepeat(string name)
    {
        StartCoroutine(GameObjectSetActive(CharacterTwoIsRepeatOrNull, name, CharacterOneName.text, CharacterThreeName.text));
    }
    private void OnCharacterThreeIsRepeat(string name)
    {
        StartCoroutine(GameObjectSetActive(CharacterThreeIsRepeatOrNull, name, CharacterOneName.text, CharacterTwoName.text));
    }

    private void OnRocaTypeValueChanged(int index)
    {
        RocaName.text = m_CreateCharacterModel.createCharacterViewModels[index].raceName;
        RocaDes.text = m_CreateCharacterModel.createCharacterViewModels[index].raceDes;
        //TODO 技能 string => uint
        //InitDes.text = createCharacterModel.createCharacterViewModels[index].initValue;
        //GrowthDes.text = createCharacterModel.createCharacterViewModels[index].growthValue;
        //AbilityOneDes.text = m_CreateCharacterModel.createCharacterViewModels[index].raceAbilityOne;
        //AbilityTowDes.text = m_CreateCharacterModel.createCharacterViewModels[index].raceAbilityTwo;
        InitDes.text = String.Format(m_InfoInitValueDes,
            m_CreateCharacterModel.createCharacterViewModels[index].initHP,
            m_CreateCharacterModel.createCharacterViewModels[index].initPow,
            m_CreateCharacterModel.createCharacterViewModels[index].initDex,
            m_CreateCharacterModel.createCharacterViewModels[index].initCon);
        GrowthDes.text = String.Format(m_InfoInitValueDes,
            m_CreateCharacterModel.createCharacterViewModels[index].growthHP,
            m_CreateCharacterModel.createCharacterViewModels[index].growthPow,
            m_CreateCharacterModel.createCharacterViewModels[index].growthDex,
            m_CreateCharacterModel.createCharacterViewModels[index].growthCon);
    }

    private bool m_IsCreatePlayer = false;

    public void OnCreateClick()
    {
        StartCoroutine(GameObjectSetActive(PlayerNameIsRepeatOrNull,PlayerName.text, string.Empty, string.Empty));
        StartCoroutine(GameObjectSetActive(CharacterOneIsRepeatOrNull, CharacterOneName.text, CharacterTwoName.text, CharacterThreeName.text));
        StartCoroutine(GameObjectSetActive(CharacterTwoIsRepeatOrNull, CharacterTwoName.text, CharacterOneName.text, CharacterThreeName.text));
        StartCoroutine(GameObjectSetActive(CharacterThreeIsRepeatOrNull, CharacterThreeName.text, CharacterOneName.text, CharacterTwoName.text));

        m_CreateCharacterModel.CreateCharacterCallback(CreateDataCallback());
        if (!m_IsCreatePlayer)
        {
            Debug.Log("创建角色失败");
            return;
        }
     
        Debug.Log("成功创建角色 —— " +
            "玩家名称：" + m_CreateData.playerName +
            "角色一名称：" + m_CreateData.characterOneName +
            "角色二名称：" + m_CreateData.characterTwoName +
            "角色三名称：" + m_CreateData.characterThreeName +
            "种族一类型：" + m_CreateData.rocaOneType +
            "种族二类型：" + m_CreateData.rocaTwoType +
            "种族三类型：" + m_CreateData.rocaThreeType);
    }

    private IEnumerator GameObjectSetActive(Text go,string nameOne,string nameTwo, string nameThree)
    {
        if (nameOne == "" || nameOne == String.Empty || nameOne == nameTwo || nameOne == nameThree)
        {
            go.gameObject.SetActive(true);
            m_IsCreatePlayer = false;
        }
        else
        {
            go.gameObject.SetActive(false);
            m_IsCreatePlayer = true;
            yield break;
        }
        yield return new WaitForSeconds(2);
        if (name == "" || name == String.Empty || nameOne == nameTwo || nameOne == nameThree)
        {
            go.gameObject.SetActive(false);
        }       
    }

    private CreateData m_CreateData;
    private CreateData CreateDataCallback()
    {
        m_CreateData = new CreateData();
        m_CreateData.playerName = PlayerName.text;
        m_CreateData.characterOneName = CharacterOneName.text;
        m_CreateData.characterTwoName = CharacterTwoName.text;
        m_CreateData.characterThreeName = CharacterThreeName.text;
        m_CreateData.rocaOneType = RocaOneType.captionText.text;
        m_CreateData.rocaTwoType = RocaTwoType.captionText.text;
        m_CreateData.rocaThreeType = RocaThreeType.captionText.text;
        return m_CreateData;
    }

    private void InitModel(object viewModel)
    {
        if (viewModel == null || !(viewModel is CreateCharacterViewModel))
        {
            return;
        }
        m_CreateCharacterModel = (CreateCharacterViewModel)viewModel;
        for (int i = 0; i < m_CreateCharacterModel.createCharacterViewModels.Count; i++)
        {
            Dropdown.OptionData op = new Dropdown.OptionData();
            op.text = m_CreateCharacterModel.createCharacterViewModels[i].raceName;
            RocaOneType.options.Add(op);
            RocaTwoType.options.Add(op);
            RocaThreeType.options.Add(op);
        }

        OnRocaTypeValueChanged(0);

        PlayerNameIsRepeatOrNull.gameObject.SetActive(false);
        CharacterOneIsRepeatOrNull.gameObject.SetActive(false);
        CharacterTwoIsRepeatOrNull.gameObject.SetActive(false);
        CharacterThreeIsRepeatOrNull.gameObject.SetActive(false);
    }
    public override void UpdatePanel(object viewModel)
    {
        InitModel(viewModel);
    }



}
