using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StaticData.Data
{
    public class TestData : BaseDataObject
    {
        
		public string mRefName = "";	//引用名称
		public string mEn = "";	//英
		public string mCn_S = "";	//中简
		public string mCn_T = "";	//中繁
		
        public override void ReadFromStream(BinaryReader br)
        {
            ID = br.ReadUInt32();	//id
			mRefName = br.ReadString();	//引用名称
			mEn = br.ReadString();	//英
			mCn_S = br.ReadString();	//中简
			mCn_T = br.ReadString();	//中繁
			
        }
    } 
} 