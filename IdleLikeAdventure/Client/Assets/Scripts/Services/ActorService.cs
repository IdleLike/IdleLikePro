using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using NetData.Message;
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
        public HeroEntity CreateHero(int userID, uint rocaID, string heroName)
        {
            //TODO 为玩家创建英雄

            return null;   
        }
        /// <summary>
        /// 生成一个英雄
        /// </summary>
        /// <param name="actorMsgData"></param>
        /// <returns></returns>
        public HeroEntity GenerateHero(ActorMsgData actorMsgData)
        {
            //TODO 生成一个英雄

            HeroEntity hero = new HeroEntity();
            //英雄ID
            hero.ID = actorMsgData.DataBaseID;
            //英雄名称
            hero.Name = actorMsgData.Name;
            //英雄经验
            hero.Exp = (uint)actorMsgData.TotalExp;

            CareerData careerData = new CareerData();
            StaticDataMgr.mInstance.mCareerDataMap.TryGetValue((uint)actorMsgData.CareerID,out careerData);
            RaceData raceData = new RaceData();
            StaticDataMgr.mInstance.mRaceDataMap.TryGetValue((uint)actorMsgData.RaceID, out raceData);
            if (careerData != null || raceData != null)
            {
                //职业数据
                hero.CareerData = careerData;
                //种族数据
                hero.RaceData = raceData;
                //职业技能
                for (int i = 0; i < careerData.AbilityList.Count; i++)
                {
                    CareerAbilityData careerAbilityData = new CareerAbilityData();
                    StaticDataMgr.mInstance.mCareerAbilityDataMap.TryGetValue(careerData.AbilityList[i], out careerAbilityData);
                    hero.CareerAbilities.Add(careerAbilityData);
                }
                //种族技能一
                RaceAbilityData raceAbilityData = new RaceAbilityData();
                StaticDataMgr.mInstance.mRaceAbilityDataMap.TryGetValue(raceData.AbilityOneID, out raceAbilityData);
                hero.RaceAbilitys.Add(raceAbilityData);
                StaticDataMgr.mInstance.mRaceAbilityDataMap.TryGetValue(raceData.AbilityTwoID, out raceAbilityData);
                hero.RaceAbilitys.Add(raceAbilityData);
                //血量
                hero.MaxHP = (int)(raceData.InitHP + GetGrowthValue(raceData.HPGrowth, hero.Level) + GetGrowthValue(careerData.HPGrowth, (uint)actorMsgData.CareerLevel));
                //魔法
                hero.MaxMP = (int)(raceData.InitMP + GetGrowthValue(raceData.MPGrowth, hero.Level) + GetGrowthValue(careerData.MPGrowth, (uint)actorMsgData.CareerLevel));
                //力量
                hero.Pow = raceData.InitPow + GetGrowthValue(raceData.PowGrowth, hero.Level) + GetGrowthValue(careerData.PowGrowth, (uint)actorMsgData.CareerLevel);
                //体质
                hero.Con = raceData.InitCon + GetGrowthValue(raceData.ConGrowth, hero.Level) + GetGrowthValue(careerData.ConGrowth, (uint)actorMsgData.CareerLevel);
                //敏捷
                hero.Dex = raceData.InitDex + GetGrowthValue(raceData.DexGrowth, hero.Level) + GetGrowthValue(careerData.DexGrowth, (uint)actorMsgData.CareerLevel);            
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

