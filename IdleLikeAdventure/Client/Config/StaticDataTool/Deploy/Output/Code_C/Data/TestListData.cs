using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StaticData.Data
{
    public class TestListData : BaseDataObject
    {
        
		public string mName = "";	//词库名称
		public string mIconName = "";	//图标名称
		public List<int> mCategoryDataName = new List<int>();	//词库数据名称
		
        public override void ReadFromStream(BinaryReader br)
        {
            mID = br.ReadUInt16();	//ID
			mName = br.ReadString();	//词库名称
			mIconName = br.ReadString();	//图标名称
			ushort listCount_3 = br.ReadUInt16();
			for(ushort i = 0; i < listCount_3; i++)
				mCategoryDataName.Add(br.ReadInt32());	//词库数据名称
			
        }
    } 
} 