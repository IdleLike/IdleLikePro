using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StaticDataTool
{
    public class XmlSheet_Strings : XmlSheet
    {
        private int mLanguageCnt = 0;//语言数量
        public static Dictionary<string, ushort> sRef2IDMap = new Dictionary<string, ushort>();
        public XmlSheet_Strings(string sheetName)
            :base(sheetName)
        {
        }
        public override void Load(XmlNode sheetRootNode, XmlNamespaceManager nsmgr)
        {
            base.Load(sheetRootNode, nsmgr);
        }

        //标题第一行，增加一个列名
        protected override void ReadHeaderRow1(string cellValue, int colIdx /*0开始*/)
        {
            while (mComments.Count < 3)
                mComments.Add("");
            if (colIdx == 0 || colIdx == 1)//ID列， 引用列
                mComments[colIdx] = cellValue;
            else//数据列
            {
                if (colIdx == 2)
                    mComments[2] = "StringTable";
                mLanguageCnt++;
            }

        }
        //标题第二行，增加列类型和变量名
        protected override void ReadHeaderRow2(string cellValue, int colIdx/*0开始*/)
        {
            if (colIdx > 2)
                return;
            
            while (mTypes.Count < 3)
                mTypes.Add("");
            while (mVarNames.Count < 3)
                mVarNames.Add("");
            if (colIdx < 2)
            {//前两列
                var valueArray = cellValue.Split("|".ToCharArray());
                if (valueArray.Length != 2)
                    throw new Exception("row 2 col " + colIdx + " format error : " + cellValue);
                if (valueArray[0] == "" || valueArray[1] == "")
                    throw new Exception("row 2 col " + colIdx + " format error : " + cellValue);
                var typeStr = valueArray[0];
                var varNameStr = valueArray[1];
                mTypes[colIdx] = typeStr;
                mVarNames[colIdx] = varNameStr;
            }
            else
            {
                mTypes[2] = "string["+mLanguageCnt+"]";
                mVarNames[2] = "mStrings";
            }
        }
        //增加标题第三行，服务器还是客户端
        protected override void ReadHeaderRow3(string cellValue, int colIdx/*0开始*/)
        {
            if (colIdx > 2)
                return;
            while (mRangeType.Count < 3)
                mRangeType.Add(ERangeType.Client);
            mRangeType[0] = ERangeType.Both;
        }

        //设定数据
        protected override void ReadCellData(string cellValue, int rowIdx/*0开始*/, int colIdx/*0开始*/, int maxColCnt)
        {
            while (mValues.Count <= rowIdx)
                mValues.Add(new List<string>());
            while (mValues[rowIdx].Count < 3)
                mValues[rowIdx].Add("");
            if (colIdx < 2)
            {
                mValues[rowIdx][colIdx] = cellValue;
                if(colIdx == 1)
                {//建立索引
                    sRef2IDMap.Add(cellValue, UInt16.Parse(mValues[rowIdx][0]));
                }
            }
            else
            {
                if (mValues[rowIdx][2] == "")
                    mValues[rowIdx][2] += cellValue;
                else
                    mValues[rowIdx][2] += "|"+cellValue;
            }
        }
    }
}
