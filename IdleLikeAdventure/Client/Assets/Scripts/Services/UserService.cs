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
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            //加载文档数据
            StaticDataMgr.mInstance.LoadData();
            //监听网络消息
            AddNetListener();
        }
        /// <summary>
        /// 监听网络消息
        /// </summary>
        public override void AddNetListener()
        {
            base.AddNetListener();
            RegisterNetMsg(OpCodeUserOperation.Register,RegisterHandler);
            RegisterNetMsg(OpCodeUserOperation.Login, LoginHandler);
            RegisterNetMsg(OpCodeUserOperation.Create, CreateHandler);

        }


        /// <summary>
        /// 打开战斗界面
        /// </summary>
        private void OnOpenBattlePanel()
        {
            if (battleRoomModel == null) battleRoomModel = new BattleRoomModel();
            battleRoomModel.characterList = new List<HeroEntity>();
            for (int i = 0; i < actorMsgDataList.Count; i++)
            {
                battleRoomModel.characterList.Add(GameService.Instance.actorService.GenerateHero(actorMsgDataList[i]));
            }
            battleRoomPanel = OpenUIForm("BattleRoom", battleRoomModel);
            battleRoomPanel.gameObject.SetActive(true);
        }

        /// <summary>
        /// 打开创建角色界面
        /// </summary>
        private void OnOpenCreateCharacterPanel()
        {
            if (createCharacterViewModel == null) createCharacterViewModel = new CreateCharacterViewModel();

            if (createCharacterViewModel.createCharacterViewModels == null) createCharacterViewModel.createCharacterViewModels = new List<StaticData.Data.RaceData>();

            Dictionary<uint,StaticData.Data.RaceData> m_RaceDataDic = StaticDataMgr.mInstance.mRaceDataMap;
            Log("StaticDataMgr.mInstance.mRaceDataMap：" + StaticDataMgr.mInstance.mRaceDataMap.Count);

            foreach (var item in StaticDataMgr.mInstance.mRaceDataMap.Keys)
            {
                Log("key：" + item);
                Log("value：" + m_RaceDataDic[item]);

            }
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
        /// <summary>
        /// 创建玩家回调，发送玩家信息至服务端
        /// </summary>
        /// <param name="data"></param>
        private void CreateCharacterCallback(CreateCharacterPanel.CreateData data)
        {
            
            CreateUserRequestAndRespondeMsgData createUserRequestAndRespondeMsgData = new CreateUserRequestAndRespondeMsgData();
            createUserRequestAndRespondeMsgData.Actors = new List<ActorMsgData>();

            createUserRequestAndRespondeMsgData.UserName = data.playerName;
            //createUserRequestAndRespondeMsgData.TeamName
            //TODO 添加 ID
            createUserRequestAndRespondeMsgData.Actors.Add(new ActorMsgData() { Name = data.actorOneName, RaceID = (int)data.rocaOneType, CareerID = 1 });
            createUserRequestAndRespondeMsgData.Actors.Add(new ActorMsgData() { Name = data.actorTwoName, RaceID = (int)data.rocaTwoType, CareerID = 1 });
            createUserRequestAndRespondeMsgData.Actors.Add(new ActorMsgData() { Name = data.actorThreeName, RaceID = (int)data.rocaThreeType, CareerID = 1 });

            SendNetMsg(OpCodeUserOperation.Create, createUserRequestAndRespondeMsgData);
            //TODO 隐藏创建玩家界面
            createCharacterPanel.gameObject.SetActive(false);
            //TODO 暂时保存玩家信息
            teamName = createUserRequestAndRespondeMsgData.TeamName;
            userName = createUserRequestAndRespondeMsgData.UserName;
            actorMsgDataList = createUserRequestAndRespondeMsgData.Actors;
      
            //TODO 打开战斗界面
            OnOpenBattlePanel();
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
                Log("登录 ： " + loginRespondeMsgData);
                Log("登录 ： " + loginRespondeMsgData.Error.ToString() + "是否有错 ：" +loginRespondeMsgData.IsError);
                if (loginRespondeMsgData.IsError)
                {
                    ArrayList m_List = new ArrayList();
                    switch (loginRespondeMsgData.Error)
                    {
                        case ErrorCode.LoginAccountError:
                        case ErrorCode.LoginPasswordError:

                            Log("注册失败：" + loginRespondeMsgData.Error.ToString() + "邮箱账号或密码错误");

                            m_List.Add("邮箱账号或密码错误！");
                            m_List.Add(loginRespondeMsgData.Error);
                            SendMessage("Login", ErrorCode.LoginAccountError.ToString(), m_List);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Log("登录成功");
                    loginPanel.gameObject.SetActive(false);
                    OnOpenCreateCharacterPanel();
                }
            }
        }
        /// <summary>
        /// 注册处理
        /// </summary>
        /// <param name="data"></param>
        private void RegisterHandler(BaseMsgData data)
        {
            Log(data.GetType().Name);
            RegisterRespondeMsgData registerRespondeMsgData = data as RegisterRespondeMsgData;
            Log("注册失败："+registerRespondeMsgData.Error.ToString());
            if (registerRespondeMsgData != null)
            {
                if(registerRespondeMsgData.IsError)
                {
                    ArrayList m_List = new ArrayList();
                    switch (registerRespondeMsgData.Error)
                    {
                        case ErrorCode.RegisterAccountError:
                            Log("注册失败：" + registerRespondeMsgData.Error.ToString() + "邮箱账号错误");

                            m_List.Add("邮箱账号不合法！");
                            m_List.Add(registerRespondeMsgData.Error);
                            SendMessage("Register", ErrorCode.RegisterAccountError.ToString(), m_List);

                            break;
                        case ErrorCode.RegisterAccountExist:
                            Log("注册失败：" + registerRespondeMsgData.Error.ToString() + "邮箱账号已注册");

                            m_List.Add("邮箱账号已注册！");
                            m_List.Add(registerRespondeMsgData.Error);
                            SendMessage("Register", ErrorCode.RegisterAccountExist.ToString(), m_List);
                            break;
                        case ErrorCode.RegisterPasswordError:
                            Log("注册失败：" + registerRespondeMsgData.Error.ToString() + "密码格式错误，请重新输入密码！");

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
                    
                    //TODO 隐藏注册界面
                    registerPanel.gameObject.SetActive(false);
                    Log("成功 邮箱账号注册成功");
                    //TODO 打开创建角色面板
                    OnOpenCreateCharacterPanel();
               
                }
            }
            else
            {
                LogError("RegisterHandler\\RegisterRespondeMsgData == null");
            }
        }
        /// <summary>
        /// 创建角色处理
        /// </summary>
        /// <param name="data"></param>
        private void CreateHandler(BaseMsgData data)
        {
            CreateUserRequestAndRespondeMsgData createUserRequestAndRespondeMsgData = data as CreateUserRequestAndRespondeMsgData;
            if (createUserRequestAndRespondeMsgData != null)
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
    }
}

