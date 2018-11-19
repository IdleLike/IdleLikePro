using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetData.Tools
{
    public static class SerializeTool
    {
        /// <summary>
        /// 注册所有网络消息
        /// </summary>
        /// <param name="register">注册方法</param>
        public static void RegisterMessage(Func<Type, byte, Func<object, byte[]>, Func<byte[], object>, bool> register)
        {
            Assembly assembly = Assembly.Load("NetData");
            Type[] dataTypes = assembly.GetTypes();
            for (byte i = 0; i < dataTypes.Length; i++)
            {
                if (dataTypes[i].Name.Contains("Data"))
                {
                    register(dataTypes[i], i, SerializeMessage, DeserializeMessage);
                }
            }
        }

        /// <summary>
        /// 获取所有MessageData
        /// </summary>
        /// <returns></returns>
        public static Type[] GetMessageTypes()
        {
            List<Type> list = new List<Type>();

            Assembly assembly = Assembly.Load("NetData");
            Type[] dataTypes = assembly.GetTypes();
            for (byte i = 0; i < dataTypes.Length; i++)
            {
                if (dataTypes[i].Name.Contains("Data"))
                {
                    list.Add(dataTypes[i]);
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// 序列化方法
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] SerializeMessage(object msg)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryWriter = new BinaryFormatter();
            binaryWriter.Serialize(memoryStream, msg);
            byte[] data = memoryStream.ToArray();
            memoryStream.Close();

            return data;
        }
        /// <summary>
        /// 反序列化方法
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static object DeserializeMessage(byte[] bytes)
        {
            MemoryStream memoryStream = new MemoryStream(bytes);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            object obj = binaryFormatter.Deserialize(memoryStream);
            return obj;
        }
    }
}
