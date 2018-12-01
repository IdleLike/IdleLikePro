using StaticData;
using StaticData.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDataHelper : ClassSingleton<StaticDataHelper> {

    static Dictionary<uint, RaceData> RaceDataDic = new Dictionary<uint, RaceData>();
    static Dictionary<uint, CareerData> CareerDataDic = new Dictionary<uint, CareerData>();
    static Dictionary<uint, CareerAbilityData> CareerAbilityDataDic = new Dictionary<uint, CareerAbilityData>(); //CareerAbility Data
    static Dictionary<uint, LevelData> LevelDataDic = new Dictionary<uint, LevelData>(); //Level Data
    static Dictionary<uint, RaceAbilityData> RaceAbilityDataDic = new Dictionary<uint, RaceAbilityData>(); //RaceAbility Data
    /// <summary>
    /// 保存加载的文档数据
    /// </summary>
    public static void SaveData()
    {
        RaceDataDic = StaticDataMgr.mInstance.mRaceDataMap;
        CareerDataDic = StaticDataMgr.mInstance.mCareerDataMap;
        CareerAbilityDataDic = StaticDataMgr.mInstance.mCareerAbilityDataMap;
        LevelDataDic = StaticDataMgr.mInstance.mLevelDataMap;
        RaceAbilityDataDic = StaticDataMgr.mInstance.mRaceAbilityDataMap;
    }
    //TODO 构建职业树， 每个子节点是职业技能数据
    //节点信息包括职业配置信息， 父节点，子节点列表， 子节点已经按照Order字段升序排序

    /// <summary>
    /// 通过ID获取职业技能数据
    /// </summary>
    /// <returns></returns>
    public static CareerAbilityData GetCareerAbilityByID(uint index)
    {
        foreach (var careerAbility in CareerAbilityDataDic.Values)
        {
            if (index == careerAbility.ID)
            {
                return careerAbility;
            }
        }
        return null;
    }
    /// <summary>
    /// 通过ID获取种族技能数据
    /// </summary>
    /// <returns></returns>
    public static RaceAbilityData GetRaceAbilityByID(uint index)
    {
        foreach (var raceAbility in RaceAbilityDataDic.Values)
        {
            if (index == raceAbility.ID)
            {

                return raceAbility;
            }
        }
        return null;
    }
    /// <summary>
    /// 通过职业名称获取职业技能
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static CareerAbilityData GetCareerAbilityByName(string name)
    {
        foreach (var race in CareerDataDic.Values)
        {
            if (name == race.Name)
            {
                return GetCareerAbilityByID(race.ID);
            }
        }
        return null;
    }
    /// <summary>
    /// 通过种族名称获取种族技能
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static RaceAbilityData GetRaceAbilityByName(string name)
    {
        foreach (var race in RaceDataDic.Values)
        {
            if (name == race.Name)
            {
                return GetRaceAbilityByID(race.ID);
            }
        }
        return null;
    }
    /// <summary>
    /// 通过种族名称获取种族ID
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static uint GetRaceIDByName(string name)
    {
        foreach (var race in RaceDataDic.Values)
        {
            if (name == race.Name)
            {
                return race.ID;
            }
        }
        return 0;
    }
    /// <summary>
    /// 通过职业名称获取职业ID
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static uint GetCareerIDByName(string name)
    {
        foreach (var race in CareerDataDic.Values)
        {
            if (name == race.Name)
            {
                return race.ID;
            }
        }
        return 0;
    }

}
