﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.Message
{
    [Serializable]
    public class PlayerMsgData
    {
        public int DatabaseID;
        public string Name;
        public int ServerID;
    }
}
