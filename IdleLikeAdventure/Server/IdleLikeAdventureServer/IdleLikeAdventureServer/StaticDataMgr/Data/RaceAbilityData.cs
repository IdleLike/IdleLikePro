using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StaticData.Data
{
    public class RaceAbilityData : BaseDataObject
    {
        
		public string AbilityName = "";	//种族技能名称
		public string AbilityDescribe = "";	//种族技能描述
		
        public override void ReadFromStream(BinaryReader br)
        {
            ID = br.ReadUInt32();	//id
			AbilityName = br.ReadString();	//种族技能名称
			AbilityDescribe = br.ReadString();	//种族技能描述
			
        }
    } 
} 