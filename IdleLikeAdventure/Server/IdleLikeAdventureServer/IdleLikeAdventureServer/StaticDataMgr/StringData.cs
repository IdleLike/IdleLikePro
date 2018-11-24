using System;
using System.IO;

namespace StaticData.Data
{
    public class StringData : BaseDataStringIDObject
    {
        public string ID;
        public string EN;
        public string CN;

        public override void ReadFromStream(BinaryReader br)
        {
            ID = br.ReadString();
            EN = br.ReadString();
            CN = br.ReadString();
        }
    }
}

