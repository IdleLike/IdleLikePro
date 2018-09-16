using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StaticDataTool
{
    public class XmlSheet
    {
        public enum ERangeType { None, Server, Client, Both }
        protected const int HeaderRowCnt = 3;
        public string mName;
        public List<string> mComments = new List<string>();//注释
        public List<string> mTypes = new List<string>();//类型
        public List<string> mVarNames = new List<string>();//变量名
        public List<ERangeType> mRangeType = new List<ERangeType>();//范围类型（服务器、客户端）
        public List<List<string>> mValues = new List<List<string>>();//数据

        public XmlSheet(string sheetName)
        {
            mName = sheetName;
        }

        public virtual void Load(XmlNode sheetRootNode, XmlNamespaceManager nsmgr)
        {
            bool readfinished = false;
            var rows = sheetRootNode.SelectNodes("default:Table/default:Row", nsmgr);
            for (int r = 0 ; r < rows.Count; r++)
            {
                XmlNodeList cells = rows[r].SelectNodes("default:Cell", nsmgr);
                if (cells.Count == 0)
                    break;
                int colIdx = 0;//有可能有空数据的列
                for (int c = 0; c < cells.Count; c++)
                {
                    XmlNode celldata = cells[c].SelectSingleNode("default:Data", nsmgr);
                    if (r == 0)
                        ReadHeaderRow1(celldata.InnerText, c);
                    else if(r == 1)
                        ReadHeaderRow2(celldata.InnerText, c);
                    else if(r == 2)
                        ReadHeaderRow3(celldata.InnerText, c);
                    else//数据
                    {
						string text = "";
						if (celldata != null)
						{
							//cells.Count不一定等于列的数目。如果中间有空格的话，count就少了
							var ssIdx = celldata.ParentNode.Attributes["ss:Index"];//看<cell>节点有没有指定index
							if (ssIdx != null && ssIdx.Value != "")//指定了列（前面有空格的情况）
								colIdx = Int32.Parse(ssIdx.Value) - 1;
							text = celldata.InnerText;
						}
                        ReadCellData(text, r - HeaderRowCnt/*去掉标题3行*/, colIdx, mComments.Count);
                        colIdx++;
                    }
                }
                if (readfinished == true)
                    break;
            }
        }

        //标题第一行，增加一个列名
        protected virtual void ReadHeaderRow1(string cellValue, int colIdx /*0开始*/)
        {
            while (mComments.Count <= colIdx)
                mComments.Add("");
            mComments[colIdx] = cellValue;
        }
        //标题第二行，增加列类型和变量名
        protected virtual void ReadHeaderRow2(string cellValue, int colIdx/*0开始*/)
        {
            var valueArray = cellValue.Split("|".ToCharArray());
            if (valueArray.Length != 2)
                throw new Exception("row 2 col " + colIdx + " format error : " + cellValue);
            if(valueArray[0] == "" || valueArray[1] == "")
                throw new Exception("row 2 col " + colIdx + " format error : " + cellValue);
            var typeStr = valueArray[0];
            var varNameStr = valueArray[1];
            while (mTypes.Count <= colIdx)
                mTypes.Add("");
            mTypes[colIdx] = typeStr;
            while (mVarNames.Count <= colIdx)
                mVarNames.Add("");
            mVarNames[colIdx] = varNameStr;
        }
        //增加标题第三行，服务器还是客户端
        protected virtual void ReadHeaderRow3(string cellValue, int colIdx/*0开始*/)
        {
            ERangeType rangeType = ERangeType.Both;
            string lStr = cellValue.ToLower();
            if (lStr.Contains("server"))
                rangeType = ERangeType.Server;
            else if(lStr.Contains("client"))
                rangeType = ERangeType.Client;
            else if(lStr.Contains("none"))
                rangeType = ERangeType.None;
            else
                rangeType = ERangeType.Both;
            while (mRangeType.Count <= colIdx)
                mRangeType.Add(ERangeType.Both);
            mRangeType[colIdx] = rangeType;
        }

        //设定数据
        protected virtual void ReadCellData(string cellValue, int rowIdx/*0开始*/, int colIdx/*0开始*/, int maxColCnt)
        {
            while (mValues.Count <= rowIdx)
                mValues.Add(new List<string>());
            while (mValues[rowIdx].Count <= maxColCnt)
                mValues[rowIdx].Add("");
            mValues[rowIdx][colIdx] = cellValue;
        }
    }
}
