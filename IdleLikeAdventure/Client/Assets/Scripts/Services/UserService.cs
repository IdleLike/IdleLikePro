using System;
using System.Collections.Generic;
using UI.Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using Entity;
using UI.Panel;
using NetData.OpCode;
using NetData.Message;
using SUIFW;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using StaticData;
using System.Collections;

namespace Service
{
    
    public class UserService : BaseService<NetData.OpCode.OpCodeUserOperation, NetData.OpCode.OpCodeUserEvent>
    {
        //ViewModel
        private CreateCharacterViewModel createCharacterModel;
        private BattleRoomModel battleRoomModel;
     
        private LoginPanel.LoginViewModel loginViewModel;
        private RegisterPanel.RegisterViewModel registerViewModel;
        private CreateCharacterViewModel createCharacterViewModel;
        private BaseUIForm battleRoomPanel;
        private BaseUIForm loginPanel;
        private BaseUIForm registerPanel;
        private BaseUIForm createCharacterPanel;

        private UserEntity userEntity = new UserEntity();
        private List<ActorMsgData> actorMsgDataList = new List<ActorMsgData>();
        private string teamName = string.Empty;
        private string userName = string.Empty;

        protected override OpCodeModule ServiceOpCode
        {
            get
            {
                return OpCodeModule.User;
            }
        }

        internal void Login()
        {
         
            if (loginViewModel == null) loginViewModel = new LoginPanel.LoginViewModel();
            loginViewModel.Btn_Action = OnOpenRegisterPanel;
            loginViewModel.LoginCallBack = LoginCallBack;
            loginPanel = OpenUIForm("Login", loginViewModel);
        }

        private void LoginCallBack(string email, string password)//,ushort serverId)
        {
            LoginRequstMsgData loginRequstMsgData = new LoginRequstMsgData();
            loginRequstMsgData.Account = email;
            loginRequstMsgData.Password = password;
            //loginRequstMsgData.ServerID = serverId;
            SendNetMsg(OpCodeUserOperation.Login, loginRequstMsgData);
        }

        private void OnOpenRegisterPanel()
        {
            if (registerViewModel == null) registerViewModel = new RegisterPanel.RegisterViewModel();
            registerViewModel.RegisterCallBack = RegisterCallBack;
            registerPanel = OpenUIForm("Register", registerViewModel);
            //TODO 隐藏登录界面
            loginPanel.gameObject.SetActive(false);
        }

        private void RegisterCallBack(string email,string password,ushort serverId)
        {            
            RegisterRequestMsgData registerRequestMsgData = new RegisterRequestMsgData();
            registerRequestMsgData.Account = email;
            registerRequestMsgData.Password = password;
            registerRequestMsgData.ServerID = serverId;
            Debug.Log("注册触发" + email + password);

            SendNetMsg(OpCodeUserOperation.Register,registerRequestMsgData);

        }
        //private bool m_IsRegisterSuccess;


        ////  创建角色界面数据
        //private void SetCreateCharacterModel()
        //{
        //    if (createCharacterModel == null) createCharacterModel = new CreateCharacterViewModel();

        //    if (createCharacterModel.createCharacterViewModels == null) createCharacterModel.createCharacterViewModels = new List<StaticData.Data.RaceData>();

        //    //初始化所有职业数据
        //    StaticData.Data.RaceData rocal = new StaticData.Data.RaceData();
        //    for (int i = 0; i < TestStaticData.Instance.RaceDatas.Count; i++)
        //    {
        //        // 数据赋值
        //        rocal.ID =  TestStaticData.Instance.RaceDatas[i].ID;
        //        rocal.Name = TestStaticData.Instance.RaceDatas[i].Name;
        //        rocal.Describe = TestStaticData.Instance.RaceDatas[i].Describe;
        //        rocal.InitCon = TestStaticData.Instance.RaceDatas[i].InitCon;
        //        rocal.InitDex = TestStaticData.Instance.RaceDatas[i].InitDex;
        //        rocal.InitPow = TestStaticData.Instance.RaceDatas[i].InitPow;
        //        rocal.InitHP = TestStaticData.Instance.RaceDatas[i].InitHP;
        //        rocal.ConGrowth = TestStaticData.Instance.RaceDatas[i].ConGrowth;
        //        rocal.DexGrowth = TestStaticData.Instance.RaceDatas[i].DexGrowth;
        //        rocal.PowGrowth = TestStaticData.Instance.RaceDatas[i].PowGrowth;
        //        rocal.HPGrowth = TestStaticData.Instance.RaceDatas[i].HPGrowth;
        //        rocal.AbilityOneID = TestStaticData.Instance.RaceDatas[i].AbilityOneID;
        //        rocal.AbilityTwoID = TestStaticData.Instance.RaceDatas[i].AbilityTwoID;
        //        createCharacterModel.createCharacterViewModels.Add(rocal);
        //    }

