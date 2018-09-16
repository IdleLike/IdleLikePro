using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StaticDataTool
{
    public class StaticDataTool
    {
        static string mDataFileTemplate = "";
        static List<XmlSheet> mSheetList = new List<XmlSheet>();
        static Dictionary<string, string> mType2ReadFunc = new Dictionary<string, string>(){
            { "long", "ReadInt64()" }, {"ulong", "ReadUInt64()"},
            { "int", "ReadInt32()" }, {"uint", "ReadUInt32()"},
            { "short", "ReadInt16()" }, {"ushort", "ReadUInt16()"},
            { "float", "ReadSingle()" }, {"double", "ReadDouble()"},
            { "string", "ReadString()" }, {"byte", "ReadByte()"},
        };

        static void Main(string[] args)
        {
            //处理所有文件
            foreach (var arg in args)
            {
                try
                {
                    FileInfo fi = new FileInfo(arg);
                    if ((((ushort)fi.Attributes) | ((ushort)FileAttributes.Directory)) > 0)
                    {//是个路径
                        ReadFolder(arg);
                    }
                    else//是个文件
                        ReadFile(arg);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            Console.WriteLine("Read Xml data done");
            //初始化输出目录
            InitOutputDirectory();
            //生成CS文件
            //加载Template
            var fs = new FileStream("./../Input/TemplateFiles/DataFileTemplate.txt", FileMode.Open);
            var sr = new StreamReader(fs);
            mDataFileTemplate = sr.ReadToEnd();
            sr.Close();
            //生成所有表对应的cs文件
            foreach (var sheet in mSheetList)
            {
                if (sheet.mName.Equals("String")) continue;
                WriteSheet2CSFile(sheet, true);
                WriteSheet2CSFile(sheet, false);
            }
            //生成bin文件
            foreach (var sheet in mSheetList)
            {
                WriteSheet2BinFile(sheet, true);
                WriteSheet2BinFile(sheet, false);
            }
            //生成StaticDataMgr.cs文件
            WriteStaticDataFile(true);
            WriteStaticDataFile(false);
            //调用处理后的脚本
            if (File.Exists("./PostConvert.bat"))
                System.Diagnostics.Process.Start("PostConvert.bat");
            else
                Console.WriteLine("Create a file named PostConvert.bat to do post convert jobs");

            Console.WriteLine("Convert finished!");
            Console.ReadKey();

        }



        //加载路径
        static void ReadFolder(string folderPath)
        {
            DirectoryInfo folderInfo = new DirectoryInfo(folderPath);
            DirectoryInfo[] subfolders = folderInfo.GetDirectories();
            //遍历文件
            FileInfo[] files = folderInfo.GetFiles();
            foreach (FileInfo file in files)
            {
				if(file.Extension.ToLower() == ".xml")
                	ReadFile(file.FullName);
            }
            foreach (DirectoryInfo folder in subfolders)
            {//遍历子文件夹
                ReadFolder(folder.FullName);
            }
        }

        //加载文件
        static void ReadFile(string filePathName)
        {
			if (filePathName.Contains("._"))
				return;
            XmlFile xmlfile = new XmlFile();
            try
            {
                xmlfile.LoadXml(filePathName, mSheetList);
                Console.WriteLine("Read File Done! [" + Path.GetFileName(filePathName) + "]");
            }
            catch (Exception e)
            {
                Console.WriteLine("!!Error[" + Path.GetFileName(filePathName) + "] \n" + e.ToString());
            }
        }
        static void InitOutputDirectory()
        {
            if (Directory.Exists("./../Output") == true)
                Directory.Delete("./../Output", true);
            Directory.CreateDirectory("./../Output");
            Directory.CreateDirectory("./../Output/Code_C");
            Directory.CreateDirectory("./../Output/Code_C/Data");
            Directory.CreateDirectory("./../Output/Code_S");
            Directory.CreateDirectory("./../Output/Code_S/Data");
            Directory.CreateDirectory("./../Output/Bin_C");
            Directory.CreateDirectory("./../Output/Bin_S");
        }
        //生成CS文件
        static void WriteSheet2CSFile(XmlSheet sheet, bool isServer)
        {
            var csFileName = "./../Output/Code_"+ (isServer?"S":"C") +"/Data/" + sheet.mName + "Data.cs";
            var finalContent = mDataFileTemplate;//保存最终结果的变量
            //类名
            var className = sheet.mName + "Data";
            finalContent = finalContent.Replace("{$ClassName}", className);
            //成员和读数据
            string memberVars = "";//成员变量
            string readDataCode = "";//读数据的代码
            //读表格中所有的列
            for (int i = 0; i < sheet.mComments.Count; i++)
            {
                if(isServer)
                {//服务器
                    if (sheet.mRangeType[i] == XmlSheet.ERangeType.Client || sheet.mRangeType[i] == XmlSheet.ERangeType.None)
                        continue;
                }
                else
                {//客户端
                    if (sheet.mRangeType[i] == XmlSheet.ERangeType.Server || sheet.mRangeType[i] == XmlSheet.ERangeType.None)
                        continue;
                }
                var curTypeStr = sheet.mTypes[i];
                if (curTypeStr == "string reference")//字符串引用,转换成ushort型的id
                    curTypeStr = "ushort";
                if (curTypeStr == "long" || curTypeStr == "ulong" || curTypeStr == "int" || curTypeStr == "uint" ||
                    curTypeStr == "short" || curTypeStr == "ushort" || curTypeStr == "byte" || curTypeStr == "float" || curTypeStr == "double")
                {//数字型
                    if (i == 0)
                    {//ID列，特殊处理
                        sheet.mVarNames[i] = "ID";
                    }
                    else
                        memberVars += "public " + curTypeStr + " " + sheet.mVarNames[i] + " = 0;\t//" + sheet.mComments[i];
                    readDataCode += sheet.mVarNames[i] + " = br." + mType2ReadFunc[curTypeStr] + ";\t//" + sheet.mComments[i];
                }
                else if (curTypeStr == "string")
                {//字符串
                    memberVars += "public " + sheet.mTypes[i] + " " + sheet.mVarNames[i] + " = \"\";\t//" + sheet.mComments[i];
                    readDataCode += sheet.mVarNames[i] + " = br.ReadString()" + ";\t//" + sheet.mComments[i]; ;
                }
                else if (curTypeStr == "bool")
                {//布尔
                    memberVars += "public " + sheet.mTypes[i] + " " + sheet.mVarNames[i] + " = false;\t//" + sheet.mComments[i];
                    readDataCode += sheet.mVarNames[i] + " = br.ReadBoolean()" + ";\t//" + sheet.mComments[i]; ;
                }
                else if (curTypeStr.Contains("enum"))
                { //枚举
                    var enumArray = sheet.mTypes[i].Split(":".ToCharArray());
                    if (enumArray.Length < 2)
                        throw new Exception("header enum type error at col " + i);
                    string enumTypeStr = enumArray[1];
                    memberVars += "public " + enumTypeStr + " " + sheet.mVarNames[i] + ";\t//" + sheet.mComments[i];
                    readDataCode += sheet.mVarNames[i] + " = (" + enumTypeStr + ")br.ReadUInt16()" + ";\t//" + sheet.mComments[i];
                }
                else if (curTypeStr.Contains("["))
                {//数组
                    int startIdx_Bracket = curTypeStr.IndexOf('[');
                    int endIdx_Bracket = curTypeStr.IndexOf(']');
                    var arrayTypeStr = curTypeStr.Substring(0, startIdx_Bracket);
                    var arrayCount = 0;
                    if (false == Int32.TryParse(curTypeStr.Substring(startIdx_Bracket + 1, endIdx_Bracket - startIdx_Bracket - 1), out arrayCount))
                        throw new Exception("Array Capacity is missing at col " + i);
                    memberVars += "public " + arrayTypeStr + "[] " + sheet.mVarNames[i] + " = new " + arrayTypeStr + "[" + arrayCount + "];\t//" + sheet.mComments[i];
                    readDataCode += "ushort cnt" + arrayCount + "_" + i + " = br.ReadUInt16();\r\n\t\t\t";
                    readDataCode += "for(ushort i = 0; i < cnt" + arrayCount + "_" + i + "; i++)\r\n\t\t\t\t" + sheet.mVarNames[i] + "[i] = br." + mType2ReadFunc[arrayTypeStr] + ";\t//" + sheet.mComments[i];
                }
                else if (curTypeStr.Contains("List"))
                {//集合
                    int startIdx_Bracket = curTypeStr.IndexOf('<');
                    int endIdx_Bracket = curTypeStr.IndexOf('>');
                    var arrayTypeStr = curTypeStr.Substring(startIdx_Bracket + 1, endIdx_Bracket - startIdx_Bracket - 1);
                    memberVars += "public List<" + arrayTypeStr + "> " + sheet.mVarNames[i] + " = new List<" + arrayTypeStr + ">();\t//" + sheet.mComments[i];
                    readDataCode += "ushort listCount_" + i + " = br.ReadUInt16();\r\n\t\t\t";
                    readDataCode += "for(ushort i = 0; i < listCount_" + i + "; i++)\r\n\t\t\t\t" + sheet.mVarNames[i] + ".Add(br." + mType2ReadFunc[arrayTypeStr] + ");\t//" + sheet.mComments[i];
                }
                else
                {//未识别类型
                    throw new Exception("Unrecognized type at col " + i);
                }
                //换行
                memberVars += "\r\n\t\t";
                readDataCode += "\r\n\t\t\t";
            }//for

            //替换模板文件中的内容
            finalContent = finalContent.Replace("{$MemberVars}", memberVars);
            finalContent = finalContent.Replace("{$ReadDataCode}", readDataCode);
            //保存到文件
            var fs = new FileStream(csFileName, FileMode.Create);
            var sWriter = new StreamWriter(fs);
            sWriter.Write(finalContent);
            sWriter.Close();
        }

        //生成Bin文件
        static void WriteSheet2BinFile(XmlSheet sheet, bool isServer)
        {
			try
			{
				string binFileName = "./../Output/Bin_" + (isServer ? "S" : "C") + "/" + sheet.mName + ".bytes";
				string binTmpFileName = binFileName + ".tmp";
				var fs = new FileStream(binTmpFileName, FileMode.Create);
				var bw = new BinaryWriter(fs);
				string funcString = "public static int WriteData" + sheet.mName + "_" + (isServer ? "S" : "C") + "(System.IO.BinaryWriter bw){\r\ntry{\r\n";
				//写一行数据
				Action<string, string, int, int> funcStringWriteLine = (tTypeStr, tValue, r, c) =>
				{
					if (tTypeStr == "long" || tTypeStr == "ulong" || tTypeStr == "int" || tTypeStr == "uint" ||
							tTypeStr == "short" || tTypeStr == "ushort" || tTypeStr == "byte" || tTypeStr == "float" || tTypeStr == "double")
					{//数字型
						if (tValue == "")
							tValue = "0";
						funcString += "bw.Write((" + tTypeStr + ")" + tValue + ");\r\n";
					}
					else if (tTypeStr == "bool")
					{//布尔布
						var lowerValue = tValue.ToLower();
						if (lowerValue == "0")
							lowerValue = "false";
						else if (lowerValue == "1")
							lowerValue = "true";
						else if (lowerValue != "true" && lowerValue != "false")
							lowerValue = "false";
						//尔
						funcString += "bw.Write((" + tTypeStr + ")" + lowerValue + ");\r\n";
					}
					else if (tTypeStr == "string")
					{//字符串型
						funcString += "bw.Write((" + tTypeStr + ")\"" + tValue + "\");\r\n";
					}
					else if (tTypeStr == "string reference")
					{//字符串引用,转换成ushort型的id
						if (tValue != "")
						{
							if (XmlSheet_Strings.sRef2IDMap.ContainsKey(tValue) == false)
								throw new Exception("Can't find string ref '" + tValue + "'. Sheet=" + sheet.mName);
							funcString += "bw.Write((ushort)" + XmlSheet_Strings.sRef2IDMap[tValue] + ");\r\n";
						}
						else
							funcString += "bw.Write((ushort)" + "0" + ");\r\n";
					}
					else if (tTypeStr.Contains("enum"))
					{ //枚举
						var enumArray = tTypeStr.Split(":".ToCharArray());
						if (enumArray.Length < 2)
							throw new Exception("header enum type error at col " + c);
						string enumTypeStr = enumArray[1];
						funcString += "bw.Write((ushort)" + enumTypeStr + "." + tValue + ");\r\n";
					}
					else
					{//未识别类型
						throw new Exception("Unrecognized type of " + tTypeStr + " at row:" + r + " col:" + c);
					}
				};

				//读表格中所有的行
				for (int r = 0; r < sheet.mValues.Count; r++)
				{//数据第0行就是真实数据。表头3行已去掉
                 //列
                    for (int c = 0; c < sheet.mComments.Count; c++)
                    {
                        if (isServer)
                        {//服务器
                            if (sheet.mRangeType[c] == XmlSheet.ERangeType.Client || sheet.mRangeType[c] == XmlSheet.ERangeType.None)
                                continue;
                        }
                        else
                        {//客户端
                            if (sheet.mRangeType[c] == XmlSheet.ERangeType.Server || sheet.mRangeType[c] == XmlSheet.ERangeType.None)
                                continue;
                        }
                        var curTypeStr = sheet.mTypes[c];
                        if (curTypeStr.Contains("["))
                        {//数组
                            int startIdx_Bracket = curTypeStr.IndexOf('[');
                            int endIdx_Bracket = curTypeStr.IndexOf(']');
                            var arrayTypeStr = curTypeStr.Substring(0, startIdx_Bracket);
                            int arrayCount = 0;
                            if (false == Int32.TryParse(curTypeStr.Substring(startIdx_Bracket + 1, endIdx_Bracket - startIdx_Bracket - 1), out arrayCount))
                                throw new Exception("Array Capacity is missing at row:" + r + " col:" + c);
                            string[] arrayValue = sheet.mValues[r][c].Split("|".ToCharArray());
                            if (arrayValue.Length != arrayCount)
                                throw new Exception("Data array count is not valid at row:" + r + " col:" + c);
                            //数量
                            funcStringWriteLine("ushort", arrayCount.ToString(), r, c);
                            for (int i = 0; i < arrayCount; i++)
                                funcStringWriteLine(arrayTypeStr, arrayValue[i], r, c);//数据
                        }
                        else if (curTypeStr.Contains("List"))
                        {//集合
                            int startIdx_Bracket = curTypeStr.IndexOf('<');
                            int endIdx_Bracket = curTypeStr.IndexOf('>');
                            var arrayTypeStr = curTypeStr.Substring(startIdx_Bracket + 1, endIdx_Bracket - startIdx_Bracket - 1);
                            string[] arrayValue = sheet.mValues[r][c].Split("|".ToCharArray());
                            funcStringWriteLine("ushort", arrayValue.Length.ToString(), r, c);
                            for (int i = 0; i < arrayValue.Length; i++)
                                funcStringWriteLine(arrayTypeStr, arrayValue[i], r, c);//数据
                        }
                        else
                        {//非数组集合
                            funcStringWriteLine(curTypeStr, sheet.mValues[r][c], r, c);
                        }
					}//for col
				}//for row
				funcString += "return 0;\r\n}catch(Exception e){Console.WriteLine(e.ToString());return 1; \r\n}\r\n}";//结束
				Console.WriteLine("--" + funcString);
				RunFunction<int>(funcString, 0, (object)bw);
				bw.Close();
				//加密
				FileDes.EncryptFile(binTmpFileName, binFileName, false);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
        }

        //执行一个字符串的函数。函数声明必须是public static的
        static CodeDomProvider provider = null;
        static CompilerParameters cp = null;
		static int RunFuncCount = 0;
        static T RunFunction<T>(string funcCode, T errorReturnValue, params object[] parameters)
        {
            int idx2 = funcCode.IndexOf("(", 0);
            int idx1 = funcCode.LastIndexOf(" ", idx2);
            string funcName = funcCode.Substring(idx1+1, idx2 - idx1 - 1);
			RunFuncCount++;
            LoadDefineCSFiles();
            
            try
            {
                CompilerResults cr = provider.CompileAssemblyFromSource(cp, "using System;namespace TempNameSpace{public class TempNameClass" + RunFuncCount + "{" + funcCode + "}}");
                if (cr.Errors.Count > 0)
                {
                    string errorstr = "";
                    foreach (CompilerError ce in cr.Errors)
                    {
                        errorstr += ce.ToString() + "\r\n";
                    }
                    throw new Exception(errorstr + "\n\n" + funcCode + "\n\n");
                }
                else
                {
                    MethodInfo mi = cr.CompiledAssembly.GetType("TempNameSpace.TempNameClass" + RunFuncCount).GetMethod(funcName);
                    T result = (T)mi.Invoke(null, parameters);         //执行 mi 所引用的方法   
                    return result;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return errorReturnValue;
            }
        }
        //加载枚举定义的cs文件
        static void LoadDefineCSFiles()
        {
            //if (cp != null)
            //    return;
            provider = CodeDomProvider.CreateProvider("CSharp");
            cp = new CompilerParameters(new string[] { "System.dll" });
            //cp.GenerateInMemory = true;
            cp.OutputAssembly = "./_Defines.dll";
            //遍历目录并编译
            DirectoryInfo folderInfo = new DirectoryInfo("./../Input/EnumDefines");
            if(folderInfo == null)
            {
                Console.WriteLine("Error: No folder named EnumDefines");
            }
            //遍历文件
            FileInfo[] files = folderInfo.GetFiles();
            List<string> csFileList = new List<string>();
            foreach (FileInfo file in files)
            {
                if (file.Extension != ".cs")
                    continue;
                csFileList.Add(file.FullName);
            }
            //编译
            try
            {
                CompilerResults cr = provider.CompileAssemblyFromFile(cp, csFileList.ToArray());
                if (cr.Errors.Count > 0)
                {
                    string errorstr = "";
                    foreach (CompilerError ce in cr.Errors)
                    {
                        errorstr += ce.ToString() + "\r\n";
                    }
                    throw new Exception(errorstr);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            cp = new CompilerParameters(new string[] { "System.dll" , "./_Defines.dll" });
            cp.GenerateInMemory = true;
        }
        //生成StaticDataMgr.cs文件
        private static void WriteStaticDataFile(bool isServer)
        {
            try
            {
                var csFileName = "./../Output/Code_" + (isServer ? "S" : "C") + "/" + "StaticDataMgr.cs";
                //加载Template
                var fs = new FileStream("./../Input/TemplateFiles/StaticDataMgrTemplate_" + (isServer ? "S" : "C") + ".txt", FileMode.Open);
                var sr = new StreamReader(fs);
                string finalContent = sr.ReadToEnd();
                sr.Close();
                //数据表定义
                string strDataDefine = "";
                foreach (var sheet in mSheetList)
                {
                    if (sheet.mName.Equals("String")) continue;
                    if (strDataDefine != "")
                        strDataDefine += "\t\t";
                    strDataDefine += "public Dictionary<uint, " + sheet.mName + "Data> m" + sheet.mName + "DataMap = new Dictionary<uint, " + sheet.mName + "Data>(); //" + sheet.mName + " Data\r\n";
                }
                finalContent = finalContent.Replace("{$DataDefine}", strDataDefine);

                //数据读取过程定义
                string strDataLoad = "";
                foreach (var sheet in mSheetList)
                {
                    if (sheet.mName.Equals("String")) continue;
                    if (strDataLoad != "")
                        strDataLoad += "\t\t\t";
                    //处理函数
                    var funcName = sheet.mName + "DataProcess";
                    if(finalContent.Contains(funcName))//有处理函数的
                        strDataLoad += "LoadDataBinWorker<" + sheet.mName + "Data>(\"" + sheet.mName + ".bytes\", m" + sheet.mName + "DataMap, " + funcName + "); //" + sheet.mName + " Data\r\n";
                    else
                        strDataLoad += "LoadDataBinWorker<" + sheet.mName + "Data>(\"" + sheet.mName + ".bytes\", m" + sheet.mName + "DataMap); //" + sheet.mName + " Data\r\n";

                }
                finalContent = finalContent.Replace("{$LoadData}", strDataLoad);
                //保存到文件
                var wfs = new FileStream(csFileName, FileMode.Create);
                var sWriter = new StreamWriter(wfs);
                sWriter.Write(finalContent);
                sWriter.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("Write StaticDataMgr.cs Error");
                Console.WriteLine(e.ToString());
            }
        }
    }



}
