using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace StaticData
{
    public class FolderCfg
    {
        private static string BinPath = @".\Bin\";
        private static string XmlPath = @".\Xml\";
        public static string XmlFolder()
        {
            return XmlPath;
        }
        public static void SetXmlFolder(string folder)
        {
            XmlPath = folder;
            if (XmlPath.Substring(XmlPath.Length - 2, 1) != "\\")
                XmlPath += "\\";
        }

        public static string BinFolder()
        {
            return BinPath;
        }
        public static void SetBinFolder(string folder)
        {
            BinPath = folder;
            if (BinPath.Substring(BinPath.Length - 2, 1) != "\\")
                BinPath += "\\";
        }
    }
}