        //    //初始化所有委托
        //    createCharacterModel.CreateCharacterCallback = Create;
        //    createCharacterModel.NameIsRepeatCallback = CheckNameRepeat;
        //    createCharacterViewModel.TeamNameIsRepeatCallback = OnTeamNameIsRepeatCallback;
        //}

        //  创建用户
        //private void Create(CreateCharacterPanel.CreateData obj)
        //{
        //    //创建用户， 想数据库填入用户数据

        //    //为用户创建英雄

        //    //跳转页面

        //}


        // 检测用户名是否重复
        //private bool CheckNameRepeat(string name)
        //{
        //    return TestDB.Instance.Users.Find(p=>p.Name == name) == null;
        //}


        private void InitBattle()
        {
            //if (battleRoomModel == null) battleRoomModel = new BattleRoomModel();

            //if (battleRoomModel.characterList == null) battleRoomModel.characterList = new List<BattleRoomModel.BattleCharacterModel>();
            //if (battleRoomModel.enemyList == null) battleRoomModel.enemyList = new List<BattleRoomModel.BattleEnemyModel>();
            //if (battleRoomModel.ReportQueue == null) battleRoomModel.ReportQueue = new Queue<BattleRoomModel.BattleReportModel>();

            //for (int i = 0; i < TestStaticData.Instance.BattleCharacterModel.Count; i++)
            //{
            //    BattleRoomModel.BattleCharacterModel model = new BattleRoomModel.BattleCharacterModel();
            //    model.ID = TestStaticData.Instance.BattleCharacterModel[i].ID;
            //    model.Name = TestStaticData.Instance.BattleCharacterModel[i].Name;
            //    model.RocaName = TestStaticData.Instance.BattleCharacterModel[i].RocaName;
            //    model.Career = TestStaticData.Instance.BattleCharacterModel[i].Career;
            //    model.Level = TestStaticData.Instance.BattleCharacterModel[i].Level;
            //    model.CurrentHP = TestStaticData.Instance.BattleCharacterModel[i].CurrentHP;
            //    model.MaxHP = TestStaticData.Instance.BattleCharacterModel[i].MaxHP;
            //    model.MaxMP_Txt = TestStaticData.Instance.BattleCharacterModel[i].MaxMP_Txt;
            //    model.CurrentMP_Txt = TestStaticData.Instance.BattleCharacterModel[i].CurrentMP_Txt;
            //    battleRoomModel.characterList.Add(model);
            //}
            //for (int i = 0; i < TestStaticData.Instance.BattleEnemyModel.Count; i++)
            //{
            //    BattleRoomModel.BattleEnemyModel model = new BattleRoomModel.BattleEnemyModel();
            //    model.ID = TestStaticData.Instance.BattleEnemyModel[i].ID;
            //    model.Name = TestStaticData.Instance.BattleEnemyModel[i].Name;
            //    model.Level = TestStaticData.Instance.BattleEnemyModel[i].Level;
            //    model.CurrentHP = TestStaticData.Instance.BattleEnemyModel[i].CurrentHP;
            //    model.MaxHP = TestStaticData.Instance.BattleEnemyModel[i].MaxHP;
            //    model.Ability_sprite = TestStaticData.Instance.BattleEnemyModel[i].Ability_sprite;
            //    battleRoomModel.enemyList.Add(model);
            //}
            //for (int i = 0; i < TestStaticData.Instance.BattleReportModel.Count; i++)
            //{
            //    BattleRoomModel.BattleReportModel model = new BattleRoomModel.BattleReportModel();
            //    model.DerateDamage = TestStaticData.Instance.BattleReportModel[i].DerateDamage;
            //    model.Attacker = TestStaticData.Instance.BattleReportModel[i].Attacker;
            //    model.AnAttacker = TestStaticData.Instance.BattleReportModel[i].AnAttacker;
            //    model.AbilityName = TestStaticData.Instance.BattleReportModel[i].AbilityName;
            //    model.ReportType = TestStaticData.Instance.BattleReportModel[i].ReportType;
            //    model.ReportNum = TestStaticData.Instance.BattleReportModel[i].ReportNum;
            //    model.IsAOE = TestStaticData.Instance.BattleReportModel[i].IsAOE;
            //    model.Gold = TestStaticData.Instance.BattleReportModel[i].Gold;
            //    model.Damage = TestStaticData.Instance.BattleReportModel[i].Damage;
            //    model.RestCountdown = TestStaticData.Instance.BattleReportModel[i].RestCountdown;

            //    model.TeamName = TestStaticData.Instance.BattleReportModel[i].TeamName;
            //    //model.AdditionalEntry = TestStaticData.Instance.BattleReportModel[i].AdditionalEntry;
            //    model.AttackHandleType = TestStaticData.Instance.BattleReportModel[i].AttackHandleType;
            //    model.AOEType = TestStaticData.Instance.BattleReportModel[i].AOEType;
            //    model.IsSussces = TestStaticData.Instance.BattleReportModel[i].IsSussces;
            //    model.BuffList = TestStaticData.Instance.BattleReportModel[i].BuffList;
            //    model.EquipmentList = TestStaticData.Instance.BattleReportModel[i].EquipmentList;
            //    model.Recovery = TestStaticData.Instance.BattleReportModel[i].Recovery;
            //    model.EXP = TestStaticData.Instance.BattleReportModel[i].EXP;
            //    model.IsDerateDamage = TestStaticData.Instance.BattleReportModel[i].IsDerateDamage;
            //    model.IsGroupAttack = TestStaticData.Instance.BattleReportModel[i].IsGroupAttack;
            //    battleRoomModel.ReportQueue.Enqueue(model);
            //}

        }

