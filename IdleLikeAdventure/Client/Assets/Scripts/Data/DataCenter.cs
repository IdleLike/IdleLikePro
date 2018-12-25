using Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCenter : ClassSingleton<DataCenter>
{
    public UserData userData;       //用户数据

    public void Initialize()
    {
        userData = new UserData();
    }
}
