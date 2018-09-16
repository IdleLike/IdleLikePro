using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System;
using StaticData;
using System.Reflection;
using GlobalDefine;
using StaticData.Data;
using UnityEngine;
using StaticDataTool;

namespace StaticData
{
    public class StaticDataMgr
    {
        private static StaticDataMgr instance = null;
        public static StaticDataMgr mInstance
        {
            get
            {
                if (instance == null)
                    instance = new StaticDataMgr();
                return instance;
            }
            protected set { instance = value; }
        }

        // *************				data	 	***************
		public Dictionary<ushort, TestListData> mTestListDataMap = new Dictionary<ushort, TestListData>(); //TestList Data

        //加载数据
        public void LoadData()
        {
			LoadDataBinWorker<TestListData>("TestList.bytes", mTestListDataMap); //TestList Data

						
			//定义如型： void SheetNameDataProcess(ClassType data) 的函数, 会被自动调用

            //设置进度
            Console.WriteLine("Read All Data Done!");
        }

        //根据指定的数据文件名，创建流。 参数格式：“Strings.bytes”
        private Stream OpenBinDataFile(string filename)
        {//
			TextAsset binDataAsset = Resources.Load(FolderCfg.BinFolder() + filename.Substring(0, filename.Length - 6)) as TextAsset;
            return FileDes.DecryptDataToStream(binDataAsset.bytes);
        }

        void LoadDataBinWorker<ClassType>(string filename, object dic, Action<ClassType> process = null) where ClassType : BaseDataObject, new()
        {
            Dictionary<ushort, ClassType> dataMap = dic as Dictionary<ushort, ClassType>;

            BinaryReader br = null;
            Stream ds = OpenBinDataFile(filename);
            br = new BinaryReader(ds);
            try
            {
                while (true)
                {
                    ClassType tNewData = new ClassType();
                    tNewData.ReadFromStream(br);
                    dataMap.Add(tNewData.mID, tNewData);
                    if (process != null)
                    {
                        process(tNewData);
                    }
                }
            }
            catch (EndOfStreamException)
            {
                Console.WriteLine(filename + "Load Data Done");
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                br.Close();
                FileDes.CloseStream();
            }
            return;
        }
    }//class
    //数据结构基类
    public abstract class BaseDataObject
    {
        public ushort mID = 0; // ID
        public abstract void ReadFromStream(BinaryReader br);
    }

}


