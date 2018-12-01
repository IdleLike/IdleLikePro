using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StaticData.Data
{
    public class RaceData : BaseDataObject
    {
        
		public string Name = "";	//名称
		public string Describe = "";	//描述
		public byte InitMP = 0;	//初始魔法
		public byte InitHP = 0;	//初始血量
		public byte InitPow = 0;	//初始力量
		public byte InitDex = 0;	//初始敏捷
		public byte InitCon = 0;	//初始体质
		public ushort MPGrowth = 0;	//种族魔法成长
		public ushort HPGrowth = 0;	//种族血量成长
		public ushort PowGrowth = 0;	//种族力量成长
		public ushort DexGrowth = 0;	//种族敏捷成长
		public ushort ConGrowth = 0;	//种族体质成长
		public uint AbilityOneID = 0;	//种族技能一ID
		public uint AbilityTwoID = 0;	//种族技能二ID
		
        public override void ReadFromStream(BinaryReader br)
        {
            ID = br.ReadUInt32();	//id
			Name = br.ReadString();	//名称
			Describe = br.ReadString();	//描述
			InitMP = br.ReadByte();	//初始魔法
			InitHP = br.ReadByte();	//初始血量
			InitPow = br.ReadByte();	//初始力量
			InitDex = br.ReadByte();	//初始敏捷
			InitCon = br.ReadByte();	//初始体质
			MPGrowth = br.ReadUInt16();	//种族魔法成长
			HPGrowth = br.ReadUInt16();	//种族血量成长
			PowGrowth = br.ReadUInt16();	//种族力量成长
			DexGrowth = br.ReadUInt16();	//种族敏捷成长
			ConGrowth = br.ReadUInt16();	//种族体质成长
			AbilityOneID = br.ReadUInt32();	//种族技能一ID
			AbilityTwoID = br.ReadUInt32();	//种族技能二ID
			
        }
    } 
} 