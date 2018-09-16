using System;

namespace Service
{
    public class GameService : ClassSingleton<GameService>
    {
        private UserService userService;                //用户服务
        private ActorService actorService;              //角色服务
        private BattleService battleService;            //战斗服务

        public void Initialize()
        {
            //初始化所有服务类
            userService = new UserService();
            actorService = new ActorService();
            battleService = new BattleService();
        }

        /// <summary>
        /// 登陆逻辑
        /// </summary>
        public void Login()
        {
            userService.Login();
        }
    }
}

