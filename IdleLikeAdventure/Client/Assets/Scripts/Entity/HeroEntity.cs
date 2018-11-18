using Entity;
using StaticData;
using StaticData.Data;
using System.Collections.Generic;


public class HeroEntity : BaseEntity
{

    //  正常属性
    private string name;
    //  战斗属性
    private int maxHP;
    private int maxMP;
    private uint exp;
    private uint pow;
    private uint dex;
    private uint con;

    //  配置信息
    private CareerData careerData;
    private RaceData raceData;

    //  技能数据
    private List<RaceAbilityData> raceAbilitys;
    private List<CareerAbilityData> careerAbilities;

    public int MaxHP
    {
        get
        {
            return maxHP;
        }

        set
        {
            maxHP = value;
        }
    }

    public int MaxMP
    {
        get
        {
            return maxMP;
        }

        set
        {
            maxMP = value;
        }
    }

    public uint Exp
    {
        get
        {
            return exp;
        }

        set
        {
            exp = value;
        }
    }

    public uint Pow
    {
        get
        {
            return pow;
        }

        set
        {
            pow = value;
        }
    }

    public uint Dex
    {
        get
        {
            return dex;
        }

        set
        {
            dex = value;
        }
    }

    public uint Con
    {
        get
        {
            return con;
        }

        set
        {
            con = value;
        }
    }

    public CareerData CareerData
    {
        get
        {
            return careerData;
        }

        set
        {
            careerData = value;
        }
    }

    public RaceData RaceData
    {
        get
        {
            return raceData;
        }

        set
        {
            raceData = value;
        }
    }

    public List<RaceAbilityData> RaceAbilitys
    {
        get
        {
            return raceAbilitys;
        }

        set
        {
            raceAbilitys = value;
        }
    }

    public List<CareerAbilityData> CareerAbilities
    {
        get
        {
            return careerAbilities;
        }

        set
        {
            careerAbilities = value;
        }
    }


    public uint Level
    {
        get
        {
            //TODO 实现根据总经验，计算等级
            for (uint i = 1; i <= StaticDataMgr.mInstance.mLevelDataMap.Count; i++)
            {
                if (Exp >= StaticDataMgr.mInstance.mLevelDataMap[i].CurrentLevelNeedExp
                    && StaticDataMgr.mInstance.mLevelDataMap[i].NextLevelNeedExp == 0)
                {
                    return StaticDataMgr.mInstance.mLevelDataMap[i].Level;
                }
                if (Exp < StaticDataMgr.mInstance.mLevelDataMap[i + 1].CurrentLevelNeedExp)
                {
                    return StaticDataMgr.mInstance.mLevelDataMap[i].Level;
                }

                //if (Exp >= StaticDataMgr.mInstance.mLevelDataMap[i].CurrentLevelNeedExp 
                //    && StaticDataMgr.mInstance.mLevelDataMap[i].NextLevelNeedExp == 0)
                //{
                //    return StaticDataMgr.mInstance.mLevelDataMap[i].Level;
                //}
                //else if(Exp >= StaticDataMgr.mInstance.mLevelDataMap[i].CurrentLevelNeedExp 
                //    && Exp < StaticDataMgr.mInstance.mLevelDataMap[i].CurrentLevelNeedExp + StaticDataMgr.mInstance.mLevelDataMap[i].NextLevelNeedExp)
                //{
                //    return StaticDataMgr.mInstance.mLevelDataMap[i].Level;
                //}
            }     
            return 0;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public string AbilityIco
    {
        get
        {
            return abilityIco;
        }

        set
        {
            abilityIco = value;
        }
    }

    private string abilityIco;
}
