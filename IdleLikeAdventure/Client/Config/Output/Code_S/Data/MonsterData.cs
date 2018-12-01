using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StaticData.Data
{
    public class MonsterData : BaseDataObject
    {
        
		public string Name = "";	//Name
		public ushort HP = 0;	//血量
		public ushort Pow = 0;	//力量
		public ushort Des = 0;	//敏捷
		public ushort Con = 0;	//体质
		public ushort MinAttack = 0;	//最小攻击
		public ushort MaxAttack = 0;	//最大攻击
		public ushort Defense = 0;	//防御
		public ushort RaceID = 0;	//种族
		public ushort CareerID = 0;	//职业
		public ushort Level = 0;	//等级
		public ushort EXP = 0;	//经验
		public List<uint> AbilityList = new List<uint>();	//技能集合
		
        public override void ReadFromStream(BinaryReader br)
        {
            ID = br.ReadUInt32();	//id
			Name = br.ReadString();	//Name
			HP = br.ReadUInt16();	//血量
			Pow = br.ReadUInt16();	//力量
			Des = br.ReadUInt16();	//敏捷
			Con = br.ReadUInt16();	//体质
			MinAttack = br.ReadUInt16();	//最小攻击
			MaxAttack = br.ReadUInt16();	//最大攻击
			Defense = br.ReadUInt16();	//防御
			RaceID = br.ReadUInt16();	//种族
			CareerID = br.ReadUInt16();	//职业
			Level = br.ReadUInt16();	//等级
			EXP = br.ReadUInt16();	//经验
			ushort listCount_13 = br.ReadUInt16();
			for(ushort i = 0; i < listCount_13; i++)
				AbilityList.Add(br.ReadUInt32());	//技能集合
			
        }
    } 
} 