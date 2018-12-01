using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StaticData.Data
{
    public class CareerData : BaseDataObject
    {
        
		public uint ParenID = 0;	//父级职业ID
		public byte Order = 0;	//职业序号
		public byte MaxLevel = 0;	//职业最大等级
		public string Name = "";	//职业名称
		public string Describe = "";	//职业描述
		public ushort MPGrowth = 0;	//职业魔法成长
		public ushort HPGrowth = 0;	//职业血量成长
		public ushort PowGrowth = 0;	//职业力量成长
		public ushort DexGrowth = 0;	//职业敏捷成长
		public ushort ConGrowth = 0;	//职业体质成长
		public List<uint> AbilityList = new List<uint>();	//职业技能集合
		
        public override void ReadFromStream(BinaryReader br)
        {
            ID = br.ReadUInt32();	//ID
			ParenID = br.ReadUInt32();	//父级职业ID
			Order = br.ReadByte();	//职业序号
			MaxLevel = br.ReadByte();	//职业最大等级
			Name = br.ReadString();	//职业名称
			Describe = br.ReadString();	//职业描述
			MPGrowth = br.ReadUInt16();	//职业魔法成长
			HPGrowth = br.ReadUInt16();	//职业血量成长
			PowGrowth = br.ReadUInt16();	//职业力量成长
			DexGrowth = br.ReadUInt16();	//职业敏捷成长
			ConGrowth = br.ReadUInt16();	//职业体质成长
			ushort listCount_11 = br.ReadUInt16();
			for(ushort i = 0; i < listCount_11; i++)
				AbilityList.Add(br.ReadUInt32());	//职业技能集合
			
        }
    } 
} 