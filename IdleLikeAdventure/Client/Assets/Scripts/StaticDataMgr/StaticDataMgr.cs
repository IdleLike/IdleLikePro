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
	
	// 多语言配置
	private Dictionary<string, StringData> mStringDataMap = new Dictionary<string, StringData>();
		

        // *************				data	 	***************
		public Dictionary<uint, CareerData> mCareerDataMap = new Dictionary<uint, CareerData>(); //Career Data
		public Dictionary<uint, CareerAbilityData> mCareerAbilityDataMap = new Dictionary<uint, CareerAbilityData>(); //CareerAbility Data
		public Dictionary<uint, LevelData> mLevelDataMap = new Dictionary<uint, LevelData>(); //Level Data
		public Dictionary<uint, MonsterData> mMonsterDataMap = new Dictionary<uint, MonsterData>(); //Monster Data
		public Dictionary<uint, RaceData> mRaceDataMap = new Dictionary<uint, RaceData>(); //Race Data
		public Dictionary<uint, RaceAbilityData> mRaceAbilityDataMap = new Dictionary<uint, RaceAbilityData>(); //RaceAbility Data
		public Dictionary<uint, StageData> mStageDataMap = new Dictionary<uint, StageData>(); //Stage Data
		public Dictionary<uint, TestData> mTestDataMap = new Dictionary<uint, TestData>(); //Test Data

        //加载数据
        public void LoadData()
        {
			LoadDataBinWorkerString<StringData>("String.bytes", mStringDataMap);
			
			LoadDataBinWorker<CareerData>("Career.bytes", mCareerDataMap); //Career Data
			LoadDataBinWorker<CareerAbilityData>("CareerAbility.bytes", mCareerAbilityDataMap); //CareerAbility Data
			LoadDataBinWorker<LevelData>("Level.bytes", mLevelDataMap); //Level Data
			LoadDataBinWorker<MonsterData>("Monster.bytes", mMonsterDataMap); //Monster Data
			LoadDataBinWorker<RaceData>("Race.bytes", mRaceDataMap); //Race Data
			LoadDataBinWorker<RaceAbilityData>("RaceAbility.bytes", mRaceAbilityDataMap); //RaceAbility Data
			LoadDataBinWorker<StageData>("Stage.bytes", mStageDataMap); //Stage Data
			LoadDataBinWorker<TestData>("Test.bytes", mTestDataMap); //Test Data

						
			//定义如型： void SheetNameDataProcess(ClassType data) 的函数, 会被自动调用

            //设置进度
            System.Console.WriteLine("Read All Data Done!");
        }

        //根据指定的数据文件名，创建流。 参数格式：“Strings.bytes”
        private Stream OpenBinDataFile(string filename)
        {//
			TextAsset binDataAsset = Resources.Load(FolderCfg.BinFolder() + filename.Substring(0, filename.Length - 6)) as TextAsset;
            return FileDes.DecryptDataToStream(binDataAsset.bytes);
        }

        void LoadDataBinWorker<ClassType>(string filename, object dic, Action<ClassType> process = null) where ClassType : BaseDataObject, new()
        {
            Dictionary<uint, ClassType> dataMap = dic as Dictionary<uint, ClassType>;

            BinaryReader br = null;
            Stream ds = OpenBinDataFile(filename);
            br = new BinaryReader(ds);
            try
            {
                while (true)
                {
                    ClassType tNewData = new ClassType();
                    tNewData.ReadFromStream(br);
                    dataMap.Add(tNewData.ID, tNewData);
                    if (process != null)
                    {
                        process(tNewData);
                    }
                }
            }
            catch (EndOfStreamException)
            {
                System.Console.WriteLine(filename + "Load Data Done");
            }
            catch (IOException e)
            {
                System.Console.WriteLine(e.ToString());
            }
            finally
            {
                br.Close();
                FileDes.CloseStream();
            }
            return;
        }

        void LoadDataBinWorkerString<ClassType>(string filename, object dic, Action<ClassType> process = null) where ClassType : BaseDataStringIDObject, new()
        {
            Dictionary<string, ClassType> dataMap = dic as Dictionary<string, ClassType>;

            BinaryReader br = null;
            Stream ds = OpenBinDataFile(filename);
            br = new BinaryReader(ds);
            try
            {
                while (true)
                {
                    ClassType tNewData = new ClassType();
                    tNewData.ReadFromStream(br);
                    dataMap.Add(tNewData.ID, tNewData);
                    if (process != null)
                    {
                        process(tNewData);
                    }
                }
            }
            catch (EndOfStreamException)
            {
                System.Console.WriteLine(filename + "Load Data Done");
            }
            catch (IOException e)
            {
                System.Console.WriteLine(e.ToString());
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
        public uint ID = 0; // ID
        public abstract void ReadFromStream(BinaryReader br);
    }
	public abstract class BaseDataStringIDObject
    {
        public string ID = string.Empty; // ID
        public abstract void ReadFromStream(BinaryReader br);
    }

}


