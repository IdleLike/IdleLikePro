using System.Collections;
using System.IO;
namespace StaticData
{
    //数据结构基类
    public abstract class BaseDataObject
    {
        public ushort mID = 0; // ID
        public abstract void ReadFromStream(BinaryReader br);
    }
}