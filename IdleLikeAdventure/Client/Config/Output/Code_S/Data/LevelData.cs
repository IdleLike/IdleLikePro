using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StaticData.Data
{
    public class LevelData : BaseDataObject
    {
        
		public ushort Level = 0;	//等级
		public uint NextLevelNeedExp = 0;	//升到下一级所需经验
		public uint CurrentLevelNeedExp = 0;	//升到本级所需的总经验
		
        public override void ReadFromStream(BinaryReader br)
        {
            ID = br.ReadUInt32();	//id
			Level = br.ReadUInt16();	//等级
			NextLevelNeedExp = br.ReadUInt32();	//升到下一级所需经验
			CurrentLevelNeedExp = br.ReadUInt32();	//升到本级所需的总经验
			
        }
    } 
} 