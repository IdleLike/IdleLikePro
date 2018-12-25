using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StaticData.Data
{
    public class EquipmentData : BaseDataObject
    {
        
		public string Name = "";	//装备名称
		
        public override void ReadFromStream(BinaryReader br)
        {
            ID = br.ReadUInt32();	//id
			Name = br.ReadString();	//装备名称
			
        }
    } 
} 