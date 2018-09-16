using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StaticData.Data
{
    public class TestListData : BaseDataObject
    {
        
		
        public override void ReadFromStream(BinaryReader br)
        {
            mID = br.ReadUInt16();	//ID
			
        }
    } 
} 