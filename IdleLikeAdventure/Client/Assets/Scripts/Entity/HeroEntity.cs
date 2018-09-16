using System;
using System.Collections.Generic;
using UnityEngine;
using StaticData.Data;

namespace Entity
{
    public class HeroEntity : BaseEntity
    {
        //用户ID
        public int UserID;
        //名称
        public string Name;
        //种族ID
        public uint RaceID;
        //职业ID
        public uint CareerID;
        //职业等级
        public uint CareerLevel;
        //职业点数
        public uint CareerPoint;
        //总经验
        public uint TotalExp;
        //创建时间
        public DateTime CreateTime;
        //更新时间
        public DateTime UpdateTime;

    }
}

