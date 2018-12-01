using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StaticData.Data
{
    public class CareerAbilityData : BaseDataObject
    {
        
		public string AbilityDescribe = "";	//职业技能描述
		public string AbilityName = "";	//职业技能名称
		public byte AbilityType = 0;	//技能类型(被动、主动)
		
        public override void ReadFromStream(BinaryReader br)
        {
            ID = br.ReadUInt32();	//id
			AbilityDescribe = br.ReadString();	//职业技能描述
			AbilityName = br.ReadString();	//职业技能名称
			AbilityType = br.ReadByte();	//技能类型(被动、主动)
			
        }
    } 
} 