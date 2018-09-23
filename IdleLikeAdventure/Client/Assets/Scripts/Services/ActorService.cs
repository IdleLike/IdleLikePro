using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using NetData.OpCode;
using StaticData;
using StaticData.Data;
using UnityEngine;

namespace Service
{
    public class ActorService : BaseService<NetData.OpCode.OpCodeActorOperation, NetData.OpCode.OpCodeActorEvent>
    {
        protected override OpCodeModule ServiceOpCode
        {
            get
            {
                return OpCodeModule.Actor;
            }
        }

        /// <summary>
        /// 创建英雄
        /// </summary>
        /// <param name="userID">User identifier.</param>
        /// <param name="heroConfigID">Hero config identifier.</param>
        /// <param name="heroName">Hero name.</param>
        public Hero CreateHero(int userID, uint rocaID, string heroName)
        {
            //TODO 为玩家创建英雄

            return null;   
        }

        public Hero GenerateHero(HeroEntity heroEntity)
        {
            //TODO 生成一个英雄

            Hero hero = new Hero();
            //英雄ID
            hero.Id = heroEntity.UserID;
            //英雄名称
            hero.Name = heroEntity.Name;
            //英雄经验
            hero.Exp = heroEntity.TotalExp;

            CareerData careerData;
            StaticDataMgr.mInstance.mCareerDataMap.TryGetValue(heroEntity.CareerID,out careerData);
            RaceData raceData;
            StaticDataMgr.mInstance.mRaceDataMap.TryGetValue(heroEntity.RaceID, out raceData);
            if (careerData != null && raceData != null)
            {
                //职业数据
                hero.CareerData = careerData;
                //种族数据
                hero.RaceData = raceData;
                //职业技能
                for (int i = 0; i < careerData.AbilityList.Count; i++)
                {
                    CareerAbilityData careerAbilityData;
                    StaticDataMgr.mInstance.mCareerAbilityDataMap.TryGetValue(careerData.AbilityList[i],out careerAbilityData);
                    hero.CareerAbilities.Add(careerAbilityData);
                }
                //种族技能一
                RaceAbilityData raceAbilityData;
                StaticDataMgr.mInstance.mRaceAbilityDataMap.TryGetValue(raceData.AbilityOneID,out raceAbilityData);
                hero.RaceAbilitys.Add(raceAbilityData);
                StaticDataMgr.mInstance.mRaceAbilityDataMap.TryGetValue(raceData.AbilityTwoID, out raceAbilityData);
                hero.RaceAbilitys.Add(raceAbilityData);
                //血量
                hero.MaxHP = (int)(raceData.InitHP + GetGrowthValue(raceData.HPGrowth, hero.Level) + GetGrowthValue(careerData.HPGrowth, heroEntity.CareerLevel));
                //魔法
                hero.MaxMP = (int)(raceData.InitMP + GetGrowthValue(raceData.MPGrowth, hero.Level) + GetGrowthValue(careerData.MPGrowth, heroEntity.CareerLevel));
                //力量
                hero.Pow = raceData.InitPow + GetGrowthValue(raceData.PowGrowth, hero.Level) + GetGrowthValue(careerData.PowGrowth, heroEntity.CareerLevel);
                //体质
                hero.Con = raceData.InitCon + GetGrowthValue(raceData.ConGrowth, hero.Level) + GetGrowthValue(careerData.ConGrowth, heroEntity.CareerLevel);
                //敏捷
                hero.Dex = raceData.InitDex + GetGrowthValue(raceData.DexGrowth, hero.Level) + GetGrowthValue(careerData.DexGrowth, heroEntity.CareerLevel);            
            }
            return hero;
        }

        public override void Init()
        {

        }

        /// <summary>
        /// 获取成长值
        /// </summary>
        /// <param name="hp"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private uint GetGrowthValue(uint hp, uint level)
        {
            uint finalValue = 0;
            while (level > 0)
            {
                level--;
                finalValue += hp;
            }
            return finalValue;
        }
    }
}

