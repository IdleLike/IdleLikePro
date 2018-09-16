/***
 * 
 *    Title: "SUIFW" UI框架项目
 *           主题： UI参数类   
 *    Description: 
 *           功能： 保存界面的传递的参数
 *                  
 *    Date: 2017
 *    Version: 0.1版本
 *    Modify Recoder: 
 *    
 *   
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUIFW
{
	public static class UIParams {
        private static object[] uiParams = null;

        public static void SetParams(params object[] args)
        {
            uiParams = args;
        }

        /// <summary>
        /// 获取参数列表
        /// </summary>
        /// <returns>The parameters.</returns>
        public static object[] GetParams()
        {
            object[] args = uiParams;
            args = null;
            return args;
        }

        /// <summary>
        /// 获取指定类型的参数列表
        /// </summary>
        /// <returns>The parameters.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T[] GetParams<T>()
        {
            T[] args = null;
            if(uiParams != null)
            {
                args = new T[uiParams.Length];
                for (int i = 0; i < uiParams.Length; i++)
                {
                    if(uiParams[i] is T)
                    {
                        args[i] = (T)uiParams[i];
                    }
                    else
                    {
                        args[i] = default(T);
                    }
                }
            }

            uiParams = null;
            return args;
        }

        /// <summary>
        /// 获取第一个参数
        /// </summary>
        /// <returns>The single parameter.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetSingleParam<T>()
        {
            if(uiParams == null || uiParams.Length <= 0 || uiParams[0] == null || !(uiParams[0] is T))
            {
                uiParams = null;
                return default(T);
            }
            T returnVO = (T)uiParams[0];
            uiParams = null;
            return returnVO;
        }

	}
}