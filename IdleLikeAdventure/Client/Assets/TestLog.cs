using Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLog : MonoBehaviour {


    public void Test()
    {
        TLog.LogInput("qqq",Level.Special,Logtype.writeFile);
    }
}
