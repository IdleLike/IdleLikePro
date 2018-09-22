/*
 
*  Title:"地下守护神"项目开发
*  
*      核心层：配置管理器
*      
*  Description:
*      
*      作用：读取系统核心XML配置信息
*      
*  Date:2018
*  
*  Version:2.12
*  
*  Modify Recoder: 
* 
*/
using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq; //XDocument 的命名空间
using System.IO;    //文件输出流

namespace Log
{
    public class ConfigManager : IConfigManager
    {
        /// <summary>
        /// 应用设置集合
        /// </summary>
        private static Dictionary<string, string> _AppSetting;

        /// <summary>
        /// 配置管理器构造函数
        /// </summary>
        /// <param name="logPath">日志路径</param>
        /// <param name="xmlRootNodeName">XML根节点名称</param>
        public ConfigManager(string logPath, string xmlRootNodeName)
        {
            _AppSetting = new Dictionary<string, string>();
            //初始化解析XML数据，到集合中(_AppSetting)
            InitAndAnalysisXML(logPath, xmlRootNodeName);
        }

        /// <summary>
        /// 初始化解析XML数据，到集合中(_AppSetting)
        /// </summary>
        /// <param name="logPath">日志路径</param>
        /// <param name="xmlRootNodeName">XML根节点名称</param>
        private void InitAndAnalysisXML(string logPath,string xmlRootNodeName)
        {
            //参数检查

            if (string.IsNullOrEmpty(logPath) || string.IsNullOrEmpty(xmlRootNodeName))
            {
                return;
            }
            //代表XML文档
            XDocument xmlDoc;
            //XML读写器
            XmlReader xmlReader = null;

            try
            {
                //加载日志路径
                xmlDoc = XDocument.Load(logPath);
                //创建XML读写器
                xmlReader = XmlReader.Create(new StringReader(xmlDoc.ToString()));
            }
            catch
            {
                //throw new System.Exception(GetType() + "/InitAndAnalysisXML()/XML Analysis Exception! ,please Check!");
                //throw new XMLAnalysisException(GetType() + "/InitAndAnalysisXML()/XML Analysis Exception! ,please Check!");
            }

            //循环解析XML
            while (xmlReader.Read())
            {
                //XML读写器从指定根节点开始读写
                if (xmlReader.IsStartElement() && xmlReader.LocalName == xmlRootNodeName)
                {
                    using (XmlReader xmlReaderItem = xmlReader.ReadSubtree())
                    {
                        while (xmlReaderItem.Read())
                        {
                            //如果是节点元素(标签)
                            if (xmlReaderItem.NodeType == XmlNodeType.Element)
                            {
                                //节点元素
                                string strNode = xmlReaderItem.Name;
                                //读XML当前行的下一个内容
                                xmlReaderItem.Read();
                                //如果是节点内容
                                if (xmlReaderItem.NodeType == XmlNodeType.Text)
                                {
                                    //XML当前行，键值对赋值
                                    _AppSetting[strNode] = xmlReaderItem.Value;
                                }
                            }
                        }
                    }
                }
            }
           
        }

        /// <summary>
        /// 属性：应用设置
        /// </summary>
        public Dictionary<string, string> AppSetting
        {
            get { return _AppSetting; }
        }

        /// <summary>
        /// 得到AppSetting的最大数量
        /// </summary>
        public int GetAppSettingMaxNumber()
        {
            if (_AppSetting != null && _AppSetting.Count >= 1)
            {
                return _AppSetting.Count;
            }
            return 0;
        }


    }
}
