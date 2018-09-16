using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StaticDataTool
{
    public class XmlFile
    {
        //加载Xml文件
        public void LoadXml(string filepathname, List<XmlSheet> allSheetList)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filepathname);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("default", "urn:schemas-microsoft-com:office:spreadsheet");
            var sheets = doc.SelectNodes("//default:Workbook/default:Worksheet", nsmgr);
            if (sheets.Count == 0)
                throw new Exception("No sheet in the xml file:" + Path.GetFileName(filepathname));
            //遍历所有sheet
            for (int i = 0; i < sheets.Count; i++)
            {
                XmlAttribute att = sheets[i].Attributes["ss:Name"];
                if (att != null)
                {
                    var sheetName = att.InnerText;
                    if (sheets.Count == 1)//如果只有1个sheet，那么用文件名作为表名
                        sheetName = Path.GetFileNameWithoutExtension(filepathname);
                    XmlSheet sheet = null; 
                    if(sheetName == "Strings")
                        sheet = new XmlSheet_Strings(sheetName);
                    else
                        sheet = new XmlSheet(sheetName);
                    sheet.Load(sheets[i], nsmgr);
                    allSheetList.Add(sheet);
                }
            }
        }
    }
}
