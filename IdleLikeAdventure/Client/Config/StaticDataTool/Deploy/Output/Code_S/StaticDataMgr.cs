using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System;
using StaticData;
using System.Reflection;
using GlobalDefine;
using StaticData.Data;

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
		
        //加载数据
        public void LoadData()
        {
			
						
			//定义如型： void SheetNameDataProcess(ClassType data) 的函数, 会被自动调用

            //设置进度
            Console.WriteLine("Read All Data Done!");
        }

        //根据指定的数据文件名，创建流。 参数格式：“Strings.bytes”
        private Stream OpenBinDataFile(string filename)
        {//
            return FileDes.DecryptFileToStream(FolderCfg.BinFolder() + filename);
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
}


