﻿using NetData.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.Message
{
    [Serializable]
    public class AttackInfoMsgData
    {
        public byte AttakPos;
        public byte DefendsPos;
        public uint AbilityID;
        public EnumAbilityEffect EffectType;
        public int AbiilityEffectValue;
    }
}
