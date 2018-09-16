using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable][CreateAssetMenu]
public class TestLocalData : ScriptableObject {

    private static TestLocalData instance;
    public static TestLocalData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<TestLocalData>("TestLocalData");
            }
            return instance;
        }
    }

    public LocalData LocalData = new LocalData();
}
