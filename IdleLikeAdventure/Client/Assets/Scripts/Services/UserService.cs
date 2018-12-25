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
        public UIBattleRoomViewModel battleRoomModel;
        public BaseUIForm battleRoomPanel;

        //ViewModel
        private CreateCharacterViewModel createCharacterModel;
  

        private LoginPanel.LoginViewModel loginViewModel;
        private RegisterPanel.RegisterViewModel registerViewModel;
        private CreateCharacterViewModel createCharacterViewModel;
        private BaseUIForm loginPanel;
        private BaseUIForm registerPanel;
        private BaseUIForm createCharacterPanel;

        //private UserEntity userEntity = new UserEntity();
        //private List<ActorMsgData> actorMsgDataList = new List<ActorMsgData>();
        //private string teamName = string.Empty;
        //private string playerName = string.Empty;
        //private int accountID;
        //private int playerID;
        //private int serverID;

        //public int teamID;
        public Dictionary<int, List<UIBattleRoomViewModel.BattleCharacter>> TeamList = new Dictionary<int, List<UIBattleRoomViewModel.BattleCharacter>>();


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
            //保存加载的文档数据
            StaticDataHelper.SaveData();
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
            //if (battleRoomModel == null) battleRoomModel = new UIBattleRoomViewModel();
            //battleRoomModel.characterList = new List<UIBattleRoomViewModel.BattleCharacter>(); //new List<HeroEntity>();
            //List<ActorMsgData> ActorDataList = DataCenter.Instance.userData.ActorDataList;
            //for (int i = 0; i < ActorDataList.Count; i++)
            //{
            //    HeroEntity hero = GameService.Instance.actorService.GenerateHero(ActorDataList[i]);
            //    UIBattleRoomViewModel.BattleCharacter character = new UIBattleRoomViewModel.BattleCharacter();
            //    character.CareerName = hero.CareerData.Name;
            //    character.MaxEXP = StaticDataMgr.mInstance.mLevelDataMap[hero.Level].NextLevelNeedExp;
            //    character.CurrentEXP = hero.Exp - StaticDataMgr.mInstance.mLevelDataMap[hero.Level].CurrentLevelNeedExp;
            //    character.Level = hero.Level;
            //    character.MaxHP = hero.MaxHP;
            //    character.Name = hero.Name;
            //    character.RaceName = hero.RaceData.Name;
            //    battleRoomModel.characterList.Add(character);
            //}
            //请求战斗哦
            GameService.Instance.battleService.BattleRequest();

   
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

            createUserRequestAndRespondeMsgData.PlayerName = data.playerName;
            createUserRequestAndRespondeMsgData.TeamName = data.teamName;
            createUserRequestAndRespondeMsgData.AccountID = DataCenter.Instance.userData.AccountID;
            //TODO 添加 ID
            createUserRequestAndRespondeMsgData.Actors.Add(new ActorMsgData() { Name = data.actorOneName , RaceID = (int)data.raceOneType });//, CareerID = (int)StaticDataHelper.GetCareerIDByName("战士")});
            createUserRequestAndRespondeMsgData.Actors.Add(new ActorMsgData() { Name = data.actorTwoName , RaceID = (int)data.raceTwoType });//, CareerID = (int)StaticDataHelper.GetCareerIDByName("战士") });
            createUserRequestAndRespondeMsgData.Actors.Add(new ActorMsgData() { Name = data.actorThreeName  , RaceID = (int)data.raceThreeType });//, CareerID = (int)StaticDataHelper.GetCareerIDByName("魔法") });

            Log("创建回调");
            SendNetMsg(OpCodeUserOperation.Create, createUserRequestAndRespondeMsgData);
            //TODO 暂时保存玩家信息
            //teamName = createUserRequestAndRespondeMsgData.TeamName;
            //playerName = createUserRequestAndRespondeMsgData.PlayerName;
            //actorMsgDataList = createUserRequestAndRespondeMsgData.Actors;
      
            //TODO 打开战斗界面
            //OnOpenBattlePanel();
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
                Log("登录 ： " + loginRespondeMsgData.Error.ToString() + "是否有错 ：" + loginRespondeMsgData.IsError);
                if (loginRespondeMsgData.IsError)
                {
                    ArrayList m_List = new ArrayList();
                    switch (loginRespondeMsgData.Error)
                    {
                        case ErrorCode.LoginAccountError:
                        case ErrorCode.LoginPasswordError:
                            Log("注册失败：" + loginRespondeMsgData.Error.ToString() + "邮箱账号或密码错误");
                            m_List.Add("邮箱账号或密码错误！");
                            break;
                        default:
                            break;
                    }
                    m_List.Add(loginRespondeMsgData.IsError);
                    SendMessage("Login", loginRespondeMsgData.Error.ToString(), m_List);
                }
                else
                {
                    DataCenter.Instance.userData.AccountID = loginRespondeMsgData.AccountID;
                    DataCenter.Instance.userData.ActorDataList = loginRespondeMsgData.Actors;
                    DataCenter.Instance.userData.PlayerID = loginRespondeMsgData.Player.DatabaseID;
                    DataCenter.Instance.userData.PlayerName = loginRespondeMsgData.Player.Name;
                    DataCenter.Instance.userData.ServerID = loginRespondeMsgData.Player.ServerID;


                    DataCenter.Instance.userData.TeamDataList = loginRespondeMsgData.Teams;
                    if (loginRespondeMsgData.IsNewPlayer)
                    {
                        Log("新用户登录成功");
                        loginPanel.gameObject.SetActive(false);
                        OnOpenCreateCharacterPanel();
                    }
                    else
                    {
                        //TODO 打开面板
                        Log("老用户登录成功");
                        loginPanel.gameObject.SetActive(false);
                        //打开战斗界面
                        OnOpenBattlePanel();
                    }
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
                            break;
                        case ErrorCode.RegisterAccountExist:
                            Log("注册失败：" + registerRespondeMsgData.Error.ToString() + "邮箱账号已注册");
                            m_List.Add("邮箱账号已注册！");
                            break;
                        case ErrorCode.RegisterPasswordError:
                            Log("注册失败：" + registerRespondeMsgData.Error.ToString() + "密码格式错误，请重新输入密码！");
                            m_List.Add("密码格式错误，请重新输入密码！");
                            break;
                        default:
                      
                            break;
                    }
                    m_List.Add(registerRespondeMsgData.IsError);
                    SendMessage("Register", registerRespondeMsgData.Error.ToString(), m_List);
                }
                else
                {
                    //TODO
                    //保存玩家信息
                    DataCenter.Instance.userData.CreateTime = registerRespondeMsgData.userData.CreateTime;
                    DataCenter.Instance.userData.AccountID = registerRespondeMsgData.userData.DatabaseID;
                    DataCenter.Instance.userData.UserName = registerRespondeMsgData.userData.Name;
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
                Log("创建 ： " + createUserRequestAndRespondeMsgData.Error.ToString() + "是否有错 ：" + createUserRequestAndRespondeMsgData.IsError);

                if (createUserRequestAndRespondeMsgData.IsError)
                {
                    ArrayList m_List = new ArrayList();
                    switch (createUserRequestAndRespondeMsgData.Error)
                    {
                        case ErrorCode.CreatePlayerError:
                            Log("创建玩家错误");
                            m_List.Add("创建玩家错误！");
                            break;
                        case ErrorCode.CreatePlayerNameExit:
                            Log("玩家名称重复");
                            m_List.Add("玩家名称重复！");
                            break;
                        case ErrorCode.CreateTeamNameExit:
                            Log("队伍名称重复");
                            m_List.Add("队伍名称重复！");
                            break;
                        case ErrorCode.CreateActorNameExit:
                            Log("英雄名称重复");
                            m_List.Add("英雄名称重复！");
                            break;
                        case ErrorCode.CreateActorRaceIDNonExit:
                            Log("种族不存在");
                            m_List.Add("种族不存在！");
                            break;
                        case ErrorCode.CreateActorCareerNonExit:
                            Log("职业不存在");
                            m_List.Add("职业不存在！");
                            break;
                        default:
                            break;
                    }
                    m_List.Add(createUserRequestAndRespondeMsgData.IsError);
                    SendMessage("Create", createUserRequestAndRespondeMsgData.Error.ToString(), m_List);
                }
                else
                {
                    Log("创建成功,打开战斗界面");
                    DataCenter.Instance.userData.TeamName = createUserRequestAndRespondeMsgData.TeamName;
                    DataCenter.Instance.userData.PlayerName = createUserRequestAndRespondeMsgData.PlayerName;
                    DataCenter.Instance.userData.ActorDataList = createUserRequestAndRespondeMsgData.Actors;
                    DataCenter.Instance.userData.AccountID = createUserRequestAndRespondeMsgData.AccountID;
                    DataCenter.Instance.userData.PlayerID = createUserRequestAndRespondeMsgData.PlayerID;
                    DataCenter.Instance.userData.ServerID = createUserRequestAndRespondeMsgData.ServerID;
                    DataCenter.Instance.userData.TeamID = createUserRequestAndRespondeMsgData.TeamID;
                    OnOpenBattlePanel();
                    //TODO 隐藏创建玩家界面
                    createCharacterPanel.gameObject.SetActive(false);
                }
            }
            else
            {
                Log(GetType() + "：createUserRequestAndRespondeMsgData == null");
            }
        }
    }
}

