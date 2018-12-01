using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StaticData.Data
{
    public class StageData : BaseDataObject
    {
        
		public string Name = "";	//关卡名称
		public uint BOSSID = 0;	//BOSSID
		public ushort BOSSProbability = 0;	//BOSS
		public List<uint> MonsterList = new List<uint>();	//怪物集合
		
        public override void ReadFromStream(BinaryReader br)
        {
            ID = br.ReadUInt32();	//id
			Name = br.ReadString();	//关卡名称
			BOSSID = br.ReadUInt32();	//BOSSID
			BOSSProbability = br.ReadUInt16();	//BOSS
			ushort listCount_4 = br.ReadUInt16();
			for(ushort i = 0; i < listCount_4; i++)
				MonsterList.Add(br.ReadUInt32());	//怪物集合
			
        }
    } 
} 