        public override void Init()
        {
            AddNetListener();
            
        }

        public override void AddNetListener()
        {
            base.AddNetListener();
            RegisterNetMsg(OpCodeUserOperation.Register,RegisterHandler);
            RegisterNetMsg(OpCodeUserOperation.Login, LoginHandler);
            RegisterNetMsg(OpCodeUserOperation.Create, CreateHandler);

        }

        private void CreateHandler(BaseMsgData data)
        {
            CreateUserRequestAndRespondeMsgData createUserRequestAndRespondeMsgData = data as CreateUserRequestAndRespondeMsgData;
            if (createUserRequestAndRespondeMsgData!= null)
            {
                if (createUserRequestAndRespondeMsgData.IsError)
                {
                    switch (createUserRequestAndRespondeMsgData.Error)
                    {
                        //TODO 错误协议
                        //case ErrorCode.CreateNameError:
                        //    SendMessage("Create", "CreateNameError", "角色名称重复！");
                        //    break;
                        //case ErrorCode.CreateTeamNameExist:
                        //    SendMessage("Create", "CreateTeamNameExist", "队伍名称重复！");
                        //    break;
                        default:
                            break;
                    }
                }
                else
                {
                    teamName = createUserRequestAndRespondeMsgData.TeamName;
                    userName = createUserRequestAndRespondeMsgData.UserName;
                    actorMsgDataList = createUserRequestAndRespondeMsgData.Actors;
                    OnOpenBattlePanel();

                }
            }
        }

