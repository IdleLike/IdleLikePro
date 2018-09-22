using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Log
{


    public class TConsole
    {

        /// <summary>
        /// TConsole
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="context"></param>
        static public void ConsoleLog(object message, Level level, UnityEngine.Object context = null)
        {
            System.Console.WriteLine(Now() + " " + message);
        }


        static private string Now()
        {
            return System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
        }
    }
}
