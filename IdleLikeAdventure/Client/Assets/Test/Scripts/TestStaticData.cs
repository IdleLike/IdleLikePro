using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using StaticData.Data;

[Serializable][CreateAssetMenu]
public class TestStaticData : ScriptableObject {

    private static TestStaticData instance;
    public static TestStaticData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<TestStaticData>("TestData");
            }
            return instance;
        }
    }

    public List<CareerData> CareerDatas = new List<CareerData>();
    public List<CareerAbilityData> CareerAbilityDatas = new List<CareerAbilityData>();
    public List<LevelData> LevelDatas = new List<LevelData>();
    public List<StringData> MultilingualDatas = new List<StringData>();
    public List<RaceAbilityData> RaceAbilityDatas = new List<RaceAbilityData>();
    public List<RaceData> RaceDatas = new List<RaceData>();


    public List<UI.Panel.BattleRoomModel.BattleCharacterModel> BattleCharacterModel = new List<UI.Panel.BattleRoomModel.BattleCharacterModel>();
    public List<UI.Panel.BattleRoomModel.BattleEnemyModel> BattleEnemyModel = new List<UI.Panel.BattleRoomModel.BattleEnemyModel>();
    public List<UI.Panel.BattleRoomModel.BattleReportModel> BattleReportModel = new List<UI.Panel.BattleRoomModel.BattleReportModel>();
}