        private void OnOpenBattlePanel()
        {
            //TODO 打开战斗界面
            if (battleRoomModel == null) battleRoomModel = new BattleRoomModel();
            #region
            //for (int i = 0; i < actorMsgDataList.Count; i++)
            //{
            //    foreach (var career in StaticDataMgr.mInstance.mCareerDataMap.Values)
            //    {
            //        if (career.ID == actorMsgDataList[i].CareerID)
            //        {
            //            vm1.Career = career.Name;
            //        }
            //    }
            //    foreach (var race in StaticDataMgr.mInstance.mRaceDataMap.Values)
            //    {
            //        if (race.ID == actorMsgDataList[i].RaceID)
            //        {
            //            vm1.MaxHP = race.m
            //        }
            //    }
            //    vm1.Level = (byte)actorMsgDataList[i].CareerLevel;
            //    vm1.MaxHP = (byte)actorMsgDataList[i]
            //}
            #endregion
            for (int i = 0; i < actorMsgDataList.Count; i++)
            {
                battleRoomModel.characterList.Add(GameService.Instance.actorService.GenerateHero(actorMsgDataList[i]));
            }

            battleRoomPanel = OpenUIForm("BattleRoom", battleRoomModel);
            battleRoomPanel.gameObject.SetActive(false);
        }
        /// <summary>
        /// 登录处理
        /// </summary>
        /// <param name="data"></param>
        private void LoginHandler(BaseMsgData data)
        {
            LoginRespondeMsgData loginRespondeMsgData = data as LoginRespondeMsgData;
            if (loginRespondeMsgData != null)
            {
                if (loginRespondeMsgData.IsError)
                {
                    switch (loginRespondeMsgData.Error)
                    {
                        case ErrorCode.LoginAccountError:
                            SendMessage("Login", "LoginAccountError", "邮箱账号或密码错误！");
                            break;
                        case ErrorCode.LoginPasswordError:
                            SendMessage("Login", "LoginPasswordError", "邮箱账号或密码错误！");
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Log("登录成功");
                    OnOpenCreateCharacterPanel();
                    loginPanel.gameObject.SetActive(false);
                }
            }
        }

        private void OnOpenCreateCharacterPanel()
        {
            
            //if (createCharacterViewModel == null) createCharacterViewModel = new CreateCharacterViewModel();
            //foreach (var item in StaticDataMgr.mInstance.mRaceDataMap.Values)
            //{
            //    createCharacterViewModel.createCharacterViewModels.Add(item);
            //}
            //createCharacterViewModel.createCharacterViewModels = createCharacterModel.createCharacterViewModels;
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
            //StaticData.Data.RaceData raceData1 = new StaticData.Data.RaceData();
            //raceData1.AbilityOneID = 1001;
            //raceData1.AbilityTwoID = 1002;
            //raceData1.ConGrowth = 1;
            //raceData1.Describe = "啊撒旦水水CDC";
            //raceData1.DexGrowth = 2;
            //raceData1.HPGrowth = 3;
            //raceData1.MPGrowth = 4;
            //raceData1.ID = 1;
            //raceData1.InitCon = 10;
            //raceData1.InitDex = 20;
            //raceData1.InitHP = 13;
            //raceData1.InitMP = 140;
            //raceData1.Name = "侏儒";
            //raceData1.PowGrowth = 5;
            //StaticData.Data.RaceData raceData2 = new StaticData.Data.RaceData();
            //raceData2.AbilityOneID = 1001;
            //raceData2.AbilityTwoID = 1002;
            //raceData2.ConGrowth = 1;
            //raceData2.Describe = "啊撒旦水水CDC";
            //raceData2.DexGrowth = 2;
            //raceData2.HPGrowth = 3;
            //raceData2.MPGrowth = 4;
            //raceData2.ID = 1;
            //raceData2.InitCon = 1;
            //raceData2.InitDex = 2;
            //raceData2.InitHP = 1;
            //raceData2.InitMP = 1;
            //raceData2.Name = "精灵";
            //raceData2.PowGrowth = 5;
            //createCharacterViewModel.createCharacterViewModels.Add(raceData);
            //createCharacterViewModel.createCharacterViewModels.Add(raceData1);
            //createCharacterViewModel.createCharacterViewModels.Add(raceData2);
            if (createCharacterViewModel == null) createCharacterViewModel = new CreateCharacterViewModel();

            if (createCharacterViewModel.createCharacterViewModels == null) createCharacterViewModel.createCharacterViewModels = new List<StaticData.Data.RaceData>();

            Dictionary<uint,StaticData.Data.RaceData> m_RaceDataDic = StaticDataMgr.mInstance.mRaceDataMap;
            //初始化所有职业数据
            foreach (var item in m_RaceDataDic.Values)
            {
                StaticData.Data.RaceData rocal = new StaticData.Data.RaceData();
                rocal.ID = item.ID;
                rocal.Name = item.Name;
                rocal.Describe = item.Describe;
                rocal.InitCon = item.InitCon;
                rocal.InitDex = item.InitDex;
                rocal.InitPow = item.InitPow;
                rocal.InitHP = item.InitHP;
                rocal.ConGrowth = item.ConGrowth;
                rocal.DexGrowth = item.DexGrowth;
                rocal.PowGrowth = item.PowGrowth;
                rocal.HPGrowth = item.HPGrowth;
                rocal.AbilityOneID = item.AbilityOneID;
                rocal.AbilityTwoID = item.AbilityTwoID;
                createCharacterViewModel.createCharacterViewModels.Add(rocal);
            }

            //初始化所有委托
            createCharacterViewModel.CreateCharacterCallback = CreateCharacterCallback;
            createCharacterViewModel.NameIsRepeatCallback = OnNameIsRepeatCallback;
            createCharacterViewModel.TeamNameIsRepeatCallback = OnTeamNameIsRepeatCallback;
            createCharacterPanel = OpenUIForm("CreateCharacter", createCharacterViewModel);
        }

        private bool OnTeamNameIsRepeatCallback(string arg1)
        {
            //TODO 检查创建角色队伍名称是否重复
            return true;
        }

        private bool OnNameIsRepeatCallback(string name)
        {
            //TODO 检查创建角色名称是否重复
            //SendNetMsg(OpCodeUserOperation.Create, name);
            return true;
        }

        private void CreateCharacterCallback(CreateCharacterPanel.CreateData data)
        {
            
            CreateUserRequestAndRespondeMsgData createUserRequestAndRespondeMsgData = new CreateUserRequestAndRespondeMsgData();
            createUserRequestAndRespondeMsgData.Actors = new List<ActorMsgData>();

            createUserRequestAndRespondeMsgData.UserName = data.playerName;
            //createUserRequestAndRespondeMsgData.TeamName
            ActorMsgData actorMsgData = new ActorMsgData();
            actorMsgData.Name = data.actorOneName;
            actorMsgData.RaceID = (int)data.rocaOneType;
            createUserRequestAndRespondeMsgData.Actors.Add(actorMsgData);
            actorMsgData.Name = data.actorTwoName;
            actorMsgData.RaceID = (int)data.rocaTwoType;
            createUserRequestAndRespondeMsgData.Actors.Add(actorMsgData);
            actorMsgData.Name = data.actorThreeName;
            actorMsgData.RaceID = (int)data.rocaThreeType;
            createUserRequestAndRespondeMsgData.Actors.Add(actorMsgData);

            SendNetMsg(OpCodeUserOperation.Create, createUserRequestAndRespondeMsgData);
            //TODO 隐藏创建角色界面
            createCharacterPanel.gameObject.SetActive(false);
            //TODO 打开战斗面板
            //OnOpenBattlePanel();
         
        }

        private void RegisterHandler(BaseMsgData data)
        {
            Log(data.GetType().Name);
            RegisterRespondeMsgData registerRespondeMsgData = data as RegisterRespondeMsgData;
            //Log(registerRespondeMsgData.userData.Name);
            Log("jinru");
            if (registerRespondeMsgData != null)
            {
                if(registerRespondeMsgData.IsError)
                {
                    ArrayList m_List = new ArrayList();
                    switch (registerRespondeMsgData.Error)
                    {
                        case ErrorCode.RegisterAccountError:
                            m_List.Add("邮箱账号错误！");
                            m_List.Add(registerRespondeMsgData.Error);
                            SendMessage("Register", ErrorCode.RegisterAccountError.ToString(), m_List);
                            break;
                        case ErrorCode.RegisterAccountExist:
                            Log("错误  邮箱账号已注册");
                            m_List.Add("邮箱账号已注册！");
                            m_List.Add(registerRespondeMsgData.Error);
                            SendMessage("Register", ErrorCode.RegisterAccountExist.ToString(), m_List);
                            break;
                        case ErrorCode.RegisterPasswordError:
                            m_List.Add("密码格式错误，请重新输入密码！");
                            m_List.Add(registerRespondeMsgData.Error);
                            SendMessage("Register", ErrorCode.RegisterPasswordError.ToString(), m_List);
                            break;
                        default:
                      
                            break;
                    }

                }
                else
                {
                    //TODO
                    //保存玩家信息
                    //前往创建界面

                    userEntity.CreateTime = registerRespondeMsgData.userData.CreateTime;
                    userEntity.ID = registerRespondeMsgData.userData.DatabaseID;
                    userEntity.Name = registerRespondeMsgData.userData.Name;

                    //TODO 打开创建角色面板
                    OnOpenCreateCharacterPanel();
                    //TODO 隐藏注册界面
                    registerPanel.gameObject.SetActive(false);
                    Log("成功 邮箱账号注册成功");
                }
            }
            else
            {
                LogError("RegisterHandler\\RegisterRespondeMsgData == null");
            }
        }
    }
}

