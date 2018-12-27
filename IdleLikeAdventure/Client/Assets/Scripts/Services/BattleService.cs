using UnityEngine;
using System.Collections;
using NetData.OpCode;
using NetData.Message;
using System;
using StaticData.Data;
using System.Collections.Generic;
using UI.Panel;
using StaticData;

namespace Service
{
    public class BattleService : BaseService<NetData.OpCode.OpCodeBattleOperation, NetData.OpCode.OpCodeBattleEvent>
    {

        public List<MonsterData> monsterList = new List<MonsterData>();
        private UIBattleRoomViewModel battleViewModel = new UIBattleRoomViewModel();
        private UIBattleRoomView battleView = new UIBattleRoomView();

        protected override OpCodeModule ServiceOpCode
        {
            get
            {
                return OpCodeModule.Battle;
            }
        }

        public override void Init()
        {
            //����������Ϣ
            AddNetListener();
            
        }


        /// <summary>
        /// ����������Ϣ
        /// </summary>
        public override void AddNetListener()
        {
            base.AddNetListener();
            RegisterNetMsg(OpCodeBattleOperation.BattleRequest, BattleHander);
        }
        /// <summary>
        /// ս������
        /// </summary>
        /// <param name="data"></param>
        private void BattleHander(BaseMsgData data)
        {
            BattleRespondeMsgData msgData = data as BattleRespondeMsgData;
            if (msgData != null)
            {
                if (msgData.IsError)
                {
                    //ս������
                }
                else
                {
                
                    //if (GameService.Instance.userService.battleRoomModel != null)
                    //{
                    //    battleViewModel = GameService.Instance.userService.battleRoomModel;
                    //}
                    //else
                    //{
                    //    Log("battleRoomModel == null");
                    //}
                    //battleView = GameService.Instance.userService.battleRoomPanel as UIBattleRoomView;


                    //��ɫ�б�
                    List<ActorMsgData> ActorDataList = new List<ActorMsgData>();

                    foreach (var item in DataCenter.Instance.userData.ActorDataList)
                    {
                        foreach (var id in DataCenter.Instance.userData.TeamDataList[msgData.BattleInfo.TeamID].ActorIDs)
                        {
                            if (item.DataBaseID == id)
                            {
                                ActorDataList.Add(item);
                            }
                        }
                    }
                    for (int i = 0; i < ActorDataList.Count; i++)
                    {
                        HeroEntity hero = GameService.Instance.actorService.GenerateHero(ActorDataList[i]);
                        UIBattleRoomViewModel.BattleCharacter character = new UIBattleRoomViewModel.BattleCharacter();
                        character.CareerName = hero.CareerData.Name;
                        character.MaxEXP = StaticDataMgr.mInstance.mLevelDataMap[hero.Level].NextLevelNeedExp;
                        character.CurrentEXP = hero.Exp - StaticDataMgr.mInstance.mLevelDataMap[hero.Level].CurrentLevelNeedExp;
                        character.Level = hero.Level;
                        character.MaxHP = hero.MaxHP;
                        character.CurrentHP = hero.MaxHP;
                        character.Name = hero.Name;
                        character.RaceName = hero.RaceData.Name;
                        battleViewModel.characterList.Add(character);
                    }
                    //�����б�
                    List<uint> monsterIDList = msgData.BattleInfo.EnemyID;
                    for (int i = 0; i < monsterIDList.Count; i++)
                    {
                        MonsterData monsterData = new MonsterData();
                        monsterData = (StaticDataHelper.GetMonsterByID(msgData.BattleInfo.EnemyID[i]));
                        UIBattleRoomViewModel.BattleEnemy enemy = new UIBattleRoomViewModel.BattleEnemy();
                        enemy.Level = monsterData.Level;
                        enemy.MaxHP = monsterData.HP;
                        enemy.CurrentHP = monsterData.HP;
                        enemy.Name = monsterData.Name;
                        for (int j = 0; j < monsterData.AbilityList.Count; j++)
                        {
                            CareerAbilityData abilityData = StaticDataHelper.GetCareerAbilityByID(monsterData.AbilityList[j]);
                            enemy.AbilityInfoList.Add(
                                    new UIBattleRoomViewModel.BattleEnemy.AbilityInfo
                                    {
                                        Name = abilityData.AbilityName,
                                        AbilityType = abilityData.AbilityType
                                    });
                        }
                        battleViewModel.enemyList.Add(enemy);
                    }
 
                    //��ͼ����

                    //��ȡս��
                    BattleMsgData BattleData = msgData.BattleInfo;
                    for (int i = 0; i < BattleData.ItemRewards.Count; i++)
                    {
                        //��ȡ����
                        List<uint> ItemRewards = BattleData.ItemRewards;
                        for (int j = 0; j < ItemRewards.Count; j++)
                        {
                            EquipmentData equipmentData = new EquipmentData();
                            equipmentData = (StaticDataHelper.GetEquipmentByID(msgData.BattleInfo.ItemRewards[j]));
                            battleViewModel.BattleReportList.ItemRewards.Add(equipmentData.Name);
                        }
                    }
                    for (int i = 0; i < BattleData.Rounds.Count; i++)
                    {
                        RoundInfoMsgData RoundData = BattleData.Rounds[i];
                        //��ȡս��������Ϣ
                        List<AttackInfoMsgData> attackInfosList = RoundData.AttackInfos;
                        for (int j = 0; j < attackInfosList.Count; j++)
                        {
                            UIBattleRoomViewModel.AttackInfos info = new UIBattleRoomViewModel.AttackInfos();
                            info.AttakPos = attackInfosList[j].AttakPos;
                            info.DefendsPos = attackInfosList[j].DefendsPos;
                            info.AbilityName = StaticDataHelper.GetCareerAbilityByID(attackInfosList[j].AbilityID).AbilityName;
                            info.AbiilityEffectValue = attackInfosList[j].AbiilityEffectValue;
                            info.EffectType = attackInfosList[j].EffectType;
                            battleViewModel.BattleReportList.AttackInfoList.Add(info);
                        }
                    }
                    //Ѱ�ҹ���ʱ��
                    battleViewModel.BattleReportList.FindEnemyTime = msgData.FindEnemyTime;
                    //��Ϣʱ��
                    battleViewModel.BattleReportList.RestTime = msgData.BattleInfo.RestTime;
                    //��ȡ���
                    battleViewModel.BattleReportList.GoldRewards = msgData.BattleInfo.GoldRewards;
                    //��Ӯ
                    battleViewModel.BattleReportList.IsWin = msgData.BattleInfo.IsWin;


                    battleView = OpenUIForm("BattleRoom", battleViewModel) as UIBattleRoomView;
                    battleView.gameObject.SetActive(true);

                    //battleView.UpdatePanel(battleViewModel);

                }
            }
        }
        /// <summary>
        /// ����ս��
        /// </summary>
        public void BattleRequest()
        {
            BattleRequestMsgData msgData = new BattleRequestMsgData();
            //TODO ս�����Ͷ��壿
            msgData.BattleType =  NetData.Enumeration.EnumBattleKind.Normal;
            msgData.TeamID = DataCenter.Instance.userData.TeamID;
            SendNetMsg(OpCodeBattleOperation.BattleRequest, msgData);
        }
    }
}

